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
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        using var enumerator = concurrentBidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('a', 0), enumerator.Current);

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('b', 1), enumerator.Current);

        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void Enumerator_ModifiedConcurrentBidirectionalDictionary_EnumeratesSnapshot()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        using var enumerator = concurrentBidirectionalDictionary.GetEnumerator();

        concurrentBidirectionalDictionary.Add('b', 1);

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('a', 0), enumerator.Current);
        Assert.False(enumerator.MoveNext());
    }
}
