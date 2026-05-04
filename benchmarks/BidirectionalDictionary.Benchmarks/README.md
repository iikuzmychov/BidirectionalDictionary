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
  Job-VDOROB : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
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
| Operation       | Type          | Mean           | Error         | StdDev         | Median         | Ratio | RatioSD | Gen0    | Gen1    | Gen2    | Allocated | Alloc Ratio |
|---------------- |-------------- |---------------:|--------------:|---------------:|---------------:|------:|--------:|--------:|--------:|--------:|----------:|------------:|
| Add             | Default       |      12.220 ns |     0.8630 ns |      2.5445 ns |      11.832 ns |  1.04 |    0.32 |       - |       - |       - |         - |          NA |
| Add             | Bidirectional |      83.545 ns |     9.1969 ns |     26.5353 ns |      80.474 ns |  7.14 |    2.78 |       - |       - |       - |      54 B |          NA |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| ContainsKeyHit  | Default       |       3.801 ns |     0.1032 ns |      0.1014 ns |       3.779 ns |  1.00 |    0.04 |       - |       - |       - |         - |          NA |
| ContainsKeyHit  | Bidirectional |       3.996 ns |     0.0987 ns |      0.0825 ns |       3.990 ns |  1.05 |    0.03 |       - |       - |       - |         - |          NA |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| ContainsKeyMiss | Default       |       3.571 ns |     0.0770 ns |      0.0643 ns |       3.546 ns |  1.00 |    0.02 |       - |       - |       - |         - |          NA |
| ContainsKeyMiss | Bidirectional |       3.833 ns |     0.1096 ns |      0.1707 ns |       3.818 ns |  1.07 |    0.05 |       - |       - |       - |         - |          NA |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| ContainsValue   | Default       |   5,277.959 ns |    98.6076 ns |     96.8459 ns |   5,237.935 ns | 1.000 |    0.02 |       - |       - |       - |         - |          NA |
| ContainsValue   | Bidirectional |       4.226 ns |     0.1168 ns |      0.1345 ns |       4.210 ns | 0.001 |    0.00 |       - |       - |       - |         - |          NA |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| Empty           | Default       |       7.445 ns |     0.2990 ns |      0.8483 ns |       7.233 ns |  1.01 |    0.16 |  0.0191 |       - |       - |      80 B |        1.00 |
| Empty           | Bidirectional |      31.820 ns |     1.3068 ns |      3.6860 ns |      30.546 ns |  4.33 |    0.68 |  0.0573 |       - |       - |     240 B |        3.00 |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| FindKeyByValue  | Default       |   5,282.318 ns |   105.5576 ns |    117.3270 ns |   5,262.606 ns | 1.000 |    0.03 |       - |       - |       - |         - |          NA |
| FindKeyByValue  | Bidirectional |       4.108 ns |     0.1095 ns |      0.0971 ns |       4.076 ns | 0.001 |    0.00 |       - |       - |       - |         - |          NA |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| FromSequence    | Default       |  73,172.467 ns | 1,416.4826 ns |  1,791.3972 ns |  73,466.272 ns |  1.00 |    0.03 | 30.5176 | 20.8740 | 20.8740 |  202332 B |        1.00 |
| FromSequence    | Bidirectional | 275,143.847 ns | 5,389.8104 ns | 11,250.5423 ns | 270,972.510 ns |  3.76 |    0.18 | 58.1055 | 39.0625 | 39.0625 |  404734 B |        2.00 |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| IndexerHit      | Default       |       3.437 ns |     0.0335 ns |      0.0314 ns |       3.437 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| IndexerHit      | Bidirectional |       3.691 ns |     0.0287 ns |      0.0255 ns |       3.686 ns |  1.07 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| Keys            | Default       |   9,747.244 ns |    76.8905 ns |     64.2071 ns |   9,764.960 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| Keys            | Bidirectional |   9,826.818 ns |    55.9686 ns |     46.7363 ns |   9,834.175 ns |  1.01 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| LinqProjection  | Default       |  83,606.267 ns | 1,617.3286 ns |  1,660.8774 ns |  83,959.766 ns |  1.00 |    0.03 | 31.1279 | 21.3623 | 21.3623 |  202310 B |        1.00 |
| LinqProjection  | Bidirectional | 262,109.801 ns | 2,457.2857 ns |  2,178.3197 ns | 262,045.776 ns |  3.14 |    0.07 | 58.1055 | 39.0625 | 39.0625 |  404754 B |        2.00 |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| Pairs           | Default       |  23,520.475 ns |   383.0035 ns |    358.2618 ns |  23,445.609 ns |  1.00 |    0.02 |       - |       - |       - |         - |          NA |
| Pairs           | Bidirectional |  88,702.595 ns | 1,777.6151 ns |  4,984.6236 ns |  86,312.219 ns |  3.77 |    0.22 | 57.3730 |       - |       - |  240048 B |          NA |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| PreSized        | Default       |  18,772.441 ns |   389.5593 ns |  1,066.4124 ns |  18,954.321 ns |  1.00 |    0.08 | 28.7781 | 19.0125 | 19.0125 |  202334 B |        1.00 |
| PreSized        | Bidirectional |  51,676.248 ns | 6,642.4669 ns | 19,585.4649 ns |  57,601.321 ns |  2.76 |    1.06 | 28.9307 | 19.1650 | 19.1650 |  202470 B |        1.00 |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| Remove          | Default       |      18.050 ns |     0.6652 ns |      1.9612 ns |      17.905 ns |  1.01 |    0.17 |       - |       - |       - |         - |          NA |
| Remove          | Bidirectional |     131.535 ns |     8.4270 ns |     24.3137 ns |     134.136 ns |  7.38 |    1.64 |       - |       - |       - |         - |          NA |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| SetOverwrite    | Default       |      14.086 ns |     0.6481 ns |      1.9007 ns |      14.077 ns |  1.02 |    0.20 |       - |       - |       - |         - |          NA |
| SetOverwrite    | Bidirectional |     353.308 ns |     6.7725 ns |      7.7992 ns |     352.498 ns | 25.57 |    3.77 |       - |       - |       - |         - |          NA |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| TryAdd          | Default       |      18.231 ns |     0.7833 ns |      2.2850 ns |      18.031 ns |  1.02 |    0.18 |       - |       - |       - |         - |          NA |
| TryAdd          | Bidirectional |     241.304 ns |    14.9537 ns |     44.0914 ns |     255.092 ns | 13.44 |    2.95 |       - |       - |       - |      54 B |          NA |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| TryGetValueHit  | Default       |       9.557 ns |     0.2587 ns |      0.5679 ns |       9.484 ns |  1.00 |    0.08 |       - |       - |       - |         - |          NA |
| TryGetValueHit  | Bidirectional |      10.484 ns |     0.3867 ns |      1.1219 ns |      10.406 ns |  1.10 |    0.13 |       - |       - |       - |         - |          NA |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| TryGetValueMiss | Default       |       9.330 ns |     0.4515 ns |      1.3313 ns |       9.253 ns |  1.02 |    0.21 |       - |       - |       - |         - |          NA |
| TryGetValueMiss | Bidirectional |       9.495 ns |     0.2000 ns |      0.1670 ns |       9.442 ns |  1.04 |    0.15 |       - |       - |       - |         - |          NA |
|                 |               |                |               |                |                |       |         |         |         |         |           |             |
| Values          | Default       |  24,646.438 ns |   798.3149 ns |  2,290.5153 ns |  25,003.053 ns |  1.01 |    0.15 |       - |       - |       - |         - |          NA |
| Values          | Bidirectional |  24,979.115 ns |   488.4960 ns |    855.5613 ns |  24,923.378 ns |  1.02 |    0.12 |       - |       - |       - |         - |          NA |

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
| Operation       | Type          | Mean           | Error         | StdDev        | Median         | Ratio | RatioSD | Gen0    | Allocated | Alloc Ratio |
|---------------- |-------------- |---------------:|--------------:|--------------:|---------------:|------:|--------:|--------:|----------:|------------:|
| ContainsKeyHit  | Default       |      10.960 ns |     0.7048 ns |     1.9411 ns |      11.701 ns |  1.04 |    0.32 |       - |         - |          NA |
| ContainsKeyHit  | Bidirectional |       5.370 ns |     1.1936 ns |     3.5193 ns |       3.497 ns |  0.51 |    0.37 |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |           |             |
| ContainsKeyMiss | Default       |       5.312 ns |     0.1393 ns |     0.2327 ns |       5.265 ns |  1.00 |    0.06 |       - |         - |          NA |
| ContainsKeyMiss | Bidirectional |       5.072 ns |     0.1423 ns |     0.1899 ns |       5.012 ns |  0.96 |    0.05 |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |           |             |
| ContainsValue   | Default       |  34,034.492 ns |   656.6713 ns |   982.8748 ns |  34,169.577 ns | 1.001 |    0.04 |       - |      48 B |        1.00 |
| ContainsValue   | Bidirectional |       5.881 ns |     0.1207 ns |     0.1008 ns |       5.900 ns | 0.000 |    0.00 |       - |         - |        0.00 |
|                 |               |                |               |               |                |       |         |         |           |             |
| FindKeyByValue  | Default       |  33,575.882 ns |   596.2469 ns |   528.5574 ns |  33,557.327 ns | 1.000 |    0.02 |       - |      48 B |        1.00 |
| FindKeyByValue  | Bidirectional |       5.146 ns |     0.0536 ns |     0.0447 ns |       5.143 ns | 0.000 |    0.00 |       - |         - |        0.00 |
|                 |               |                |               |               |                |       |         |         |           |             |
| IndexerHit      | Default       |       4.507 ns |     0.1147 ns |     0.1073 ns |       4.522 ns |  1.00 |    0.03 |       - |         - |          NA |
| IndexerHit      | Bidirectional |       4.534 ns |     0.1163 ns |     0.1031 ns |       4.538 ns |  1.01 |    0.03 |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |           |             |
| Keys            | Default       |  24,795.281 ns |   493.0025 ns |   506.2773 ns |  24,744.365 ns |  1.00 |    0.03 |       - |      40 B |        1.00 |
| Keys            | Bidirectional |  12,741.540 ns |   251.5364 ns |   299.4363 ns |  12,646.333 ns |  0.51 |    0.02 |       - |         - |        0.00 |
|                 |               |                |               |               |                |       |         |         |           |             |
| Pairs           | Default       |  71,749.565 ns | 1,362.2761 ns | 1,137.5623 ns |  71,503.070 ns |  1.00 |    0.02 |       - |      48 B |        1.00 |
| Pairs           | Bidirectional | 135,145.619 ns | 2,675.8218 ns | 2,974.1689 ns | 134,469.275 ns |  1.88 |    0.05 | 57.3730 |  240048 B |    5,001.00 |
|                 |               |                |               |               |                |       |         |         |           |             |
| TryGetValueHit  | Default       |       4.628 ns |     0.1033 ns |     0.0863 ns |       4.624 ns |  1.00 |    0.03 |       - |         - |          NA |
| TryGetValueHit  | Bidirectional |       4.782 ns |     0.0978 ns |     0.0817 ns |       4.791 ns |  1.03 |    0.03 |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |           |             |
| TryGetValueMiss | Default       |       4.508 ns |     0.1224 ns |     0.1410 ns |       4.519 ns |  1.00 |    0.04 |       - |         - |          NA |
| TryGetValueMiss | Bidirectional |       4.709 ns |     0.1058 ns |     0.0990 ns |       4.718 ns |  1.05 |    0.04 |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |           |             |
| Values          | Default       |  24,702.434 ns |   492.3556 ns |   690.2138 ns |  24,627.191 ns |  1.00 |    0.04 |       - |      40 B |        1.00 |
| Values          | Bidirectional |  12,871.555 ns |   252.3061 ns |   480.0387 ns |  12,793.271 ns |  0.52 |    0.02 |       - |         - |        0.00 |
|                 |               |                |               |               |                |       |         |         |           |             |
| Wrap            | Default       |      12.482 ns |     0.2938 ns |     0.4745 ns |      12.419 ns |  1.00 |    0.05 |  0.0095 |      40 B |        1.00 |
| Wrap            | Bidirectional |      23.813 ns |     0.5198 ns |     0.7938 ns |      23.791 ns |  1.91 |    0.09 |  0.0153 |      64 B |        1.60 |

