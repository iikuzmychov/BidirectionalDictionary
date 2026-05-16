namespace BidirectionalDictionary.Tests;

public partial class BidirectionalDictionaryTests
{
    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData('a', 0)]
    [InlineData('b', 1)]
    public void ICollectionKeyValuePair_Add_EmptyBiDictionaryAndNonDuplicatePair_AddsEntrySuccessfully(char key, int value)
    {
        var biDictionary = new BidirectionalDictionary<char, int>();
        var pair         = new KeyValuePair<char, int>(key, value);

        ((ICollection<KeyValuePair<char, int>>)biDictionary).Add(pair);

        Assert.Single(biDictionary, pair);
        Assert.Single(biDictionary.Keys, key);
        Assert.Single(biDictionary.Values, value);
        Assert.Single(biDictionary.Inverse, new KeyValuePair<int, char>(value, key));
        Assert.Single(biDictionary.Inverse.Keys, value);
        Assert.Single(biDictionary.Inverse.Values, key);
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData(null, null)]
    [InlineData(null, 0)]
    [InlineData('a', null)]
    public void ICollectionKeyValuePair_Add_EmptyBiDictionaryAndPairWithNullKeyOrValue_ThrowsArgumentNullException(char? key, int? value)
    {
#pragma warning disable CS8714
        var biDictionary = new BidirectionalDictionary<char?, int?>();
#pragma warning restore CS8714

        var pair = new KeyValuePair<char?, int?>(key, value);
        
        Assert.Throws<ArgumentNullException>(() => ((ICollection<KeyValuePair<char?, int?>>)biDictionary).Add(pair));

        // checking that biDictionary has not changed
        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Keys);
        Assert.Empty(biDictionary.Values);
        Assert.Empty(biDictionary.Inverse.Keys);
        Assert.Empty(biDictionary.Inverse.Values);
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData('a', 0)]
    [InlineData('a', 1)]
    [InlineData('b', 0)]
    public void ICollectionKeyValuePair_Add_FilledBiDictionaryAndDuplicatePair_ThrowsArgumentException(char key, int value)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };
        
        var pair = new KeyValuePair<char, int>(key, value);

        Assert.Throws<ArgumentException>(() => ((ICollection<KeyValuePair<char, int>>)biDictionary).Add(pair));

        // checking that biDictionary has not changed
        Assert.Single(biDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(biDictionary.Keys, 'a');
        Assert.Single(biDictionary.Values, 0);
        Assert.Single(biDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(biDictionary.Inverse.Keys, 0);
        Assert.Single(biDictionary.Inverse.Values, 'a');
    }

    [Fact]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    public void ICollectionKeyValuePair_Remove_FilledBiDictionaryAndExistingPair_RemovesEntrySuccessfullyAndReturnsTrue()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var pair = new KeyValuePair<char, int>('a', 0);

        var isRemoved = ((ICollection<KeyValuePair<char, int>>)biDictionary).Remove(pair);

        Assert.True(isRemoved);
        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Keys);
        Assert.Empty(biDictionary.Values);
        Assert.Empty(biDictionary.Inverse.Keys);
        Assert.Empty(biDictionary.Inverse.Values);
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData('a', 1)]
    [InlineData('b', 0)]
    [InlineData('c', 2)]
    public void ICollectionKeyValuePair_Remove_FilledBiDictionaryAndMissingPair_ReturnsFalse(char key, int value)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };
        
        var pair = new KeyValuePair<char, int>(key, value);

        var isRemoved = ((ICollection<KeyValuePair<char, int>>)biDictionary).Remove(pair);

        Assert.False(isRemoved);

        // checking that biDictionary has not changed
        Assert.Single(biDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(biDictionary.Keys, 'a');
        Assert.Single(biDictionary.Values, 0);
        Assert.Single(biDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
        Assert.Single(biDictionary.Inverse.Keys, 0);
        Assert.Single(biDictionary.Inverse.Values, 'a');
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData(null, 0)]
    [InlineData('a', null)]
    [InlineData(null, null)]
    public void ICollectionKeyValuePair_Remove_EmptyBiDictionaryAndPairWithNullKeyOrValue_ThrowsArgumentNullException(char? key, int? value)
    {
#pragma warning disable CS8714
        var biDictionary = new BidirectionalDictionary<char?, int?>();
#pragma warning restore CS8714

        var pair = new KeyValuePair<char?, int?>(key, value);

        Assert.Throws<ArgumentNullException>(() => ((ICollection<KeyValuePair<char?, int?>>)biDictionary).Remove(pair));

        // checking that biDictionary has not changed
        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Keys);
        Assert.Empty(biDictionary.Values);
        Assert.Empty(biDictionary.Inverse.Keys);
        Assert.Empty(biDictionary.Inverse.Values);
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData('a', 0, true)]
    [InlineData('b', 0, false)]
    [InlineData('a', 1, false)]
    [InlineData('c', 2, false)]
    public void ICollectionKeyValuePair_Contains_FilledBiDictionaryAndMissingPair_ReturnsExpectedResult(
        char key, int value, bool expectedResult)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var pair = new KeyValuePair<char, int>(key, value);

        var isExists = ((ICollection<KeyValuePair<char, int>>)biDictionary).Contains(pair);

        Assert.Equal(expectedResult, isExists);
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData(null, 0)]
    [InlineData('a', null)]
    [InlineData(null, null)]
    public void ICollectionKeyValuePair_Contains_FilledBiDictionaryAndPairWithNullKeyOrValue_ThrowsArgumentException(char? key, int? value)
    {
#pragma warning disable CS8714
        var biDictionary = new BidirectionalDictionary<char?, int?>()
        {
            { 'a', 0 },
        };
#pragma warning restore CS8714

        var pair = new KeyValuePair<char?, int?>(key, value);

        Assert.Throws<ArgumentNullException>(() => ((ICollection<KeyValuePair<char?, int?>>)biDictionary).Contains(pair));
    }

    [Fact]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    public void ICollectionKeyValuePair_Contains_CustomValueComparer_UsesValueComparer()
    {
        var biDictionary = new BidirectionalDictionary<string, string>(
            keyComparer: null,
            valueComparer: StringComparer.OrdinalIgnoreCase)
        {
            { "x", "abc" },
        };

        var collection = (ICollection<KeyValuePair<string, string>>)biDictionary;

        Assert.True(collection.Contains(new KeyValuePair<string, string>("x", "ABC")));
        Assert.True(collection.Contains(new KeyValuePair<string, string>("x", "abc")));
        Assert.False(collection.Contains(new KeyValuePair<string, string>("x", "xyz")));
    }

    [Fact]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    public void ICollectionKeyValuePair_Contains_ReferenceEqualityValueComparer_DistinguishesInstances()
    {
        var s1 = new string("abc".ToCharArray());
        var s2 = new string("abc".ToCharArray());

        var biDictionary = new BidirectionalDictionary<int, string>(
            keyComparer: null,
            valueComparer: ReferenceEqualityComparer.Instance)
        {
            { 1, s1 },
        };

        var collection = (ICollection<KeyValuePair<int, string>>)biDictionary;

        Assert.True(collection.Contains(new KeyValuePair<int, string>(1, s1)));
        Assert.False(collection.Contains(new KeyValuePair<int, string>(1, s2)));
    }

    [Fact]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    public void ICollectionKeyValuePair_Remove_ReferenceEqualityValueComparer_DoesNotDesyncMaps()
    {
        var s1 = new string("abc".ToCharArray());
        var s2 = new string("abc".ToCharArray());

        var biDictionary = new BidirectionalDictionary<int, string>(
            keyComparer: null,
            valueComparer: ReferenceEqualityComparer.Instance)
        {
            { 1, s1 },
        };

        var collection = (ICollection<KeyValuePair<int, string>>)biDictionary;

        var isRemoved = collection.Remove(new KeyValuePair<int, string>(1, s2));

        Assert.False(isRemoved);
        Assert.Single(biDictionary, new KeyValuePair<int, string>(1, s1));
        Assert.Single(biDictionary.Inverse.Keys, s1);
        Assert.Equal(biDictionary.Count, biDictionary.Inverse.Count);
    }

    [Fact]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    public void ICollectionKeyValuePair_Remove_CustomValueComparerMatchingPair_RemovesFromBothMaps()
    {
        var biDictionary = new BidirectionalDictionary<string, string>(
            keyComparer: null,
            valueComparer: StringComparer.OrdinalIgnoreCase)
        {
            { "x", "abc" },
        };

        var collection = (ICollection<KeyValuePair<string, string>>)biDictionary;

        var isRemoved = collection.Remove(new KeyValuePair<string, string>("x", "ABC"));

        Assert.True(isRemoved);
        Assert.Empty(biDictionary);
        Assert.Empty(biDictionary.Inverse);
    }
}