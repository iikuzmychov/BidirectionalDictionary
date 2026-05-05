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
  Job-JMUINI : .NET 10.0.7 (10.0.726.21808), X64 RyuJIT AVX2
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
| Add             | Default       |       6.997 ns |     0.6291 ns |     1.8550 ns |       6.332 ns |  1.06 |    0.38 |       - |       - |       - |         - |          NA |
| Add             | Bidirectional |      25.174 ns |     0.7124 ns |     2.0094 ns |      24.917 ns |  3.83 |    0.94 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| ContainsKeyHit  | Default       |       3.678 ns |     0.0378 ns |     0.0295 ns |       3.675 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| ContainsKeyHit  | Bidirectional |       3.887 ns |     0.0546 ns |     0.0456 ns |       3.897 ns |  1.06 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| ContainsKeyMiss | Default       |       3.514 ns |     0.0686 ns |     0.0608 ns |       3.492 ns |  1.00 |    0.02 |       - |       - |       - |         - |          NA |
| ContainsKeyMiss | Bidirectional |       3.601 ns |     0.0630 ns |     0.0526 ns |       3.604 ns |  1.03 |    0.02 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| ContainsValue   | Default       |   5,145.282 ns |    33.2324 ns |    25.9457 ns |   5,150.268 ns | 1.000 |    0.01 |       - |       - |       - |         - |          NA |
| ContainsValue   | Bidirectional |       4.166 ns |     0.0765 ns |     0.0639 ns |       4.156 ns | 0.001 |    0.00 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Empty           | Default       |       6.248 ns |     0.1898 ns |     0.4511 ns |       6.050 ns |  1.00 |    0.10 |  0.0191 |       - |       - |      80 B |        1.00 |
| Empty           | Bidirectional |      27.346 ns |     0.5905 ns |     1.2836 ns |      26.832 ns |  4.40 |    0.36 |  0.0573 |       - |       - |     240 B |        3.00 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| FindKeyByValue  | Default       |   5,149.664 ns |    48.1100 ns |    37.5611 ns |   5,140.184 ns | 1.000 |    0.01 |       - |       - |       - |         - |          NA |
| FindKeyByValue  | Bidirectional |       4.033 ns |     0.0633 ns |     0.0592 ns |       4.020 ns | 0.001 |    0.00 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| FromSequence    | Default       |  73,674.701 ns | 1,383.1084 ns | 1,537.3213 ns |  74,020.874 ns |  1.00 |    0.03 | 30.6396 | 20.8740 | 20.8740 |  202316 B |        1.00 |
| FromSequence    | Bidirectional | 158,677.951 ns | 3,154.2208 ns | 6,857.0169 ns | 156,957.251 ns |  2.15 |    0.10 | 59.5703 | 40.2832 | 40.2832 |  404694 B |        2.00 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| IndexerHit      | Default       |       3.416 ns |     0.0430 ns |     0.0359 ns |       3.428 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| IndexerHit      | Bidirectional |       3.680 ns |     0.0627 ns |     0.0556 ns |       3.667 ns |  1.08 |    0.02 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Keys            | Default       |   9,727.489 ns |    57.8310 ns |    48.2915 ns |   9,718.990 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| Keys            | Bidirectional |   9,826.928 ns |    52.6147 ns |    41.0781 ns |   9,831.872 ns |  1.01 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| LinqProjection  | Default       |  89,900.616 ns | 1,686.6362 ns | 1,495.1591 ns |  89,855.768 ns |  1.00 |    0.02 | 30.1514 | 20.5078 | 20.5078 |  202291 B |        1.00 |
| LinqProjection  | Bidirectional | 157,824.470 ns | 3,143.1511 ns | 5,826.0326 ns | 155,914.355 ns |  1.76 |    0.07 | 59.8145 | 40.5273 | 40.5273 |  404700 B |        2.00 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Pairs           | Default       |  25,858.582 ns |   159.8450 ns |   133.4778 ns |  25,865.454 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| Pairs           | Bidirectional |  23,644.336 ns |   461.7509 ns |   567.0716 ns |  23,374.751 ns |  0.91 |    0.02 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| PreSized        | Default       |  18,446.031 ns |   357.6613 ns | 1,014.6258 ns |  18,758.801 ns |  1.00 |    0.08 | 28.8696 | 19.1040 | 19.1040 |  202333 B |        1.00 |
| PreSized        | Bidirectional |  37,781.868 ns |   538.8773 ns |   504.0662 ns |  37,843.091 ns |  2.05 |    0.12 | 56.7017 | 37.1704 | 37.1704 |  404727 B |        2.00 |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Remove          | Default       |       5.820 ns |     0.1090 ns |     0.0910 ns |       5.795 ns |  1.00 |    0.02 |       - |       - |       - |         - |          NA |
| Remove          | Bidirectional |      26.178 ns |     1.5659 ns |     4.5678 ns |      24.389 ns |  4.50 |    0.78 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| SetOverwrite    | Default       |       4.380 ns |     0.1057 ns |     0.3068 ns |       4.311 ns |  1.00 |    0.10 |       - |       - |       - |         - |          NA |
| SetOverwrite    | Bidirectional |     109.088 ns |     1.4945 ns |     1.3248 ns |     109.280 ns | 25.03 |    1.72 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| TryAdd          | Default       |       6.460 ns |     0.3990 ns |     1.1765 ns |       5.891 ns |  1.03 |    0.26 |       - |       - |       - |         - |          NA |
| TryAdd          | Bidirectional |      24.544 ns |     0.7747 ns |     2.2476 ns |      24.257 ns |  3.92 |    0.75 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| TryGetValueHit  | Default       |       3.319 ns |     0.0361 ns |     0.0320 ns |       3.326 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| TryGetValueHit  | Bidirectional |       3.549 ns |     0.0598 ns |     0.0499 ns |       3.547 ns |  1.07 |    0.02 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| TryGetValueMiss | Default       |       3.190 ns |     0.0316 ns |     0.0280 ns |       3.199 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| TryGetValueMiss | Bidirectional |       3.316 ns |     0.0445 ns |     0.0372 ns |       3.315 ns |  1.04 |    0.01 |       - |       - |       - |         - |          NA |
|                 |               |                |               |               |                |       |         |         |         |         |           |             |
| Values          | Default       |   9,716.741 ns |    84.0708 ns |    74.5266 ns |   9,720.512 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |
| Values          | Bidirectional |   9,755.361 ns |    78.9373 ns |    69.9758 ns |   9,767.499 ns |  1.00 |    0.01 |       - |       - |       - |         - |          NA |

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
| ContainsKeyHit  | Default       |      4.302 ns |     0.0541 ns |     0.0452 ns |      4.305 ns |  1.00 |    0.01 |       - |         - |          NA |
| ContainsKeyHit  | Bidirectional |      4.537 ns |     0.0326 ns |     0.0305 ns |      4.542 ns |  1.05 |    0.01 |       - |         - |          NA |
|                 |               |               |               |               |               |       |         |         |           |             |
| ContainsKeyMiss | Default       |      4.068 ns |     0.0332 ns |     0.0277 ns |      4.064 ns |  1.00 |    0.01 |       - |         - |          NA |
| ContainsKeyMiss | Bidirectional |      4.325 ns |     0.0580 ns |     0.0514 ns |      4.330 ns |  1.06 |    0.01 |       - |         - |          NA |
|                 |               |               |               |               |               |       |         |         |           |             |
| ContainsValue   | Default       | 30,887.824 ns |   222.4539 ns |   185.7591 ns | 30,959.253 ns | 1.000 |    0.01 |       - |      48 B |        1.00 |
| ContainsValue   | Bidirectional |      5.154 ns |     0.0480 ns |     0.0425 ns |      5.160 ns | 0.000 |    0.00 |       - |         - |        0.00 |
|                 |               |               |               |               |               |       |         |         |           |             |
| FindKeyByValue  | Default       | 30,958.448 ns |   136.4123 ns |   120.9259 ns | 30,981.006 ns | 1.000 |    0.01 |       - |      48 B |        1.00 |
| FindKeyByValue  | Bidirectional |      4.695 ns |     0.0385 ns |     0.0321 ns |      4.709 ns | 0.000 |    0.00 |       - |         - |        0.00 |
|                 |               |               |               |               |               |       |         |         |           |             |
| IndexerHit      | Default       |      4.049 ns |     0.0801 ns |     0.0669 ns |      4.047 ns |  1.00 |    0.02 |       - |         - |          NA |
| IndexerHit      | Bidirectional |      4.218 ns |     0.0444 ns |     0.0371 ns |      4.216 ns |  1.04 |    0.02 |       - |         - |          NA |
|                 |               |               |               |               |               |       |         |         |           |             |
| Keys            | Default       | 20,958.546 ns |   186.3042 ns |   174.2691 ns | 21,002.844 ns |  1.00 |    0.01 |       - |      40 B |        1.00 |
| Keys            | Bidirectional | 10,458.812 ns |   131.5364 ns |   116.6036 ns | 10,461.401 ns |  0.50 |    0.01 |       - |         - |        0.00 |
|                 |               |               |               |               |               |       |         |         |           |             |
| Pairs           | Default       | 64,910.764 ns |   233.2534 ns |   218.1853 ns | 64,914.001 ns |  1.00 |    0.00 |       - |      48 B |        1.00 |
| Pairs           | Bidirectional | 86,936.498 ns | 1,713.4845 ns | 1,602.7944 ns | 86,699.817 ns |  1.34 |    0.02 | 57.3730 |  240048 B |    5,001.00 |
|                 |               |               |               |               |               |       |         |         |           |             |
| TryGetValueHit  | Default       |      4.165 ns |     0.0441 ns |     0.0391 ns |      4.165 ns |  1.00 |    0.01 |       - |         - |          NA |
| TryGetValueHit  | Bidirectional |      4.548 ns |     0.1189 ns |     0.2402 ns |      4.436 ns |  1.09 |    0.06 |       - |         - |          NA |
|                 |               |               |               |               |               |       |         |         |           |             |
| TryGetValueMiss | Default       |      4.047 ns |     0.0773 ns |     0.0645 ns |      4.056 ns |  1.00 |    0.02 |       - |         - |          NA |
| TryGetValueMiss | Bidirectional |      4.135 ns |     0.0657 ns |     0.0582 ns |      4.157 ns |  1.02 |    0.02 |       - |         - |          NA |
|                 |               |               |               |               |               |       |         |         |           |             |
| Values          | Default       | 21,503.964 ns |   426.3061 ns |   377.9092 ns | 21,470.067 ns |  1.00 |    0.02 |       - |      40 B |        1.00 |
| Values          | Bidirectional | 10,974.691 ns |   218.4323 ns |   415.5901 ns | 10,777.277 ns |  0.51 |    0.02 |       - |         - |        0.00 |
|                 |               |               |               |               |               |       |         |         |           |             |
| Wrap            | Default       |      6.824 ns |     0.2476 ns |     0.7065 ns |      6.715 ns |  1.01 |    0.14 |  0.0096 |      40 B |        1.00 |
| Wrap            | Bidirectional |     14.656 ns |     0.5144 ns |     1.4592 ns |     14.397 ns |  2.17 |    0.30 |  0.0153 |      64 B |        1.60 |
