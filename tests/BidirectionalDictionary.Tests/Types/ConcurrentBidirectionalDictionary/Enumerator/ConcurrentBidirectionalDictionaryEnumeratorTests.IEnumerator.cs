using System.Collections;
using System.Collections.Concurrent;

namespace BidirectionalDictionary.Tests.Types.ConcurrentBidirectionalDictionary.Enumerator;

public partial class ConcurrentBidirectionalDictionaryEnumeratorTests
{
    [Fact]
    public void Current_StartedEnumerator_ReturnsCurrent()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('a', 0), enumerator.Current);
    }

    [Fact]
    public void Current_NotStartedEnumerator_ThrowsInvalidOperationException()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.Throws<InvalidOperationException>(() => _ = enumerator.Current);
    }

    [Fact]
    public void Current_FinishedEnumerator_ThrowsInvalidOperationException()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.False(enumerator.MoveNext());

        Assert.Throws<InvalidOperationException>(() => _ = enumerator.Current);
    }

    [Fact]
    public void Reset_StartedEnumerator_ResetsEnumerator()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());

        enumerator.Reset();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('a', 0), enumerator.Current);
    }

    [Fact]
    public void Reset_ModifiedConcurrentBidirectionalDictionary_ResetsSnapshotEnumerator()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        concurrentBidirectionalDictionary.Add('b', 1);

        enumerator.Reset();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('a', 0), enumerator.Current);
        Assert.False(enumerator.MoveNext());
    }
}
