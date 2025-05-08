```

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.5737/22H2/2022Update)
Intel Core i7-6600U CPU 2.60GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET SDK 9.0.203
  [Host]     : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX2 [AttachedDebugger]
  Job-TPHPCM : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX2

IterationCount=10  LaunchCount=1  WarmupCount=3  

```
| Method                         | Mean             | Error            | StdDev           | Gen0      | Gen1     | Gen2     | Allocated |
|------------------------------- |-----------------:|-----------------:|-----------------:|----------:|---------:|---------:|----------:|
| GetProductsSync                | 13,985,031.88 ns | 2,930,207.492 ns | 1,938,150.830 ns | 1250.0000 |        - |        - | 2746520 B |
| GetProductsAsync               | 25,445,712.50 ns |   626,193.177 ns |   414,188.016 ns |  968.7500 | 406.2500 | 125.0000 | 5509547 B |
| GetProductsWithSlowLinq        | 18,117,936.56 ns | 1,704,462.120 ns | 1,127,396.159 ns |  687.5000 | 281.2500 |  93.7500 | 3907718 B |
| GetProductsWithCaching         |         50.05 ns |         2.549 ns |         1.517 ns |         - |        - |        - |         - |
| GetProductsWithUnoptimizedLinq |  9,958,737.33 ns |   523,424.463 ns |   311,481.502 ns | 1265.6250 |  31.2500 |  15.6250 | 2746536 B |
| GetProductsWithOptimizedLinq   | 16,793,996.48 ns |   542,457.830 ns |   283,715.934 ns |  687.5000 | 281.2500 |  93.7500 | 3907216 B |
