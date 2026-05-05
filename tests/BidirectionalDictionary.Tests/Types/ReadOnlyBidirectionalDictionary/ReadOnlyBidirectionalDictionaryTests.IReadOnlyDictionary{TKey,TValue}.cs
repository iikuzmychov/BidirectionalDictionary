using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Fact]
    public void IReadOnlyDictionary_Keys_FilledReadOnlyBidirectionalDictionary_ReturnsKeys()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionaryForReadOnlyDictionary();

        var keys = ((IReadOnlyDictionary<char, int>)readOnlyBidirectionalDictionary).Keys;

        Assert.Single(keys, 'a');
    }

    [Fact]
    public void IReadOnlyDictionary_Values_FilledReadOnlyBidirectionalDictionary_ReturnsValues()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionaryForReadOnlyDictionary();

        var values = ((IReadOnlyDictionary<char, int>)readOnlyBidirectionalDictionary).Values;

        Assert.Single(values, 0);
    }

    private static ReadOnlyBidirectionalDictionary<char, int> CreateReadOnlyBidirectionalDictionaryForReadOnlyDictionary()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        return new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);
    }
}
