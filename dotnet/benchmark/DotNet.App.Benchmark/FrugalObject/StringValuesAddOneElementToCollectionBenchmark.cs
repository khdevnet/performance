using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Primitives;

namespace DotNet.App.Benchmark.FrugalObject
{
    public class StringValuesAddOneElementToCollectionBenchmark
    {
        private const int ItemsCount = 10000000;

        [Benchmark]
        public List<StringValues> Run()
        {
            var testStr = "str";
            var stringArrays = new List<StringValues>();
            for (int i = 0; i < ItemsCount; i++)
            {
                stringArrays.Add(new StringValues(testStr));
            }

            return stringArrays;
        }

    }
}
