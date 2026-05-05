using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Fact]
    [Trait("Indexer", "IDictionary<TKey, TValue>")]
    public void IDictionary_IndexerGet_FilledReadOnlyBidirectionalDictionary_ReturnsValue()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionaryForDictionary();

        var value = ((IDictionary<char, int>)readOnlyBidirectionalDictionary)['a'];

        Assert.Equal(0, value);
    }

    [Fact]
    [Trait("Indexer", "IDictionary<TKey, TValue>")]
    public void IDictionary_IndexerSet_FilledReadOnlyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionaryForDictionary();

        Assert.Throws<NotSupportedException>(() => ((IDictionary<char, int>)readOnlyBidirectionalDictionary)['a'] = 1);
    }

    [Fact]
    [Trait("Method", "IDictionary<TKey, TValue>")]
    public void IDictionary_Keys_FilledReadOnlyBidirectionalDictionary_ReturnsKeys()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionaryForDictionary();

        var keys = ((IDictionary<char, int>)readOnlyBidirectionalDictionary).Keys;

        Assert.Single(keys, 'a');
    }

    [Fact]
    [Trait("Method", "IDictionary<TKey, TValue>")]
    public void IDictionary_Values_FilledReadOnlyBidirectionalDictionary_ReturnsValues()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionaryForDictionary();

        var values = ((IDictionary<char, int>)readOnlyBidirectionalDictionary).Values;

        Assert.Single(values, 0);
    }

    [Fact]
    [Trait("Method", "IDictionary<TKey, TValue>")]
    public void IDictionary_Add_FilledReadOnlyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionaryForDictionary();

        Assert.Throws<NotSupportedException>(() => ((IDictionary<char, int>)readOnlyBidirectionalDictionary).Add('b', 1));
    }

    [Fact]
    [Trait("Method", "IDictionary<TKey, TValue>")]
    public void IDictionary_Remove_FilledReadOnlyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionaryForDictionary();

        Assert.Throws<NotSupportedException>(() => ((IDictionary<char, int>)readOnlyBidirectionalDictionary).Remove('a'));
    }

    private static ReadOnlyBidirectionalDictionary<char, int> CreateReadOnlyBidirectionalDictionaryForDictionary()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        return new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);
    }
}
