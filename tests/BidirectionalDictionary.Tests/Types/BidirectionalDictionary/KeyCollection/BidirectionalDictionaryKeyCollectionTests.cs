namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.KeyCollection;

public partial class BidirectionalDictionaryKeyCollectionTests
{
    [Fact]
    [Trait("Property", "Keys")]
    public void Keys_FilledBiDictionary_ReturnsKeys()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        Assert.Equal(['a', 'b'], biDictionary.Keys);
    }

    [Fact]
    [Trait("Constructor", "KeyCollection")]
    public void Constructor_NullBiDictionary_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new BidirectionalDictionary<char, int>.KeyCollection(null!));
    }

    [Fact]
    [Trait("Method", "KeyCollection")]
    public void Count_FilledBiDictionary_ReturnsCount()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Single(biDictionary.Keys);
    }

    [Theory]
    [Trait("Method", "KeyCollection")]
    [InlineData('a', true)]
    [InlineData('b', false)]
    public void Contains_FilledBiDictionary_ReturnsExpectedResult(char key, bool expectedResult)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var result = biDictionary.Keys.Contains(key);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    [Trait("Method", "KeyCollection")]
    public void CopyTo_FilledBiDictionary_CopiesKeys()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var array = new char[3];

        biDictionary.Keys.CopyTo(array, 1);

        Assert.Equal([default, 'a', 'b'], array);
    }

}
