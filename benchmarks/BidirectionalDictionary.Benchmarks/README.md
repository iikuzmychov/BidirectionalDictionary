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
  Job-KLJGFH : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
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
| **AddOrUpdate**    | **Default**       | **1**                |      **37.698 ns** |       **0.5700 ns** |       **0.5053 ns** |      **37.820 ns** |  **1.00** |    **0.02** | **0.0050** | **0.0010** |       **24 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 1                |     482.864 ns |       5.1636 ns |       4.8300 ns |     483.553 ns | 12.81 |    0.21 | 0.0250 | 0.0060 |      160 B |        6.67 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **AddOrUpdate**    | **Default**       | **2**                |      **60.704 ns** |       **1.6150 ns** |       **4.7365 ns** |      **59.509 ns** |  **1.01** |    **0.11** | **0.0200** | **0.0010** |       **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 2                |     661.411 ns |      13.2192 ns |      25.4690 ns |     650.191 ns | 10.96 |    0.91 | 0.0460 | 0.0110 |      288 B |        3.27 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **AddOrUpdate**    | **Default**       | **4**                |      **47.978 ns** |       **0.8787 ns** |       **1.2880 ns** |      **47.936 ns** |  **1.00** |    **0.04** | **0.0200** | **0.0010** |       **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 4                |     636.711 ns |       8.7067 ns |       7.2705 ns |     637.333 ns | 13.28 |    0.38 | 0.0460 | 0.0110 |      288 B |        3.27 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **AddOrUpdate**    | **Default**       | **8**                |      **33.600 ns** |       **0.6714 ns** |       **1.3867 ns** |      **33.530 ns** |  **1.00** |    **0.06** | **0.0200** | **0.0010** |       **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 8                |     560.682 ns |       9.0016 ns |       8.8408 ns |     559.357 ns | 16.71 |    0.72 | 0.0460 | 0.0110 |      288 B |        3.27 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **AddOrUpdate**    | **Default**       | **16**               |      **32.883 ns** |       **0.6567 ns** |       **1.3852 ns** |      **32.957 ns** |  **1.00** |    **0.06** | **0.0200** | **0.0010** |       **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 16               |     480.438 ns |       8.3543 ns |      19.5278 ns |     476.521 ns | 14.64 |    0.85 | 0.0460 | 0.0110 |      288 B |        3.27 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **1**                | **355,113.187 ns** |  **10,814.5386 ns** |  **30,325.1279 ns** | **343,600.000 ns** |  **1.01** |    **0.11** |      **-** |      **-** |  **9302008 B** |        **1.00** |
| Clear          | Bidirectional | 1                | 616,603.000 ns |  92,394.8533 ns | 272,428.3294 ns | 776,950.000 ns |  1.75 |    0.78 |      - |      - | 18603616 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **2**                | **371,423.913 ns** |  **11,189.1774 ns** |  **31,559.2757 ns** | **366,500.000 ns** |  **1.01** |    **0.12** |      **-** |      **-** |  **9302008 B** |        **1.00** |
| Clear          | Bidirectional | 2                | 637,789.000 ns |  93,204.7224 ns | 274,816.2467 ns | 792,050.000 ns |  1.73 |    0.76 |      - |      - | 18603616 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **4**                | **362,722.222 ns** |  **11,044.9575 ns** |  **30,788.9291 ns** | **354,050.000 ns** |  **1.01** |    **0.12** |      **-** |      **-** |  **9301680 B** |        **1.00** |
| Clear          | Bidirectional | 4                | 652,138.384 ns | 102,100.3718 ns | 299,442.7381 ns | 647,500.000 ns |  1.81 |    0.84 |      - |      - | 18603632 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **8**                | **371,125.269 ns** |  **12,695.8787 ns** |  **36,016.0974 ns** | **364,250.000 ns** |  **1.01** |    **0.13** |      **-** |      **-** |  **9302032 B** |        **1.00** |
| Clear          | Bidirectional | 8                | 670,704.000 ns | 102,561.3995 ns | 302,404.6224 ns | 796,500.000 ns |  1.82 |    0.84 |      - |      - | 18603664 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **Clear**          | **Default**       | **16**               | **389,943.478 ns** |  **15,746.6978 ns** |  **44,413.8438 ns** | **379,800.000 ns** |  **1.01** |    **0.16** |      **-** |      **-** |  **9302064 B** |        **1.00** |
| Clear          | Bidirectional | 16               | 681,568.000 ns | 101,937.0112 ns | 300,563.5992 ns | 805,000.000 ns |  1.77 |    0.80 |      - |      - | 18603728 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **1**                |       **5.875 ns** |       **0.0138 ns** |       **0.0122 ns** |       **5.873 ns** |  **1.00** |    **0.00** | **0.0000** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 1                |       6.466 ns |       0.0290 ns |       0.0257 ns |       6.465 ns |  1.10 |    0.00 | 0.0000 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **2**                |       **3.438 ns** |       **0.0516 ns** |       **0.0483 ns** |       **3.427 ns** |  **1.00** |    **0.02** | **0.0000** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 2                |       4.016 ns |       0.0796 ns |       0.1115 ns |       4.033 ns |  1.17 |    0.04 | 0.0000 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **4**                |       **2.812 ns** |       **0.0139 ns** |       **0.0116 ns** |       **2.809 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 4                |       3.053 ns |       0.0586 ns |       0.0697 ns |       3.028 ns |  1.09 |    0.02 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **8**                |       **2.596 ns** |       **0.0393 ns** |       **0.0367 ns** |       **2.590 ns** |  **1.00** |    **0.02** | **0.0001** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 8                |       2.760 ns |       0.0535 ns |       0.0848 ns |       2.730 ns |  1.06 |    0.04 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsKey**    | **Default**       | **16**               |       **3.118 ns** |       **0.0301 ns** |       **0.0281 ns** |       **3.110 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |          **-** |          **NA** |
| ContainsKey    | Bidirectional | 16               |       3.209 ns |       0.0380 ns |       0.0337 ns |       3.212 ns |  1.03 |    0.01 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **1**                |  **16,802.358 ns** |     **334.1251 ns** |     **862.4837 ns** |  **17,098.640 ns** | **1.003** |    **0.08** |      **-** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 1                |       7.320 ns |       0.0398 ns |       0.0352 ns |       7.315 ns | 0.000 |    0.00 | 0.0000 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **2**                |   **9,423.875 ns** |     **188.1919 ns** |     **396.9607 ns** |   **9,537.547 ns** | **1.002** |    **0.06** |      **-** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 2                |       4.894 ns |       0.0947 ns |       0.0972 ns |       4.864 ns | 0.001 |    0.00 | 0.0000 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **4**                |   **6,744.940 ns** |     **117.5268 ns** |     **186.4099 ns** |   **6,727.703 ns** | **1.001** |    **0.04** | **0.0125** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 4                |       3.391 ns |       0.0584 ns |       0.0488 ns |       3.365 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **8**                |   **7,297.872 ns** |     **143.6812 ns** |     **303.0725 ns** |   **7,189.458 ns** | **1.002** |    **0.06** | **0.0125** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 8                |       2.962 ns |       0.0585 ns |       0.1273 ns |       2.902 ns | 0.000 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **ContainsValue**  | **Default**       | **16**               |   **6,993.203 ns** |      **24.5526 ns** |      **20.5025 ns** |   **6,991.389 ns** | **1.000** |    **0.00** | **0.0125** |      **-** |       **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 16               |       3.384 ns |       0.0333 ns |       0.0278 ns |       3.381 ns | 0.000 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **1**                |  **14,911.347 ns** |     **292.8574 ns** |     **420.0074 ns** |  **15,168.787 ns** | **1.001** |    **0.04** |      **-** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 1                |       7.708 ns |       0.0287 ns |       0.0268 ns |       7.700 ns | 0.001 |    0.00 | 0.0000 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **2**                |  **10,179.544 ns** |      **91.3564 ns** |      **85.4549 ns** |  **10,193.800 ns** | **1.000** |    **0.01** |      **-** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 2                |       5.455 ns |       0.0769 ns |       0.0790 ns |       5.437 ns | 0.001 |    0.00 | 0.0000 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **4**                |   **7,381.640 ns** |     **121.1616 ns** |     **139.5299 ns** |   **7,364.238 ns** | **1.000** |    **0.03** |      **-** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 4                |       3.842 ns |       0.0503 ns |       0.0420 ns |       3.835 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **8**                |   **5,816.196 ns** |     **113.0189 ns** |     **110.9997 ns** |   **5,780.938 ns** | **1.000** |    **0.03** | **0.0091** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 8                |       3.142 ns |       0.0606 ns |       0.0622 ns |       3.130 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **FindKeyByValue** | **Default**       | **16**               |   **6,719.349 ns** |      **59.5328 ns** |      **55.6870 ns** |   **6,714.629 ns** | **1.000** |    **0.01** | **0.0100** |      **-** |       **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 16               |       3.582 ns |       0.0712 ns |       0.0999 ns |       3.529 ns | 0.001 |    0.00 | 0.0001 |      - |          - |        0.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **1**                |      **92.449 ns** |       **1.8198 ns** |       **2.1663 ns** |      **92.521 ns** |  **1.00** |    **0.03** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 1                |     341.430 ns |       2.9370 ns |       2.6036 ns |     341.767 ns |  3.70 |    0.09 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **2**                |     **131.102 ns** |       **2.5585 ns** |       **3.0457 ns** |     **130.780 ns** |  **1.00** |    **0.03** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 2                |     418.585 ns |       4.6270 ns |       3.8638 ns |     418.162 ns |  3.19 |    0.08 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **4**                |     **116.018 ns** |       **2.2890 ns** |       **3.8245 ns** |     **115.644 ns** |  **1.00** |    **0.05** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 4                |     422.996 ns |       5.5247 ns |       5.1678 ns |     424.056 ns |  3.65 |    0.13 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **8**                |      **92.049 ns** |       **1.8391 ns** |       **3.8793 ns** |      **91.783 ns** |  **1.00** |    **0.06** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 8                |     366.819 ns |       5.5848 ns |       5.2240 ns |     368.772 ns |  3.99 |    0.17 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryAdd**         | **Default**       | **16**               |      **90.507 ns** |       **1.7768 ns** |       **2.7133 ns** |      **90.321 ns** |  **1.00** |    **0.04** | **0.0060** | **0.0030** |       **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 16               |     322.010 ns |       5.1338 ns |       4.8021 ns |     321.812 ns |  3.56 |    0.12 | 0.0120 | 0.0060 |       80 B |        2.00 |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **1**                |       **5.809 ns** |       **0.0319 ns** |       **0.0283 ns** |       **5.801 ns** |  **1.00** |    **0.01** | **0.0000** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 1                |       6.457 ns |       0.0436 ns |       0.0408 ns |       6.454 ns |  1.11 |    0.01 | 0.0000 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **2**                |       **3.409 ns** |       **0.0600 ns** |       **0.0561 ns** |       **3.403 ns** |  **1.00** |    **0.02** | **0.0000** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 2                |       4.210 ns |       0.0444 ns |       0.0416 ns |       4.215 ns |  1.24 |    0.02 | 0.0000 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **4**                |       **2.847 ns** |       **0.0199 ns** |       **0.0176 ns** |       **2.847 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 4                |       2.974 ns |       0.0136 ns |       0.0120 ns |       2.973 ns |  1.04 |    0.01 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **8**                |       **2.672 ns** |       **0.0527 ns** |       **0.0755 ns** |       **2.638 ns** |  **1.00** |    **0.04** | **0.0001** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 8                |       2.677 ns |       0.0183 ns |       0.0153 ns |       2.676 ns |  1.00 |    0.03 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryGetValue**    | **Default**       | **16**               |       **3.157 ns** |       **0.0627 ns** |       **0.1115 ns** |       **3.103 ns** |  **1.00** |    **0.05** | **0.0001** |      **-** |          **-** |          **NA** |
| TryGetValue    | Bidirectional | 16               |       3.202 ns |       0.0394 ns |       0.0369 ns |       3.199 ns |  1.02 |    0.04 | 0.0001 |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **1**                |      **29.050 ns** |       **0.5686 ns** |       **0.5584 ns** |      **29.361 ns** |  **1.00** |    **0.03** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 1                |     125.787 ns |       1.2620 ns |       0.9853 ns |     125.741 ns |  4.33 |    0.09 |      - |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **2**                |      **64.379 ns** |       **1.8701 ns** |       **5.4256 ns** |      **64.677 ns** |  **1.01** |    **0.14** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 2                |     214.502 ns |       4.2719 ns |       8.8222 ns |     213.888 ns |  3.36 |    0.39 |      - |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **4**                |      **56.918 ns** |       **1.3003 ns** |       **3.8135 ns** |      **56.951 ns** |  **1.00** |    **0.10** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 4                |     210.951 ns |       4.1707 ns |       5.7089 ns |     210.166 ns |  3.72 |    0.27 |      - |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **8**                |      **36.689 ns** |       **0.7292 ns** |       **1.8159 ns** |      **37.001 ns** |  **1.00** |    **0.07** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 8                |     146.461 ns |       2.9291 ns |       4.0093 ns |     146.528 ns |  4.00 |    0.22 |      - |      - |          - |          NA |
|                |               |                  |                |                 |                 |                |       |         |        |        |            |             |
| **TryRemove**      | **Default**       | **16**               |      **32.211 ns** |       **0.8371 ns** |       **2.4550 ns** |      **32.201 ns** |  **1.01** |    **0.11** |      **-** |      **-** |          **-** |          **NA** |
| TryRemove      | Bidirectional | 16               |     120.738 ns |       2.3957 ns |       3.5858 ns |     120.019 ns |  3.77 |    0.30 |      - |      - |          - |          NA |
