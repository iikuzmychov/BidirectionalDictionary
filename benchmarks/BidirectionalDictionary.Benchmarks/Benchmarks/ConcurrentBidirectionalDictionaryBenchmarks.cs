using System.Collections.Concurrent;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace BidirectionalDictionary.Benchmarks.Benchmarks;

[Config(typeof(BenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ConcurrentBidirectionalDictionaryBenchmarks
{
    private KeyValuePair<int, int>[] _pairsForRead = null!;
    private KeyValuePair<int, int>[] _pairsForMutation = null!;
    private ParallelOptions _parallelOptions = null!;

    private ConcurrentDictionary<int, int> _dictionaryForRead = null!;
    private ConcurrentBidirectionalDictionary<int, int> _bidirectionalDictionaryForRead = null!;

    private ConcurrentDictionary<int, int> _dictionaryForMutation = null!;
    private ConcurrentBidirectionalDictionary<int, int> _bidirectionalDictionaryForMutation = null!;

    private int _hitKey;
    private int _hitValue;

    [Params(1, 2, 4, 8, 16)]
    public int ConcurrencyLevel { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _pairsForRead = BenchmarkData.GenerateSource(BenchmarkConstants.ReadPairCount);
        _pairsForMutation = BenchmarkData.GenerateSource(BenchmarkConstants.MutationPairCount);
        _parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = ConcurrencyLevel };

        _dictionaryForRead = CreateConcurrentDictionary(_pairsForRead);
        _bidirectionalDictionaryForRead = CreateConcurrentBidirectionalDictionary(_pairsForRead);

        var hitIndex = BenchmarkConstants.ReadPairCount / 2;
        _hitKey = _pairsForRead[hitIndex].Key;
        _hitValue = _pairsForRead[hitIndex].Value;
    }

    [IterationSetup(Targets = new[]
    {
        nameof(TryAdd_Default),
        nameof(TryAdd_Bidirectional),
    })]
    public void SetupEmpty()
    {
        _dictionaryForMutation = new ConcurrentDictionary<int, int>(ConcurrencyLevel, BenchmarkConstants.MutationPairCount);
        _bidirectionalDictionaryForMutation = new ConcurrentBidirectionalDictionary<int, int>(ConcurrencyLevel, BenchmarkConstants.MutationPairCount);
    }

    [IterationSetup(Targets = new[]
    {
        nameof(TryRemove_Default),
        nameof(TryRemove_Bidirectional),
        nameof(AddOrUpdate_Default),
        nameof(AddOrUpdate_Bidirectional),
        nameof(Clear_Default),
        nameof(Clear_Bidirectional),
    })]
    public void SetupPopulated()
    {
        _dictionaryForMutation = CreateConcurrentDictionary(_pairsForMutation);
        _bidirectionalDictionaryForMutation = CreateConcurrentBidirectionalDictionary(_pairsForMutation);
    }

    // --- Lookup ---

    [BenchmarkCategory("TryGetValue"), Benchmark(Baseline = true, OperationsPerInvoke = BenchmarkConstants.ReadPairCount)]
    public int TryGetValue_Default() => CountInParallel(i => _dictionaryForRead.TryGetValue(_pairsForRead[i].Key, out _), BenchmarkConstants.ReadPairCount);

    [BenchmarkCategory("TryGetValue"), Benchmark(OperationsPerInvoke = BenchmarkConstants.ReadPairCount)]
    public int TryGetValue_Bidirectional() => CountInParallel(i => _bidirectionalDictionaryForRead.TryGetValue(_pairsForRead[i].Key, out _), BenchmarkConstants.ReadPairCount);

    [BenchmarkCategory("ContainsKey"), Benchmark(Baseline = true, OperationsPerInvoke = BenchmarkConstants.ReadPairCount)]
    public int ContainsKey_Default() => CountInParallel(i => _dictionaryForRead.ContainsKey(_pairsForRead[i].Key), BenchmarkConstants.ReadPairCount);

    [BenchmarkCategory("ContainsKey"), Benchmark(OperationsPerInvoke = BenchmarkConstants.ReadPairCount)]
    public int ContainsKey_Bidirectional() => CountInParallel(i => _bidirectionalDictionaryForRead.ContainsKey(_pairsForRead[i].Key), BenchmarkConstants.ReadPairCount);

    // --- Reverse lookup ---

    [BenchmarkCategory("ContainsValue"), Benchmark(Baseline = true, OperationsPerInvoke = BenchmarkConstants.ReadPairCount)]
    public int ContainsValue_Default() => CountInParallel(i => ContainsValue(_dictionaryForRead, _pairsForRead[i].Value), BenchmarkConstants.ReadPairCount);

    [BenchmarkCategory("ContainsValue"), Benchmark(OperationsPerInvoke = BenchmarkConstants.ReadPairCount)]
    public int ContainsValue_Bidirectional() => CountInParallel(i => _bidirectionalDictionaryForRead.ContainsValue(_pairsForRead[i].Value), BenchmarkConstants.ReadPairCount);

    [BenchmarkCategory("FindKeyByValue"), Benchmark(Baseline = true, OperationsPerInvoke = BenchmarkConstants.ReadPairCount)]
    public int FindKeyByValue_Default() => CountInParallel(i => TryFindKeyByValue(_dictionaryForRead, _pairsForRead[i].Value, out _), BenchmarkConstants.ReadPairCount);

    [BenchmarkCategory("FindKeyByValue"), Benchmark(OperationsPerInvoke = BenchmarkConstants.ReadPairCount)]
    public int FindKeyByValue_Bidirectional() => CountInParallel(i => _bidirectionalDictionaryForRead.Inverse.TryGetValue(_pairsForRead[i].Value, out _), BenchmarkConstants.ReadPairCount);

    // --- Mutation ---

    [BenchmarkCategory("TryAdd"), Benchmark(Baseline = true, OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public int TryAdd_Default() => CountInParallel(i => _dictionaryForMutation.TryAdd(_pairsForMutation[i].Key, _pairsForMutation[i].Value), BenchmarkConstants.MutationPairCount);

    [BenchmarkCategory("TryAdd"), Benchmark(OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public int TryAdd_Bidirectional() => CountInParallel(i => _bidirectionalDictionaryForMutation.TryAdd(_pairsForMutation[i].Key, _pairsForMutation[i].Value), BenchmarkConstants.MutationPairCount);

    [BenchmarkCategory("TryRemove"), Benchmark(Baseline = true, OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public int TryRemove_Default() => CountInParallel(i => _dictionaryForMutation.TryRemove(_pairsForMutation[i].Key, out _), BenchmarkConstants.MutationPairCount);

    [BenchmarkCategory("TryRemove"), Benchmark(OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public int TryRemove_Bidirectional() => CountInParallel(i => _bidirectionalDictionaryForMutation.TryRemove(_pairsForMutation[i].Key, out _), BenchmarkConstants.MutationPairCount);

    [BenchmarkCategory("AddOrUpdate"), Benchmark(Baseline = true, OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public int AddOrUpdate_Default() => CountInParallel(i =>
    {
        var key = _pairsForMutation[i].Key;
        _dictionaryForMutation.AddOrUpdate(key, UniqueReplacementValue(key), (_, _) => UniqueReplacementValue(key));
        return true;
    }, BenchmarkConstants.MutationPairCount);

    [BenchmarkCategory("AddOrUpdate"), Benchmark(OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public int AddOrUpdate_Bidirectional() => CountInParallel(i =>
    {
        var key = _pairsForMutation[i].Key;
        _bidirectionalDictionaryForMutation.AddOrUpdate(key, UniqueReplacementValue(key), (_, _) => UniqueReplacementValue(key));
        return true;
    }, BenchmarkConstants.MutationPairCount);

    [BenchmarkCategory("Clear"), Benchmark(Baseline = true)]
    public void Clear_Default() => _dictionaryForMutation.Clear();

    [BenchmarkCategory("Clear"), Benchmark]
    public void Clear_Bidirectional() => _bidirectionalDictionaryForMutation.Clear();

    private int CountInParallel(Func<int, bool> action, int count)
    {
        var matches = 0;

        Parallel.For(
            0,
            count,
            _parallelOptions,
            () => 0,
            (i, _, localMatches) => action(i) ? localMatches + 1 : localMatches,
            localMatches => Interlocked.Add(ref matches, localMatches));

        return matches;
    }

    private ConcurrentDictionary<int, int> CreateConcurrentDictionary(KeyValuePair<int, int>[] source)
    {
        var dictionary = new ConcurrentDictionary<int, int>(ConcurrencyLevel, source.Length);

        foreach (var pair in source)
        {
            dictionary.TryAdd(pair.Key, pair.Value);
        }

        return dictionary;
    }

    private ConcurrentBidirectionalDictionary<int, int> CreateConcurrentBidirectionalDictionary(KeyValuePair<int, int>[] source)
    {
        return new ConcurrentBidirectionalDictionary<int, int>(ConcurrencyLevel, source, keyComparer: null, valueComparer: null);
    }

    private static bool ContainsValue(ConcurrentDictionary<int, int> dictionary, int value)
    {
        foreach (var pair in dictionary)
        {
            if (pair.Value == value)
            {
                return true;
            }
        }

        return false;
    }

    private static bool TryFindKeyByValue(ConcurrentDictionary<int, int> dictionary, int value, out int key)
    {
        foreach (var pair in dictionary)
        {
            if (pair.Value == value)
            {
                key = pair.Key;
                return true;
            }
        }

        key = default;
        return false;
    }

    private static int UniqueReplacementValue(int key) => -key - 1;
}
