using System.Collections.ObjectModel;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;

namespace BidirectionalDictionary.Benchmarks.Benchmarks;

[Config(typeof(BenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class ReadOnlyBenchmarks
{
    private readonly Consumer _consumer = new();

    private Dictionary<int, int> _defaultForWrap = null!;
    private BidirectionalDictionary<int, int> _bidirectionalForWrap = null!;

    private ReadOnlyDictionary<int, int> _defaultForRead = null!;
    private ReadOnlyBidirectionalDictionary<int, int> _bidirectionalForRead = null!;

    private int _hitKey;
    private int _hitValue;

    [GlobalSetup]
    public void Setup()
    {
        var pairs = BenchmarkData.GenerateSource(BenchmarkConstants.ReadPairCount);

        _defaultForWrap = BenchmarkData.CreateDictionary(pairs);
        _bidirectionalForWrap = new BidirectionalDictionary<int, int>(_defaultForWrap);

        _defaultForRead = new ReadOnlyDictionary<int, int>(_defaultForWrap);
        _bidirectionalForRead = _bidirectionalForWrap.AsReadOnly();

        var hitIndex = BenchmarkConstants.ReadPairCount / 2;
        _hitKey = pairs[hitIndex].Key;
        _hitValue = pairs[hitIndex].Value;
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
    public bool ContainsValue_Default()
    {
        var target = _hitValue;

        foreach (var pair in _defaultForRead)
        {
            if (pair.Value == target)
            {
                return true;
            }
        }

        return false;
    }

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

    [BenchmarkCategory("Wrap"), Benchmark(Baseline = true)]
    public ReadOnlyDictionary<int, int> Wrap_Default() => new(_defaultForWrap);

    [BenchmarkCategory("Wrap"), Benchmark]
    public ReadOnlyBidirectionalDictionary<int, int> Wrap_Bidirectional() => new(_bidirectionalForWrap);
}
