using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary.KeyCollection;

public partial class ReadOnlyBidirectionalDictionaryKeyCollectionTests
{
    [Fact]
    public void Keys_FilledReadOnlyBidirectionalDictionary_ReturnsReadOnlyKeyCollection()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        Assert.IsType<ReadOnlyBidirectionalDictionary<char, int>.KeyCollection>(readOnlyBidirectionalDictionary.Keys);
        Assert.Same(readOnlyBidirectionalDictionary.Keys, readOnlyBidirectionalDictionary.Keys);
        Assert.Equal(['a', 'b'], readOnlyBidirectionalDictionary.Keys);
    }

    [Fact]
    public void Constructor_NullCollection_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new ReadOnlyBidirectionalDictionary<char, int>.KeyCollection(null!));
    }

    [Fact]
    public void Count_FilledReadOnlyBidirectionalDictionary_ReturnsCount()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        Assert.Single(readOnlyBidirectionalDictionary.Keys);
    }

    [Theory]
    [InlineData('a', true)]
    [InlineData('b', false)]
    public void Contains_FilledReadOnlyBidirectionalDictionary_ReturnsExpectedResult(char key, bool expectedResult)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var result = readOnlyBidirectionalDictionary.Keys.Contains(key);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CopyTo_FilledReadOnlyBidirectionalDictionary_CopiesKeys()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);
        var array = new char[3];

        readOnlyBidirectionalDictionary.Keys.CopyTo(array, 1);

        Assert.Equal([default, 'a', 'b'], array);
    }
}
