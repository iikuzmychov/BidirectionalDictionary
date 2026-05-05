using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Fact]
    public void IBidirectionalDictionary_Inverse_FilledReadOnlyBidirectionalDictionary_ReturnsInverse()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionaryForBidirectionalDictionary();

        var inverse = ((IBidirectionalDictionary<char, int>)readOnlyBidirectionalDictionary).Inverse;

        Assert.Same(readOnlyBidirectionalDictionary.Inverse, inverse);
    }

    private static ReadOnlyBidirectionalDictionary<char, int> CreateReadOnlyBidirectionalDictionaryForBidirectionalDictionary()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        return new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);
    }
}
