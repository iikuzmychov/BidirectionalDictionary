namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary;

public partial class BidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "IBidirectionalDictionary<TKey, TValue>")]
    public void IBidirectionalDictionary_Inverse_FilledBiDictionary_ReturnsInverse()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var inverse = ((IBidirectionalDictionary<char, int>)biDictionary).Inverse;

        Assert.Same(biDictionary.Inverse, inverse);
    }
}
