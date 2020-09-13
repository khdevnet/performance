using DotNet.App.Benchmark.FrugalObject;
using Xunit;
using Xunit.Abstractions;

namespace DotNet.App.Test.FrugalObject
{
    public class FrugalObjectTests : BenchmarkTestBase
    {
        public FrugalObjectTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void StringValuesAddOneElementToCollectionBenchmarkTest()
        {
            Run<StringValuesAddOneElementToCollectionBenchmark>();
        }

        [Fact]
        public void StringArrayAddOneElementToCollectionBenchmarkTest()
        {
            Run<StringArrayAddOneElementToCollectionBenchmark>();
        }
    }
}
