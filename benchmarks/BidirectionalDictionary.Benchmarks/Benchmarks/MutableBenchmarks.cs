using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;

namespace BidirectionalDictionary.Benchmarks.Benchmarks;

public class MutableBenchmarks : BenchmarkBase
{
    private readonly Consumer _consumer = new();

    private Dictionary<int, int> _workingDictionary = null!;
    private BidirectionalDictionary<int, int> _workingBidirectionalDictionary = null!;

    [IterationSetup(Targets = new[]
    {
        nameof(Add_Default),
        nameof(Add_Bidirectional),
        nameof(TryAdd_Default),
        nameof(TryAdd_Bidirectional),
    })]
    public void SetupEmpty()
    {
        _workingDictionary = new Dictionary<int, int>(MutationOperations);
        _workingBidirectionalDictionary = new BidirectionalDictionary<int, int>(MutationOperations);
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
        _workingDictionary = new Dictionary<int, int>(MutationSource);
        _workingBidirectionalDictionary = new BidirectionalDictionary<int, int>(MutationSource);
    }

    // --- Lookup ---

    [BenchmarkCategory("IndexerHit"), Benchmark(Baseline = true)]
    public int IndexerHit_Default() => Dictionary[HitKey];

    [BenchmarkCategory("IndexerHit"), Benchmark]
    public int IndexerHit_Bidirectional() => BidirectionalDictionary[HitKey];

    [BenchmarkCategory("TryGetValueHit"), Benchmark(Baseline = true)]
    public bool TryGetValueHit_Default() => Dictionary.TryGetValue(HitKey, out _);

    [BenchmarkCategory("TryGetValueHit"), Benchmark]
    public bool TryGetValueHit_Bidirectional() => BidirectionalDictionary.TryGetValue(HitKey, out _);

    [BenchmarkCategory("TryGetValueMiss"), Benchmark(Baseline = true)]
    public bool TryGetValueMiss_Default() => Dictionary.TryGetValue(MissingKey, out _);

    [BenchmarkCategory("TryGetValueMiss"), Benchmark]
    public bool TryGetValueMiss_Bidirectional() => BidirectionalDictionary.TryGetValue(MissingKey, out _);

    [BenchmarkCategory("ContainsKeyHit"), Benchmark(Baseline = true)]
    public bool ContainsKeyHit_Default() => Dictionary.ContainsKey(HitKey);

    [BenchmarkCategory("ContainsKeyHit"), Benchmark]
    public bool ContainsKeyHit_Bidirectional() => BidirectionalDictionary.ContainsKey(HitKey);

    [BenchmarkCategory("ContainsKeyMiss"), Benchmark(Baseline = true)]
    public bool ContainsKeyMiss_Default() => Dictionary.ContainsKey(MissingKey);

    [BenchmarkCategory("ContainsKeyMiss"), Benchmark]
    public bool ContainsKeyMiss_Bidirectional() => BidirectionalDictionary.ContainsKey(MissingKey);

    // --- Reverse lookup ---

    [BenchmarkCategory("ContainsValue"), Benchmark(Baseline = true)]
    public bool ContainsValue_Default() => Dictionary.ContainsValue(HitValue);

    [BenchmarkCategory("ContainsValue"), Benchmark]
    public bool ContainsValue_Bidirectional() => BidirectionalDictionary.ContainsValue(HitValue);

    [BenchmarkCategory("FindKeyByValue"), Benchmark(Baseline = true)]
    public int FindKeyByValue_Default()
    {
        var target = HitValue;

        foreach (var pair in Dictionary)
        {
            if (pair.Value == target)
            {
                return pair.Key;
            }
        }

        throw new KeyNotFoundException();
    }

    [BenchmarkCategory("FindKeyByValue"), Benchmark]
    public int FindKeyByValue_Bidirectional() => BidirectionalDictionary.Inverse[HitValue];

    // --- Enumeration ---

    [BenchmarkCategory("Pairs"), Benchmark(Baseline = true)]
    public void Pairs_Default()
    {
        foreach (var pair in Dictionary)
        {
            _consumer.Consume(pair);
        }
    }

    [BenchmarkCategory("Pairs"), Benchmark]
    public void Pairs_Bidirectional()
    {
        foreach (var pair in BidirectionalDictionary)
        {
            _consumer.Consume(pair);
        }
    }

    [BenchmarkCategory("Keys"), Benchmark(Baseline = true)]
    public void Keys_Default()
    {
        foreach (var key in Dictionary.Keys)
        {
            _consumer.Consume(key);
        }
    }

    [BenchmarkCategory("Keys"), Benchmark]
    public void Keys_Bidirectional()
    {
        foreach (var key in BidirectionalDictionary.Keys)
        {
            _consumer.Consume(key);
        }
    }

    [BenchmarkCategory("Values"), Benchmark(Baseline = true)]
    public void Values_Default()
    {
        foreach (var value in Dictionary.Values)
        {
            _consumer.Consume(value);
        }
    }

    [BenchmarkCategory("Values"), Benchmark]
    public void Values_Bidirectional()
    {
        foreach (var value in BidirectionalDictionary.Values)
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
    public Dictionary<int, int> PreSized_Default() => new(DataSize);

    [BenchmarkCategory("PreSized"), Benchmark]
    public BidirectionalDictionary<int, int> PreSized_Bidirectional() => new(DataSize);

    [BenchmarkCategory("FromSequence"), Benchmark(Baseline = true)]
    public Dictionary<int, int> FromSequence_Default() => new(DataSource);

    [BenchmarkCategory("FromSequence"), Benchmark]
    public BidirectionalDictionary<int, int> FromSequence_Bidirectional() => new(DataSource);

    [BenchmarkCategory("LinqProjection"), Benchmark(Baseline = true)]
    public Dictionary<int, int> LinqProjection_Default() => DataSource.ToDictionary(p => p.Key, p => p.Value);

    [BenchmarkCategory("LinqProjection"), Benchmark]
    public BidirectionalDictionary<int, int> LinqProjection_Bidirectional() => DataSource.ToBidirectionalDictionary();

    // --- Mutation ---

    [BenchmarkCategory("Add"), Benchmark(Baseline = true, OperationsPerInvoke = MutationOperations)]
    public void Add_Default()
    {
        foreach (var pair in MutationSource)
        {
            _workingDictionary.Add(pair.Key, pair.Value);
        }
    }

    [BenchmarkCategory("Add"), Benchmark(OperationsPerInvoke = MutationOperations)]
    public void Add_Bidirectional()
    {
        foreach (var pair in MutationSource)
        {
            _workingBidirectionalDictionary.Add(pair.Key, pair.Value);
        }
    }

    [BenchmarkCategory("TryAdd"), Benchmark(Baseline = true, OperationsPerInvoke = MutationOperations)]
    public int TryAdd_Default()
    {
        var added = 0;

        foreach (var pair in MutationSource)
        {
            if (_workingDictionary.TryAdd(pair.Key, pair.Value))
            {
                added++;
            }
        }

        return added;
    }

    [BenchmarkCategory("TryAdd"), Benchmark(OperationsPerInvoke = MutationOperations)]
    public int TryAdd_Bidirectional()
    {
        var added = 0;

        foreach (var pair in MutationSource)
        {
            if (_workingBidirectionalDictionary.TryAdd(pair.Key, pair.Value))
            {
                added++;
            }
        }

        return added;
    }

    [BenchmarkCategory("SetOverwrite"), Benchmark(Baseline = true, OperationsPerInvoke = MutationOperations)]
    public void SetOverwrite_Default()
    {
        foreach (var pair in MutationSource)
        {
            _workingDictionary[pair.Key] = UniqueReplacementValue(pair.Key);
        }
    }

    [BenchmarkCategory("SetOverwrite"), Benchmark(OperationsPerInvoke = MutationOperations)]
    public void SetOverwrite_Bidirectional()
    {
        foreach (var pair in MutationSource)
        {
            _workingBidirectionalDictionary[pair.Key] = UniqueReplacementValue(pair.Key);
        }
    }

    [BenchmarkCategory("Remove"), Benchmark(Baseline = true, OperationsPerInvoke = MutationOperations)]
    public int Remove_Default()
    {
        var removed = 0;

        foreach (var pair in MutationSource)
        {
            if (_workingDictionary.Remove(pair.Key))
            {
                removed++;
            }
        }

        return removed;
    }

    [BenchmarkCategory("Remove"), Benchmark(OperationsPerInvoke = MutationOperations)]
    public int Remove_Bidirectional()
    {
        var removed = 0;

        foreach (var pair in MutationSource)
        {
            if (_workingBidirectionalDictionary.Remove(pair.Key))
            {
                removed++;
            }
        }

        return removed;
    }

    private static int UniqueReplacementValue(int key) => -key - 1;
}
