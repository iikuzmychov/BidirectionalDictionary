namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary;

public partial class BidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "IReadOnlyDictionary<TKey, TValue>")]
    public void IReadOnlyDictionary_Keys_FilledBidirectionalDictionary_ReturnsKeys()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var keys = ((IReadOnlyDictionary<char, int>)bidirectionalDictionary).Keys;

        Assert.Equal(['a', 'b'], keys);
    }

    [Fact]
    [Trait("Method", "IReadOnlyDictionary<TKey, TValue>")]
    public void IReadOnlyDictionary_Values_FilledBidirectionalDictionary_ReturnsValues()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var values = ((IReadOnlyDictionary<char, int>)bidirectionalDictionary).Values;

        Assert.Equal([0, 1], values);
    }
}
