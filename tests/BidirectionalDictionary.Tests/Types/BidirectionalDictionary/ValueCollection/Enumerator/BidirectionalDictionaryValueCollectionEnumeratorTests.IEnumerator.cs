using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.ValueCollection.Enumerator;

public partial class BidirectionalDictionaryValueCollectionEnumeratorTests
{
    [Fact]
    public void Current_StartedEnumerator_ReturnsCurrent()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)bidirectionalDictionary.Values.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(0, enumerator.Current);
    }

    [Fact]
    public void Current_NotStartedEnumerator_ThrowsInvalidOperationException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)bidirectionalDictionary.Values.GetEnumerator();

        Assert.Throws<InvalidOperationException>(() => _ = enumerator.Current);
    }

    [Fact]
    public void Current_FinishedEnumerator_ThrowsInvalidOperationException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)bidirectionalDictionary.Values.GetEnumerator();

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

        var enumerator = (IEnumerator)bidirectionalDictionary.Values.GetEnumerator();

        Assert.True(enumerator.MoveNext());

        enumerator.Reset();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(0, enumerator.Current);
    }

    [Fact]
    public void Reset_ModifiedBidirectionalDictionary_ThrowsInvalidOperationException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)bidirectionalDictionary.Values.GetEnumerator();

        bidirectionalDictionary.Add('b', 1);

        Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
    }
}
