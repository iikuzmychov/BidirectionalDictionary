namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary;

public partial class BidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "IBidirectionalDictionary<TKey, TValue>")]
    public void IBidirectionalDictionary_Inverse_FilledBidirectionalDictionary_ReturnsInverse()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var inverse = ((IBidirectionalDictionary<char, int>)bidirectionalDictionary).Inverse;

        Assert.Same(bidirectionalDictionary.Inverse, inverse);
    }
}
