namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.ValueCollection.Enumerator;

public partial class BidirectionalDictionaryValueCollectionEnumeratorTests
{
    [Fact]
    [Trait("Method", "Enumerator")]
    public void Enumerator_EmptyBiDictionary_ReturnsFalse()
    {
        var biDictionary = new BidirectionalDictionary<char, int>();

        using var enumerator = biDictionary.Values.GetEnumerator();

        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    [Trait("Method", "Enumerator")]
    public void Enumerator_FilledBiDictionary_EnumeratesValues()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        using var enumerator = biDictionary.Values.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(0, enumerator.Current);

        Assert.True(enumerator.MoveNext());
        Assert.Equal(1, enumerator.Current);

        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    [Trait("Method", "Enumerator")]
    public void Enumerator_ModifiedBiDictionary_ThrowsInvalidOperationException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        using var enumerator = biDictionary.Values.GetEnumerator();

        biDictionary.Add('b', 1);

        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }
}
