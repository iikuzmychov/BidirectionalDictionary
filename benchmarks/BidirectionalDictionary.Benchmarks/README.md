# Benchmarks

BenchmarkDotNet suite comparing:

- `BidirectionalDictionary<TKey, TValue>` vs `Dictionary<TKey, TValue>`
- `ReadOnlyBidirectionalDictionary<TKey, TValue>` vs `ReadOnlyDictionary<TKey, TValue>`

The suite uses deterministic `int -> int` read data with `10_000` items for reading benchmarks,
and `1_000_000` items for mutation benchmarks.

## Running

Run benchmarks in Release mode:

```bash
dotnet run -c Release --project benchmarks/BidirectionalDictionary.Benchmarks -- --filter '*Mutable*'
dotnet run -c Release --project benchmarks/BidirectionalDictionary.Benchmarks -- --filter '*ReadOnly*'
dotnet run -c Release --project benchmarks/BidirectionalDictionary.Benchmarks -- --filter '*'
```

## Reports

Reports are written to `BenchmarkDotNet.Artifacts/results/` relative to the
working directory used to launch the benchmark process.

Each benchmark class emits:

- `*-report.html`
- `*-report-github.md`
- `*-report.csv`

## Latest Results

### Mutable

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26200.8246)
Unknown processor
.NET SDK 10.0.103
  [Host]     : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
  Job-REKOJT : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
  DefaultJob : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2

AnalyzeLaunchVariance=False  EvaluateOverhead=Default  MaxAbsoluteError=Default  
MaxRelativeError=Default  MinInvokeCount=Default  MinIterationTime=Default  
OutlierMode=Default  Affinity=11111111  EnvironmentVariables=Empty  
Jit=RyuJit  LargeAddressAware=Default  Platform=X64  
PowerPlanMode=8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c  Runtime=.NET 10.0  AllowVeryLargeObjects=False  
Concurrent=True  CpuGroups=False  Force=True  
HeapAffinitizeMask=Default  HeapCount=Default  NoAffinitize=False  
RetainVm=False  Server=False  Arguments=Default  
BuildConfiguration=Default  Clock=Default  EngineFactory=Default  
NuGetReferences=Default  Toolchain=Default  IsMutator=Default  
IterationCount=Default  IterationTime=Default  LaunchCount=Default  
MaxIterationCount=Default  MaxWarmupIterationCount=Default  MemoryRandomization=Default  
MinIterationCount=Default  MinWarmupIterationCount=Default  RunStrategy=Default  
WarmupCount=Default  

```
| Operation       | Type          | Mean           | Error         | StdDev        | Median         | Ratio | RatioSD | Gen0    | Gen1    | Gen2    | Allocated | Alloc Ratio |
|---------------- |-------------- |---------------:|--------------:|--------------:|---------------:|------:|--------:|--------:|--------:|--------:|----------:|------------:|
| Add             | Default       |       6.640 ns |     0.4982 ns |     1.4454 ns |       6.166 ns |  1.04 |    0.31 |       - |       - |       - |         - |          NA |
| Add             | Bidirectional |      24.335 ns |     0.8006 ns |     2.3227 ns |      24.347 ns |  3.83 |    0.85 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| ContainsKeyHit  | Default       |       3.748 ns |     0.0240 ns |     0.0212 ns |       3.743 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| ContainsKeyHit  | Bidirectional |       3.967 ns |     0.0210 ns |     0.0197 ns |       3.958 ns |  1.06 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| ContainsKeyMiss | Default       |       3.559 ns |     0.0231 ns |     0.0216 ns |       3.556 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| ContainsKeyMiss | Bidirectional |       3.728 ns |     0.0882 ns |     0.0782 ns |       3.694 ns |  1.05 |    0.02 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| ContainsValue   | Default       |   5,169.534 ns |    23.1002 ns |    19.2897 ns |   5,161.778 ns | 1.000 |    0.01 |       - |       - |       - |         - |          NA |
| ContainsValue   | Bidirectional |       4.245 ns |     0.0359 ns |     0.0300 ns |       4.244 ns | 0.001 |    0.00 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Empty           | Default       |       5.860 ns |     0.0642 ns |     0.0536 ns |       5.860 ns |  1.00 |    0.01 |  0.0191 |       - |       - |      80 B |        1.00 |
| Empty           | Bidirectional |      29.334 ns |     0.6465 ns |     1.1152 ns |      29.303 ns |  5.01 |    0.19 |  0.0650 |       - |       - |     272 B |        3.40 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| FindKeyByValue  | Default       |   5,153.619 ns |    20.0717 ns |    16.7608 ns |   5,148.613 ns | 1.000 |    0.00 |       - |       - |       - |         - |          NA |
| FindKeyByValue  | Bidirectional |       4.101 ns |     0.0411 ns |     0.0344 ns |       4.090 ns | 0.001 |    0.00 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| FromSequence    | Default       |  76,242.148 ns | 1,382.8742 ns | 1,293.5414 ns |  76,118.518 ns |  1.00 |    0.02 | 30.6396 | 20.8740 | 20.8740 |  202302 B |        1.00 |
| FromSequence    | Bidirectional | 163,432.737 ns | 3,245.9269 ns | 6,703.4025 ns | 162,385.095 ns |  2.14 |    0.09 | 57.8613 | 38.5742 | 38.5742 |  404755 B |        2.00 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| IndexerHit      | Default       |       3.560 ns |     0.0468 ns |     0.0415 ns |       3.558 ns |  1.00 |    0.02 |       - |       - |       - |         - |          NA |
| IndexerHit      | Bidirectional |       4.660 ns |     0.1012 ns |     0.0897 ns |       4.629 ns |  1.31 |    0.03 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Keys            | Default       |  10,085.841 ns |   193.4670 ns |   180.9692 ns |  10,080.026 ns |  1.00 |    0.02 |       - |       - |       - |         - |          NA |
| Keys            | Bidirectional |  15,917.438 ns |   305.8008 ns |   448.2389 ns |  15,722.018 ns |  1.58 |    0.05 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| LinqProjection  | Default       |  89,945.190 ns | 1,737.0271 ns | 2,599.9006 ns |  89,029.102 ns |  1.00 |    0.04 | 30.5176 | 20.8740 | 20.8740 |  202301 B |        1.00 |
| LinqProjection  | Bidirectional | 164,707.706 ns | 3,261.8749 ns | 7,159.8950 ns | 164,567.749 ns |  1.83 |    0.09 | 60.5469 | 41.2598 | 41.2598 |  404722 B |        2.00 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Pairs           | Default       |  26,172.270 ns |   250.5305 ns |   222.0887 ns |  26,158.792 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| Pairs           | Bidirectional |  23,262.727 ns |   108.9438 ns |   101.9061 ns |  23,251.273 ns |  0.89 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| PreSized        | Default       |  18,162.091 ns |   522.9868 ns | 1,525.5771 ns |  18,699.205 ns |  1.01 |    0.13 | 28.8391 | 19.0735 | 19.0735 |  202333 B |        1.00 |
| PreSized        | Bidirectional |  37,170.140 ns |   467.6194 ns |   437.4115 ns |  37,232.019 ns |  2.06 |    0.19 | 57.2510 | 38.0249 | 37.7197 |  404758 B |        2.00 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Remove          | Default       |       6.030 ns |     0.2856 ns |     0.8420 ns |       6.006 ns |  1.02 |    0.20 |       - |       - |       - |         - |          NA |
| Remove          | Bidirectional |      21.792 ns |     0.9596 ns |     2.7839 ns |      20.743 ns |  3.68 |    0.69 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| SetOverwrite    | Default       |       4.397 ns |     0.0941 ns |     0.2760 ns |       4.350 ns |  1.00 |    0.09 |       - |       - |       - |         - |          NA |
| SetOverwrite    | Bidirectional |     109.338 ns |     2.1732 ns |     2.0328 ns |     108.975 ns | 24.96 |    1.60 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| TryAdd          | Default       |       6.635 ns |     0.4028 ns |     1.1813 ns |       6.281 ns |  1.03 |    0.26 |       - |       - |       - |         - |          NA |
| TryAdd          | Bidirectional |      23.836 ns |     0.7610 ns |     2.1958 ns |      23.934 ns |  3.71 |    0.74 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| TryGetValueHit  | Default       |       3.410 ns |     0.0126 ns |     0.0112 ns |       3.412 ns |  1.00 |    0.00 |       - |       - |       - |         - |          NA |
| TryGetValueHit  | Bidirectional |       3.598 ns |     0.0120 ns |     0.0094 ns |       3.603 ns |  1.06 |    0.00 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| TryGetValueMiss | Default       |       3.231 ns |     0.0959 ns |     0.0985 ns |       3.202 ns |  1.00 |    0.04 |       - |       - |       - |         - |          NA |
| TryGetValueMiss | Bidirectional |       3.368 ns |     0.0223 ns |     0.0198 ns |       3.372 ns |  1.04 |    0.03 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Values          | Default       |   9,729.858 ns |    38.9080 ns |    34.4909 ns |   9,724.738 ns |  1.00 |    0.00 |       - |       - |       - |         - |          NA |
| Values          | Bidirectional |  15,558.828 ns |    72.1572 ns |    63.9655 ns |  15,538.148 ns |  1.60 |    0.01 |       - |       - |       - |         - |          NA |

### ReadOnly

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26200.8246)
Unknown processor
.NET SDK 10.0.103
  [Host]     : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
  DefaultJob : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2

Job=DefaultJob  AnalyzeLaunchVariance=False  EvaluateOverhead=Default  
MaxAbsoluteError=Default  MaxRelativeError=Default  MinInvokeCount=Default  
MinIterationTime=Default  OutlierMode=Default  Affinity=11111111  
EnvironmentVariables=Empty  Jit=RyuJit  LargeAddressAware=Default  
Platform=X64  PowerPlanMode=8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c  Runtime=.NET 10.0  
AllowVeryLargeObjects=False  Concurrent=True  CpuGroups=False  
Force=True  HeapAffinitizeMask=Default  HeapCount=Default  
NoAffinitize=False  RetainVm=False  Server=False  
Arguments=Default  BuildConfiguration=Default  Clock=Default  
EngineFactory=Default  NuGetReferences=Default  Toolchain=Default  
IsMutator=Default  InvocationCount=Default  IterationCount=Default  
IterationTime=Default  LaunchCount=Default  MaxIterationCount=Default  
MaxWarmupIterationCount=Default  MemoryRandomization=Default  MinIterationCount=Default  
MinWarmupIterationCount=Default  RunStrategy=Default  UnrollFactor=16  
WarmupCount=Default  

```
| Operation       | Type          | Mean          | Error       | StdDev      | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------- |-------------- |--------------:|------------:|------------:|------:|--------:|-------:|----------:|------------:|
| ContainsKeyHit  | Default       |      4.332 ns |   0.0375 ns |   0.0333 ns |  1.00 |    0.01 |      - |         - |          NA |
| ContainsKeyHit  | Bidirectional |      4.550 ns |   0.0212 ns |   0.0198 ns |  1.05 |    0.01 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| ContainsKeyMiss | Default       |      4.032 ns |   0.0209 ns |   0.0195 ns |  1.00 |    0.01 |      - |         - |          NA |
| ContainsKeyMiss | Bidirectional |      4.293 ns |   0.0200 ns |   0.0187 ns |  1.06 |    0.01 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| ContainsValue   | Default       | 30,959.441 ns |  82.7111 ns |  73.3212 ns | 1.000 |    0.00 |      - |      48 B |        1.00 |
| ContainsValue   | Bidirectional |      5.229 ns |   0.0244 ns |   0.0228 ns | 0.000 |    0.00 |      - |         - |        0.00 |
|                 |               |               |             |             |       |         |        |           |             |
| FindKeyByValue  | Default       | 31,019.455 ns |  74.2630 ns |  69.4656 ns | 1.000 |    0.00 |      - |      48 B |        1.00 |
| FindKeyByValue  | Bidirectional |      4.732 ns |   0.0264 ns |   0.0220 ns | 0.000 |    0.00 |      - |         - |        0.00 |
|                 |               |               |             |             |       |         |        |           |             |
| IndexerHit      | Default       |      3.989 ns |   0.0539 ns |   0.0450 ns |  1.00 |    0.02 |      - |         - |          NA |
| IndexerHit      | Bidirectional |      4.202 ns |   0.0680 ns |   0.0603 ns |  1.05 |    0.02 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| Keys            | Default       | 20,805.697 ns |  70.2570 ns |  65.7184 ns |  1.00 |    0.00 |      - |      40 B |        1.00 |
| Keys            | Bidirectional | 21,046.117 ns |  39.3683 ns |  34.8990 ns |  1.01 |    0.00 |      - |      48 B |        1.20 |
|                 |               |               |             |             |       |         |        |           |             |
| Pairs           | Default       | 64,507.112 ns |  97.5949 ns |  91.2904 ns |  1.00 |    0.00 |      - |      48 B |        1.00 |
| Pairs           | Bidirectional | 64,669.632 ns | 203.9623 ns | 170.3178 ns |  1.00 |    0.00 |      - |      48 B |        1.00 |
|                 |               |               |             |             |       |         |        |           |             |
| TryGetValueHit  | Default       |      4.166 ns |   0.0164 ns |   0.0137 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryGetValueHit  | Bidirectional |      4.314 ns |   0.0189 ns |   0.0167 ns |  1.04 |    0.01 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| TryGetValueMiss | Default       |      4.181 ns |   0.0195 ns |   0.0183 ns |  1.00 |    0.01 |      - |         - |          NA |
| TryGetValueMiss | Bidirectional |      4.126 ns |   0.0286 ns |   0.0253 ns |  0.99 |    0.01 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| Values          | Default       | 20,834.763 ns |  79.2079 ns |  66.1422 ns |  1.00 |    0.00 |      - |      40 B |        1.00 |
| Values          | Bidirectional | 21,239.693 ns | 211.4047 ns | 176.5325 ns |  1.02 |    0.01 |      - |      48 B |        1.20 |
|                 |               |               |             |             |       |         |        |           |             |
| Wrap            | Default       |      5.949 ns |   0.0938 ns |   0.0921 ns |  1.00 |    0.02 | 0.0096 |      40 B |        1.00 |
| Wrap            | Bidirectional |     13.220 ns |   0.0859 ns |   0.0671 ns |  2.22 |    0.03 | 0.0229 |      96 B |        2.40 |
