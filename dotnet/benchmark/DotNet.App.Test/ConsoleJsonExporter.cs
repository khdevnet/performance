using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Horology;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace DotNet.App.Test
{
    public static class ConsoleJsonExporter
    {
        public static void Log(Summary summary, ITestOutputHelper output)
        {
            var logger = new OutputLogger(output);

            // We construct HostEnvironmentInfo manually, so that we can have the HardwareTimerKind enum as text, rather than an integer
            // SimpleJson serializer doesn't seem to have an enum String/Value option (to-be-fair, it is meant to be "Simple")
            //var environmentInfo = new
            //{
            //    HostEnvironmentInfo.BenchmarkDotNetCaption,
            //    summary.HostEnvironmentInfo.BenchmarkDotNetVersion,
            //    OsVersion = summary.HostEnvironmentInfo.OsVersion.Value,
            //    ProcessorName = ProcessorBrandStringHelper.Prettify(summary.HostEnvironmentInfo.CpuInfo.Value),
            //    summary.HostEnvironmentInfo.CpuInfo.Value?.PhysicalProcessorCount,
            //    summary.HostEnvironmentInfo.CpuInfo.Value?.PhysicalCoreCount,
            //    summary.HostEnvironmentInfo.CpuInfo.Value?.LogicalCoreCount,
            //    summary.HostEnvironmentInfo.RuntimeVersion,
            //    summary.HostEnvironmentInfo.Architecture,
            //    summary.HostEnvironmentInfo.HasAttachedDebugger,
            //    summary.HostEnvironmentInfo.HasRyuJit,
            //    summary.HostEnvironmentInfo.Configuration,
            //    DotNetCliVersion = summary.HostEnvironmentInfo.DotNetSdkVersion.Value,
            //    summary.HostEnvironmentInfo.ChronometerFrequency,
            //    HardwareTimerKind = summary.HostEnvironmentInfo.HardwareTimerKind.ToString()
            //};

            // If we just ask SimpleJson to serialize the entire "summary" object it throws several errors.
            // So we are more specific in what we serialize (plus some fields/properties aren't relevant)

            var benchmarks = summary.Reports.Select(report =>
            {
                var data = new Dictionary<string, object>
                {
                    // We don't need Benchmark.ShortInfo, that info is available via Benchmark.Parameters below
                    { "DisplayInfo", report.BenchmarkCase.DisplayInfo },
                    { "Namespace", report.BenchmarkCase.Descriptor.Type.Namespace },
                    { "Type", GetTypeName(report.BenchmarkCase.Descriptor.Type) },
                    { "Method", report.BenchmarkCase.Descriptor.WorkloadMethod.Name },
                    { "MethodTitle", report.BenchmarkCase.Descriptor.WorkloadMethodDisplayInfo },
                    { "Parameters", report.BenchmarkCase.Parameters.PrintInfo },
                    { "FullName", FullNameProvider.GetBenchmarkName(report.BenchmarkCase) }, // do NOT remove this property, it is used for xunit-performance migration
                    { "ExecutionTime", $"{report.ResultStatistics.Mean.ToTimeStr(TimeUnit.GetBestTimeUnit(report.ResultStatistics.Mean))}" },
                };

                // We show MemoryDiagnoser's results only if it is being used
                if (report.BenchmarkCase.Config.HasMemoryDiagnoser())
                {
                    data.Add("Memory", report.GcStats);
                }

                return data;
            });

            logger.WriteLine(JsonConvert.SerializeObject(new Dictionary<string, object>
            {
                { "Title", summary.Title },
                // { "HostEnvironmentInfo", environmentInfo },
                { "Benchmarks", benchmarks }
            }, Formatting.Indented));
        }
        public static string GetTypeName(Type type)
        {
            if (!type.IsGenericType)
                return type.Name;

            string mainName = type.Name.Substring(0, type.Name.IndexOf('`'));
            string args = string.Join(", ", type.GetGenericArguments().Select(GetTypeName).ToArray());

            return $"{mainName}<{args}>";
        }

        public static string ToTimeStr(this double value, TimeUnit unit = null, int unitNameWidth = 1, bool showUnit = true, string format = "N4",
   Encoding encoding = null)
        {
            unit = unit ?? TimeUnit.GetBestTimeUnit(value);
            double unitValue = TimeUnit.Convert(value, TimeUnit.Nanosecond, unit);
            if (showUnit)
            {
                string unitName = unit.Name.ToString(encoding ?? Encoding.ASCII).PadLeft(unitNameWidth);
                return $"{unitValue.ToStr(format)} {unitName}";
            }

            return $"{unitValue.ToStr(format)}";
        }

        public static string ToStr(this double value, string format = "0.##")
        {
            var args = new object[] { value };
            return string.Format(HostEnvironmentInfo.MainCultureInfo, $"{{0:{format}}}", args);
        }
    }

}
