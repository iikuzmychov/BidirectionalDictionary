using System.Collections.ObjectModel;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;

namespace BidirectionalDictionary.Benchmarks.Benchmarks;

[Config(typeof(BenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ReadOnlyBidirectionalDictionaryBenchmarks
{
    private readonly Consumer _consumer = new();

    private Dictionary<int, int> _dictionaryForWrap = null!;
    private BidirectionalDictionary<int, int> _bidirectionalDictionaryForWrap = null!;

    private ReadOnlyDictionary<int, int> _readOnlyDictionary = null!;
    private ReadOnlyBidirectionalDictionary<int, int> _readOnlyBidirectionalDictionary = null!;

    private int _hitKey;
    private int _hitValue;

    [GlobalSetup]
    public void Setup()
    {
        var pairs = BenchmarkData.GenerateSource(BenchmarkConstants.ReadPairCount);

        _dictionaryForWrap = BenchmarkData.CreateDictionary(pairs);
        _bidirectionalDictionaryForWrap = new BidirectionalDictionary<int, int>(_dictionaryForWrap);

        _readOnlyDictionary = new ReadOnlyDictionary<int, int>(_dictionaryForWrap);
        _readOnlyBidirectionalDictionary = _bidirectionalDictionaryForWrap.AsReadOnly();

        var hitIndex = BenchmarkConstants.ReadPairCount / 2;
        _hitKey = pairs[hitIndex].Key;
        _hitValue = pairs[hitIndex].Value;
    }

    // --- Lookup ---

    [BenchmarkCategory("IndexerHit"), Benchmark(Baseline = true)]
    public int IndexerHit_Default() => _readOnlyDictionary[_hitKey];

    [BenchmarkCategory("IndexerHit"), Benchmark]
    public int IndexerHit_Bidirectional() => _readOnlyBidirectionalDictionary[_hitKey];

    [BenchmarkCategory("TryGetValueHit"), Benchmark(Baseline = true)]
    public bool TryGetValueHit_Default() => _readOnlyDictionary.TryGetValue(_hitKey, out _);

    [BenchmarkCategory("TryGetValueHit"), Benchmark]
    public bool TryGetValueHit_Bidirectional() => _readOnlyBidirectionalDictionary.TryGetValue(_hitKey, out _);

    [BenchmarkCategory("TryGetValueMiss"), Benchmark(Baseline = true)]
    public bool TryGetValueMiss_Default() => _readOnlyDictionary.TryGetValue(BenchmarkConstants.MissingKey, out _);

    [BenchmarkCategory("TryGetValueMiss"), Benchmark]
    public bool TryGetValueMiss_Bidirectional() => _readOnlyBidirectionalDictionary.TryGetValue(BenchmarkConstants.MissingKey, out _);

    [BenchmarkCategory("ContainsKeyHit"), Benchmark(Baseline = true)]
    public bool ContainsKeyHit_Default() => _readOnlyDictionary.ContainsKey(_hitKey);

    [BenchmarkCategory("ContainsKeyHit"), Benchmark]
    public bool ContainsKeyHit_Bidirectional() => _readOnlyBidirectionalDictionary.ContainsKey(_hitKey);

    [BenchmarkCategory("ContainsKeyMiss"), Benchmark(Baseline = true)]
    public bool ContainsKeyMiss_Default() => _readOnlyDictionary.ContainsKey(BenchmarkConstants.MissingKey);

    [BenchmarkCategory("ContainsKeyMiss"), Benchmark]
    public bool ContainsKeyMiss_Bidirectional() => _readOnlyBidirectionalDictionary.ContainsKey(BenchmarkConstants.MissingKey);

    // --- Reverse lookup ---

    [BenchmarkCategory("ContainsValue"), Benchmark(Baseline = true)]
    public bool ContainsValue_Default()
    {
        var target = _hitValue;

        foreach (var pair in _readOnlyDictionary)
        {
            if (pair.Value == target)
            {
                return true;
            }
        }

        return false;
    }

    [BenchmarkCategory("ContainsValue"), Benchmark]
    public bool ContainsValue_Bidirectional() => _readOnlyBidirectionalDictionary.ContainsValue(_hitValue);

    [BenchmarkCategory("FindKeyByValue"), Benchmark(Baseline = true)]
    public int FindKeyByValue_Default()
    {
        var target = _hitValue;

        foreach (var pair in _readOnlyDictionary)
        {
            if (pair.Value == target)
            {
                return pair.Key;
            }
        }

        throw new KeyNotFoundException();
    }

    [BenchmarkCategory("FindKeyByValue"), Benchmark]
    public int FindKeyByValue_Bidirectional() => _readOnlyBidirectionalDictionary.Inverse[_hitValue];

    // --- Enumeration ---

    [BenchmarkCategory("Pairs"), Benchmark(Baseline = true)]
    public void Pairs_Default()
    {
        foreach (var pair in _readOnlyDictionary)
        {
            _consumer.Consume(pair);
        }
    }

    [BenchmarkCategory("Pairs"), Benchmark]
    public void Pairs_Bidirectional()
    {
        foreach (var pair in _readOnlyBidirectionalDictionary)
        {
            _consumer.Consume(pair);
        }
    }

    [BenchmarkCategory("Keys"), Benchmark(Baseline = true)]
    public void Keys_Default()
    {
        foreach (var key in _readOnlyDictionary.Keys)
        {
            _consumer.Consume(key);
        }
    }

    [BenchmarkCategory("Keys"), Benchmark]
    public void Keys_Bidirectional()
    {
        foreach (var key in _readOnlyBidirectionalDictionary.Keys)
        {
            _consumer.Consume(key);
        }
    }

    [BenchmarkCategory("Values"), Benchmark(Baseline = true)]
    public void Values_Default()
    {
        foreach (var value in _readOnlyDictionary.Values)
        {
            _consumer.Consume(value);
        }
    }

    [BenchmarkCategory("Values"), Benchmark]
    public void Values_Bidirectional()
    {
        foreach (var value in _readOnlyBidirectionalDictionary.Values)
        {
            _consumer.Consume(value);
        }
    }

    // --- Construction ---

    [BenchmarkCategory("Wrap"), Benchmark(Baseline = true)]
    public ReadOnlyDictionary<int, int> Wrap_Default() => new(_dictionaryForWrap);

    [BenchmarkCategory("Wrap"), Benchmark]
    public ReadOnlyBidirectionalDictionary<int, int> Wrap_Bidirectional() => new(_bidirectionalDictionaryForWrap);
}
