namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.Enumerator;

public partial class BidirectionalDictionaryEnumeratorTests
{
    [Fact]
    public void Enumerator_EmptyBidirectionalDictionary_ReturnsFalse()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        using var enumerator = bidirectionalDictionary.GetEnumerator();

        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void Enumerator_FilledBidirectionalDictionary_EnumeratesEntries()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        using var enumerator = bidirectionalDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('a', 0), enumerator.Current);

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('b', 1), enumerator.Current);

        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void Enumerator_ModifiedBidirectionalDictionary_ThrowsInvalidOperationException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        using var enumerator = bidirectionalDictionary.GetEnumerator();

        bidirectionalDictionary.Add('b', 1);

        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }
}
