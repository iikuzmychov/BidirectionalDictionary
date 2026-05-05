namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.KeyCollection.Enumerator;

public partial class BidirectionalDictionaryKeyCollectionEnumeratorTests
{
    [Fact]
    [Trait("Method", "Enumerator")]
    public void Enumerator_EmptyBiDictionary_ReturnsFalse()
    {
        var biDictionary = new BidirectionalDictionary<char, int>();

        using var enumerator = biDictionary.Keys.GetEnumerator();

        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    [Trait("Method", "Enumerator")]
    public void Enumerator_FilledBiDictionary_EnumeratesKeys()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        using var enumerator = biDictionary.Keys.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal('a', enumerator.Current);

        Assert.True(enumerator.MoveNext());
        Assert.Equal('b', enumerator.Current);

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

        using var enumerator = biDictionary.Keys.GetEnumerator();

        biDictionary.Add('b', 1);

        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }
}
