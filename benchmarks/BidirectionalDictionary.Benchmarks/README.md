# Benchmarks

BenchmarkDotNet suite comparing:

- `BidirectionalDictionary<TKey, TValue>` vs `Dictionary<TKey, TValue>`
- `ReadOnlyBidirectionalDictionary<TKey, TValue>` vs `ReadOnlyDictionary<TKey, TValue>`
- `ConcurrentBidirectionalDictionary<TKey, TValue>` vs `ConcurrentDictionary<TKey, TValue>`

The suite uses deterministic `int -> int` read data with `10_000` items for reading benchmarks,
and `1_000_000` items for mutation benchmarks.

## Running

Run benchmarks in Release mode:

```bash
dotnet run -c Release --project benchmarks/BidirectionalDictionary.Benchmarks -- --filter '*.BidirectionalDictionaryBenchmarks.*'
dotnet run -c Release --project benchmarks/BidirectionalDictionary.Benchmarks -- --filter '*.ReadOnlyBidirectionalDictionaryBenchmarks.*'
dotnet run -c Release --project benchmarks/BidirectionalDictionary.Benchmarks -- --filter '*.ConcurrentBidirectionalDictionaryBenchmarks.*'
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

### BidirectionalDictionary

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26200.8457)
Unknown processor
.NET SDK 10.0.103
  [Host]     : .NET 10.0.8 (10.0.826.23019), X64 RyuJIT AVX2
  Job-ZASPXO : .NET 10.0.8 (10.0.826.23019), X64 RyuJIT AVX2
  DefaultJob : .NET 10.0.8 (10.0.826.23019), X64 RyuJIT AVX2

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
| Add             | Default       |       5.528 ns |     0.2869 ns |     0.8092 ns |       5.261 ns |  1.02 |    0.20 |       - |       - |       - |         - |          NA |
| Add             | Bidirectional |      27.583 ns |     1.8858 ns |     5.4409 ns |      25.845 ns |  5.08 |    1.20 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| ContainsKeyHit  | Default       |       3.731 ns |     0.0741 ns |     0.0618 ns |       3.706 ns |  1.00 |    0.02 |       - |       - |       - |         - |          NA |
| ContainsKeyHit  | Bidirectional |       3.914 ns |     0.0224 ns |     0.0199 ns |       3.908 ns |  1.05 |    0.02 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| ContainsKeyMiss | Default       |       3.469 ns |     0.0278 ns |     0.0246 ns |       3.462 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| ContainsKeyMiss | Bidirectional |       3.586 ns |     0.0253 ns |     0.0224 ns |       3.581 ns |  1.03 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| ContainsValue   | Default       |   5,023.268 ns |    15.9588 ns |    14.9279 ns |   5,020.636 ns | 1.000 |    0.00 |       - |       - |       - |         - |          NA |
| ContainsValue   | Bidirectional |       4.078 ns |     0.0282 ns |     0.0250 ns |       4.074 ns | 0.001 |    0.00 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Empty           | Default       |       5.968 ns |     0.0849 ns |     0.0709 ns |       5.952 ns |  1.00 |    0.02 |  0.0191 |       - |       - |      80 B |        1.00 |
| Empty           | Bidirectional |      26.726 ns |     0.4112 ns |     0.3210 ns |      26.680 ns |  4.48 |    0.07 |  0.0650 |       - |       - |     272 B |        3.40 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| FindKeyByValue  | Default       |   5,020.293 ns |    13.5185 ns |    11.9838 ns |   5,018.185 ns | 1.000 |    0.00 |       - |       - |       - |         - |          NA |
| FindKeyByValue  | Bidirectional |       3.963 ns |     0.0225 ns |     0.0210 ns |       3.956 ns | 0.001 |    0.00 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| FromSequence    | Default       |  68,299.456 ns | 1,326.5160 ns | 1,419.3573 ns |  68,157.440 ns |  1.00 |    0.03 | 30.1514 | 20.3857 | 20.3857 |  202314 B |        1.00 |
| FromSequence    | Bidirectional | 150,794.530 ns | 2,977.8091 ns | 6,015.3218 ns | 149,365.479 ns |  2.21 |    0.10 | 61.0352 | 41.5039 | 41.5039 |  404731 B |        2.00 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| IndexerHit      | Default       |       3.345 ns |     0.0168 ns |     0.0157 ns |       3.348 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| IndexerHit      | Bidirectional |       4.345 ns |     0.0741 ns |     0.0694 ns |       4.360 ns |  1.30 |    0.02 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Keys            | Default       |   9,629.707 ns |   148.6747 ns |   171.2140 ns |   9,585.995 ns |  1.00 |    0.02 |       - |       - |       - |         - |          NA |
| Keys            | Bidirectional |  15,272.694 ns |   100.5462 ns |    94.0510 ns |  15,291.672 ns |  1.59 |    0.03 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| LinqProjection  | Default       |  86,155.788 ns | 1,505.5972 ns | 3,142.7422 ns |  85,790.466 ns |  1.00 |    0.05 | 29.9072 | 20.2637 | 20.2637 |  202285 B |        1.00 |
| LinqProjection  | Bidirectional | 151,223.866 ns | 2,972.0916 ns | 7,456.3972 ns | 149,291.016 ns |  1.76 |    0.11 | 58.3496 | 39.0625 | 39.0625 |  404738 B |        2.00 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Pairs           | Default       |  25,423.456 ns |   167.3330 ns |   148.3363 ns |  25,394.160 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| Pairs           | Bidirectional |  22,774.176 ns |    61.7146 ns |    57.7278 ns |  22,784.259 ns |  0.90 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| PreSized        | Default       |  16,679.312 ns |   207.1218 ns |   193.7419 ns |  16,647.348 ns |  1.00 |    0.02 | 28.6255 | 18.8599 | 18.8599 |  202330 B |        1.00 |
| PreSized        | Bidirectional |  34,414.816 ns |   509.3456 ns |   425.3267 ns |  34,544.247 ns |  2.06 |    0.03 | 56.6406 | 37.1094 | 37.1094 |  404746 B |        2.00 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Remove          | Default       |       5.803 ns |     0.1153 ns |     0.1829 ns |       5.766 ns |  1.00 |    0.04 |       - |       - |       - |         - |          NA |
| Remove          | Bidirectional |      22.766 ns |     1.3601 ns |     3.9889 ns |      21.400 ns |  3.93 |    0.70 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| SetOverwrite    | Default       |       4.158 ns |     0.0760 ns |     0.0780 ns |       4.146 ns |  1.00 |    0.03 |       - |       - |       - |         - |          NA |
| SetOverwrite    | Bidirectional |     103.365 ns |     2.0510 ns |     2.1946 ns |     103.094 ns | 24.87 |    0.68 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| TryAdd          | Default       |       5.304 ns |     0.1057 ns |     0.2553 ns |       5.416 ns |  1.00 |    0.07 |       - |       - |       - |         - |          NA |
| TryAdd          | Bidirectional |      23.636 ns |     0.8684 ns |     2.5195 ns |      23.087 ns |  4.47 |    0.52 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| TryGetValueHit  | Default       |       3.305 ns |     0.0207 ns |     0.0194 ns |       3.302 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| TryGetValueHit  | Bidirectional |       3.444 ns |     0.0219 ns |     0.0194 ns |       3.442 ns |  1.04 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| TryGetValueMiss | Default       |       3.030 ns |     0.0189 ns |     0.0167 ns |       3.034 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| TryGetValueMiss | Bidirectional |       3.247 ns |     0.0177 ns |     0.0157 ns |       3.245 ns |  1.07 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Values          | Default       |   9,446.539 ns |    40.7992 ns |    36.1674 ns |   9,438.403 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| Values          | Bidirectional |  15,044.486 ns |    31.2226 ns |    27.6780 ns |  15,035.773 ns |  1.59 |    0.01 |       - |       - |       - |         - |          NA |

### ReadOnlyBidirectionalDictionary

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26200.8457)
Unknown processor
.NET SDK 10.0.103
  [Host]     : .NET 10.0.8 (10.0.826.23019), X64 RyuJIT AVX2
  DefaultJob : .NET 10.0.8 (10.0.826.23019), X64 RyuJIT AVX2

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
| ContainsKeyHit  | Default       |      4.167 ns |   0.0202 ns |   0.0169 ns |  1.00 |    0.01 |      - |         - |          NA |
| ContainsKeyHit  | Bidirectional |      4.398 ns |   0.0221 ns |   0.0185 ns |  1.06 |    0.01 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| ContainsKeyMiss | Default       |      3.893 ns |   0.0164 ns |   0.0154 ns |  1.00 |    0.01 |      - |         - |          NA |
| ContainsKeyMiss | Bidirectional |      4.144 ns |   0.0151 ns |   0.0141 ns |  1.06 |    0.01 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| ContainsValue   | Default       | 30,003.324 ns |  45.7167 ns |  38.1755 ns | 1.000 |    0.00 |      - |      48 B |        1.00 |
| ContainsValue   | Bidirectional |      5.062 ns |   0.0119 ns |   0.0112 ns | 0.000 |    0.00 |      - |         - |        0.00 |
|                 |               |               |             |             |       |         |        |           |             |
| FindKeyByValue  | Default       | 30,012.412 ns |  51.1360 ns |  42.7008 ns | 1.000 |    0.00 |      - |      48 B |        1.00 |
| FindKeyByValue  | Bidirectional |      4.582 ns |   0.0236 ns |   0.0197 ns | 0.000 |    0.00 |      - |         - |        0.00 |
|                 |               |               |             |             |       |         |        |           |             |
| IndexerHit      | Default       |      3.825 ns |   0.0225 ns |   0.0211 ns |  1.00 |    0.01 |      - |         - |          NA |
| IndexerHit      | Bidirectional |      4.003 ns |   0.0116 ns |   0.0097 ns |  1.05 |    0.01 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| Keys            | Default       | 20,193.522 ns | 124.6387 ns | 116.5871 ns |  1.00 |    0.01 |      - |      40 B |        1.00 |
| Keys            | Bidirectional | 20,416.692 ns | 125.8082 ns | 117.6811 ns |  1.01 |    0.01 |      - |      48 B |        1.20 |
|                 |               |               |             |             |       |         |        |           |             |
| Pairs           | Default       | 62,585.454 ns | 134.9599 ns | 119.6384 ns |  1.00 |    0.00 |      - |      48 B |        1.00 |
| Pairs           | Bidirectional | 62,620.005 ns | 151.8685 ns | 126.8170 ns |  1.00 |    0.00 |      - |      48 B |        1.00 |
|                 |               |               |             |             |       |         |        |           |             |
| TryGetValueHit  | Default       |      4.025 ns |   0.0408 ns |   0.0382 ns |  1.00 |    0.01 |      - |         - |          NA |
| TryGetValueHit  | Bidirectional |      4.182 ns |   0.0228 ns |   0.0213 ns |  1.04 |    0.01 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| TryGetValueMiss | Default       |      4.071 ns |   0.0130 ns |   0.0102 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryGetValueMiss | Bidirectional |      4.012 ns |   0.0144 ns |   0.0134 ns |  0.99 |    0.00 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| Values          | Default       | 20,201.724 ns | 144.3000 ns | 120.4970 ns |  1.00 |    0.01 |      - |      40 B |        1.00 |
| Values          | Bidirectional | 20,515.188 ns |  69.3747 ns |  61.4989 ns |  1.02 |    0.01 |      - |      48 B |        1.20 |
|                 |               |               |             |             |       |         |        |           |             |
| Wrap            | Default       |      5.652 ns |   0.0385 ns |   0.0321 ns |  1.00 |    0.01 | 0.0096 |      40 B |        1.00 |
| Wrap            | Bidirectional |     12.552 ns |   0.1472 ns |   0.1377 ns |  2.22 |    0.03 | 0.0229 |      96 B |        2.40 |


### ConcurrentBidirectionalDictionary

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26200.8457)
Unknown processor
.NET SDK 10.0.103
  [Host]     : .NET 10.0.8 (10.0.826.23019), X64 RyuJIT AVX2
  Job-ZASPXO : .NET 10.0.8 (10.0.826.23019), X64 RyuJIT AVX2
  DefaultJob : .NET 10.0.8 (10.0.826.23019), X64 RyuJIT AVX2

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
| Operation      | Type          | ConcurrencyLevel | Mean           | Error          | StdDev          | Median         | Ratio | RatioSD | Gen0   | Gen1   | Allocated  | Alloc Ratio |
|--------------- |-------------- |----------------- |---------------:|---------------:|----------------:|---------------:|------:|--------:|-------:|-------:|-----------:|------------:|
| **AddOrUpdate**    | **Default**       | **1**                |      **36.515 ns** |      **0.3595 ns** |       **0.3363 ns** |      **36.454 ns** |  **1.00** |    **0.01** | **0.0050** | **0.0010** |       **24 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 1                |     376.615 ns |      2.9064 ns |       2.7186 ns |     377.399 ns | 10.31 |    0.12 | 0.0100 | 0.0040 |       64 B |        2.67 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **AddOrUpdate**    | **Default**       | **2**                |      **62.247 ns** |      **1.2152 ns** |       **2.0635 ns** |      **62.567 ns** |  **1.00** |    **0.05** | **0.0200** | **0.0010** |       **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 2                |     529.651 ns |     10.2742 ns |      15.6898 ns |     532.099 ns |  8.52 |    0.38 | 0.0200 | 0.0050 |      128 B |        1.45 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **AddOrUpdate**    | **Default**       | **4**                |      **46.756 ns** |      **0.9260 ns** |       **1.3280 ns** |      **46.830 ns** |  **1.00** |    **0.04** | **0.0200** | **0.0010** |       **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 4                |     565.938 ns |      5.8937 ns |       5.2246 ns |     565.607 ns | 12.11 |    0.36 | 0.0200 | 0.0050 |      128 B |        1.45 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **AddOrUpdate**    | **Default**       | **8**                |      **33.148 ns** |      **0.5525 ns** |       **0.5168 ns** |      **33.199 ns** |  **1.00** |    **0.02** | **0.0200** | **0.0010** |       **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 8                |     532.099 ns |      7.1201 ns |       6.6601 ns |     531.695 ns | 16.06 |    0.31 | 0.0200 | 0.0050 |      128 B |        1.45 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **AddOrUpdate**    | **Default**       | **16**               |      **32.310 ns** |      **0.6413 ns** |       **1.1232 ns** |      **32.323 ns** |  **1.00** |    **0.05** | **0.0200** | **0.0010** |       **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 16               |     445.573 ns |      8.6284 ns |      10.2715 ns |     445.988 ns | 13.81 |    0.57 | 0.0200 | 0.0050 |      128 B |        1.45 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **1**                | **378,317.647 ns** | **11,368.1693 ns** |  **30,734.5090 ns** | **379,900.000 ns** |  **1.01** |    **0.11** |      **-** |      **-** |  **9301720 B** |        **1.00** |
| Clear          | Bidirectional | 1                | 605,996.000 ns | 84,379.1566 ns | 248,793.8650 ns | 653,150.000 ns |  1.61 |    0.67 |      - |      - | 18603616 B |        2.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **2**                | **367,980.460 ns** |  **9,930.2540 ns** |  **27,183.9121 ns** | **362,900.000 ns** |  **1.01** |    **0.10** |      **-** |      **-** |  **9301720 B** |        **1.00** |
| Clear          | Bidirectional | 2                | 613,255.556 ns | 83,349.1256 ns | 244,448.5749 ns | 682,800.000 ns |  1.68 |    0.68 |      - |      - | 18603616 B |        2.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **4**                | **365,297.674 ns** |  **8,002.2068 ns** |  **21,770.5953 ns** | **362,050.000 ns** |  **1.00** |    **0.08** |      **-** |      **-** |  **9302016 B** |        **1.00** |
| Clear          | Bidirectional | 4                | 611,271.717 ns | 86,346.2135 ns | 253,238.5155 ns | 789,700.000 ns |  1.68 |    0.70 |      - |      - | 18603632 B |        2.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **8**                | **369,429.348 ns** | **11,067.5427 ns** |  **31,216.2030 ns** | **363,100.000 ns** |  **1.01** |    **0.12** |      **-** |      **-** |  **9302032 B** |        **1.00** |
| Clear          | Bidirectional | 8                | 619,885.000 ns | 88,541.4834 ns | 261,066.5805 ns | 705,850.000 ns |  1.69 |    0.72 |      - |      - | 18603664 B |        2.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **16**               | **366,802.151 ns** |  **8,643.7378 ns** |  **24,520.8473 ns** | **358,500.000 ns** |  **1.00** |    **0.09** |      **-** |      **-** |  **9301776 B** |        **1.00** |
| Clear          | Bidirectional | 16               | 625,137.000 ns | 91,611.0762 ns | 270,117.3447 ns | 768,000.000 ns |  1.71 |    0.75 |      - |      - | 18603728 B |        2.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **1**                |       **5.682 ns** |      **0.0163 ns** |       **0.0145 ns** |       **5.677 ns** |  **1.00** |    **0.00** | **0.0000** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 1                |       6.652 ns |      0.0330 ns |       0.0308 ns |       6.650 ns |  1.17 |    0.01 | 0.0000 |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **2**                |       **3.392 ns** |      **0.0615 ns** |       **0.0480 ns** |       **3.398 ns** |  **1.00** |    **0.02** | **0.0000** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 2                |       4.223 ns |      0.0613 ns |       0.0573 ns |       4.200 ns |  1.25 |    0.02 | 0.0000 |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **4**                |       **2.972 ns** |      **0.0250 ns** |       **0.0234 ns** |       **2.965 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 4                |       3.182 ns |      0.0317 ns |       0.0281 ns |       3.173 ns |  1.07 |    0.01 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **8**                |       **2.688 ns** |      **0.0201 ns** |       **0.0167 ns** |       **2.688 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 8                |       2.800 ns |      0.0278 ns |       0.0260 ns |       2.795 ns |  1.04 |    0.01 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **16**               |       **3.357 ns** |      **0.0638 ns** |       **0.0656 ns** |       **3.366 ns** |  **1.00** |    **0.03** | **0.0001** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 16               |       3.372 ns |      0.0419 ns |       0.0392 ns |       3.385 ns |  1.00 |    0.02 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **1**                |  **20,057.082 ns** |    **162.6271 ns** |     **135.8010 ns** |  **20,024.827 ns** | **1.000** |    **0.01** |      **-** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 1                |       7.126 ns |      0.0512 ns |       0.0399 ns |       7.128 ns | 0.000 |    0.00 | 0.0000 |      - |          - |        0.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **2**                |   **9,143.060 ns** |    **312.3188 ns** |     **849.6865 ns** |   **8,792.572 ns** | **1.007** |    **0.12** |      **-** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 2                |       4.640 ns |      0.0909 ns |       0.1837 ns |       4.689 ns | 0.001 |    0.00 | 0.0000 |      - |          - |        0.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **4**                |   **4,940.935 ns** |     **75.3324 ns** |      **70.4660 ns** |   **4,957.065 ns** | **1.000** |    **0.02** | **0.0091** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 4                |       3.483 ns |      0.0436 ns |       0.0387 ns |       3.470 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **8**                |   **5,712.800 ns** |     **14.5517 ns** |      **11.3610 ns** |   **5,715.535 ns** | **1.000** |    **0.00** | **0.0091** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 8                |       3.028 ns |      0.0303 ns |       0.0253 ns |       3.030 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **16**               |   **6,481.617 ns** |    **128.5765 ns** |     **167.1858 ns** |   **6,477.497 ns** | **1.001** |    **0.04** | **0.0125** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 16               |       3.594 ns |      0.0612 ns |       0.0542 ns |       3.585 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **1**                |  **16,335.050 ns** |    **324.6798 ns** |     **784.1383 ns** |  **16,548.920 ns** | **1.003** |    **0.07** |      **-** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 1                |       7.521 ns |      0.0879 ns |       0.0734 ns |       7.493 ns | 0.000 |    0.00 | 0.0000 |      - |          - |        0.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **2**                |   **7,619.085 ns** |     **77.5872 ns** |      **64.7888 ns** |   **7,636.389 ns** | **1.000** |    **0.01** |      **-** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 2                |       4.347 ns |      0.0698 ns |       0.0686 ns |       4.333 ns | 0.001 |    0.00 | 0.0000 |      - |          - |        0.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **4**                |   **4,917.896 ns** |     **47.4703 ns** |      **42.0811 ns** |   **4,905.535 ns** | **1.000** |    **0.01** | **0.0091** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 4                |       3.593 ns |      0.0579 ns |       0.0483 ns |       3.571 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **8**                |   **6,225.537 ns** |     **19.7563 ns** |      **15.4244 ns** |   **6,225.343 ns** | **1.000** |    **0.00** | **0.0111** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 8                |       3.200 ns |      0.0446 ns |       0.0395 ns |       3.199 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **16**               |   **5,607.663 ns** |     **13.2584 ns** |      **11.0714 ns** |   **5,608.265 ns** | **1.000** |    **0.00** | **0.0100** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 16               |       3.631 ns |      0.0324 ns |       0.0287 ns |       3.632 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **1**                |      **90.675 ns** |      **1.7608 ns** |       **2.2268 ns** |      **91.495 ns** |  **1.00** |    **0.03** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 1                |     336.205 ns |      3.8927 ns |       3.6412 ns |     336.749 ns |  3.71 |    0.10 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **2**                |     **127.833 ns** |      **2.5482 ns** |       **3.1295 ns** |     **127.363 ns** |  **1.00** |    **0.03** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 2                |     414.728 ns |      3.7804 ns |       3.5362 ns |     415.008 ns |  3.25 |    0.08 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **4**                |     **117.818 ns** |      **2.3550 ns** |       **3.8693 ns** |     **118.872 ns** |  **1.00** |    **0.05** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 4                |     437.600 ns |      6.3438 ns |       5.9340 ns |     437.378 ns |  3.72 |    0.13 | 0.0120 | 0.0050 |       80 B |        2.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **8**                |      **91.543 ns** |      **1.7838 ns** |       **1.9087 ns** |      **91.892 ns** |  **1.00** |    **0.03** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 8                |     370.687 ns |      5.0910 ns |       4.7621 ns |     369.981 ns |  4.05 |    0.10 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **16**               |      **89.385 ns** |      **1.7168 ns** |       **2.3499 ns** |      **89.191 ns** |  **1.00** |    **0.04** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 16               |     320.135 ns |      4.0996 ns |       3.8348 ns |     319.820 ns |  3.58 |    0.10 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **1**                |       **5.689 ns** |      **0.0458 ns** |       **0.0383 ns** |       **5.691 ns** |  **1.00** |    **0.01** | **0.0000** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 1                |       6.289 ns |      0.0366 ns |       0.0306 ns |       6.284 ns |  1.11 |    0.01 | 0.0000 |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **2**                |       **3.812 ns** |      **0.0756 ns** |       **0.1869 ns** |       **3.865 ns** |  **1.00** |    **0.07** | **0.0000** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 2                |       4.247 ns |      0.0619 ns |       0.0483 ns |       4.238 ns |  1.12 |    0.06 | 0.0000 |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **4**                |       **2.939 ns** |      **0.0231 ns** |       **0.0216 ns** |       **2.939 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 4                |       3.148 ns |      0.0380 ns |       0.0356 ns |       3.137 ns |  1.07 |    0.01 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **8**                |       **2.696 ns** |      **0.0408 ns** |       **0.0714 ns** |       **2.655 ns** |  **1.00** |    **0.04** | **0.0001** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 8                |       2.838 ns |      0.0563 ns |       0.0956 ns |       2.801 ns |  1.05 |    0.04 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **16**               |       **3.254 ns** |      **0.0246 ns** |       **0.0218 ns** |       **3.252 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 16               |       3.331 ns |      0.0301 ns |       0.0266 ns |       3.326 ns |  1.02 |    0.01 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **1**                |      **27.585 ns** |      **0.4743 ns** |       **0.4437 ns** |      **27.641 ns** |  **1.00** |    **0.02** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 1                |     125.789 ns |      2.4892 ns |       2.3284 ns |     125.611 ns |  4.56 |    0.11 |      - |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **2**                |      **65.114 ns** |      **1.7247 ns** |       **5.0581 ns** |      **65.008 ns** |  **1.01** |    **0.11** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 2                |     209.890 ns |      4.1449 ns |       6.9252 ns |     209.278 ns |  3.24 |    0.28 |      - |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **4**                |      **57.112 ns** |      **1.3548 ns** |       **3.9521 ns** |      **57.186 ns** |  **1.00** |    **0.10** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 4                |     225.050 ns |      4.4805 ns |       9.5483 ns |     227.200 ns |  3.96 |    0.33 |      - |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **8**                |      **36.203 ns** |      **0.7159 ns** |       **1.7561 ns** |      **35.951 ns** |  **1.00** |    **0.07** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 8                |     152.740 ns |      2.9886 ns |       4.2861 ns |     150.846 ns |  4.23 |    0.23 |      - |      - |          - |          NA |
|                |               |                  |                |                |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **16**               |      **32.604 ns** |      **0.7999 ns** |       **2.3586 ns** |      **32.615 ns** |  **1.01** |    **0.10** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 16               |     125.084 ns |      2.4402 ns |       3.5768 ns |     125.467 ns |  3.86 |    0.30 |      - |      - |          - |          NA |
