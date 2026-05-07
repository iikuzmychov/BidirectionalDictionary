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

        var enumerator = ((IDictionary)concurrentBidirectionalDictionary).GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new DictionaryEntry('a', 0), enumerator.Entry);
    }

    [Fact]
    public void Entry_NotStartedEnumerator_ReturnsDefaultEntry()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = ((IDictionary)concurrentBidirectionalDictionary).GetEnumerator();

        Assert.Equal(new DictionaryEntry(default(char), default(int)), enumerator.Entry);
    }

    [Fact]
    public void Entry_FinishedEnumerator_ReturnsLastEntry()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = ((IDictionary)concurrentBidirectionalDictionary).GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.False(enumerator.MoveNext());

        Assert.Equal(new DictionaryEntry('a', 0), enumerator.Entry);
    }

    [Fact]
    public void Key_StartedEnumerator_ReturnsKey()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = ((IDictionary)concurrentBidirectionalDictionary).GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal('a', enumerator.Key);
    }

    [Fact]
    public void Key_NotStartedEnumerator_ReturnsDefaultKey()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = ((IDictionary)concurrentBidirectionalDictionary).GetEnumerator();

        Assert.Equal(default(char), enumerator.Key);
    }

    [Fact]
    public void Key_FinishedEnumerator_ReturnsLastKey()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = ((IDictionary)concurrentBidirectionalDictionary).GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.False(enumerator.MoveNext());

        Assert.Equal('a', enumerator.Key);
    }

    [Fact]
    public void Value_StartedEnumerator_ReturnsValue()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = ((IDictionary)concurrentBidirectionalDictionary).GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(0, enumerator.Value);
    }

    [Fact]
    public void Value_NotStartedEnumerator_ReturnsDefaultValue()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = ((IDictionary)concurrentBidirectionalDictionary).GetEnumerator();

        Assert.Equal(default(int), enumerator.Value);
    }

    [Fact]
    public void Value_FinishedEnumerator_ReturnsLastValue()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = ((IDictionary)concurrentBidirectionalDictionary).GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.False(enumerator.MoveNext());

        Assert.Equal(0, enumerator.Value);
    }
}
