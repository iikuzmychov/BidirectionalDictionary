namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary;

public partial class BidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    public void ICollectionKeyValuePair_IsReadOnly_FilledBidirectionalDictionary_ReturnsFalse()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isReadOnly = ((ICollection<KeyValuePair<char, int>>)bidirectionalDictionary).IsReadOnly;

        Assert.False(isReadOnly);
    }

    [Fact]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    public void ICollectionKeyValuePair_CopyTo_FilledBidirectionalDictionary_CopiesEntries()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var entries = new KeyValuePair<char, int>[3];

        ((ICollection<KeyValuePair<char, int>>)bidirectionalDictionary).CopyTo(entries, 1);

        Assert.Equal(default, entries[0]);
        Assert.Equal(new KeyValuePair<char, int>('a', 0), entries[1]);
        Assert.Equal(new KeyValuePair<char, int>('b', 1), entries[2]);
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData('a', 0)]
    [InlineData('b', 1)]
    public void ICollectionKeyValuePair_Add_EmptyBidirectionalDictionaryAndNonDuplicatePair_AddsEntrySuccessfully(char key, int value)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();
        var pair = new KeyValuePair<char, int>(key, value);

        ((ICollection<KeyValuePair<char, int>>)bidirectionalDictionary).Add(pair);

        Assert.Single(bidirectionalDictionary, pair);
        Assert.Single(bidirectionalDictionary.Keys, key);
        Assert.Single(bidirectionalDictionary.Values, value);
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(value, key));
        Assert.Single(bidirectionalDictionary.Inverse.Keys, value);
        Assert.Single(bidirectionalDictionary.Inverse.Values, key);
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData(null, null)]
    [InlineData(null, 0)]
    [InlineData('a', null)]
    public void ICollectionKeyValuePair_Add_EmptyBidirectionalDictionaryAndPairWithNullKeyOrValue_ThrowsArgumentNullException(char? key, int? value)
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char?, int?>();
#pragma warning restore CS8714

        var pair = new KeyValuePair<char?, int?>(key, value);
        
        Assert.Throws<ArgumentNullException>(() => ((ICollection<KeyValuePair<char?, int?>>)bidirectionalDictionary).Add(pair));

        // checking that bidirectionalDictionary has not changed
        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Keys);
        Assert.Empty(bidirectionalDictionary.Values);
        Assert.Empty(bidirectionalDictionary.Inverse.Keys);
        Assert.Empty(bidirectionalDictionary.Inverse.Values);
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData('a', 0)]
    [InlineData('a', 1)]
    [InlineData('b', 0)]
    public void ICollectionKeyValuePair_Add_FilledBidirectionalDictionaryAndDuplicatePair_ThrowsArgumentException(char key, int value)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };
        
        var pair = new KeyValuePair<char, int>(key, value);

        Assert.Throws<ArgumentException>(() => ((ICollection<KeyValuePair<char, int>>)bidirectionalDictionary).Add(pair));

        // checking that bidirectionalDictionary has not changed
        Assert.Single(bidirectionalDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(bidirectionalDictionary.Keys, 'a');
        Assert.Single(bidirectionalDictionary.Values, 0);
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(bidirectionalDictionary.Inverse.Keys, 0);
        Assert.Single(bidirectionalDictionary.Inverse.Values, 'a');
    }

    [Fact]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    public void ICollectionKeyValuePair_Remove_FilledBidirectionalDictionaryAndExistingPair_RemovesEntrySuccessfullyAndReturnsTrue()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var pair = new KeyValuePair<char, int>('a', 0);

        var isRemoved = ((ICollection<KeyValuePair<char, int>>)bidirectionalDictionary).Remove(pair);

        Assert.True(isRemoved);
        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Keys);
        Assert.Empty(bidirectionalDictionary.Values);
        Assert.Empty(bidirectionalDictionary.Inverse.Keys);
        Assert.Empty(bidirectionalDictionary.Inverse.Values);
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData('a', 1)]
    [InlineData('b', 0)]
    [InlineData('c', 2)]
    public void ICollectionKeyValuePair_Remove_FilledBidirectionalDictionaryAndMissingPair_ReturnsFalse(char key, int value)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };
        
        var pair = new KeyValuePair<char, int>(key, value);

        var isRemoved = ((ICollection<KeyValuePair<char, int>>)bidirectionalDictionary).Remove(pair);

        Assert.False(isRemoved);

        // checking that bidirectionalDictionary has not changed
        Assert.Single(bidirectionalDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(bidirectionalDictionary.Keys, 'a');
        Assert.Single(bidirectionalDictionary.Values, 0);
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(bidirectionalDictionary.Inverse.Keys, 0);
        Assert.Single(bidirectionalDictionary.Inverse.Values, 'a');
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData(null, 0)]
    [InlineData('a', null)]
    [InlineData(null, null)]
    public void ICollectionKeyValuePair_Remove_EmptyBidirectionalDictionaryAndPairWithNullKeyOrValue_ThrowsArgumentNullException(char? key, int? value)
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char?, int?>();
#pragma warning restore CS8714

        var pair = new KeyValuePair<char?, int?>(key, value);

        Assert.Throws<ArgumentNullException>(() => ((ICollection<KeyValuePair<char?, int?>>)bidirectionalDictionary).Remove(pair));

        // checking that bidirectionalDictionary has not changed
        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Keys);
        Assert.Empty(bidirectionalDictionary.Values);
        Assert.Empty(bidirectionalDictionary.Inverse.Keys);
        Assert.Empty(bidirectionalDictionary.Inverse.Values);
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData('a', 0, true)]
    [InlineData('b', 0, false)]
    [InlineData('a', 1, false)]
    [InlineData('c', 2, false)]
    public void ICollectionKeyValuePair_Contains_FilledBidirectionalDictionaryAndMissingPair_ReturnsExpectedResult(
        char key, int value, bool expectedResult)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var pair = new KeyValuePair<char, int>(key, value);

        var isExists = ((ICollection<KeyValuePair<char, int>>)bidirectionalDictionary).Contains(pair);

        Assert.Equal(expectedResult, isExists);
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData(null, 0)]
    [InlineData('a', null)]
    [InlineData(null, null)]
    public void ICollectionKeyValuePair_Contains_FilledBidirectionalDictionaryAndPairWithNullKeyOrValue_ThrowsArgumentException(char? key, int? value)
    {
#pragma warning disable CS8714
        var bidirectionalDictionary = new BidirectionalDictionary<char?, int?>()
        {
            { 'a', 0 },
        };
#pragma warning restore CS8714

        var pair = new KeyValuePair<char?, int?>(key, value);

        Assert.Throws<ArgumentNullException>(() => ((ICollection<KeyValuePair<char?, int?>>)bidirectionalDictionary).Contains(pair));
    }
}
