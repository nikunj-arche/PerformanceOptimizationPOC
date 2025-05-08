using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkRunnerApp;

public class Program
{
    public static void Main(string[] args)
    {
        //BenchmarkRunner.Run<BenchmarkTests>();
        Summary summary = BenchmarkRunner.Run<BenchmarkTests>();

        Console.WriteLine("\n--- Converted Benchmark Summary (in μs) ---");
        foreach (var report in summary.Reports)
        {
            var method = report.BenchmarkCase.Descriptor.WorkloadMethod.Name;
            var stats = report.ResultStatistics;

            if (stats != null)
            {
                double meanUs = stats.Mean / 1_000.0; // Convert ns to μs  
                double allocatedKb = report.GcStats.GetBytesAllocatedPerOperation(report.BenchmarkCase) >= 0
                    ? report.GcStats.GetBytesAllocatedPerOperation(report.BenchmarkCase).Value / 1024.0
                    : 0;

                double gen0 = report.GcStats.Gen0Collections;

                Console.WriteLine($"{method,-35} | Mean: {meanUs,10:F4} μs | Allocated: {allocatedKb,8:F4} KB | Gen0: {gen0}");
            }
        }
        Console.ReadLine();
    }
}
