using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Primitives;

namespace DotNet.App.Benchmark.FrugalObject
{

    public class StringValuesVsStringArrayBenchmark
    {
        private const int ItemsCount = 10000000;

        [Benchmark(Baseline = true)]
        public List<StringValues> RunStringValues()
        {
            var testStr = "str";
            var stringArrays = new List<StringValues>();
            for (int i = 0; i < ItemsCount; i++)
            {
                stringArrays.Add(new StringValues(testStr));
            }

            return stringArrays;
        }

        [Benchmark]
        public List<string[]> RunStringArray()
        {
            var testStr = "str";
            var stringArrays = new List<string[]>();
            for (int i = 0; i < ItemsCount; i++)
            {
                stringArrays.Add(new[] { testStr });
            }

            return stringArrays;
        }

    }
}
