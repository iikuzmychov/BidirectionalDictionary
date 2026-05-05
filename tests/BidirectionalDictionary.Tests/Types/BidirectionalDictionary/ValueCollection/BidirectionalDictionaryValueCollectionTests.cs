namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.ValueCollection;

public partial class BidirectionalDictionaryValueCollectionTests
{
    [Fact]
    [Trait("Property", "Values")]
    public void Values_FilledBiDictionary_ReturnsValues()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        Assert.Equal([0, 1], biDictionary.Values);
    }

    [Fact]
    [Trait("Constructor", "ValueCollection")]
    public void Constructor_NullBiDictionary_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new BidirectionalDictionary<char, int>.ValueCollection(null!));
    }

    [Fact]
    [Trait("Method", "ValueCollection")]
    public void Count_FilledBiDictionary_ReturnsCount()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Single(biDictionary.Values);
    }

    [Theory]
    [Trait("Method", "ValueCollection")]
    [InlineData(0, true)]
    [InlineData(1, false)]
    public void Contains_FilledBiDictionary_ReturnsExpectedResult(int value, bool expectedResult)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var result = biDictionary.Values.Contains(value);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    [Trait("Method", "ValueCollection")]
    public void CopyTo_FilledBiDictionary_CopiesValues()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var array = new int[3];

        biDictionary.Values.CopyTo(array, 1);

        Assert.Equal([default, 0, 1], array);
    }

}
