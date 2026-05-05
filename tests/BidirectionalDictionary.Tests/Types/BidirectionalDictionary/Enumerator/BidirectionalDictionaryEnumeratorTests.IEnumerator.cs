using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.Enumerator;

public partial class BidirectionalDictionaryEnumeratorTests
{
    [Fact]
    public void Current_StartedEnumerator_ReturnsCurrent()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)bidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('a', 0), enumerator.Current);
    }

    [Fact]
    public void Current_NotStartedEnumerator_ThrowsInvalidOperationException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)bidirectionalDictionary.GetEnumerator();

        Assert.Throws<InvalidOperationException>(() => _ = enumerator.Current);
    }

    [Fact]
    public void Current_FinishedEnumerator_ThrowsInvalidOperationException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)bidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.False(enumerator.MoveNext());

        Assert.Throws<InvalidOperationException>(() => _ = enumerator.Current);
    }

    [Fact]
    public void Reset_StartedEnumerator_ResetsEnumerator()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)bidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());

        enumerator.Reset();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('a', 0), enumerator.Current);
    }

    [Fact]
    public void Reset_ModifiedBidirectionalDictionary_ThrowsInvalidOperationException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)bidirectionalDictionary.GetEnumerator();

        bidirectionalDictionary.Add('b', 1);

        Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
    }
}
