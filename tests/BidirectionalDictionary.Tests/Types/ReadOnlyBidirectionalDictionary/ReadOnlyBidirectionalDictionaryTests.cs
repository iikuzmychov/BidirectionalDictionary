using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    #region Constructor tests

    [Fact]
    public void Constructor_FilledSourceBidirectionalDictionary_CreatesEmptyBidirectionalDictionary()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 }
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        Assert.Equal(bidirectionalDictionary, readOnlyBidirectionalDictionary);
        Assert.Equal(bidirectionalDictionary.Keys, readOnlyBidirectionalDictionary.Keys);
        Assert.Equal(bidirectionalDictionary.Values, readOnlyBidirectionalDictionary.Values);
        Assert.Equal(bidirectionalDictionary.Keys, readOnlyBidirectionalDictionary.Inverse.Values);
        Assert.Equal(bidirectionalDictionary.Values, readOnlyBidirectionalDictionary.Inverse.Keys);
    }

    [Fact]
    public void Count_FilledReadOnlyBidirectionalDictionary_ReturnsCount()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var count = readOnlyBidirectionalDictionary.Count;

        Assert.Equal(1, count);
    }

    [Fact]
    public void Constructor_NullSourceBidirectionalDictionary_ThrowsArgumentNullException()
    {
        var bidirectionalDictionary = (BidirectionalDictionary<char, int>?)null;

        Assert.Throws<ArgumentNullException>(() => _ = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary!));
    }

    #endregion

    #region Method tests

    [Theory]
    [InlineData('a', true)]
    [InlineData('b', false)]
    [InlineData('c', false)]
    public void ContainsKey_FilledReadOnlyBidirectionalDictionaryAndExistingKey_ReturnsExpectedResult(char key, bool expectedResult)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var isExists = readOnlyBidirectionalDictionary.ContainsKey(key);

        Assert.Equal(expectedResult, isExists);
    }

    [Fact]
    public void ContainsKey_FilledReadOnlyBidirectionalDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char?, int>()
        {
            { 'a', 0 },
        };
        
        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char?, int>(bidirectionalDictionary);
#pragma warning restore CS8714

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => readOnlyBidirectionalDictionary.ContainsKey(key));
    }

    [Theory]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(2, false)]
    public void ContainsValue_FilledReadOnlyBidirectionalDictionaryAndExistingKey_ReturnsExpectedResult(int value, bool expectedResult)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var isExists = readOnlyBidirectionalDictionary.ContainsValue(value);

        Assert.Equal(expectedResult, isExists);
    }

    [Fact]
    public void ContainsValue_FilledReadOnlyBidirectionalDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char, int?>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int?>(bidirectionalDictionary);
#pragma warning restore CS8714

        var value = (char?)null;

        Assert.Throws<ArgumentNullException>(() => readOnlyBidirectionalDictionary.ContainsValue(value));
    }


    [Fact]
    public void TryGetValue_FilledReadOnlyBidirectionalDictionaryAndExistingKey_ReturnsTrueAndReturnsOutExpectedValue()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 }
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var key = 'a';
        var expectedValue = 0;

        var isExists = readOnlyBidirectionalDictionary.TryGetValue(key, out var value);

        Assert.True(isExists);
        Assert.Equal(expectedValue, value);

    }

    [Theory]
    [InlineData('a')]
    [InlineData('b')]
    public void TryGetValue_EmptyReadOnlyBidirectionalDictionaryAndMissingKey_ReturnsFalse(char key)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();
        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var isExists = readOnlyBidirectionalDictionary.TryGetValue(key, out _);

        Assert.False(isExists);
    }

    [Fact]
    public void TryGetValue_EmptyReadOnlyBidirectionalDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char?, int?>();
        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char?, int?>(bidirectionalDictionary);
#pragma warning restore CS8714

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => _ = readOnlyBidirectionalDictionary.TryGetValue(key, out _));
    }

    [Fact]
    public void GetEnumerator_FilledReadOnlyBidirectionalDictionary_EnumeratesEntries()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var entries = readOnlyBidirectionalDictionary.Cast<KeyValuePair<char, int>>().ToArray();

        Assert.Single(entries, new KeyValuePair<char, int>('a', 0));
    }

    #endregion

    #region Indexer tests

    [Fact]
    public void Indexer_Get_FilledReadOnlyBidirectionalDictionaryAndExistingKey_ReturnsExpectedValue()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var key = 'a';
        var expectedValue = 0;

        var value = readOnlyBidirectionalDictionary[key];

        Assert.Equal(expectedValue, value);
    }

    [Fact]
    public void Indexer_Get_EmptyReadOnlyBidirectionalDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char?, int>();
        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char?, int>(bidirectionalDictionary);
#pragma warning restore CS8714

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => _ = readOnlyBidirectionalDictionary[key]);
    }

    [Theory]
    [InlineData('a')]
    [InlineData('b')]
    public void Indexer_Get_EmptyReadOnlyBidirectionalDictionaryAndMissingKey_ThrowsKeyNotFoundException(char key)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();
        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        Assert.Throws<KeyNotFoundException>(() => _ = readOnlyBidirectionalDictionary[key]);
    }

    #endregion
}
