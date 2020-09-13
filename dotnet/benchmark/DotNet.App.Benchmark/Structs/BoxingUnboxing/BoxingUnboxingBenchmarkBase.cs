using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet.App.Benchmark.Structs.BoxingUnboxing
{
    public abstract class BoxingUnboxingBenchmarkBase
    {
        protected const int ItemForSearch = 999999;
        protected const int ItemsCount = 10000000;
        protected const int RangeStart = 1;
        protected const int RangeEnd = 999999;
    }
}
