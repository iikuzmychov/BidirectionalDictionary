using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Fact]
    [Trait("Indexer", "IDictionary<TKey, TValue>")]
    public void IDictionary_IndexerGet_FilledReadOnlyBiDictionary_ReturnsValue()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionaryForDictionary();

        var value = ((IDictionary<char, int>)readOnlyBiDictionary)['a'];

        Assert.Equal(0, value);
    }

    [Fact]
    [Trait("Indexer", "IDictionary<TKey, TValue>")]
    public void IDictionary_IndexerSet_FilledReadOnlyBiDictionary_ThrowsNotSupportedException()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionaryForDictionary();

        Assert.Throws<NotSupportedException>(() => ((IDictionary<char, int>)readOnlyBiDictionary)['a'] = 1);
    }

    [Fact]
    [Trait("Method", "IDictionary<TKey, TValue>")]
    public void IDictionary_Keys_FilledReadOnlyBiDictionary_ReturnsKeys()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionaryForDictionary();

        var keys = ((IDictionary<char, int>)readOnlyBiDictionary).Keys;

        Assert.Single(keys, 'a');
    }

    [Fact]
    [Trait("Method", "IDictionary<TKey, TValue>")]
    public void IDictionary_Values_FilledReadOnlyBiDictionary_ReturnsValues()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionaryForDictionary();

        var values = ((IDictionary<char, int>)readOnlyBiDictionary).Values;

        Assert.Single(values, 0);
    }

    [Fact]
    [Trait("Method", "IDictionary<TKey, TValue>")]
    public void IDictionary_Add_FilledReadOnlyBiDictionary_ThrowsNotSupportedException()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionaryForDictionary();

        Assert.Throws<NotSupportedException>(() => ((IDictionary<char, int>)readOnlyBiDictionary).Add('b', 1));
    }

    [Fact]
    [Trait("Method", "IDictionary<TKey, TValue>")]
    public void IDictionary_Remove_FilledReadOnlyBiDictionary_ThrowsNotSupportedException()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionaryForDictionary();

        Assert.Throws<NotSupportedException>(() => ((IDictionary<char, int>)readOnlyBiDictionary).Remove('a'));
    }

    private static ReadOnlyBidirectionalDictionary<char, int> CreateReadOnlyBiDictionaryForDictionary()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        return new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);
    }
}
