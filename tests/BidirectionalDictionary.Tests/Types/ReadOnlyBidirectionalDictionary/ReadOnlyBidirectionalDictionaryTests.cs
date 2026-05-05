using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    #region Constructor tests

    [Fact]
    [Trait("Constructor", null)]
    public void Constructor_FilledSourceBiDictionary_CreatesEmptyBiDictionary()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 }
        };

        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);

        Assert.Equal(biDictionary, readOnlyBiDictionary);
        Assert.Equal(biDictionary.Keys, readOnlyBiDictionary.Keys);
        Assert.Equal(biDictionary.Values, readOnlyBiDictionary.Values);
        Assert.Equal(biDictionary.Keys, readOnlyBiDictionary.Inverse.Values);
        Assert.Equal(biDictionary.Values, readOnlyBiDictionary.Inverse.Keys);
    }

    [Fact]
    [Trait("Property", null)]
    public void Count_FilledReadOnlyBiDictionary_ReturnsCount()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);

        var count = readOnlyBiDictionary.Count;

        Assert.Equal(1, count);
    }

    [Fact]
    [Trait("Constructor", null)]
    public void Constructor_NullSourceBiDictionary_ThrowsArgumentNullException()
    {
        var biDictionary = (BidirectionalDictionary<char, int>?)null;

        Assert.Throws<ArgumentNullException>(() => _ = new ReadOnlyBidirectionalDictionary<char, int>(biDictionary!));
    }

    #endregion

    #region Method tests

    [Theory]
    [Trait("Method", null)]
    [InlineData('a', true)]
    [InlineData('b', false)]
    [InlineData('c', false)]
    public void ContainsKey_FilledReadOnlyBiDictionaryAndExistingKey_ReturnsExpectedResult(char key, bool expectedResult)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);

        var isExists = readOnlyBiDictionary.ContainsKey(key);

        Assert.Equal(expectedResult, isExists);
    }

    [Fact]
    [Trait("Method", null)]
    public void ContainsKey_FilledReadOnlyBiDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var biDictionary = new BidirectionalDictionary<char?, int>()
        {
            { 'a', 0 },
        };
        
        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char?, int>(biDictionary);
#pragma warning restore CS8714

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => readOnlyBiDictionary.ContainsKey(key));
    }

    [Theory]
    [Trait("Method", null)]
    [InlineData(0, true)]
    [InlineData(1, false)]
    [InlineData(2, false)]
    public void ContainsValue_FilledReadOnlyBiDictionaryAndExistingKey_ReturnsExpectedResult(int value, bool expectedResult)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);

        var isExists = readOnlyBiDictionary.ContainsValue(value);

        Assert.Equal(expectedResult, isExists);
    }

    [Fact]
    [Trait("Method", null)]
    public void ContainsValue_FilledReadOnlyBiDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var biDictionary = new BidirectionalDictionary<char, int?>()
        {
            { 'a', 0 },
        };

        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char, int?>(biDictionary);
#pragma warning restore CS8714

        var value = (char?)null;

        Assert.Throws<ArgumentNullException>(() => readOnlyBiDictionary.ContainsValue(value));
    }


    [Fact]
    [Trait("Method", null)]
    public void TryGetValue_FilledReadOnlyBiDictionaryAndExistingKey_ReturnsTrueAndReturnsOutExpectedValue()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 }
        };

        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);

        var key           = 'a';
        var expectedValue = 0;

        var isExists = readOnlyBiDictionary.TryGetValue(key, out var value);

        Assert.True(isExists);
        Assert.Equal(expectedValue, value);

    }

    [Theory]
    [Trait("Method", null)]
    [InlineData('a')]
    [InlineData('b')]
    public void TryGetValue_EmptyReadOnlyBiDictionaryAndMissingKey_ReturnsFalse(char key)
    {
        var biDictionary         = new BidirectionalDictionary<char, int>();
        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);

        var isExists = readOnlyBiDictionary.TryGetValue(key, out _);

        Assert.False(isExists);
    }

    [Fact]
    [Trait("Method", null)]
    public void TryGetValue_EmptyReadOnlyBiDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var biDictionary         = new BidirectionalDictionary<char?, int?>();
        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char?, int?>(biDictionary);
#pragma warning restore CS8714

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => _ = readOnlyBiDictionary.TryGetValue(key, out _));
    }

    [Fact]
    [Trait("Method", null)]
    public void GetEnumerator_FilledReadOnlyBiDictionary_EnumeratesEntries()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);

        var entries = readOnlyBiDictionary.Cast<KeyValuePair<char, int>>().ToArray();

        Assert.Single(entries, new KeyValuePair<char, int>('a', 0));
    }

    #endregion

    #region Indexer tests

    [Fact]
    [Trait("Indexer", null)]
    public void Indexer_Get_FilledReadOnlyBiDictionaryAndExistingKey_ReturnsExpectedValue()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);

        var key           = 'a';
        var expectedValue = 0;

        var value = readOnlyBiDictionary[key];

        Assert.Equal(expectedValue, value);
    }

    [Fact]
    [Trait("Indexer", null)]
    public void Indexer_Get_EmptyReadOnlyBiDictionaryAndNullKey_ThrowsArgumentNullException()
    {
#pragma warning disable CS8714
        var biDictionary         = new BidirectionalDictionary<char?, int>();
        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char?, int>(biDictionary);
#pragma warning restore CS8714

        var key = (char?)null;

        Assert.Throws<ArgumentNullException>(() => _ = readOnlyBiDictionary[key]);
    }

    [Theory]
    [Trait("Indexer", null)]
    [InlineData('a')]
    [InlineData('b')]
    public void Indexer_Get_EmptyReadOnlyBiDictionaryAndMissingKey_ThrowsKeyNotFoundException(char key)
    {
        var biDictionary         = new BidirectionalDictionary<char, int>();
        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);

        Assert.Throws<KeyNotFoundException>(() => _ = readOnlyBiDictionary[key]);
    }

    #endregion
}
