using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using DotNet.App.Structs.BoxingUnboxing;

namespace DotNet.App.Benchmark.Structs.BoxingUnboxing
{
    public class Point2DManagerStructUseCustomEqualsWithoutBoxing : BoxingUnboxingBenchmarkBase
    {
        private Point2DManager<Point2DCustomEquals> manager;

        [GlobalSetup(Target = nameof(Benchmark))]
        public void StructUseCustomEqualsWithoutBoxingSetup()
        {
            var r = new Random();
            var points = new List<Point2DCustomEquals>();
            for (int i = 0; i < ItemsCount; i++)
            {
                points.Add(new Point2DCustomEquals(r.Next(RangeStart, RangeEnd), r.Next(RangeStart, RangeEnd)));
            }
            points.Add(new Point2DCustomEquals(ItemForSearch, ItemForSearch));
            manager = new Point2DManager<Point2DCustomEquals>(points);
        }

        [Benchmark]
        public void Benchmark()
        {
            var searchItem = new Point2DCustomEquals(ItemForSearch, ItemForSearch);
            manager.ContainsCustom(searchItem);
        }

    }
}
