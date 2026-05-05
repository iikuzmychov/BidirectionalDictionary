# Benchmarks

BenchmarkDotNet suite comparing:

- `BidirectionalDictionary<TKey, TValue>` vs `Dictionary<TKey, TValue>`
- `ReadOnlyBidirectionalDictionary<TKey, TValue>` vs `ReadOnlyDictionary<TKey, TValue>`

The suite uses deterministic `int -> int` data with `DataSize = 10_000`.
Mutation benchmarks use `MutationOperations = 1_000_000` and
`OperationsPerInvoke = MutationOperations`, so their reported means are
per-operation averages across batched mutations.

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
  Job-SMITDH : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
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
| Add             | Default       |      10.364 ns |     0.8667 ns |     2.5553 ns |       9.773 ns |  1.06 |    0.36 |       - |       - |       - |         - |          NA |
| Add             | Bidirectional |      35.684 ns |     1.8420 ns |     5.1954 ns |      34.903 ns |  3.64 |    1.00 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| ContainsKeyHit  | Default       |       4.002 ns |     0.1148 ns |     0.3180 ns |       3.854 ns |  1.01 |    0.11 |       - |       - |       - |         - |          NA |
| ContainsKeyHit  | Bidirectional |       4.031 ns |     0.0613 ns |     0.0573 ns |       4.026 ns |  1.01 |    0.07 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| ContainsKeyMiss | Default       |       4.177 ns |     0.2400 ns |     0.7039 ns |       4.003 ns |  1.03 |    0.23 |       - |       - |       - |         - |          NA |
| ContainsKeyMiss | Bidirectional |       4.062 ns |     0.2347 ns |     0.6733 ns |       3.740 ns |  1.00 |    0.23 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| ContainsValue   | Default       |   6,442.381 ns |   305.7275 ns |   891.8216 ns |   6,008.649 ns | 1.017 |    0.19 |       - |       - |       - |         - |          NA |
| ContainsValue   | Bidirectional |       4.365 ns |     0.1205 ns |     0.1068 ns |       4.348 ns | 0.001 |    0.00 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Empty           | Default       |       8.495 ns |     0.2135 ns |     0.1783 ns |       8.527 ns |  1.00 |    0.03 |  0.0191 |       - |       - |      80 B |        1.00 |
| Empty           | Bidirectional |      31.696 ns |     1.9285 ns |     5.5021 ns |      29.602 ns |  3.73 |    0.65 |  0.0650 |       - |       - |     272 B |        3.40 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| FindKeyByValue  | Default       |   5,136.921 ns |    15.3338 ns |    11.9716 ns |   5,135.837 ns | 1.000 |    0.00 |       - |       - |       - |         - |          NA |
| FindKeyByValue  | Bidirectional |       4.067 ns |     0.0960 ns |     0.1106 ns |       4.031 ns | 0.001 |    0.00 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| FromSequence    | Default       |  70,379.585 ns |   847.0875 ns |   750.9210 ns |  70,349.335 ns |  1.00 |    0.01 | 30.7617 | 20.9961 | 20.9961 |  202326 B |        1.00 |
| FromSequence    | Bidirectional | 157,402.343 ns | 3,142.5710 ns | 4,799.0383 ns | 156,686.987 ns |  2.24 |    0.07 | 61.0352 | 41.5039 | 41.5039 |  404738 B |        2.00 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| IndexerHit      | Default       |       3.430 ns |     0.0171 ns |     0.0160 ns |       3.426 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| IndexerHit      | Bidirectional |       3.684 ns |     0.0160 ns |     0.0149 ns |       3.681 ns |  1.07 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Keys            | Default       |   9,737.620 ns |    40.2509 ns |    37.6507 ns |   9,735.659 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| Keys            | Bidirectional |  15,535.371 ns |    46.1300 ns |    40.8931 ns |  15,533.246 ns |  1.60 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| LinqProjection  | Default       |  88,870.876 ns | 1,245.6017 ns | 1,165.1366 ns |  88,536.304 ns |  1.00 |    0.02 | 30.1514 | 20.5078 | 20.5078 |  202286 B |        1.00 |
| LinqProjection  | Bidirectional | 151,620.413 ns | 2,846.6154 ns | 2,923.2644 ns | 152,265.601 ns |  1.71 |    0.04 | 59.3262 | 39.7949 | 39.7949 |  404751 B |        2.00 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Pairs           | Default       |  23,269.827 ns |    73.6705 ns |    65.3070 ns |  23,250.534 ns |  1.00 |    0.00 |       - |       - |       - |         - |          NA |
| Pairs           | Bidirectional |  23,652.246 ns |   471.8554 ns |   463.4251 ns |  23,459.778 ns |  1.02 |    0.02 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| PreSized        | Default       |  19,506.739 ns |   638.9094 ns | 1,853.5921 ns |  19,497.951 ns |  1.01 |    0.14 | 28.5339 | 18.7683 | 18.7683 |  202332 B |        1.00 |
| PreSized        | Bidirectional |  43,162.941 ns | 1,277.0493 ns | 3,580.9833 ns |  42,899.573 ns |  2.23 |    0.29 | 56.9458 | 37.4146 | 37.4146 |  404753 B |        2.00 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Remove          | Default       |       5.835 ns |     0.2403 ns |     0.6933 ns |       5.763 ns |  1.01 |    0.17 |       - |       - |       - |         - |          NA |
| Remove          | Bidirectional |      27.815 ns |     1.9046 ns |     5.4030 ns |      28.033 ns |  4.83 |    1.08 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| SetOverwrite    | Default       |       4.520 ns |     0.1562 ns |     0.4406 ns |       4.553 ns |  1.01 |    0.14 |       - |       - |       - |         - |          NA |
| SetOverwrite    | Bidirectional |     110.548 ns |     2.7058 ns |     7.4070 ns |     111.203 ns | 24.68 |    2.86 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| TryAdd          | Default       |       7.252 ns |     0.5676 ns |     1.6646 ns |       6.740 ns |  1.05 |    0.33 |       - |       - |       - |         - |          NA |
| TryAdd          | Bidirectional |      30.006 ns |     1.1699 ns |     3.3754 ns |      30.366 ns |  4.34 |    1.04 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| TryGetValueHit  | Default       |       4.610 ns |     0.4195 ns |     1.2037 ns |       4.304 ns |  1.06 |    0.37 |       - |       - |       - |         - |          NA |
| TryGetValueHit  | Bidirectional |       3.520 ns |     0.0240 ns |     0.0225 ns |       3.517 ns |  0.81 |    0.18 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| TryGetValueMiss | Default       |       3.389 ns |     0.0991 ns |     0.1709 ns |       3.304 ns |  1.00 |    0.07 |       - |       - |       - |         - |          NA |
| TryGetValueMiss | Bidirectional |       3.416 ns |     0.0339 ns |     0.0283 ns |       3.418 ns |  1.01 |    0.05 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Values          | Default       |  10,028.351 ns |   175.7203 ns |   155.7714 ns |  10,054.310 ns |  1.00 |    0.02 |       - |       - |       - |         - |          NA |
| Values          | Bidirectional |  15,700.510 ns |   124.1660 ns |   103.6842 ns |  15,727.110 ns |  1.57 |    0.03 |       - |       - |       - |         - |          NA |

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
| Operation       | Type          | Mean          | Error         | StdDev        | Median        | Ratio | RatioSD | Gen0    | Allocated | Alloc Ratio |
|---------------- |-------------- |--------------:|--------------:|--------------:|--------------:|------:|--------:|--------:|----------:|------------:|
| ContainsKeyHit  | Default       |      4.290 ns |     0.0524 ns |     0.0490 ns |      4.267 ns |  1.00 |    0.02 |       - |         - |          NA |
| ContainsKeyHit  | Bidirectional |      4.720 ns |     0.1281 ns |     0.1315 ns |      4.687 ns |  1.10 |    0.03 |       - |         - |          NA |
|                 |               |               |               |               |               |       |         |         |           |             |
| ContainsKeyMiss | Default       |      4.625 ns |     0.2235 ns |     0.6267 ns |      4.340 ns |  1.02 |    0.18 |       - |         - |          NA |
| ContainsKeyMiss | Bidirectional |      4.491 ns |     0.1047 ns |     0.1501 ns |      4.437 ns |  0.99 |    0.12 |       - |         - |          NA |
|                 |               |               |               |               |               |       |         |         |           |             |
| ContainsValue   | Default       | 31,853.939 ns |   570.9007 ns |   657.4501 ns | 31,616.559 ns | 1.000 |    0.03 |       - |      48 B |        1.00 |
| ContainsValue   | Bidirectional |      5.355 ns |     0.1263 ns |     0.1181 ns |      5.305 ns | 0.000 |    0.00 |       - |         - |        0.00 |
|                 |               |               |               |               |               |       |         |         |           |             |
| FindKeyByValue  | Default       | 31,582.183 ns |   160.8932 ns |   142.6277 ns | 31,561.374 ns | 1.000 |    0.01 |       - |      48 B |        1.00 |
| FindKeyByValue  | Bidirectional |      5.019 ns |     0.1326 ns |     0.3563 ns |      4.850 ns | 0.000 |    0.00 |       - |         - |        0.00 |
|                 |               |               |               |               |               |       |         |         |           |             |
| IndexerHit      | Default       |      4.047 ns |     0.0417 ns |     0.0369 ns |      4.057 ns |  1.00 |    0.01 |       - |         - |          NA |
| IndexerHit      | Bidirectional |      4.220 ns |     0.0535 ns |     0.0501 ns |      4.203 ns |  1.04 |    0.02 |       - |         - |          NA |
|                 |               |               |               |               |               |       |         |         |           |             |
| Keys            | Default       | 21,612.259 ns |   123.9897 ns |   115.9800 ns | 21,609.627 ns |  1.00 |    0.01 |       - |      40 B |        1.00 |
| Keys            | Bidirectional | 22,255.263 ns |   136.7965 ns |   121.2665 ns | 22,262.592 ns |  1.03 |    0.01 |       - |         - |        0.00 |
|                 |               |               |               |               |               |       |         |         |           |             |
| Pairs           | Default       | 65,848.037 ns |   361.5204 ns |   320.4784 ns | 65,783.923 ns |  1.00 |    0.01 |       - |      48 B |        1.00 |
| Pairs           | Bidirectional | 93,337.783 ns | 1,802.9743 ns | 1,686.5032 ns | 92,810.767 ns |  1.42 |    0.03 | 57.3730 |  240048 B |    5,001.00 |
|                 |               |               |               |               |               |       |         |         |           |             |
| TryGetValueHit  | Default       |      4.250 ns |     0.0471 ns |     0.0441 ns |      4.239 ns |  1.00 |    0.01 |       - |         - |          NA |
| TryGetValueHit  | Bidirectional |      4.445 ns |     0.1003 ns |     0.0985 ns |      4.426 ns |  1.05 |    0.02 |       - |         - |          NA |
|                 |               |               |               |               |               |       |         |         |           |             |
| TryGetValueMiss | Default       |      4.086 ns |     0.0647 ns |     0.0540 ns |      4.094 ns |  1.00 |    0.02 |       - |         - |          NA |
| TryGetValueMiss | Bidirectional |      4.225 ns |     0.0459 ns |     0.0383 ns |      4.224 ns |  1.03 |    0.02 |       - |         - |          NA |
|                 |               |               |               |               |               |       |         |         |           |             |
| Values          | Default       | 21,505.556 ns |   284.7703 ns |   237.7961 ns | 21,400.943 ns |  1.00 |    0.01 |       - |      40 B |        1.00 |
| Values          | Bidirectional | 22,142.871 ns |   395.6256 ns |   330.3653 ns | 21,986.961 ns |  1.03 |    0.02 |       - |         - |        0.00 |
|                 |               |               |               |               |               |       |         |         |           |             |
| Wrap            | Default       |      7.010 ns |     0.1113 ns |     0.0987 ns |      7.006 ns |  1.00 |    0.02 |  0.0096 |      40 B |        1.00 |
| Wrap            | Bidirectional |     14.314 ns |     0.2245 ns |     0.1753 ns |     14.263 ns |  2.04 |    0.04 |  0.0153 |      64 B |        1.60 |
