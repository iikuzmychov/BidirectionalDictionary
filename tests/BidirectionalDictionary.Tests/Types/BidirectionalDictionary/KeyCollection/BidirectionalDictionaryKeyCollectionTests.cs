namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.KeyCollection;

public partial class BidirectionalDictionaryKeyCollectionTests
{
    [Fact]
    public void Keys_FilledBidirectionalDictionary_ReturnsKeys()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        Assert.Equal(['a', 'b'], bidirectionalDictionary.Keys);
    }

    [Fact]
    public void Constructor_NullBidirectionalDictionary_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new BidirectionalDictionary<char, int>.KeyCollection(null!));
    }

    [Fact]
    public void Count_FilledBidirectionalDictionary_ReturnsCount()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Single(bidirectionalDictionary.Keys);
    }

    [Theory]
    [InlineData('a', true)]
    [InlineData('b', false)]
    public void Contains_FilledBidirectionalDictionary_ReturnsExpectedResult(char key, bool expectedResult)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var result = bidirectionalDictionary.Keys.Contains(key);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CopyTo_FilledBidirectionalDictionary_CopiesKeys()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var array = new char[3];

        bidirectionalDictionary.Keys.CopyTo(array, 1);

        Assert.Equal([default, 'a', 'b'], array);
    }

}
