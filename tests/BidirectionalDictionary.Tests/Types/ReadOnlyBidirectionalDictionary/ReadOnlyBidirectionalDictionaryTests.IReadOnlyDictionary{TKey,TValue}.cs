using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "IReadOnlyDictionary<TKey, TValue>")]
    public void IReadOnlyDictionary_Keys_FilledReadOnlyBiDictionary_ReturnsKeys()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionaryForReadOnlyDictionary();

        var keys = ((IReadOnlyDictionary<char, int>)readOnlyBiDictionary).Keys;

        Assert.Single(keys, 'a');
    }

    [Fact]
    [Trait("Method", "IReadOnlyDictionary<TKey, TValue>")]
    public void IReadOnlyDictionary_Values_FilledReadOnlyBiDictionary_ReturnsValues()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionaryForReadOnlyDictionary();

        var values = ((IReadOnlyDictionary<char, int>)readOnlyBiDictionary).Values;

        Assert.Single(values, 0);
    }

    private static ReadOnlyBidirectionalDictionary<char, int> CreateReadOnlyBiDictionaryForReadOnlyDictionary()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        return new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);
    }
}
