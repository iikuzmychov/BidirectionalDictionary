using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;

namespace BidirectionalDictionary.Benchmarks.Benchmarks;

[Config(typeof(BenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class BidirectionalDictionaryBenchmarks
{
    private readonly Consumer _consumer = new();

    private KeyValuePair<int, int>[] _pairsForRead = null!;
    private KeyValuePair<int, int>[] _pairsForMutation = null!;

    private Dictionary<int, int> _dictionaryForRead = null!;
    private BidirectionalDictionary<int, int> _bidirectionalDictionaryForRead = null!;

    private Dictionary<int, int> _dictionaryForMutation = null!;
    private BidirectionalDictionary<int, int> _bidirectionalDictionaryForMutation = null!;

    private int _hitKey;
    private int _hitValue;

    [GlobalSetup]
    public void Setup()
    {
        _pairsForRead = BenchmarkData.GenerateSource(BenchmarkConstants.ReadPairCount);
        _pairsForMutation = BenchmarkData.GenerateSource(BenchmarkConstants.MutationPairCount);

        _dictionaryForRead = BenchmarkData.CreateDictionary(_pairsForRead);
        _bidirectionalDictionaryForRead = new BidirectionalDictionary<int, int>(_dictionaryForRead);

        var hitIndex = BenchmarkConstants.ReadPairCount / 2;
        _hitKey = _pairsForRead[hitIndex].Key;
        _hitValue = _pairsForRead[hitIndex].Value;
    }

    [IterationSetup(Targets = new[]
    {
        nameof(Add_Default),
        nameof(Add_Bidirectional),
        nameof(TryAdd_Default),
        nameof(TryAdd_Bidirectional),
    })]
    public void SetupEmpty()
    {
        _dictionaryForMutation = new Dictionary<int, int>(BenchmarkConstants.MutationPairCount);
        _bidirectionalDictionaryForMutation = new BidirectionalDictionary<int, int>(BenchmarkConstants.MutationPairCount);
    }

    [IterationSetup(Targets = new[]
    {
        nameof(SetOverwrite_Default),
        nameof(SetOverwrite_Bidirectional),
        nameof(Remove_Default),
        nameof(Remove_Bidirectional),
    })]
    public void SetupPopulated()
    {
        _dictionaryForMutation = new Dictionary<int, int>(_pairsForMutation);
        _bidirectionalDictionaryForMutation = new BidirectionalDictionary<int, int>(_pairsForMutation);
    }

    // --- Lookup ---

    [BenchmarkCategory("IndexerHit"), Benchmark(Baseline = true)]
    public int IndexerHit_Default() => _dictionaryForRead[_hitKey];

    [BenchmarkCategory("IndexerHit"), Benchmark]
    public int IndexerHit_Bidirectional() => _bidirectionalDictionaryForRead[_hitKey];

    [BenchmarkCategory("TryGetValueHit"), Benchmark(Baseline = true)]
    public bool TryGetValueHit_Default() => _dictionaryForRead.TryGetValue(_hitKey, out _);

    [BenchmarkCategory("TryGetValueHit"), Benchmark]
    public bool TryGetValueHit_Bidirectional() => _bidirectionalDictionaryForRead.TryGetValue(_hitKey, out _);

    [BenchmarkCategory("TryGetValueMiss"), Benchmark(Baseline = true)]
    public bool TryGetValueMiss_Default() => _dictionaryForRead.TryGetValue(BenchmarkConstants.MissingKey, out _);

    [BenchmarkCategory("TryGetValueMiss"), Benchmark]
    public bool TryGetValueMiss_Bidirectional() => _bidirectionalDictionaryForRead.TryGetValue(BenchmarkConstants.MissingKey, out _);

    [BenchmarkCategory("ContainsKeyHit"), Benchmark(Baseline = true)]
    public bool ContainsKeyHit_Default() => _dictionaryForRead.ContainsKey(_hitKey);

    [BenchmarkCategory("ContainsKeyHit"), Benchmark]
    public bool ContainsKeyHit_Bidirectional() => _bidirectionalDictionaryForRead.ContainsKey(_hitKey);

    [BenchmarkCategory("ContainsKeyMiss"), Benchmark(Baseline = true)]
    public bool ContainsKeyMiss_Default() => _dictionaryForRead.ContainsKey(BenchmarkConstants.MissingKey);

    [BenchmarkCategory("ContainsKeyMiss"), Benchmark]
    public bool ContainsKeyMiss_Bidirectional() => _bidirectionalDictionaryForRead.ContainsKey(BenchmarkConstants.MissingKey);

    // --- Reverse lookup ---

    [BenchmarkCategory("ContainsValue"), Benchmark(Baseline = true)]
    public bool ContainsValue_Default() => _dictionaryForRead.ContainsValue(_hitValue);

    [BenchmarkCategory("ContainsValue"), Benchmark]
    public bool ContainsValue_Bidirectional() => _bidirectionalDictionaryForRead.ContainsValue(_hitValue);

    [BenchmarkCategory("FindKeyByValue"), Benchmark(Baseline = true)]
    public int FindKeyByValue_Default()
    {
        var target = _hitValue;

        foreach (var pair in _dictionaryForRead)
        {
            if (pair.Value == target)
            {
                return pair.Key;
            }
        }

        throw new KeyNotFoundException();
    }

    [BenchmarkCategory("FindKeyByValue"), Benchmark]
    public int FindKeyByValue_Bidirectional() => _bidirectionalDictionaryForRead.Inverse[_hitValue];

    // --- Enumeration ---

    [BenchmarkCategory("Pairs"), Benchmark(Baseline = true)]
    public void Pairs_Default()
    {
        foreach (var pair in _dictionaryForRead)
        {
            _consumer.Consume(pair);
        }
    }

    [BenchmarkCategory("Pairs"), Benchmark]
    public void Pairs_Bidirectional()
    {
        foreach (var pair in _bidirectionalDictionaryForRead)
        {
            _consumer.Consume(pair);
        }
    }

    [BenchmarkCategory("Keys"), Benchmark(Baseline = true)]
    public void Keys_Default()
    {
        foreach (var key in _dictionaryForRead.Keys)
        {
            _consumer.Consume(key);
        }
    }

    [BenchmarkCategory("Keys"), Benchmark]
    public void Keys_Bidirectional()
    {
        foreach (var key in _bidirectionalDictionaryForRead.Keys)
        {
            _consumer.Consume(key);
        }
    }

    [BenchmarkCategory("Values"), Benchmark(Baseline = true)]
    public void Values_Default()
    {
        foreach (var value in _dictionaryForRead.Values)
        {
            _consumer.Consume(value);
        }
    }

    [BenchmarkCategory("Values"), Benchmark]
    public void Values_Bidirectional()
    {
        foreach (var value in _bidirectionalDictionaryForRead.Values)
        {
            _consumer.Consume(value);
        }
    }

    // --- Construction ---

    [BenchmarkCategory("Empty"), Benchmark(Baseline = true)]
    public Dictionary<int, int> Empty_Default() => new();

    [BenchmarkCategory("Empty"), Benchmark]
    public BidirectionalDictionary<int, int> Empty_Bidirectional() => new();

    [BenchmarkCategory("PreSized"), Benchmark(Baseline = true)]
    public Dictionary<int, int> PreSized_Default() => new(BenchmarkConstants.ReadPairCount);

    [BenchmarkCategory("PreSized"), Benchmark]
    public BidirectionalDictionary<int, int> PreSized_Bidirectional() => new(BenchmarkConstants.ReadPairCount);

    [BenchmarkCategory("FromSequence"), Benchmark(Baseline = true)]
    public Dictionary<int, int> FromSequence_Default() => new(_pairsForRead);

    [BenchmarkCategory("FromSequence"), Benchmark]
    public BidirectionalDictionary<int, int> FromSequence_Bidirectional() => new(_pairsForRead);

    [BenchmarkCategory("LinqProjection"), Benchmark(Baseline = true)]
    public Dictionary<int, int> LinqProjection_Default() => _pairsForRead.ToDictionary(p => p.Key, p => p.Value);

    [BenchmarkCategory("LinqProjection"), Benchmark]
    public BidirectionalDictionary<int, int> LinqProjection_Bidirectional() => _pairsForRead.ToBidirectionalDictionary();

    // --- Mutation ---

    [BenchmarkCategory("Add"), Benchmark(Baseline = true, OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public void Add_Default()
    {
        foreach (var pair in _pairsForMutation)
        {
            _dictionaryForMutation.Add(pair.Key, pair.Value);
        }
    }

    [BenchmarkCategory("Add"), Benchmark(OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public void Add_Bidirectional()
    {
        foreach (var pair in _pairsForMutation)
        {
            _bidirectionalDictionaryForMutation.Add(pair.Key, pair.Value);
        }
    }

    [BenchmarkCategory("TryAdd"), Benchmark(Baseline = true, OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public int TryAdd_Default()
    {
        var added = 0;

        foreach (var pair in _pairsForMutation)
        {
            if (_dictionaryForMutation.TryAdd(pair.Key, pair.Value))
            {
                added++;
            }
        }

        return added;
    }

    [BenchmarkCategory("TryAdd"), Benchmark(OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public int TryAdd_Bidirectional()
    {
        var added = 0;

        foreach (var pair in _pairsForMutation)
        {
            if (_bidirectionalDictionaryForMutation.TryAdd(pair.Key, pair.Value))
            {
                added++;
            }
        }

        return added;
    }

    [BenchmarkCategory("SetOverwrite"), Benchmark(Baseline = true, OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public void SetOverwrite_Default()
    {
        foreach (var pair in _pairsForMutation)
        {
            _dictionaryForMutation[pair.Key] = UniqueReplacementValue(pair.Key);
        }
    }

    [BenchmarkCategory("SetOverwrite"), Benchmark(OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public void SetOverwrite_Bidirectional()
    {
        foreach (var pair in _pairsForMutation)
        {
            _bidirectionalDictionaryForMutation[pair.Key] = UniqueReplacementValue(pair.Key);
        }
    }

    [BenchmarkCategory("Remove"), Benchmark(Baseline = true, OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public int Remove_Default()
    {
        var removed = 0;

        foreach (var pair in _pairsForMutation)
        {
            if (_dictionaryForMutation.Remove(pair.Key))
            {
                removed++;
            }
        }

        return removed;
    }

    [BenchmarkCategory("Remove"), Benchmark(OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public int Remove_Bidirectional()
    {
        var removed = 0;

        foreach (var pair in _pairsForMutation)
        {
            if (_bidirectionalDictionaryForMutation.Remove(pair.Key))
            {
                removed++;
            }
        }

        return removed;
    }

    private static int UniqueReplacementValue(int key) => -key - 1;
}
