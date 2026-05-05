namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary;

public partial class BidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "IReadOnlyDictionary<TKey, TValue>")]
    public void IReadOnlyDictionary_Keys_FilledBiDictionary_ReturnsKeys()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var keys = ((IReadOnlyDictionary<char, int>)biDictionary).Keys;

        Assert.Equal(['a', 'b'], keys);
    }

    [Fact]
    [Trait("Method", "IReadOnlyDictionary<TKey, TValue>")]
    public void IReadOnlyDictionary_Values_FilledBiDictionary_ReturnsValues()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var values = ((IReadOnlyDictionary<char, int>)biDictionary).Values;

        Assert.Equal([0, 1], values);
    }
}
