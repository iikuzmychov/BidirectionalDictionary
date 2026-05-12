using System.Collections.Concurrent;

namespace BidirectionalDictionary.Tests.Types.ConcurrentBidirectionalDictionary.Enumerator;

public partial class ConcurrentBidirectionalDictionaryEnumeratorTests
{
    [Fact]
    public void Enumerator_EmptyConcurrentBidirectionalDictionary_ReturnsFalse()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>();

        using var enumerator = concurrentBidirectionalDictionary.GetEnumerator();

        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void Enumerator_FilledConcurrentBidirectionalDictionary_EnumeratesEntries()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>(
        [
            new KeyValuePair<char, int>('a', 0),
            new KeyValuePair<char, int>('b', 1),
        ]);

        using var enumerator = concurrentBidirectionalDictionary.GetEnumerator();
        var observed = new List<KeyValuePair<char, int>>();

        while (enumerator.MoveNext())
        {
            observed.Add(enumerator.Current);
        }

        Assert.False(enumerator.MoveNext());
        Assert.Equal(2, observed.Count);
        Assert.Contains(new KeyValuePair<char, int>('a', 0), observed);
        Assert.Contains(new KeyValuePair<char, int>('b', 1), observed);
    }

    [Fact]
    public void Enumerator_ModifiedConcurrentBidirectionalDictionary_MayObserveModification()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>(
        [
            new KeyValuePair<char, int>('a', 0),
        ]);

        using var enumerator = concurrentBidirectionalDictionary.GetEnumerator();

        concurrentBidirectionalDictionary.TryAdd('b', 1);

        var entries = new List<KeyValuePair<char, int>>();
        while (enumerator.MoveNext())
        {
            entries.Add(enumerator.Current);
        }

        Assert.Contains(new KeyValuePair<char, int>('a', 0), entries);
        Assert.False(enumerator.MoveNext());
    }
}
