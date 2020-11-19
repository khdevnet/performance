using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using DataProvider.Web.Services;

namespace BackgroundTasksSample.Services
{
    internal class TimedHostedService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly TimeSpan refreshIntervalInSeconds = TimeSpan.FromSeconds(5); // 60*60
        private readonly RoleClaimsProvider roleClaimsProvider;
        private readonly ILogger<TimedHostedService> timedHostedServiceLogger;

        public TimedHostedService(RoleClaimsProvider roleClaimsProvider, ILogger<TimedHostedService> timedHostedServiceLogger)
        {
            this.roleClaimsProvider = roleClaimsProvider;
            this.timedHostedServiceLogger = timedHostedServiceLogger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
                async (obj) => await RefreshDataAsync(cancellationToken),
                null,
                refreshIntervalInSeconds,
                TimeSpan.Zero);
            await roleClaimsProvider.InitAsync(cancellationToken);
        }

        private async Task RefreshDataAsync(CancellationToken cancellationToken)
        {
            timedHostedServiceLogger.LogError("#### RefreshDataAsync: " + DateTime.Now);

            await RestartTimerWhenActionCompleteAsync(() => roleClaimsProvider.RefreshDataAsync(cancellationToken));
        }

        private async Task RestartTimerWhenActionCompleteAsync(Func<Task> actionAsync)
        {
            try
            {
                await actionAsync();
            }
            catch (Exception ex)
            {
                timedHostedServiceLogger.LogError("#### Refresh roles error, restart timer", ex);
            }
            finally
            {
                timedHostedServiceLogger.LogError("#### Reset timer");
                _timer.Change(refreshIntervalInSeconds, TimeSpan.Zero);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}