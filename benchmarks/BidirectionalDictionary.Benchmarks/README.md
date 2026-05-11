# Benchmarks

BenchmarkDotNet suite comparing:

- `BidirectionalDictionary<TKey, TValue>` vs `Dictionary<TKey, TValue>`
- `ReadOnlyBidirectionalDictionary<TKey, TValue>` vs `ReadOnlyDictionary<TKey, TValue>`

The suite uses deterministic `int -> int` read data with `10_000` items for reading benchmarks,
and `1_000_000` items for mutation benchmarks.

## Running

Run benchmarks in Release mode:

```bash
dotnet run -c Release --project benchmarks/BidirectionalDictionary.Benchmarks -- --filter '*BidirectionalDictionary*'
dotnet run -c Release --project benchmarks/BidirectionalDictionary.Benchmarks -- --filter '*ReadOnlyBidirectionalDictionary*'
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
  Job-MAFTOU : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
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
| Operation      | Type          | ConcurrencyLevel | Mean             | Error           | StdDev          | Median           | Ratio | RatioSD | Gen0   | Gen1   | Gen2   | Allocated | Alloc Ratio |
|--------------- |-------------- |----------------- |-----------------:|----------------:|----------------:|-----------------:|------:|--------:|-------:|-------:|-------:|----------:|------------:|
| **AddOrUpdate**    | **Default**       | **1**                |        **34.825 ns** |       **0.3029 ns** |       **0.2529 ns** |        **34.811 ns** |  **1.00** |    **0.01** | **0.0050** | **0.0010** |      **-** |      **24 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 1                |       253.317 ns |      14.2562 ns |      39.5039 ns |       236.892 ns |  7.27 |    1.13 | 0.0590 | 0.0020 | 0.0010 |     248 B |       10.33 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **AddOrUpdate**    | **Default**       | **2**                |        **66.807 ns** |       **1.2991 ns** |       **3.2352 ns** |        **66.792 ns** |  **1.00** |    **0.07** | **0.0200** | **0.0010** |      **-** |      **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 2                |       335.805 ns |       6.2927 ns |      11.6639 ns |       330.805 ns |  5.04 |    0.31 | 0.0580 | 0.0010 |      - |     248 B |        2.82 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **AddOrUpdate**    | **Default**       | **4**                |        **44.068 ns** |       **0.6961 ns** |       **0.5813 ns** |        **44.113 ns** |  **1.00** |    **0.02** | **0.0200** | **0.0010** |      **-** |      **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 4                |       326.933 ns |       6.2098 ns |      10.0277 ns |       323.384 ns |  7.42 |    0.24 | 0.0590 | 0.0010 |      - |     248 B |        2.82 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **AddOrUpdate**    | **Default**       | **8**                |        **32.728 ns** |       **0.6115 ns** |       **0.6279 ns** |        **32.856 ns** |  **1.00** |    **0.03** | **0.0200** | **0.0010** |      **-** |      **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 8                |       277.909 ns |       6.7276 ns |      19.8365 ns |       275.483 ns |  8.49 |    0.62 | 0.0590 | 0.0010 |      - |     248 B |        2.82 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **AddOrUpdate**    | **Default**       | **16**               |        **30.028 ns** |       **0.5930 ns** |       **1.3016 ns** |        **29.924 ns** |  **1.00** |    **0.06** | **0.0200** | **0.0010** |      **-** |      **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 16               |       205.865 ns |       4.0530 ns |       9.5535 ns |       203.971 ns |  6.87 |    0.43 | 0.0590 | 0.0010 |      - |     248 B |        2.82 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **Clear**          | **Default**       | **1**                |   **435,686.458 ns** |  **39,120.4163 ns** | **112,871.3540 ns** |   **425,600.000 ns** |  **1.07** |    **0.38** |      **-** |      **-** |      **-** | **9301672 B** |        **1.00** |
| Clear          | Bidirectional | 1                | 2,739,464.894 ns | 164,244.6036 ns | 468,599.1303 ns | 2,658,000.000 ns |  6.70 |    2.01 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **Clear**          | **Default**       | **2**                |   **345,425.281 ns** |  **14,056.3784 ns** |  **38,950.1613 ns** |   **329,050.000 ns** |  **1.01** |    **0.15** |      **-** |      **-** |      **-** | **9302008 B** |        **1.00** |
| Clear          | Bidirectional | 2                | 2,502,036.842 ns | 121,851.1712 ns | 349,613.8773 ns | 2,420,800.000 ns |  7.32 |    1.25 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **Clear**          | **Default**       | **4**                |   **352,144.048 ns** |  **14,394.5916 ns** |  **38,670.1384 ns** |   **336,200.000 ns** |  **1.01** |    **0.15** |      **-** |      **-** |      **-** | **9302016 B** |        **1.00** |
| Clear          | Bidirectional | 4                | 2,479,773.958 ns | 119,878.0481 ns | 345,875.6035 ns | 2,420,100.000 ns |  7.11 |    1.20 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **Clear**          | **Default**       | **8**                |   **346,333.333 ns** |  **10,476.6797 ns** |  **29,720.5987 ns** |   **334,800.000 ns** |  **1.01** |    **0.12** |      **-** |      **-** |      **-** | **9302032 B** |        **1.00** |
| Clear          | Bidirectional | 8                | 2,525,925.806 ns | 139,486.4677 ns | 395,699.9209 ns | 2,432,200.000 ns |  7.34 |    1.28 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **Clear**          | **Default**       | **16**               |   **357,736.364 ns** |  **16,668.1542 ns** |  **45,908.9408 ns** |   **338,100.000 ns** |  **1.01** |    **0.17** |      **-** |      **-** |      **-** | **9302064 B** |        **1.00** |
| Clear          | Bidirectional | 16               | 2,524,130.928 ns | 135,718.9547 ns | 393,745.3252 ns | 2,408,900.000 ns |  7.15 |    1.34 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **ContainsKey**    | **Default**       | **1**                |         **5.894 ns** |       **0.0514 ns** |       **0.0429 ns** |         **5.889 ns** |  **1.00** |    **0.01** | **0.0000** |      **-** |      **-** |         **-** |          **NA** |
| ContainsKey    | Bidirectional | 1                |        23.973 ns |       0.1914 ns |       0.1599 ns |        24.043 ns |  4.07 |    0.04 |      - |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **ContainsKey**    | **Default**       | **2**                |         **3.977 ns** |       **0.0859 ns** |       **0.2396 ns** |         **3.901 ns** |  **1.00** |    **0.08** | **0.0000** |      **-** |      **-** |         **-** |          **NA** |
| ContainsKey    | Bidirectional | 2                |        48.512 ns |       0.5253 ns |       0.4914 ns |        48.435 ns | 12.24 |    0.70 |      - |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **ContainsKey**    | **Default**       | **4**                |         **3.064 ns** |       **0.0540 ns** |       **0.0451 ns** |         **3.066 ns** |  **1.00** |    **0.02** | **0.0000** |      **-** |      **-** |         **-** |          **NA** |
| ContainsKey    | Bidirectional | 4                |        44.010 ns |       0.3235 ns |       0.3026 ns |        44.055 ns | 14.37 |    0.23 | 0.0000 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **ContainsKey**    | **Default**       | **8**                |         **2.786 ns** |       **0.0263 ns** |       **0.0258 ns** |         **2.789 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |      **-** |         **-** |          **NA** |
| ContainsKey    | Bidirectional | 8                |        26.761 ns |       0.3733 ns |       0.3309 ns |        26.778 ns |  9.60 |    0.14 | 0.0000 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **ContainsKey**    | **Default**       | **16**               |         **3.468 ns** |       **0.0674 ns** |       **0.0828 ns** |         **3.443 ns** |  **1.00** |    **0.03** | **0.0001** |      **-** |      **-** |         **-** |          **NA** |
| ContainsKey    | Bidirectional | 16               |        20.362 ns |       0.3901 ns |       0.4933 ns |        20.282 ns |  5.87 |    0.19 | 0.0001 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **ContainsValue**  | **Default**       | **1**                |    **20,240.469 ns** |     **800.9431 ns** |   **2,206.0301 ns** |    **19,755.100 ns** | **1.011** |    **0.15** |      **-** |      **-** |      **-** |      **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 1                |        24.890 ns |       0.3175 ns |       0.2479 ns |        24.867 ns | 0.001 |    0.00 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **ContainsValue**  | **Default**       | **2**                |    **10,143.373 ns** |     **486.1720 ns** |   **1,387.0762 ns** |    **10,209.135 ns** | **1.024** |    **0.23** |      **-** |      **-** |      **-** |      **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 2                |        53.460 ns |       1.8551 ns |       5.4697 ns |        52.648 ns | 0.005 |    0.00 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **ContainsValue**  | **Default**       | **4**                |     **9,421.871 ns** |     **489.4908 ns** |   **1,443.2748 ns** |     **9,422.999 ns** | **1.024** |    **0.23** |      **-** |      **-** |      **-** |      **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 4                |        49.717 ns |       0.9587 ns |       1.4052 ns |        49.131 ns | 0.005 |    0.00 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **ContainsValue**  | **Default**       | **8**                |     **5,514.240 ns** |     **108.4587 ns** |     **101.4523 ns** |     **5,495.919 ns** | **1.000** |    **0.02** | **0.0100** |      **-** |      **-** |      **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 8                |        32.879 ns |       0.4041 ns |       0.3583 ns |        32.853 ns | 0.006 |    0.00 | 0.0000 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **ContainsValue**  | **Default**       | **16**               |     **7,027.016 ns** |     **138.0627 ns** |     **226.8411 ns** |     **6,953.001 ns** | **1.001** |    **0.04** | **0.0125** |      **-** |      **-** |      **57 B** |        **1.00** |
| ContainsValue  | Bidirectional | 16               |        24.648 ns |       0.3990 ns |       0.3732 ns |        24.711 ns | 0.004 |    0.00 | 0.0000 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **FindKeyByValue** | **Default**       | **1**                |    **20,054.047 ns** |     **399.9573 ns** |     **950.5408 ns** |    **19,867.500 ns** | **1.003** |    **0.07** |      **-** |      **-** |      **-** |      **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 1                |        23.820 ns |       0.1545 ns |       0.1517 ns |        23.747 ns | 0.001 |    0.00 | 0.0000 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **FindKeyByValue** | **Default**       | **2**                |    **10,725.672 ns** |     **207.2516 ns** |     **496.5617 ns** |    **10,876.016 ns** | **1.002** |    **0.07** |      **-** |      **-** |      **-** |      **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 2                |        44.841 ns |       0.5767 ns |       0.5112 ns |        44.947 ns | 0.004 |    0.00 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **FindKeyByValue** | **Default**       | **4**                |     **6,183.277 ns** |      **54.4786 ns** |      **42.5333 ns** |     **6,193.169 ns** | **1.000** |    **0.01** | **0.0125** |      **-** |      **-** |      **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 4                |        48.269 ns |       0.1510 ns |       0.1179 ns |        48.295 ns | 0.008 |    0.00 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **FindKeyByValue** | **Default**       | **8**                |     **5,570.984 ns** |      **62.0575 ns** |      **51.8208 ns** |     **5,568.034 ns** | **1.000** |    **0.01** | **0.0111** |      **-** |      **-** |      **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 8                |        33.463 ns |       0.2294 ns |       0.2146 ns |        33.494 ns | 0.006 |    0.00 | 0.0000 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **FindKeyByValue** | **Default**       | **16**               |     **4,681.511 ns** |      **30.4837 ns** |      **27.0230 ns** |     **4,683.961 ns** | **1.000** |    **0.01** | **0.0091** |      **-** |      **-** |      **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 16               |        23.975 ns |       0.3856 ns |       0.3607 ns |        23.866 ns | 0.005 |    0.00 | 0.0001 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryAdd**         | **Default**       | **1**                |        **98.235 ns** |       **1.9118 ns** |       **2.0456 ns** |        **98.303 ns** |  **1.00** |    **0.03** | **0.0060** | **0.0030** |      **-** |      **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 1                |        74.513 ns |       1.4867 ns |       2.9345 ns |        73.979 ns |  0.76 |    0.03 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryAdd**         | **Default**       | **2**                |       **129.517 ns** |       **2.5149 ns** |       **2.5826 ns** |       **129.528 ns** |  **1.00** |    **0.03** | **0.0060** | **0.0030** |      **-** |      **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 2                |       167.047 ns |       3.3398 ns |       4.5716 ns |       166.226 ns |  1.29 |    0.04 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryAdd**         | **Default**       | **4**                |       **121.260 ns** |       **2.2219 ns** |       **3.0414 ns** |       **121.729 ns** |  **1.00** |    **0.03** | **0.0060** | **0.0030** |      **-** |      **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 4                |       143.027 ns |       2.8249 ns |       4.9475 ns |       144.168 ns |  1.18 |    0.05 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryAdd**         | **Default**       | **8**                |       **102.837 ns** |       **1.9650 ns** |       **3.7387 ns** |       **102.945 ns** |  **1.00** |    **0.05** | **0.0060** | **0.0030** |      **-** |      **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 8                |       100.239 ns |       1.9128 ns |       1.6956 ns |       100.187 ns |  0.98 |    0.04 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryAdd**         | **Default**       | **16**               |       **102.684 ns** |       **2.0135 ns** |       **2.6181 ns** |       **102.769 ns** |  **1.00** |    **0.04** | **0.0060** | **0.0030** |      **-** |      **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 16               |        74.503 ns |       1.4820 ns |       2.9253 ns |        74.674 ns |  0.73 |    0.03 |      - |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryGetValue**    | **Default**       | **1**                |         **6.008 ns** |       **0.0990 ns** |       **0.0773 ns** |         **6.001 ns** |  **1.00** |    **0.02** | **0.0000** |      **-** |      **-** |         **-** |          **NA** |
| TryGetValue    | Bidirectional | 1                |        23.498 ns |       0.2889 ns |       0.2256 ns |        23.421 ns |  3.91 |    0.06 | 0.0000 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryGetValue**    | **Default**       | **2**                |         **4.410 ns** |       **0.0672 ns** |       **0.0561 ns** |         **4.418 ns** |  **1.00** |    **0.02** | **0.0000** |      **-** |      **-** |         **-** |          **NA** |
| TryGetValue    | Bidirectional | 2                |        51.536 ns |       0.9846 ns |       1.0944 ns |        51.322 ns | 11.69 |    0.28 |      - |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryGetValue**    | **Default**       | **4**                |         **3.225 ns** |       **0.0277 ns** |       **0.0232 ns** |         **3.219 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |      **-** |         **-** |          **NA** |
| TryGetValue    | Bidirectional | 4                |        37.463 ns |       0.0693 ns |       0.0579 ns |        37.467 ns | 11.62 |    0.08 | 0.0000 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryGetValue**    | **Default**       | **8**                |         **2.868 ns** |       **0.0251 ns** |       **0.0234 ns** |         **2.868 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |      **-** |         **-** |          **NA** |
| TryGetValue    | Bidirectional | 8                |        27.365 ns |       0.2303 ns |       0.1923 ns |        27.382 ns |  9.54 |    0.10 | 0.0000 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryGetValue**    | **Default**       | **16**               |         **3.371 ns** |       **0.0382 ns** |       **0.0358 ns** |         **3.375 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |      **-** |         **-** |          **NA** |
| TryGetValue    | Bidirectional | 16               |        20.558 ns |       0.2910 ns |       0.2722 ns |        20.570 ns |  6.10 |    0.10 | 0.0001 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryRemove**      | **Default**       | **1**                |        **30.625 ns** |       **0.5942 ns** |       **0.7074 ns** |        **30.550 ns** |  **1.00** |    **0.03** |      **-** |      **-** |      **-** |         **-** |          **NA** |
| TryRemove      | Bidirectional | 1                |       107.476 ns |       1.9022 ns |       1.5884 ns |       107.768 ns |  3.51 |    0.09 |      - |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryRemove**      | **Default**       | **2**                |        **71.543 ns** |       **1.5023 ns** |       **4.4295 ns** |        **71.749 ns** |  **1.00** |    **0.09** |      **-** |      **-** |      **-** |         **-** |          **NA** |
| TryRemove      | Bidirectional | 2                |       192.145 ns |       3.7165 ns |       6.9805 ns |       190.307 ns |  2.70 |    0.20 |      - |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryRemove**      | **Default**       | **4**                |        **54.128 ns** |       **1.0772 ns** |       **1.0580 ns** |        **54.280 ns** |  **1.00** |    **0.03** |      **-** |      **-** |      **-** |         **-** |          **NA** |
| TryRemove      | Bidirectional | 4                |       158.200 ns |       3.1272 ns |       4.8687 ns |       158.120 ns |  2.92 |    0.10 |      - |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryRemove**      | **Default**       | **8**                |        **35.883 ns** |       **0.7090 ns** |       **0.9219 ns** |        **35.787 ns** |  **1.00** |    **0.04** |      **-** |      **-** |      **-** |         **-** |          **NA** |
| TryRemove      | Bidirectional | 8                |       107.500 ns |       2.1147 ns |       2.3505 ns |       107.787 ns |  3.00 |    0.10 |      - |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |        |           |             |
| **TryRemove**      | **Default**       | **16**               |        **36.471 ns** |       **0.6233 ns** |       **0.5830 ns** |        **36.647 ns** |  **1.00** |    **0.02** |      **-** |      **-** |      **-** |         **-** |          **NA** |
| TryRemove      | Bidirectional | 16               |        83.536 ns |       1.6539 ns |       2.3185 ns |        83.406 ns |  2.29 |    0.07 |      - |      - |      - |         - |          NA |
