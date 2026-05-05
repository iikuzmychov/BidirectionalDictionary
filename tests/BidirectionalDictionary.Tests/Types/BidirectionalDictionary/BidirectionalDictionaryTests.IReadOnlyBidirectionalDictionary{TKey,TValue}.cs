using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary;

public partial class BidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "IReadOnlyBidirectionalDictionary<TKey, TValue>")]
    public void IReadOnlyBidirectionalDictionary_Inverse_FilledBiDictionary_ReturnsReadOnlyInverse()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var inverse = ((IReadOnlyBidirectionalDictionary<char, int>)biDictionary).Inverse;

        Assert.Single(inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.IsType<ReadOnlyBidirectionalDictionary<int, char>>(inverse);
    }
}
