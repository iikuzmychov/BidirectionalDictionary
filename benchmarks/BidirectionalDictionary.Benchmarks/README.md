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
dotnet run -c Release --project benchmarks/BidirectionalDictionary.Benchmarks -- --filter 'BidirectionalDictionary*'
dotnet run -c Release --project benchmarks/BidirectionalDictionary.Benchmarks -- --filter 'ReadOnlyBidirectionalDictionary*'
dotnet run -c Release --project benchmarks/BidirectionalDictionary.Benchmarks -- --filter 'ConcurrentBidirectionalDictionary*'
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

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26200.8246)
Unknown processor
.NET SDK 10.0.103
  [Host]     : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
  Job-MGWKIR : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
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
| Operation       | Type          | Mean           | Error          | StdDev         | Median         | Ratio | RatioSD | Gen0    | Gen1    | Gen2    | Allocated | Alloc Ratio |
|---------------- |-------------- |---------------:|---------------:|---------------:|---------------:|------:|--------:|--------:|--------:|--------:|----------:|------------:|
| Add             | Default       |      10.060 ns |      0.8243 ns |      2.4306 ns |       9.879 ns |  1.06 |    0.37 |       - |       - |       - |         - |          NA |
| Add             | Bidirectional |      38.360 ns |      3.8474 ns |     11.0388 ns |      34.678 ns |  4.04 |    1.54 |       - |       - |       - |         - |          NA |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| ContainsKeyHit  | Default       |       3.842 ns |      0.1087 ns |      0.1413 ns |       3.779 ns |  1.00 |    0.05 |       - |       - |       - |         - |          NA |
| ContainsKeyHit  | Bidirectional |       3.983 ns |      0.0806 ns |      0.0754 ns |       3.940 ns |  1.04 |    0.04 |       - |       - |       - |         - |          NA |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| ContainsKeyMiss | Default       |       3.532 ns |      0.0221 ns |      0.0184 ns |       3.533 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| ContainsKeyMiss | Bidirectional |       3.671 ns |      0.1004 ns |      0.0986 ns |       3.631 ns |  1.04 |    0.03 |       - |       - |       - |         - |          NA |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| ContainsValue   | Default       |   5,174.305 ns |     16.6529 ns |     14.7623 ns |   5,172.353 ns | 1.000 |    0.00 |       - |       - |       - |         - |          NA |
| ContainsValue   | Bidirectional |       4.167 ns |      0.0277 ns |      0.0231 ns |       4.167 ns | 0.001 |    0.00 |       - |       - |       - |         - |          NA |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| Empty           | Default       |       6.374 ns |      0.1605 ns |      0.1340 ns |       6.399 ns |  1.00 |    0.03 |  0.0191 |       - |       - |      80 B |        1.00 |
| Empty           | Bidirectional |      31.479 ns |      0.3694 ns |      0.3085 ns |      31.540 ns |  4.94 |    0.11 |  0.0650 |       - |       - |     272 B |        3.40 |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| FindKeyByValue  | Default       |   5,206.782 ns |     98.3634 ns |     96.6060 ns |   5,162.465 ns | 1.000 |    0.03 |       - |       - |       - |         - |          NA |
| FindKeyByValue  | Bidirectional |       3.724 ns |      0.0214 ns |      0.0179 ns |       3.722 ns | 0.001 |    0.00 |       - |       - |       - |         - |          NA |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| FromSequence    | Default       |  72,041.988 ns |  1,153.4195 ns |    963.1575 ns |  71,713.953 ns |  1.00 |    0.02 | 30.2734 | 20.6299 | 20.6299 |  202285 B |        1.00 |
| FromSequence    | Bidirectional | 233,839.096 ns | 27,025.6817 ns | 79,685.8381 ns | 191,897.241 ns |  3.25 |    1.10 | 52.7344 | 33.6914 | 33.6914 |  404667 B |        2.00 |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| IndexerHit      | Default       |       3.662 ns |      0.1550 ns |      0.4296 ns |       3.507 ns |  1.01 |    0.16 |       - |       - |       - |         - |          NA |
| IndexerHit      | Bidirectional |       3.678 ns |      0.0205 ns |      0.0192 ns |       3.675 ns |  1.02 |    0.10 |       - |       - |       - |         - |          NA |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| Keys            | Default       |   9,769.016 ns |     87.7233 ns |     77.7644 ns |   9,768.065 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| Keys            | Bidirectional |  15,551.098 ns |    119.3638 ns |     99.6742 ns |  15,592.365 ns |  1.59 |    0.02 |       - |       - |       - |         - |          NA |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| LinqProjection  | Default       |  89,321.732 ns |  1,404.4192 ns |  1,313.6946 ns |  89,312.195 ns |  1.00 |    0.02 | 30.7617 | 21.1182 | 21.1182 |  202299 B |        1.00 |
| LinqProjection  | Bidirectional | 155,260.859 ns |  3,022.6142 ns |  6,174.4003 ns | 153,453.638 ns |  1.74 |    0.07 | 58.3496 | 38.8184 | 38.8184 |  404751 B |        2.00 |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| Pairs           | Default       |  23,929.998 ns |    475.8325 ns |    726.6466 ns |  23,588.928 ns |  1.00 |    0.04 |       - |       - |       - |         - |          NA |
| Pairs           | Bidirectional |  24,188.385 ns |    374.2535 ns |    655.4748 ns |  24,000.528 ns |  1.01 |    0.04 |       - |       - |       - |         - |          NA |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| PreSized        | Default       |  19,435.377 ns |    529.4690 ns |  1,484.6879 ns |  19,399.677 ns |  1.01 |    0.11 | 28.8086 | 19.0430 | 19.0430 |  202333 B |        1.00 |
| PreSized        | Bidirectional |  38,379.031 ns |    661.7242 ns |    969.9469 ns |  38,180.133 ns |  1.99 |    0.16 | 57.0068 | 37.4756 | 37.4756 |  404756 B |        2.00 |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| Remove          | Default       |       5.462 ns |      0.1487 ns |      0.4336 ns |       5.298 ns |  1.01 |    0.11 |       - |       - |       - |         - |          NA |
| Remove          | Bidirectional |      22.643 ns |      0.9331 ns |      2.7220 ns |      21.598 ns |  4.17 |    0.59 |       - |       - |       - |         - |          NA |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| SetOverwrite    | Default       |       4.352 ns |      0.1120 ns |      0.3230 ns |       4.281 ns |  1.01 |    0.10 |       - |       - |       - |         - |          NA |
| SetOverwrite    | Bidirectional |     113.896 ns |      5.8131 ns |     16.3005 ns |     107.775 ns | 26.31 |    4.19 |       - |       - |       - |         - |          NA |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| TryAdd          | Default       |       6.568 ns |      0.4606 ns |      1.3435 ns |       6.024 ns |  1.04 |    0.29 |       - |       - |       - |         - |          NA |
| TryAdd          | Bidirectional |      25.895 ns |      0.9850 ns |      2.7619 ns |      25.338 ns |  4.09 |    0.87 |       - |       - |       - |         - |          NA |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| TryGetValueHit  | Default       |       3.424 ns |      0.0662 ns |      0.0587 ns |       3.414 ns |  1.00 |    0.02 |       - |       - |       - |         - |          NA |
| TryGetValueHit  | Bidirectional |       3.550 ns |      0.0355 ns |      0.0296 ns |       3.554 ns |  1.04 |    0.02 |       - |       - |       - |         - |          NA |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| TryGetValueMiss | Default       |       3.059 ns |      0.0216 ns |      0.0191 ns |       3.061 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| TryGetValueMiss | Bidirectional |       3.412 ns |      0.0184 ns |      0.0172 ns |       3.416 ns |  1.12 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |                |                |                |       |         |         |         |         |           |             |
| Values          | Default       |   9,738.040 ns |     32.4359 ns |     28.7536 ns |   9,732.645 ns |  1.00 |    0.00 |       - |       - |       - |         - |          NA |
| Values          | Bidirectional |  12,908.630 ns |     34.0487 ns |     31.8492 ns |  12,918.018 ns |  1.33 |    0.00 |       - |       - |       - |         - |          NA |

### ReadOnlyBidirectionalDictionary

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
| ContainsKeyHit  | Default       |      4.376 ns |   0.0579 ns |   0.0513 ns |  1.00 |    0.02 |      - |         - |          NA |
| ContainsKeyHit  | Bidirectional |      4.596 ns |   0.0597 ns |   0.0530 ns |  1.05 |    0.02 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| ContainsKeyMiss | Default       |      4.019 ns |   0.0209 ns |   0.0185 ns |  1.00 |    0.01 |      - |         - |          NA |
| ContainsKeyMiss | Bidirectional |      4.339 ns |   0.0160 ns |   0.0133 ns |  1.08 |    0.01 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| ContainsValue   | Default       | 31,043.107 ns | 111.1706 ns | 103.9891 ns | 1.000 |    0.00 |      - |      48 B |        1.00 |
| ContainsValue   | Bidirectional |      5.247 ns |   0.0178 ns |   0.0139 ns | 0.000 |    0.00 |      - |         - |        0.00 |
|                 |               |               |             |             |       |         |        |           |             |
| FindKeyByValue  | Default       | 31,026.103 ns | 299.2703 ns | 249.9043 ns | 1.000 |    0.01 |      - |      48 B |        1.00 |
| FindKeyByValue  | Bidirectional |      4.867 ns |   0.1223 ns |   0.1084 ns | 0.000 |    0.00 |      - |         - |        0.00 |
|                 |               |               |             |             |       |         |        |           |             |
| IndexerHit      | Default       |      4.008 ns |   0.0209 ns |   0.0196 ns |  1.00 |    0.01 |      - |         - |          NA |
| IndexerHit      | Bidirectional |      4.117 ns |   0.0296 ns |   0.0263 ns |  1.03 |    0.01 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| Keys            | Default       | 20,750.597 ns | 158.7704 ns | 148.5139 ns |  1.00 |    0.01 |      - |      40 B |        1.00 |
| Keys            | Bidirectional | 21,097.819 ns |  71.5137 ns |  63.3951 ns |  1.02 |    0.01 |      - |      48 B |        1.20 |
|                 |               |               |             |             |       |         |        |           |             |
| Pairs           | Default       | 64,252.746 ns | 382.7578 ns | 358.0319 ns |  1.00 |    0.01 |      - |      48 B |        1.00 |
| Pairs           | Bidirectional | 64,310.494 ns | 324.5159 ns | 303.5524 ns |  1.00 |    0.01 |      - |      48 B |        1.00 |
|                 |               |               |             |             |       |         |        |           |             |
| TryGetValueHit  | Default       |      4.180 ns |   0.0447 ns |   0.0396 ns |  1.00 |    0.01 |      - |         - |          NA |
| TryGetValueHit  | Bidirectional |      4.327 ns |   0.0314 ns |   0.0262 ns |  1.04 |    0.01 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| TryGetValueMiss | Default       |      3.744 ns |   0.0151 ns |   0.0134 ns |  1.00 |    0.00 |      - |         - |          NA |
| TryGetValueMiss | Bidirectional |      4.743 ns |   0.0155 ns |   0.0137 ns |  1.27 |    0.01 |      - |         - |          NA |
|                 |               |               |             |             |       |         |        |           |             |
| Values          | Default       | 20,727.208 ns | 107.3377 ns |  95.1521 ns |  1.00 |    0.01 |      - |      40 B |        1.00 |
| Values          | Bidirectional | 21,073.183 ns | 157.9203 ns | 123.2938 ns |  1.02 |    0.01 |      - |      48 B |        1.20 |
|                 |               |               |             |             |       |         |        |           |             |
| Wrap            | Default       |      5.958 ns |   0.0883 ns |   0.0737 ns |  1.00 |    0.02 | 0.0096 |      40 B |        1.00 |
| Wrap            | Bidirectional |     13.412 ns |   0.3212 ns |   0.3155 ns |  2.25 |    0.06 | 0.0229 |      96 B |        2.40 |


### ConcurrentBidirectionalDictionary

```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26200.8246)
Unknown processor
.NET SDK 10.0.103
  [Host]     : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
  Job-WUJEBZ : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
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
| Operation      | Type          | ConcurrencyLevel | Mean           | Error           | StdDev          | Median         | Ratio | RatioSD | Gen0   | Gen1   | Allocated  | Alloc Ratio |
|--------------- |-------------- |----------------- |---------------:|----------------:|----------------:|---------------:|------:|--------:|-------:|-------:|-----------:|------------:|
| **AddOrUpdate**    | **Default**       | **1**                |      **46.958 ns** |       **1.8700 ns** |       **5.3351 ns** |      **49.529 ns** |  **1.02** |    **0.18** | **0.0200** | **0.0010** |       **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 1                |     406.700 ns |       3.7120 ns |       3.0997 ns |     406.054 ns |  8.79 |    1.16 | 0.0100 | 0.0040 |       64 B |        0.73 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **AddOrUpdate**    | **Default**       | **2**                |      **63.996 ns** |       **1.6403 ns** |       **4.8365 ns** |      **63.653 ns** |  **1.01** |    **0.11** | **0.0200** | **0.0010** |       **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 2                |     586.708 ns |       8.0618 ns |       7.1466 ns |     589.084 ns |  9.22 |    0.71 | 0.0200 | 0.0050 |      128 B |        1.45 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **AddOrUpdate**    | **Default**       | **4**                |      **48.342 ns** |       **0.9482 ns** |       **1.2658 ns** |      **48.251 ns** |  **1.00** |    **0.04** | **0.0200** | **0.0010** |       **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 4                |     609.412 ns |       8.8008 ns |       7.8017 ns |     610.727 ns | 12.61 |    0.36 | 0.0200 | 0.0050 |      128 B |        1.45 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **AddOrUpdate**    | **Default**       | **8**                |      **32.108 ns** |       **0.6294 ns** |       **1.0516 ns** |      **31.882 ns** |  **1.00** |    **0.05** | **0.0200** | **0.0010** |       **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 8                |     577.984 ns |       7.6928 ns |       7.1959 ns |     578.596 ns | 18.02 |    0.61 | 0.0200 | 0.0050 |      128 B |        1.45 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **AddOrUpdate**    | **Default**       | **16**               |      **32.236 ns** |       **0.6341 ns** |       **1.1434 ns** |      **32.207 ns** |  **1.00** |    **0.05** | **0.0200** | **0.0010** |       **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 16               |     497.922 ns |       9.9382 ns |      20.7448 ns |     502.625 ns | 15.47 |    0.84 | 0.0200 | 0.0050 |      128 B |        1.45 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **1**                | **344,022.368 ns** |   **6,882.6710 ns** |  **17,518.5966 ns** | **340,800.000 ns** |  **1.00** |    **0.07** |      **-** |      **-** |  **9302008 B** |        **1.00** |
| Clear          | Bidirectional | 1                | 648,870.707 ns | 105,869.3420 ns | 310,496.4761 ns | 780,200.000 ns |  1.89 |    0.91 |      - |      - | 18603616 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **2**                | **363,194.253 ns** |   **9,741.7900 ns** |  **26,667.9947 ns** | **357,200.000 ns** |  **1.01** |    **0.10** |      **-** |      **-** |  **9302008 B** |        **1.00** |
| Clear          | Bidirectional | 2                | 620,660.000 ns |  89,438.9691 ns | 263,712.8374 ns | 791,950.000 ns |  1.72 |    0.74 |      - |      - | 18603616 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **4**                | **356,981.579 ns** |   **7,148.7407 ns** |  **18,195.8289 ns** | **354,700.000 ns** |  **1.00** |    **0.07** |      **-** |      **-** |  **9302016 B** |        **1.00** |
| Clear          | Bidirectional | 4                | 629,981.633 ns |  96,439.7976 ns | 281,319.4366 ns | 728,900.000 ns |  1.77 |    0.79 |      - |      - | 18603632 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **8**                | **362,314.773 ns** |   **8,063.0846 ns** |  **22,208.0784 ns** | **359,700.000 ns** |  **1.00** |    **0.09** |      **-** |      **-** |  **9302032 B** |        **1.00** |
| Clear          | Bidirectional | 8                | 632,924.242 ns |  96,148.6138 ns | 281,987.2609 ns | 788,300.000 ns |  1.75 |    0.79 |      - |      - | 18603664 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **16**               | **357,831.081 ns** |   **7,177.3821 ns** |  **18,006.6497 ns** | **356,300.000 ns** |  **1.00** |    **0.07** |      **-** |      **-** |  **9302064 B** |        **1.00** |
| Clear          | Bidirectional | 16               | 644,859.000 ns |  95,483.6173 ns | 281,535.6203 ns | 776,000.000 ns |  1.81 |    0.79 |      - |      - | 18603728 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **1**                |       **5.816 ns** |       **0.0189 ns** |       **0.0158 ns** |       **5.818 ns** |  **1.00** |    **0.00** | **0.0000** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 1                |       6.428 ns |       0.0290 ns |       0.0271 ns |       6.426 ns |  1.11 |    0.01 | 0.0000 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **2**                |       **3.489 ns** |       **0.0697 ns** |       **0.0652 ns** |       **3.477 ns** |  **1.00** |    **0.03** | **0.0000** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 2                |       3.954 ns |       0.0781 ns |       0.1872 ns |       4.020 ns |  1.13 |    0.06 | 0.0000 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **4**                |       **2.878 ns** |       **0.0303 ns** |       **0.0253 ns** |       **2.874 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 4                |       3.017 ns |       0.0138 ns |       0.0122 ns |       3.016 ns |  1.05 |    0.01 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **8**                |       **2.585 ns** |       **0.0119 ns** |       **0.0106 ns** |       **2.585 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 8                |       2.741 ns |       0.0429 ns |       0.0615 ns |       2.730 ns |  1.06 |    0.02 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **16**               |       **3.176 ns** |       **0.0260 ns** |       **0.0231 ns** |       **3.168 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 16               |       3.316 ns |       0.0655 ns |       0.0644 ns |       3.293 ns |  1.04 |    0.02 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **1**                |  **17,864.003 ns** |     **354.4018 ns** |     **821.3801 ns** |  **18,069.645 ns** | **1.003** |    **0.07** |      **-** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 1                |       7.303 ns |       0.0359 ns |       0.0300 ns |       7.302 ns | 0.000 |    0.00 | 0.0000 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **2**                |   **8,400.191 ns** |     **159.4504 ns** |     **133.1483 ns** |   **8,359.401 ns** | **1.000** |    **0.02** |      **-** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 2                |       4.227 ns |       0.0653 ns |       0.0510 ns |       4.208 ns | 0.001 |    0.00 | 0.0000 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **4**                |   **6,741.663 ns** |      **78.5949 ns** |      **87.3581 ns** |   **6,746.201 ns** | **1.000** |    **0.02** | **0.0125** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 4                |       3.430 ns |       0.0339 ns |       0.0301 ns |       3.428 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **8**                |   **6,725.890 ns** |      **46.9660 ns** |      **41.6341 ns** |   **6,718.313 ns** | **1.000** |    **0.01** | **0.0125** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 8                |       3.013 ns |       0.0306 ns |       0.0271 ns |       3.010 ns | 0.000 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **16**               |   **5,152.655 ns** |      **29.6645 ns** |      **23.1601 ns** |   **5,156.879 ns** | **1.000** |    **0.01** | **0.0100** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 16               |       3.476 ns |       0.0345 ns |       0.0306 ns |       3.466 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **1**                |  **16,130.551 ns** |     **316.3401 ns** |     **570.4265 ns** |  **15,793.173 ns** | **1.001** |    **0.05** |      **-** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 1                |       7.692 ns |       0.0278 ns |       0.0232 ns |       7.692 ns | 0.000 |    0.00 | 0.0000 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **2**                |   **8,106.352 ns** |     **140.6088 ns** |     **117.4147 ns** |   **8,075.323 ns** | **1.000** |    **0.02** |      **-** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 2                |       4.675 ns |       0.0928 ns |       0.1417 ns |       4.731 ns | 0.001 |    0.00 | 0.0000 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **4**                |   **5,668.667 ns** |      **81.2821 ns** |      **76.0313 ns** |   **5,668.351 ns** | **1.000** |    **0.02** | **0.0111** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 4                |       3.554 ns |       0.0190 ns |       0.0158 ns |       3.549 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **8**                |   **5,697.315 ns** |     **111.2862 ns** |     **104.0972 ns** |   **5,690.413 ns** | **1.000** |    **0.02** | **0.0111** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 8                |       3.061 ns |       0.0309 ns |       0.0274 ns |       3.054 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **16**               |   **5,494.619 ns** |      **47.0236 ns** |      **39.2668 ns** |   **5,480.895 ns** | **1.000** |    **0.01** | **0.0100** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 16               |       3.527 ns |       0.0459 ns |       0.0429 ns |       3.527 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **1**                |      **93.081 ns** |       **1.7626 ns** |       **1.8859 ns** |      **93.341 ns** |  **1.00** |    **0.03** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 1                |     342.183 ns |       4.9775 ns |       4.6560 ns |     343.096 ns |  3.68 |    0.09 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **2**                |     **133.995 ns** |       **2.5296 ns** |       **2.8117 ns** |     **133.291 ns** |  **1.00** |    **0.03** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 2                |     427.238 ns |       3.3320 ns |       3.1168 ns |     426.622 ns |  3.19 |    0.07 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **4**                |     **115.744 ns** |       **2.2725 ns** |       **3.8589 ns** |     **116.292 ns** |  **1.00** |    **0.05** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 4                |     434.830 ns |       3.7850 ns |       3.5405 ns |     435.217 ns |  3.76 |    0.13 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **8**                |      **93.212 ns** |       **1.7387 ns** |       **2.9050 ns** |      **93.524 ns** |  **1.00** |    **0.04** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 8                |     373.434 ns |       7.4422 ns |       6.9614 ns |     375.790 ns |  4.01 |    0.14 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **16**               |      **93.101 ns** |       **1.8483 ns** |       **3.0368 ns** |      **92.558 ns** |  **1.00** |    **0.05** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 16               |     331.613 ns |       6.5343 ns |       6.4176 ns |     333.583 ns |  3.57 |    0.13 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **1**                |       **5.820 ns** |       **0.0243 ns** |       **0.0227 ns** |       **5.816 ns** |  **1.00** |    **0.01** | **0.0000** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 1                |       6.442 ns |       0.0341 ns |       0.0319 ns |       6.443 ns |  1.11 |    0.01 | 0.0000 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **2**                |       **3.707 ns** |       **0.0737 ns** |       **0.1489 ns** |       **3.753 ns** |  **1.00** |    **0.06** | **0.0000** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 2                |       4.109 ns |       0.0411 ns |       0.0364 ns |       4.111 ns |  1.11 |    0.05 | 0.0000 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **4**                |       **2.892 ns** |       **0.0248 ns** |       **0.0232 ns** |       **2.884 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 4                |       3.087 ns |       0.0335 ns |       0.0279 ns |       3.082 ns |  1.07 |    0.01 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **8**                |       **2.629 ns** |       **0.0200 ns** |       **0.0167 ns** |       **2.623 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 8                |       2.799 ns |       0.0550 ns |       0.0873 ns |       2.756 ns |  1.06 |    0.03 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **16**               |       **3.107 ns** |       **0.0550 ns** |       **0.0515 ns** |       **3.087 ns** |  **1.00** |    **0.02** | **0.0001** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 16               |       3.230 ns |       0.0490 ns |       0.0458 ns |       3.228 ns |  1.04 |    0.02 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **1**                |      **29.657 ns** |       **0.5869 ns** |       **1.0432 ns** |      **29.204 ns** |  **1.00** |    **0.05** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 1                |     126.865 ns |       1.8700 ns |       2.1535 ns |     126.331 ns |  4.28 |    0.16 |      - |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **2**                |      **67.424 ns** |       **1.8599 ns** |       **5.4253 ns** |      **67.961 ns** |  **1.01** |    **0.13** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 2                |     227.357 ns |       4.4995 ns |       6.7346 ns |     226.791 ns |  3.40 |    0.34 |      - |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **4**                |      **56.893 ns** |       **1.1298 ns** |       **3.1118 ns** |      **56.364 ns** |  **1.00** |    **0.08** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 4                |     234.407 ns |       4.6323 ns |       9.1437 ns |     233.914 ns |  4.13 |    0.27 |      - |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **8**                |      **35.595 ns** |       **0.7080 ns** |       **1.6410 ns** |      **35.270 ns** |  **1.00** |    **0.06** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 8                |     156.792 ns |       1.2880 ns |       1.0755 ns |     156.811 ns |  4.41 |    0.20 |      - |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **16**               |      **33.081 ns** |       **0.7934 ns** |       **2.3392 ns** |      **33.073 ns** |  **1.00** |    **0.10** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 16               |     145.348 ns |       2.8335 ns |       4.2411 ns |     144.623 ns |  4.42 |    0.33 |      - |      - |          - |          NA |
