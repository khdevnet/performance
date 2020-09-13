using BenchmarkDotNet.Attributes;

namespace DotNet.App.Benchmark.Samples
{
    public class BoxingSampleBenchmark
    {
        [Benchmark]
        public object Test()
        {
            object s = 5;
            return s;
        }
    }
}
