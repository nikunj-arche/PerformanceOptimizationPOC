//using NBomber.Contracts;
//using NBomber.CSharp;
//using System.Diagnostics;
//using System.Net.Http;

//class Program
//{
//    static void Main(string[] args)
//    {
//        var baseUrl = "https://localhost:7232";
//        using var httpClient = new HttpClient();

//        var stepSync = Step.Create("sync_call", async context =>
//        {
//            var response = await httpClient.GetAsync($"{baseUrl}/api/products/sync");
//            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
//        });

//        var stepAsync = Step.Create("async_call", async context =>
//        {
//            var response = await httpClient.GetAsync($"{baseUrl}/api/products/async");
//            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
//        });

//        var scenario = ScenarioBuilder.CreateScenario("sync_vs_async", stepSync, stepAsync)
//            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
//            .WithLoadSimulations(Simulation.KeepConstant(10, TimeSpan.FromSeconds(30)));

//        NBomberRunner
//            .RegisterScenarios(scenario)
//            .Run();
//    }
//}


//using NBomber.Contracts;
//using NBomber.CSharp;
//using System;
//using System.Diagnostics;
//using System.Linq;
//using System.Net.Http;

//class Program
//{
//    static void Main(string[] args)
//    {
//        var baseUrl = "https://localhost:7232";
//        using var httpClient = new HttpClient();

//        long totalSyncMemoryUsed = 0;
//        long totalAsyncMemoryUsed = 0;
//        long totalSyncCpuTime = 0;
//        long totalAsyncCpuTime = 0;
//        int syncRequestCount = 0;
//        int asyncRequestCount = 0;

//        var stepSync = Step.Create("sync_call", async context =>
//        {
//            var stopwatch = Stopwatch.StartNew();
//            var initialMemory = GC.GetTotalMemory(false);

//            var response = await httpClient.GetAsync($"{baseUrl}/api/products/sync");

//            stopwatch.Stop();
//            var finalMemory = GC.GetTotalMemory(false);

//            var memoryUsed = finalMemory - initialMemory;
//            totalSyncMemoryUsed += memoryUsed;
//            totalSyncCpuTime += stopwatch.ElapsedMilliseconds;
//            syncRequestCount++;

//            return response.IsSuccessStatusCode
//                ? Response.Ok(payload: stopwatch.ElapsedMilliseconds)
//                : Response.Fail();
//        });

//        var stepAsync = Step.Create("async_call", async context =>
//        {
//            var stopwatch = Stopwatch.StartNew();
//            var initialMemory = GC.GetTotalMemory(false);

//            var response = await httpClient.GetAsync($"{baseUrl}/api/products/async");

//            stopwatch.Stop();
//            var finalMemory = GC.GetTotalMemory(false);

//            var memoryUsed = finalMemory - initialMemory;
//            totalAsyncMemoryUsed += memoryUsed;
//            totalAsyncCpuTime += stopwatch.ElapsedMilliseconds;
//            asyncRequestCount++;

//            return response.IsSuccessStatusCode
//                ? Response.Ok(payload: stopwatch.ElapsedMilliseconds)
//                : Response.Fail();
//        });

//        var scenario = ScenarioBuilder.CreateScenario("sync_vs_async", stepSync, stepAsync)
//            .WithLoadSimulations(Simulation.KeepConstant(10, TimeSpan.FromSeconds(100)));

//        var stats = NBomberRunner
//            .RegisterScenarios(scenario)
//            .Run();

//        var syncStats = stats.ScenarioStats[0].StepStats.First(s => s.StepName == "sync_call");
//        var asyncStats = stats.ScenarioStats[0].StepStats.First(s => s.StepName == "async_call");

//        // Printing key metrics  
//        Console.WriteLine($"Sync Call - Avg Response Time: {syncStats.Ok.Latency.MeanMs} ms");
//        Console.WriteLine($"Async Call - Avg Response Time: {asyncStats.Ok.Latency.MeanMs} ms");

//        Console.WriteLine($"Sync Call - RPS: {syncStats.Ok.Request.Count / 100.0} req/sec");
//        Console.WriteLine($"Async Call - RPS: {asyncStats.Ok.Request.Count / 100.0} req/sec");

//        Console.WriteLine($"Sync Call - Error Rate: {syncStats.Fail.Request.Count / (double)(syncStats.Ok.Request.Count + syncStats.Fail.Request.Count) * 100}%");
//        Console.WriteLine($"Async Call - Error Rate: {asyncStats.Fail.Request.Count / (double)(asyncStats.Ok.Request.Count + asyncStats.Fail.Request.Count) * 100}%");

//        // Printing average CPU and memory usage  
//        Console.WriteLine($"Sync Call - Avg CPU Time: {totalSyncCpuTime / (double)syncRequestCount} ms");
//        Console.WriteLine($"Sync Call - Avg Memory Used: {totalSyncMemoryUsed / (double)syncRequestCount / 1024.0} KB");

//        Console.WriteLine($"Async Call - Avg CPU Time: {totalAsyncCpuTime / (double)asyncRequestCount} ms");
//        Console.WriteLine($"Async Call - Avg Memory Used: {totalAsyncMemoryUsed / (double)asyncRequestCount / 1024.0} KB");

//        Console.ReadLine();
//    }
//}











using NBomber.Contracts;
using NBomber.CSharp;
using System.Net.Http;
using System.Threading.Tasks;

public class Program
{
    public static void Main()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };

        var stepAsync = Step.Create("async_call", async context =>
        {
            var response = await httpClient.GetAsync("/api/products/getproductsasync");
            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
        });

        var stepSync = Step.Create("sync_call", async context =>
        {
            var response = await httpClient.GetAsync("/api/products/getproductssync");
            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
        });

        var scenario = ScenarioBuilder.CreateScenario("API Load Test", stepAsync, stepSync)
            .WithLoadSimulations(Simulation.InjectPerSec(rate: 100, during: TimeSpan.FromMinutes(1)));

        NBomberRunner.RegisterScenarios(scenario).Run();
    }
}
