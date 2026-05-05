using System.Collections.ObjectModel;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace BidirectionalDictionary.Benchmarks.Benchmarks;

public class ReadOnlyBenchmarks : BenchmarkBase
{
    private readonly Consumer _consumer = new();

    // --- Lookup ---

    [BenchmarkCategory("IndexerHit"), Benchmark(Baseline = true)]
    public int IndexerHit_Default() => ReadOnlyDictionary[HitKey];

    [BenchmarkCategory("IndexerHit"), Benchmark]
    public int IndexerHit_Bidirectional() => ReadOnlyBidirectionalDictionary[HitKey];

    [BenchmarkCategory("TryGetValueHit"), Benchmark(Baseline = true)]
    public bool TryGetValueHit_Default() => ReadOnlyDictionary.TryGetValue(HitKey, out _);

    [BenchmarkCategory("TryGetValueHit"), Benchmark]
    public bool TryGetValueHit_Bidirectional() => ReadOnlyBidirectionalDictionary.TryGetValue(HitKey, out _);

    [BenchmarkCategory("TryGetValueMiss"), Benchmark(Baseline = true)]
    public bool TryGetValueMiss_Default() => ReadOnlyDictionary.TryGetValue(MissingKey, out _);

    [BenchmarkCategory("TryGetValueMiss"), Benchmark]
    public bool TryGetValueMiss_Bidirectional() => ReadOnlyBidirectionalDictionary.TryGetValue(MissingKey, out _);

    [BenchmarkCategory("ContainsKeyHit"), Benchmark(Baseline = true)]
    public bool ContainsKeyHit_Default() => ReadOnlyDictionary.ContainsKey(HitKey);

    [BenchmarkCategory("ContainsKeyHit"), Benchmark]
    public bool ContainsKeyHit_Bidirectional() => ReadOnlyBidirectionalDictionary.ContainsKey(HitKey);

    [BenchmarkCategory("ContainsKeyMiss"), Benchmark(Baseline = true)]
    public bool ContainsKeyMiss_Default() => ReadOnlyDictionary.ContainsKey(MissingKey);

    [BenchmarkCategory("ContainsKeyMiss"), Benchmark]
    public bool ContainsKeyMiss_Bidirectional() => ReadOnlyBidirectionalDictionary.ContainsKey(MissingKey);

    // --- Reverse lookup ---

    [BenchmarkCategory("ContainsValue"), Benchmark(Baseline = true)]
    public bool ContainsValue_Default()
    {
        var target = HitValue;

        foreach (var pair in ReadOnlyDictionary)
        {
            if (pair.Value == target)
            {
                return true;
            }
        }

        return false;
    }

    [BenchmarkCategory("ContainsValue"), Benchmark]
    public bool ContainsValue_Bidirectional() => ReadOnlyBidirectionalDictionary.ContainsValue(HitValue);

    [BenchmarkCategory("FindKeyByValue"), Benchmark(Baseline = true)]
    public int FindKeyByValue_Default()
    {
        var target = HitValue;

        foreach (var pair in ReadOnlyDictionary)
        {
            if (pair.Value == target)
            {
                return pair.Key;
            }
        }

        throw new KeyNotFoundException();
    }

    [BenchmarkCategory("FindKeyByValue"), Benchmark]
    public int FindKeyByValue_Bidirectional() => ReadOnlyBidirectionalDictionary.Inverse[HitValue];

    // --- Enumeration ---

    [BenchmarkCategory("Pairs"), Benchmark(Baseline = true)]
    public void Pairs_Default()
    {
        foreach (var pair in ReadOnlyDictionary)
        {
            _consumer.Consume(pair);
        }
    }

    [BenchmarkCategory("Pairs"), Benchmark]
    public void Pairs_Bidirectional()
    {
        foreach (var pair in ReadOnlyBidirectionalDictionary)
        {
            _consumer.Consume(pair);
        }
    }

    [BenchmarkCategory("Keys"), Benchmark(Baseline = true)]
    public void Keys_Default()
    {
        foreach (var key in ReadOnlyDictionary.Keys)
        {
            _consumer.Consume(key);
        }
    }

    [BenchmarkCategory("Keys"), Benchmark]
    public void Keys_Bidirectional()
    {
        foreach (var key in ReadOnlyBidirectionalDictionary.Keys)
        {
            _consumer.Consume(key);
        }
    }

    [BenchmarkCategory("Values"), Benchmark(Baseline = true)]
    public void Values_Default()
    {
        foreach (var value in ReadOnlyDictionary.Values)
        {
            _consumer.Consume(value);
        }
    }

    [BenchmarkCategory("Values"), Benchmark]
    public void Values_Bidirectional()
    {
        foreach (var value in ReadOnlyBidirectionalDictionary.Values)
        {
            _consumer.Consume(value);
        }
    }

    // --- Construction ---

    [BenchmarkCategory("Wrap"), Benchmark(Baseline = true)]
    public ReadOnlyDictionary<int, int> Wrap_Default() => new(Dictionary);

    [BenchmarkCategory("Wrap"), Benchmark]
    public ReadOnlyBidirectionalDictionary<int, int> Wrap_Bidirectional() => BidirectionalDictionary.AsReadOnly();
}
