using System.Collections;
using System.Collections.Concurrent;

namespace BidirectionalDictionary.Tests.Types.ConcurrentBidirectionalDictionary.Enumerator;

public partial class ConcurrentBidirectionalDictionaryEnumeratorTests
{
    [Fact]
    public void Entry_StartedEnumerator_ReturnsEntry()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IDictionaryEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new DictionaryEntry('a', 0), enumerator.Entry);
    }

    [Fact]
    public void Entry_NotStartedEnumerator_ThrowsInvalidOperationException()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IDictionaryEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.Throws<InvalidOperationException>(() => _ = enumerator.Entry);
    }

    [Fact]
    public void Entry_FinishedEnumerator_ThrowsInvalidOperationException()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IDictionaryEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.False(enumerator.MoveNext());

        Assert.Throws<InvalidOperationException>(() => _ = enumerator.Entry);
    }

    [Fact]
    public void Key_StartedEnumerator_ReturnsKey()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IDictionaryEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal('a', enumerator.Key);
    }

    [Fact]
    public void Key_NotStartedEnumerator_ThrowsInvalidOperationException()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IDictionaryEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.Throws<InvalidOperationException>(() => _ = enumerator.Key);
    }

    [Fact]
    public void Key_FinishedEnumerator_ThrowsInvalidOperationException()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IDictionaryEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.False(enumerator.MoveNext());

        Assert.Throws<InvalidOperationException>(() => _ = enumerator.Key);
    }

    [Fact]
    public void Value_StartedEnumerator_ReturnsValue()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IDictionaryEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(0, enumerator.Value);
    }

    [Fact]
    public void Value_NotStartedEnumerator_ThrowsInvalidOperationException()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IDictionaryEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.Throws<InvalidOperationException>(() => _ = enumerator.Value);
    }

    [Fact]
    public void Value_FinishedEnumerator_ThrowsInvalidOperationException()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IDictionaryEnumerator)concurrentBidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.False(enumerator.MoveNext());

        Assert.Throws<InvalidOperationException>(() => _ = enumerator.Value);
    }
}
