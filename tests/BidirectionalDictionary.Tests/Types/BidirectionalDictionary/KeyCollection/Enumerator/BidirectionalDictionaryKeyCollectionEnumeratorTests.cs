namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.KeyCollection.Enumerator;

public partial class BidirectionalDictionaryKeyCollectionEnumeratorTests
{
    [Fact]
    public void Enumerator_EmptyBidirectionalDictionary_ReturnsFalse()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        using var enumerator = bidirectionalDictionary.Keys.GetEnumerator();

        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void Enumerator_FilledBidirectionalDictionary_EnumeratesKeys()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        using var enumerator = bidirectionalDictionary.Keys.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal('a', enumerator.Current);

        Assert.True(enumerator.MoveNext());
        Assert.Equal('b', enumerator.Current);

        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void Enumerator_ModifiedBidirectionalDictionary_ThrowsInvalidOperationException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        using var enumerator = bidirectionalDictionary.Keys.GetEnumerator();

        bidirectionalDictionary.Add('b', 1);

        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }
}
