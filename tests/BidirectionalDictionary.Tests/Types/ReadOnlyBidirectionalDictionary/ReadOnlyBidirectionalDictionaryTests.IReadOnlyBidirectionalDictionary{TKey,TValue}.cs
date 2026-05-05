using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "IReadOnlyBidirectionalDictionary<TKey, TValue>")]
    public void IReadOnlyBidirectionalDictionary_Inverse_FilledReadOnlyBiDictionary_ReturnsInverse()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionaryForReadOnlyBidirectionalDictionary();

        var inverse = ((IReadOnlyBidirectionalDictionary<char, int>)readOnlyBiDictionary).Inverse;

        Assert.Same(readOnlyBiDictionary.Inverse, inverse);
    }

    private static ReadOnlyBidirectionalDictionary<char, int> CreateReadOnlyBiDictionaryForReadOnlyBidirectionalDictionary()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        return new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);
    }
}
