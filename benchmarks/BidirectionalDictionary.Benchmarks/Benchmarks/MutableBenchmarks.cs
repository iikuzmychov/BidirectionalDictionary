using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;

namespace BidirectionalDictionary.Benchmarks.Benchmarks;

[Config(typeof(BenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class MutableBenchmarks
{
    private readonly Consumer _consumer = new();

    private KeyValuePair<int, int>[] _pairsForRead = null!;
    private KeyValuePair<int, int>[] _pairsForMutation = null!;

    private Dictionary<int, int> _defaultForRead = null!;
    private BidirectionalDictionary<int, int> _bidirectionalForRead = null!;

    private Dictionary<int, int> _defaultForMutation = null!;
    private BidirectionalDictionary<int, int> _bidirectionalForMutation = null!;

    private int _hitKey;
    private int _hitValue;

    [GlobalSetup]
    public void Setup()
    {
        _pairsForRead = BenchmarkData.GenerateSource(BenchmarkConstants.ReadPairCount);
        _pairsForMutation = BenchmarkData.GenerateSource(BenchmarkConstants.MutationPairCount);

        _defaultForRead = BenchmarkData.CreateDictionary(_pairsForRead);
        _bidirectionalForRead = new BidirectionalDictionary<int, int>(_defaultForRead);

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
        _defaultForMutation = new Dictionary<int, int>(BenchmarkConstants.MutationPairCount);
        _bidirectionalForMutation = new BidirectionalDictionary<int, int>(BenchmarkConstants.MutationPairCount);
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
        _defaultForMutation = new Dictionary<int, int>(_pairsForMutation);
        _bidirectionalForMutation = new BidirectionalDictionary<int, int>(_pairsForMutation);
    }

    // --- Lookup ---

    [BenchmarkCategory("IndexerHit"), Benchmark(Baseline = true)]
    public int IndexerHit_Default() => _defaultForRead[_hitKey];

    [BenchmarkCategory("IndexerHit"), Benchmark]
    public int IndexerHit_Bidirectional() => _bidirectionalForRead[_hitKey];

    [BenchmarkCategory("TryGetValueHit"), Benchmark(Baseline = true)]
    public bool TryGetValueHit_Default() => _defaultForRead.TryGetValue(_hitKey, out _);

    [BenchmarkCategory("TryGetValueHit"), Benchmark]
    public bool TryGetValueHit_Bidirectional() => _bidirectionalForRead.TryGetValue(_hitKey, out _);

    [BenchmarkCategory("TryGetValueMiss"), Benchmark(Baseline = true)]
    public bool TryGetValueMiss_Default() => _defaultForRead.TryGetValue(BenchmarkConstants.MissingKey, out _);

    [BenchmarkCategory("TryGetValueMiss"), Benchmark]
    public bool TryGetValueMiss_Bidirectional() => _bidirectionalForRead.TryGetValue(BenchmarkConstants.MissingKey, out _);

    [BenchmarkCategory("ContainsKeyHit"), Benchmark(Baseline = true)]
    public bool ContainsKeyHit_Default() => _defaultForRead.ContainsKey(_hitKey);

    [BenchmarkCategory("ContainsKeyHit"), Benchmark]
    public bool ContainsKeyHit_Bidirectional() => _bidirectionalForRead.ContainsKey(_hitKey);

    [BenchmarkCategory("ContainsKeyMiss"), Benchmark(Baseline = true)]
    public bool ContainsKeyMiss_Default() => _defaultForRead.ContainsKey(BenchmarkConstants.MissingKey);

    [BenchmarkCategory("ContainsKeyMiss"), Benchmark]
    public bool ContainsKeyMiss_Bidirectional() => _bidirectionalForRead.ContainsKey(BenchmarkConstants.MissingKey);

    // --- Reverse lookup ---

    [BenchmarkCategory("ContainsValue"), Benchmark(Baseline = true)]
    public bool ContainsValue_Default() => _defaultForRead.ContainsValue(_hitValue);

    [BenchmarkCategory("ContainsValue"), Benchmark]
    public bool ContainsValue_Bidirectional() => _bidirectionalForRead.ContainsValue(_hitValue);

    [BenchmarkCategory("FindKeyByValue"), Benchmark(Baseline = true)]
    public int FindKeyByValue_Default()
    {
        var target = _hitValue;

        foreach (var pair in _defaultForRead)
        {
            if (pair.Value == target)
            {
                return pair.Key;
            }
        }

        throw new KeyNotFoundException();
    }

    [BenchmarkCategory("FindKeyByValue"), Benchmark]
    public int FindKeyByValue_Bidirectional() => _bidirectionalForRead.Inverse[_hitValue];

    // --- Enumeration ---

    [BenchmarkCategory("Pairs"), Benchmark(Baseline = true)]
    public void Pairs_Default()
    {
        foreach (var pair in _defaultForRead)
        {
            _consumer.Consume(pair);
        }
    }

    [BenchmarkCategory("Pairs"), Benchmark]
    public void Pairs_Bidirectional()
    {
        foreach (var pair in _bidirectionalForRead)
        {
            _consumer.Consume(pair);
        }
    }

    [BenchmarkCategory("Keys"), Benchmark(Baseline = true)]
    public void Keys_Default()
    {
        foreach (var key in _defaultForRead.Keys)
        {
            _consumer.Consume(key);
        }
    }

    [BenchmarkCategory("Keys"), Benchmark]
    public void Keys_Bidirectional()
    {
        foreach (var key in _bidirectionalForRead.Keys)
        {
            _consumer.Consume(key);
        }
    }

    [BenchmarkCategory("Values"), Benchmark(Baseline = true)]
    public void Values_Default()
    {
        foreach (var value in _defaultForRead.Values)
        {
            _consumer.Consume(value);
        }
    }

    [BenchmarkCategory("Values"), Benchmark]
    public void Values_Bidirectional()
    {
        foreach (var value in _bidirectionalForRead.Values)
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
            _defaultForMutation.Add(pair.Key, pair.Value);
        }
    }

    [BenchmarkCategory("Add"), Benchmark(OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public void Add_Bidirectional()
    {
        foreach (var pair in _pairsForMutation)
        {
            _bidirectionalForMutation.Add(pair.Key, pair.Value);
        }
    }

    [BenchmarkCategory("TryAdd"), Benchmark(Baseline = true, OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public int TryAdd_Default()
    {
        var added = 0;

        foreach (var pair in _pairsForMutation)
        {
            if (_defaultForMutation.TryAdd(pair.Key, pair.Value))
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
            if (_bidirectionalForMutation.TryAdd(pair.Key, pair.Value))
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
            _defaultForMutation[pair.Key] = UniqueReplacementValue(pair.Key);
        }
    }

    [BenchmarkCategory("SetOverwrite"), Benchmark(OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public void SetOverwrite_Bidirectional()
    {
        foreach (var pair in _pairsForMutation)
        {
            _bidirectionalForMutation[pair.Key] = UniqueReplacementValue(pair.Key);
        }
    }

    [BenchmarkCategory("Remove"), Benchmark(Baseline = true, OperationsPerInvoke = BenchmarkConstants.MutationPairCount)]
    public int Remove_Default()
    {
        var removed = 0;

        foreach (var pair in _pairsForMutation)
        {
            if (_defaultForMutation.Remove(pair.Key))
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
            if (_bidirectionalForMutation.Remove(pair.Key))
            {
                removed++;
            }
        }

        return removed;
    }

    private static int UniqueReplacementValue(int key) => -key - 1;
}
