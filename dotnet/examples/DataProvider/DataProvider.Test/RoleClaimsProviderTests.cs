using Microsoft.Extensions.Logging;
using NSubstitute;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataProvider.Web.Services;
using Xunit;
using Xunit.Abstractions;

namespace XUnitTestProject1
{
    public class RoleClaimsProviderTest
    {
        private readonly ITestOutputHelper output;
        private readonly RoleClaimsProvider roleClaimsProvider;

        public RoleClaimsProviderTest(ITestOutputHelper output)
        {
            var logger = Substitute.For<ILogger<RoleClaimsProvider>>();
            logger.When(x => x.Log<object>(Arg.Any<LogLevel>(), Arg.Any<EventId>(), Arg.Any<object>(), Arg.Any<Exception>(), Arg.Any<Func<object, Exception, string>>()))
                .Do(x =>
                {
                    output.WriteLine("Current threadid: " + Task.CurrentId);
                });
            roleClaimsProvider = new RoleClaimsProvider(Substitute.For<DataInfrastructure>(), logger);
            this.output = output;
        }

        [Fact]
        public async Task MultipleThreadsGetDataTest()
        {
            await StartThreads(1000, () => roleClaimsProvider.GetDataAsync(default(CancellationToken)));
            await StartThreads(1000, () => roleClaimsProvider.GetDataAsync(default(CancellationToken)));
            await StartThreads(1000, () => roleClaimsProvider.GetDataAsync(default(CancellationToken)));
        }

        private Task StartThreads(int count, Func<Task<int>> getData)
        {
            //Enumerable.Range(1, count).AsParallel().ForAll(async n => await Task.Run(async () => await getData()));

            var tasks = Enumerable.Range(1, count).Select(n => Task.Run(async () => await getData()));
            return Task.WhenAll(tasks);// tasks.AsParallel().ForAll(t => { if (t.Status != TaskStatus.Created && t.Status != TaskStatus.WaitingForActivation) t.Start(); });
        }
    }
}
