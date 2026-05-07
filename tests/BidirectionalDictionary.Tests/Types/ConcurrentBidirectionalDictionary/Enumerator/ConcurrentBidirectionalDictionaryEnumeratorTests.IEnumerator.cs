using System.Collections;
using System.Collections.Concurrent;

namespace BidirectionalDictionary.Tests.Types.ConcurrentBidirectionalDictionary.Enumerator;

public partial class ConcurrentBidirectionalDictionaryEnumeratorTests
{
    [Fact]
    public void Current_StartedEnumerator_ReturnsCurrent()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>(
        [
            new KeyValuePair<char, int>('a', 0),
        ]);

        var enumerator = (IEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('a', 0), enumerator.Current);
    }

    [Fact]
    public void Current_NotStartedEnumerator_ReturnsDefault()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>(
        [
            new KeyValuePair<char, int>('a', 0),
        ]);

        var enumerator = (IEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.Equal(default(KeyValuePair<char, int>), enumerator.Current);
    }

    [Fact]
    public void Current_FinishedEnumerator_ReturnsLastCurrent()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>(
        [
            new KeyValuePair<char, int>('a', 0),
        ]);

        var enumerator = (IEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.False(enumerator.MoveNext());

        Assert.Equal(new KeyValuePair<char, int>('a', 0), enumerator.Current);
    }

    [Fact]
    public void Reset_StartedEnumerator_ResetsEnumerator()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>(
        [
            new KeyValuePair<char, int>('a', 0),
        ]);

        var enumerator = (IEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());

        enumerator.Reset();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('a', 0), enumerator.Current);
    }

    [Fact]
    public void Reset_ModifiedConcurrentBidirectionalDictionary_ResetsEnumerator()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>(
        [
            new KeyValuePair<char, int>('a', 0),
        ]);

        var enumerator = (IEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        concurrentBidirectionalDictionary.TryAdd('b', 1);

        enumerator.Reset();

        var entries = new List<KeyValuePair<char, int>>();
        while (enumerator.MoveNext())
        {
            entries.Add((KeyValuePair<char, int>)enumerator.Current);
        }

        Assert.Contains(new KeyValuePair<char, int>('a', 0), entries);
        Assert.False(enumerator.MoveNext());
    }
}
