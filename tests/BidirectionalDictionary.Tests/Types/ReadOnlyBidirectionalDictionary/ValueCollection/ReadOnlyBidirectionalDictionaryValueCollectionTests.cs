using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary.ValueCollection;

public partial class ReadOnlyBidirectionalDictionaryValueCollectionTests
{
    [Fact]
    public void Values_FilledReadOnlyBidirectionalDictionary_ReturnsReadOnlyValueCollection()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        Assert.IsType<ReadOnlyBidirectionalDictionary<char, int>.ValueCollection>(readOnlyBidirectionalDictionary.Values);
        Assert.Same(readOnlyBidirectionalDictionary.Values, readOnlyBidirectionalDictionary.Values);
        Assert.Equal([0, 1], readOnlyBidirectionalDictionary.Values);
    }

    [Fact]
    public void Constructor_NullCollection_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => _ = new ReadOnlyBidirectionalDictionary<char, int>.ValueCollection(null!));
    }

    [Fact]
    public void Count_FilledReadOnlyBidirectionalDictionary_ReturnsCount()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        Assert.Single(readOnlyBidirectionalDictionary.Values);
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    public void Contains_FilledReadOnlyBidirectionalDictionary_ReturnsExpectedResult(int value, bool expectedResult)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var result = readOnlyBidirectionalDictionary.Values.Contains(value);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public void CopyTo_FilledReadOnlyBidirectionalDictionary_CopiesValues()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);
        var array = new int[3];

        readOnlyBidirectionalDictionary.Values.CopyTo(array, 1);

        Assert.Equal([default, 0, 1], array);
    }
}
