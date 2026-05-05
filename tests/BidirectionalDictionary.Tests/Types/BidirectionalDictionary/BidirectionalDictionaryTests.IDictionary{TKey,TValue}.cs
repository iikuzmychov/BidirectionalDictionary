namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary;

public partial class BidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "IDictionary<TKey, TValue>")]
    public void IDictionary_Keys_FilledBiDictionary_ReturnsKeys()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var keys = ((IDictionary<char, int>)biDictionary).Keys;

        Assert.Equal(['a', 'b'], keys);
    }

    [Fact]
    [Trait("Method", "IDictionary<TKey, TValue>")]
    public void IDictionary_Values_FilledBiDictionary_ReturnsValues()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var values = ((IDictionary<char, int>)biDictionary).Values;

        Assert.Equal([0, 1], values);
    }
}
