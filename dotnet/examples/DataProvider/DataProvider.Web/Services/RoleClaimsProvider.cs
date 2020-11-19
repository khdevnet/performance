using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DataProvider.Web.Services
{
    public class RoleClaimsProvider
    {
        private readonly DataInfrastructure dataInfrastructure;
        private readonly ILogger<RoleClaimsProvider> logger;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private static Task<int> dataTask;
        private static int? data;

        public RoleClaimsProvider(DataInfrastructure dataInfrastructure, ILogger<RoleClaimsProvider> logger)
        {
            this.dataInfrastructure = dataInfrastructure;
            this.logger = logger;
        }

        public async Task RefreshDataAsync(CancellationToken cancellationToken)
        {
            if (IsDataLoaded())
            {
                logger.LogError("#### RefreshDataAsync: data loaded");
                data = await RetryWhenDeadLockAsync(() => GetDataFromDatabaseAsync(cancellationToken));
            }
        }

        public async Task InitAsync(CancellationToken cancellationToken)
        {
            logger.LogError("#### InitAsync TreadId: " + Thread.CurrentThread.ManagedThreadId);
            dataTask = GetDataFromDatabaseAsync(cancellationToken);
            data = await dataTask;
        }

        private Task<int> GetDataFromDatabaseAsync(CancellationToken cancellationToken)
        {
            return dataInfrastructure.GetDataAsync();
        }

        private async Task<TData> RetryWhenDeadLockAsync<TData>(Func<Task<TData>> actionAsync)
        {
            Exception deadLockEx = null;
            for (int attempt = 1; attempt <= 5; attempt++)
            {
                try
                {
                    return await actionAsync();
                }
                catch (DeadLockException ex) when (attempt <= 5)//when(ex is DatabaseException || ex is MySqlException) add deadlock conditions
                {
                    deadLockEx = ex;
                    logger.LogError("#### Deadlcok, Retry: " + attempt, ex);
                    await Task.Delay(TimeSpan.FromMilliseconds(500 * attempt));
                }
                catch (Exception)
                {
                    throw;
                }
            }

            throw deadLockEx;
        }

        public async Task<int> GetDataAsync(CancellationToken cancellationToken)
        {
            if (!IsInitialized())
            {
                await semaphoreSlim.WaitAsync();
                try
                {
                    if (IsInitialized()) return await dataTask;
                    await InitAsync(cancellationToken);
                    return await dataTask;
                }
                catch (Exception ex) //TODO: Test only
                {
                    logger.LogError("#### InitAsync Error TreadId: " + Thread.CurrentThread.ManagedThreadId);
                    throw;
                }
                finally
                {
                    semaphoreSlim.Release();
                }
            }

            if (!IsDataLoaded())
            {
                return await dataTask;
            }

            return data.Value;
        }

        private static bool IsInitialized()
        {
            return dataTask != null && !dataTask.IsFaulted;
        }

        private static bool IsDataLoaded()
        {
            return data != null;
        }
    }
}
