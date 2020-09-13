using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace DotNet.App.Benchmark.FrugalObject
{
    public class StringArrayAddOneElementToCollectionBenchmark
    {
        private const int ItemsCount = 10000000;

        [Benchmark]
        public List<string[]> Run()
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
