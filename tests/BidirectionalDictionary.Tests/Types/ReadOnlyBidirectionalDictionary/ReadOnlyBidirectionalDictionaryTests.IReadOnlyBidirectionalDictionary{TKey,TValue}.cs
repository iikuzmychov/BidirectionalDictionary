using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Fact]
    public void IReadOnlyBidirectionalDictionary_Inverse_FilledReadOnlyBidirectionalDictionary_ReturnsInverse()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionaryForReadOnlyBidirectionalDictionary();

        var inverse = ((IReadOnlyBidirectionalDictionary<char, int>)readOnlyBidirectionalDictionary).Inverse;

        Assert.Same(readOnlyBidirectionalDictionary.Inverse, inverse);
    }

    private static ReadOnlyBidirectionalDictionary<char, int> CreateReadOnlyBidirectionalDictionaryForReadOnlyBidirectionalDictionary()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        return new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);
    }
}
