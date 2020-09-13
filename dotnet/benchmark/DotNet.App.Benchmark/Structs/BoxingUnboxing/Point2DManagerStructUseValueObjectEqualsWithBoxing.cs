using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using DotNet.App.Structs.BoxingUnboxing;

namespace DotNet.App.Benchmark.Structs.BoxingUnboxing
{
    public class Point2DManagerStructUseValueObjectEqualsWithBoxing : BoxingUnboxingBenchmarkBase
    {
        private Point2DManager<Point2D> manager;

        [GlobalSetup(Target = nameof(Benchmark))]
        public void Setup()
        {
            var r = new Random();
            var points = new List<Point2D>();
            for (int i = 0; i < ItemsCount; i++)
            {
                points.Add(new Point2D(r.Next(RangeStart, RangeEnd), r.Next(RangeStart, RangeEnd)));
            }
            points.Add(new Point2D(ItemForSearch, ItemForSearch));
            manager = new Point2DManager<Point2D>(points);
        }

        [Benchmark(Baseline = true)]
        public void Benchmark()
        {
            var searchItem = new Point2D(ItemForSearch, ItemForSearch);
            var isFound = manager.Contains(searchItem);
        }
    }
}
