using DotNet.App.Benchmark.Structs.BoxingUnboxing;
using Xunit;
using Xunit.Abstractions;

namespace DotNet.App.Test.Structs.BoxingUnboxing
{
    public class Point2DManagerTests : BenchmarkTestBase
    {
        public Point2DManagerTests(ITestOutputHelper outputHelper)
            : base(outputHelper)
        {
        }

        [Fact]
        public void StructUseValueObjectEqualsWithBoxingBenchmarkTest()
        {
            Run<Point2DManagerStructUseValueObjectEqualsWithBoxing>();
        }

        [Fact]
        public void StructUseCustomEqualsWithoutBoxingBenchmarkTest()
        {
            Run<Point2DManagerStructUseCustomEqualsWithBoxingInContains>();
        }

    }
}
