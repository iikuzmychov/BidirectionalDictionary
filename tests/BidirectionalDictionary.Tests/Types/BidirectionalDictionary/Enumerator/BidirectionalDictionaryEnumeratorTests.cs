namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.Enumerator;

public partial class BidirectionalDictionaryEnumeratorTests
{
    [Fact]
    [Trait("Method", "Enumerator")]
    public void Enumerator_EmptyBiDictionary_ReturnsFalse()
    {
        var biDictionary = new BidirectionalDictionary<char, int>();

        using var enumerator = biDictionary.GetEnumerator();

        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    [Trait("Method", "Enumerator")]
    public void Enumerator_FilledBiDictionary_EnumeratesEntries()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        using var enumerator = biDictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('a', 0), enumerator.Current);

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('b', 1), enumerator.Current);

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

        using var enumerator = biDictionary.GetEnumerator();

        biDictionary.Add('b', 1);

        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }
}
