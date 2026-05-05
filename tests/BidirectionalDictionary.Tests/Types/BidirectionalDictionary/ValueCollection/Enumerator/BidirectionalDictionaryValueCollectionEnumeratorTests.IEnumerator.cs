using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.ValueCollection.Enumerator;

public partial class BidirectionalDictionaryValueCollectionEnumeratorTests
{
    [Fact]
    [Trait("Method", "IEnumerator")]
    public void Current_StartedEnumerator_ReturnsCurrent()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)biDictionary.Values.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(0, enumerator.Current);
    }

    [Fact]
    [Trait("Method", "IEnumerator")]
    public void Current_NotStartedEnumerator_ThrowsInvalidOperationException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)biDictionary.Values.GetEnumerator();

        Assert.Throws<InvalidOperationException>(() => _ = enumerator.Current);
    }

    [Fact]
    [Trait("Method", "IEnumerator")]
    public void Current_FinishedEnumerator_ThrowsInvalidOperationException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)biDictionary.Values.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.False(enumerator.MoveNext());

        Assert.Throws<InvalidOperationException>(() => _ = enumerator.Current);
    }

    [Fact]
    [Trait("Method", "IEnumerator")]
    public void Reset_StartedEnumerator_ResetsEnumerator()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)biDictionary.Values.GetEnumerator();

        Assert.True(enumerator.MoveNext());

        enumerator.Reset();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(0, enumerator.Current);
    }

    [Fact]
    [Trait("Method", "IEnumerator")]
    public void Reset_ModifiedBiDictionary_ThrowsInvalidOperationException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var enumerator = (IEnumerator)biDictionary.Values.GetEnumerator();

        biDictionary.Add('b', 1);

        Assert.Throws<InvalidOperationException>(() => enumerator.Reset());
    }
}
