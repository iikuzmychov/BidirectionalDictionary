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
  Job-RJLILX : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
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
| Operation      | Type          | ConcurrencyLevel | Mean             | Error           | StdDev          | Median           | Ratio | RatioSD | Gen0   | Gen1   | Allocated | Alloc Ratio |
|--------------- |-------------- |----------------- |-----------------:|----------------:|----------------:|-----------------:|------:|--------:|-------:|-------:|----------:|------------:|
| **AddOrUpdate**    | **Default**       | **1**                |        **45.537 ns** |       **1.5154 ns** |       **4.2989 ns** |        **46.890 ns** |  **1.01** |    **0.15** | **0.0200** | **0.0010** |      **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 1                |       180.557 ns |       3.6032 ns |       4.0050 ns |       178.610 ns |  4.01 |    0.45 | 0.0580 | 0.0010 |     247 B |        2.81 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **AddOrUpdate**    | **Default**       | **2**                |        **57.557 ns** |       **2.0477 ns** |       **6.0376 ns** |        **56.713 ns** |  **1.01** |    **0.15** | **0.0200** | **0.0010** |      **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 2                |       252.317 ns |       4.9592 ns |       9.4355 ns |       255.016 ns |  4.43 |    0.49 | 0.0580 | 0.0010 |     248 B |        2.82 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **AddOrUpdate**    | **Default**       | **4**                |        **44.013 ns** |       **0.8540 ns** |       **0.9492 ns** |        **44.039 ns** |  **1.00** |    **0.03** | **0.0200** | **0.0010** |      **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 4                |       364.155 ns |       6.1222 ns |       5.4272 ns |       364.229 ns |  8.28 |    0.21 | 0.0590 | 0.0010 |     248 B |        2.82 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **AddOrUpdate**    | **Default**       | **8**                |        **31.228 ns** |       **0.6079 ns** |       **0.8116 ns** |        **31.150 ns** |  **1.00** |    **0.04** | **0.0200** | **0.0010** |      **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 8                |       478.653 ns |       6.5096 ns |       5.7706 ns |       480.666 ns | 15.34 |    0.42 | 0.0590 | 0.0010 |     248 B |        2.82 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **AddOrUpdate**    | **Default**       | **16**               |        **28.191 ns** |       **0.5580 ns** |       **1.3370 ns** |        **28.051 ns** |  **1.00** |    **0.07** | **0.0200** | **0.0010** |      **88 B** |        **1.00** |
| AddOrUpdate    | Bidirectional | 16               |       497.888 ns |       9.8519 ns |      19.9014 ns |       499.569 ns | 17.70 |    1.08 | 0.0590 | 0.0010 |     248 B |        2.82 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **Clear**          | **Default**       | **1**                |   **441,397.980 ns** |  **37,156.4423 ns** | **108,973.4212 ns** |   **391,600.000 ns** |  **1.05** |    **0.34** |      **-** |      **-** | **9302008 B** |        **1.00** |
| Clear          | Bidirectional | 1                | 2,847,627.368 ns | 131,390.7551 ns | 376,984.7340 ns | 2,768,800.000 ns |  6.79 |    1.69 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **Clear**          | **Default**       | **2**                |   **472,760.417 ns** |  **37,516.1674 ns** | **108,242.7287 ns** |   **437,950.000 ns** |  **1.05** |    **0.33** |      **-** |      **-** | **9302008 B** |        **1.00** |
| Clear          | Bidirectional | 2                | 2,392,936.559 ns |  80,840.1167 ns | 229,329.9724 ns | 2,350,700.000 ns |  5.31 |    1.21 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **Clear**          | **Default**       | **4**                |   **424,829.798 ns** |  **32,652.5767 ns** |  **95,764.3621 ns** |   **381,150.000 ns** |  **1.05** |    **0.31** |      **-** |      **-** | **9301728 B** |        **1.00** |
| Clear          | Bidirectional | 4                | 2,351,667.742 ns |  68,333.4365 ns | 193,850.5998 ns | 2,318,100.000 ns |  5.78 |    1.23 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **Clear**          | **Default**       | **8**                |   **426,461.000 ns** |  **34,400.9178 ns** | **101,431.8898 ns** |   **378,300.000 ns** |  **1.05** |    **0.32** |      **-** |      **-** | **9302032 B** |        **1.00** |
| Clear          | Bidirectional | 8                | 2,294,430.000 ns |  55,579.6518 ns | 154,933.8652 ns | 2,303,500.000 ns |  5.64 |    1.17 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **Clear**          | **Default**       | **16**               |   **436,164.286 ns** |  **34,101.7967 ns** |  **99,476.5488 ns** |   **383,700.000 ns** |  **1.05** |    **0.31** |      **-** |      **-** | **9302064 B** |        **1.00** |
| Clear          | Bidirectional | 16               | 2,351,692.632 ns |  75,408.4161 ns | 216,360.8974 ns | 2,305,200.000 ns |  5.63 |    1.21 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **ContainsKey**    | **Default**       | **1**                |         **5.767 ns** |       **0.0108 ns** |       **0.0084 ns** |         **5.768 ns** |  **1.00** |    **0.00** | **0.0000** |      **-** |         **-** |          **NA** |
| ContainsKey    | Bidirectional | 1                |        23.377 ns |       0.0761 ns |       0.0712 ns |        23.378 ns |  4.05 |    0.01 | 0.0000 |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **ContainsKey**    | **Default**       | **2**                |         **3.420 ns** |       **0.0632 ns** |       **0.0621 ns** |         **3.436 ns** |  **1.00** |    **0.02** | **0.0000** |      **-** |         **-** |          **NA** |
| ContainsKey    | Bidirectional | 2                |        24.894 ns |       0.2223 ns |       0.1856 ns |        24.927 ns |  7.28 |    0.14 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **ContainsKey**    | **Default**       | **4**                |         **2.891 ns** |       **0.0349 ns** |       **0.0309 ns** |         **2.883 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |         **-** |          **NA** |
| ContainsKey    | Bidirectional | 4                |        30.957 ns |       0.6161 ns |       0.8836 ns |        31.053 ns | 10.71 |    0.32 | 0.0000 |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **ContainsKey**    | **Default**       | **8**                |         **2.647 ns** |       **0.0276 ns** |       **0.0244 ns** |         **2.646 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |         **-** |          **NA** |
| ContainsKey    | Bidirectional | 8                |        40.913 ns |       0.5342 ns |       0.4997 ns |        40.965 ns | 15.46 |    0.23 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **ContainsKey**    | **Default**       | **16**               |         **3.220 ns** |       **0.0594 ns** |       **0.0526 ns** |         **3.240 ns** |  **1.00** |    **0.02** | **0.0001** |      **-** |         **-** |          **NA** |
| ContainsKey    | Bidirectional | 16               |        43.248 ns |       0.3682 ns |       0.3445 ns |        43.333 ns | 13.43 |    0.24 | 0.0001 |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **ContainsValue**  | **Default**       | **1**                |    **16,771.998 ns** |     **226.7297 ns** |     **408.8405 ns** |    **16,710.393 ns** | **1.001** |    **0.03** |      **-** |      **-** |      **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 1                |        24.922 ns |       0.2054 ns |       0.1921 ns |        24.875 ns | 0.001 |    0.00 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **ContainsValue**  | **Default**       | **2**                |     **9,210.468 ns** |     **155.1949 ns** |     **353.4574 ns** |     **9,311.592 ns** | **1.001** |    **0.05** |      **-** |      **-** |      **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 2                |        25.934 ns |       0.3448 ns |       0.3056 ns |        26.032 ns | 0.003 |    0.00 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **ContainsValue**  | **Default**       | **4**                |     **6,266.955 ns** |     **125.2682 ns** |     **247.2671 ns** |     **6,230.542 ns** | **1.001** |    **0.05** | **0.0125** |      **-** |      **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 4                |        29.540 ns |       0.4435 ns |       0.4148 ns |        29.554 ns | 0.005 |    0.00 | 0.0000 |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **ContainsValue**  | **Default**       | **8**                |     **5,109.327 ns** |      **45.1092 ns** |      **39.9881 ns** |     **5,111.910 ns** | **1.000** |    **0.01** | **0.0100** |      **-** |      **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 8                |        44.380 ns |       0.3765 ns |       0.3338 ns |        44.335 ns | 0.009 |    0.00 | 0.0000 |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **ContainsValue**  | **Default**       | **16**               |     **6,573.926 ns** |      **20.3742 ns** |      **18.0612 ns** |     **6,573.633 ns** | **1.000** |    **0.00** | **0.0125** |      **-** |      **56 B** |        **1.00** |
| ContainsValue  | Bidirectional | 16               |        43.003 ns |       0.2988 ns |       0.2795 ns |        43.077 ns | 0.007 |    0.00 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **FindKeyByValue** | **Default**       | **1**                |    **16,775.948 ns** |     **293.3928 ns** |     **288.1509 ns** |    **16,694.765 ns** | **1.000** |    **0.02** |      **-** |      **-** |      **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 1                |        28.548 ns |       0.1149 ns |       0.1019 ns |        28.531 ns | 0.002 |    0.00 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **FindKeyByValue** | **Default**       | **2**                |     **8,665.825 ns** |     **171.8559 ns** |     **287.1326 ns** |     **8,575.844 ns** | **1.001** |    **0.05** |      **-** |      **-** |      **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 2                |        25.228 ns |       0.3749 ns |       0.3323 ns |        25.155 ns | 0.003 |    0.00 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **FindKeyByValue** | **Default**       | **4**                |     **6,455.983 ns** |     **127.8776 ns** |     **293.8200 ns** |     **6,473.796 ns** | **1.002** |    **0.07** | **0.0111** |      **-** |      **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 4                |        29.645 ns |       0.5916 ns |       0.8294 ns |        29.466 ns | 0.005 |    0.00 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **FindKeyByValue** | **Default**       | **8**                |     **6,281.156 ns** |      **45.3482 ns** |      **42.4187 ns** |     **6,283.309 ns** | **1.000** |    **0.01** | **0.0125** |      **-** |      **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 8                |        44.699 ns |       0.5280 ns |       0.4939 ns |        44.681 ns | 0.007 |    0.00 | 0.0000 |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **FindKeyByValue** | **Default**       | **16**               |     **7,149.595 ns** |     **131.5436 ns** |     **116.6100 ns** |     **7,123.559 ns** | **1.000** |    **0.02** | **0.0100** |      **-** |      **56 B** |        **1.00** |
| FindKeyByValue | Bidirectional | 16               |        43.039 ns |       0.1982 ns |       0.1757 ns |        43.048 ns | 0.006 |    0.00 | 0.0001 |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryAdd**         | **Default**       | **1**                |        **90.761 ns** |       **1.7950 ns** |       **2.8987 ns** |        **90.211 ns** |  **1.00** |    **0.04** | **0.0060** | **0.0030** |      **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 1                |        44.525 ns |       0.8537 ns |       1.7245 ns |        44.112 ns |  0.49 |    0.02 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryAdd**         | **Default**       | **2**                |       **133.182 ns** |       **2.5669 ns** |       **3.5135 ns** |       **132.871 ns** |  **1.00** |    **0.04** | **0.0060** | **0.0030** |      **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 2                |        77.437 ns |       1.5444 ns |       4.0955 ns |        77.263 ns |  0.58 |    0.03 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryAdd**         | **Default**       | **4**                |       **112.014 ns** |       **2.1645 ns** |       **2.5767 ns** |       **111.486 ns** |  **1.00** |    **0.03** | **0.0060** | **0.0030** |      **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 4                |       128.962 ns |       6.1216 ns |      18.0496 ns |       120.009 ns |  1.15 |    0.16 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryAdd**         | **Default**       | **8**                |        **91.730 ns** |       **1.8304 ns** |       **2.6830 ns** |        **91.422 ns** |  **1.00** |    **0.04** | **0.0060** | **0.0030** |      **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 8                |       251.825 ns |       7.9052 ns |      23.3086 ns |       243.649 ns |  2.75 |    0.27 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryAdd**         | **Default**       | **16**               |        **91.324 ns** |       **1.8079 ns** |       **3.9301 ns** |        **90.338 ns** |  **1.00** |    **0.06** | **0.0060** | **0.0030** |      **40 B** |        **1.00** |
| TryAdd         | Bidirectional | 16               |       277.934 ns |       7.2973 ns |      21.5162 ns |       281.706 ns |  3.05 |    0.27 |      - |      - |         - |        0.00 |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryGetValue**    | **Default**       | **1**                |         **5.750 ns** |       **0.0416 ns** |       **0.0348 ns** |         **5.740 ns** |  **1.00** |    **0.01** | **0.0000** |      **-** |         **-** |          **NA** |
| TryGetValue    | Bidirectional | 1                |        23.015 ns |       0.0835 ns |       0.0781 ns |        23.025 ns |  4.00 |    0.03 | 0.0000 |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryGetValue**    | **Default**       | **2**                |         **3.362 ns** |       **0.0506 ns** |       **0.0473 ns** |         **3.352 ns** |  **1.00** |    **0.02** | **0.0000** |      **-** |         **-** |          **NA** |
| TryGetValue    | Bidirectional | 2                |        23.940 ns |       0.3176 ns |       0.2971 ns |        23.952 ns |  7.12 |    0.13 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryGetValue**    | **Default**       | **4**                |         **2.815 ns** |       **0.0219 ns** |       **0.0183 ns** |         **2.814 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |         **-** |          **NA** |
| TryGetValue    | Bidirectional | 4                |        28.962 ns |       0.3773 ns |       0.3529 ns |        28.883 ns | 10.29 |    0.14 | 0.0000 |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryGetValue**    | **Default**       | **8**                |         **2.648 ns** |       **0.0523 ns** |       **0.0917 ns** |         **2.604 ns** |  **1.00** |    **0.05** | **0.0001** |      **-** |         **-** |          **NA** |
| TryGetValue    | Bidirectional | 8                |        39.397 ns |       0.1505 ns |       0.1256 ns |        39.371 ns | 14.89 |    0.50 | 0.0000 |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryGetValue**    | **Default**       | **16**               |         **3.095 ns** |       **0.0337 ns** |       **0.0316 ns** |         **3.092 ns** |  **1.00** |    **0.01** | **0.0001** |      **-** |         **-** |          **NA** |
| TryGetValue    | Bidirectional | 16               |        42.514 ns |       0.3208 ns |       0.2844 ns |        42.523 ns | 13.74 |    0.16 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryRemove**      | **Default**       | **1**                |        **30.775 ns** |       **0.5742 ns** |       **0.5090 ns** |        **30.860 ns** |  **1.00** |    **0.02** |      **-** |      **-** |         **-** |          **NA** |
| TryRemove      | Bidirectional | 1                |        47.479 ns |       1.0984 ns |       3.1515 ns |        47.534 ns |  1.54 |    0.11 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryRemove**      | **Default**       | **2**                |        **65.792 ns** |       **2.3536 ns** |       **6.9398 ns** |        **67.403 ns** |  **1.01** |    **0.18** |      **-** |      **-** |         **-** |          **NA** |
| TryRemove      | Bidirectional | 2                |        88.564 ns |       3.3745 ns |       9.8969 ns |        86.770 ns |  1.37 |    0.25 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryRemove**      | **Default**       | **4**                |        **59.144 ns** |       **1.3839 ns** |       **4.0804 ns** |        **58.809 ns** |  **1.00** |    **0.10** |      **-** |      **-** |         **-** |          **NA** |
| TryRemove      | Bidirectional | 4                |       129.129 ns |       4.3652 ns |      12.8710 ns |       126.279 ns |  2.19 |    0.27 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryRemove**      | **Default**       | **8**                |        **36.591 ns** |       **0.7274 ns** |       **1.5501 ns** |        **36.340 ns** |  **1.00** |    **0.06** |      **-** |      **-** |         **-** |          **NA** |
| TryRemove      | Bidirectional | 8                |       255.578 ns |       8.0643 ns |      23.3961 ns |       255.041 ns |  7.00 |    0.70 |      - |      - |         - |          NA |
|                |               |                  |                  |                 |                 |                  |       |         |        |        |           |             |
| **TryRemove**      | **Default**       | **16**               |        **35.837 ns** |       **0.7018 ns** |       **0.6565 ns** |        **35.814 ns** |  **1.00** |    **0.03** |      **-** |      **-** |         **-** |          **NA** |
| TryRemove      | Bidirectional | 16               |       288.083 ns |      15.2591 ns |      44.9918 ns |       294.757 ns |  8.04 |    1.26 |      - |      - |         - |          NA |
