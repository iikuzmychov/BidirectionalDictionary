namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.ValueCollection;

public partial class BidirectionalDictionaryValueCollectionTests
{
    [Fact]
    [Trait("Property", "Values")]
    public void Values_FilledBidirectionalDictionary_ReturnsValues()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        Assert.Equal([0, 1], bidirectionalDictionary.Values);
    }

    [Fact]
    [Trait("Constructor", "ValueCollection")]
    public void Constructor_NullBidirectionalDictionary_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new BidirectionalDictionary<char, int>.ValueCollection(null!));
    }

    [Fact]
    [Trait("Method", "ValueCollection")]
    public void Count_FilledBidirectionalDictionary_ReturnsCount()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Single(bidirectionalDictionary.Values);
    }

    [Theory]
    [Trait("Method", "ValueCollection")]
    [InlineData(0, true)]
    [InlineData(1, false)]
    public void Contains_FilledBidirectionalDictionary_ReturnsExpectedResult(int value, bool expectedResult)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var result = bidirectionalDictionary.Values.Contains(value);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    [Trait("Method", "ValueCollection")]
    public void CopyTo_FilledBidirectionalDictionary_CopiesValues()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var array = new int[3];

        bidirectionalDictionary.Values.CopyTo(array, 1);

        Assert.Equal([default, 0, 1], array);
    }

}
