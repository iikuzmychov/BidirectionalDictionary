namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary;

public partial class BidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "IDictionary<TKey, TValue>")]
    public void IDictionary_Keys_FilledBidirectionalDictionary_ReturnsKeys()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var keys = ((IDictionary<char, int>)bidirectionalDictionary).Keys;

        Assert.Equal(['a', 'b'], keys);
    }

    [Fact]
    [Trait("Method", "IDictionary<TKey, TValue>")]
    public void IDictionary_Values_FilledBidirectionalDictionary_ReturnsValues()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var values = ((IDictionary<char, int>)bidirectionalDictionary).Values;

        Assert.Equal([0, 1], values);
    }
}
