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
