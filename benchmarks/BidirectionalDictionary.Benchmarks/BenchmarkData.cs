namespace BidirectionalDictionary.Benchmarks;

internal static class BenchmarkData
{
    public static KeyValuePair<int, int>[] GenerateSource(int size)
    {
        var random = new Random(BenchmarkConstants.Seed);
        var values = new int[size];

        for (var i = 0; i < size; i++)
        {
            values[i] = i;
        }

        random.Shuffle(values);

        var result = new KeyValuePair<int, int>[size];

        for (var i = 0; i < size; i++)
        {
            result[i] = new(i, values[i]);
        }

        return result;
    }

    public static Dictionary<int, int> CreateDictionary(KeyValuePair<int, int>[] source)
    {
        var dictionary = new Dictionary<int, int>(source.Length);

        foreach (var pair in source)
        {
            dictionary.Add(pair.Key, pair.Value);
        }

        return dictionary;
    }
}
