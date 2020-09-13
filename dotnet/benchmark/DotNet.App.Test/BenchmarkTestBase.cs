using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace DotNet.App.Test
{
    public abstract class BenchmarkTestBase
    {
        private readonly ITestOutputHelper output;

        public BenchmarkTestBase(ITestOutputHelper outputHelper)
        {
            output = outputHelper;
        }

        public void Run<TBenchmark>()
        {
            var config = DefaultConfig.Instance
                           .With(MemoryDiagnoser.Default);

            var summary = BenchmarkRunner.Run<TBenchmark>(config);

            ConsoleJsonExporter.Log(summary, output);
        }
    }
}
