using System.Collections.ObjectModel;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace Benchmarks;

[Config(typeof(BenchmarkConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public abstract class BenchmarkBase
{
    private const int Seed = 42;

    protected const int MissingKey = -1;

    public const int DataSize = 10_000;
    public const int MutationOperations = 1_000_000;
    
    protected KeyValuePair<int, int>[] DataSource { get; private set; } = null!;
    protected KeyValuePair<int, int>[] MutationSource { get; private set; } = null!;
    protected Dictionary<int, int> Dictionary { get; private set; } = null!;
    protected BidirectionalDictionary<int, int> BidirectionalDictionary { get; private set; } = null!;
    protected ReadOnlyDictionary<int, int> ReadOnlyDictionary { get; private set; } = null!;
    protected ReadOnlyBidirectionalDictionary<int, int> ReadOnlyBidirectionalDictionary { get; private set; } = null!;

    protected int HitKey { get; private set; }
    protected int HitValue { get; private set; }

    [GlobalSetup]
    public virtual void Setup()
    {
        DataSource = GenerateSource(DataSize, new Random(Seed));
        MutationSource = GenerateSource(MutationOperations, new Random(Seed));

        Dictionary = new Dictionary<int, int>(DataSize);

        foreach (var pair in DataSource)
        {
            Dictionary.Add(pair.Key, pair.Value);
        }

        BidirectionalDictionary = new BidirectionalDictionary<int, int>(Dictionary);
        ReadOnlyDictionary = new ReadOnlyDictionary<int, int>(Dictionary);
        ReadOnlyBidirectionalDictionary = BidirectionalDictionary.AsReadOnly();

        var hitIndex = DataSize / 2;
        HitKey = DataSource[hitIndex].Key;
        HitValue = DataSource[hitIndex].Value;
    }

    private static KeyValuePair<int, int>[] GenerateSource(int size, Random random)
    {
        var values = new int[size];

        for (var i = 0; i < size; i++)
        {
            values[i] = i;
        }

        for (var i = size - 1; i > 0; i--)
        {
            var j = random.Next(i + 1);
            (values[i], values[j]) = (values[j], values[i]);
        }

        var result = new KeyValuePair<int, int>[size];

        for (var i = 0; i < size; i++)
        {
            result[i] = new(i, values[i]);
        }

        return result;
    }
}
