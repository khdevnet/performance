using System;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using DotNet.App.Benchmark.Samples;
using DotNet.App.Benchmark.Structs.BoxingUnboxing;

namespace DotNet.App.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = DefaultConfig.Instance
                .With(MemoryDiagnoser.Default)
                .With(DisassemblyDiagnoser.Create(new DisassemblyDiagnoserConfig(printAsm: true, printIL: true, recursiveDepth: 3, printDiff: true)));
            var summary = BenchmarkRunner.Run<Point2DManagerStructUseCustomEqualsWithoutBoxing>(config);
            Console.ReadKey();
        }
    }
}
