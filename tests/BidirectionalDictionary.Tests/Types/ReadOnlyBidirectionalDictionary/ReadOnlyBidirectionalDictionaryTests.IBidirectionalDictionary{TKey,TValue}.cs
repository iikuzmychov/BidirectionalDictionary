using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "IBidirectionalDictionary<TKey, TValue>")]
    public void IBidirectionalDictionary_Inverse_FilledReadOnlyBiDictionary_ReturnsInverse()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionaryForBidirectionalDictionary();

        var inverse = ((IBidirectionalDictionary<char, int>)readOnlyBiDictionary).Inverse;

        Assert.Same(readOnlyBiDictionary.Inverse, inverse);
    }

    private static ReadOnlyBidirectionalDictionary<char, int> CreateReadOnlyBiDictionaryForBidirectionalDictionary()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        return new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);
    }
}
