using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    public void ICollectionKeyValuePair_IsReadOnly_FilledReadOnlyBiDictionary_ReturnsTrue()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionary();

        var isReadOnly = ((ICollection<KeyValuePair<char, int>>)readOnlyBiDictionary).IsReadOnly;

        Assert.True(isReadOnly);
    }

    [Fact]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    public void ICollectionKeyValuePair_Add_FilledReadOnlyBiDictionary_ThrowsNotSupportedException()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionary();

        Assert.Throws<NotSupportedException>(
            () => ((ICollection<KeyValuePair<char, int>>)readOnlyBiDictionary).Add(new KeyValuePair<char, int>('b', 1)));
    }

    [Fact]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    public void ICollectionKeyValuePair_Remove_FilledReadOnlyBiDictionary_ThrowsNotSupportedException()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionary();

        Assert.Throws<NotSupportedException>(
            () => ((ICollection<KeyValuePair<char, int>>)readOnlyBiDictionary).Remove(new KeyValuePair<char, int>('a', 0)));
    }

    [Fact]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    public void ICollectionKeyValuePair_Clear_FilledReadOnlyBiDictionary_ThrowsNotSupportedException()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionary();

        Assert.Throws<NotSupportedException>(() => ((ICollection<KeyValuePair<char, int>>)readOnlyBiDictionary).Clear());
    }

    [Fact]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    public void ICollectionKeyValuePair_CopyTo_FilledReadOnlyBiDictionary_CopiesEntries()
    {
        var readOnlyBiDictionary = CreateReadOnlyBiDictionary();
        var entries              = new KeyValuePair<char, int>[2];

        ((ICollection<KeyValuePair<char, int>>)readOnlyBiDictionary).CopyTo(entries, 1);

        Assert.Equal(default, entries[0]);
        Assert.Equal(new KeyValuePair<char, int>('a', 0), entries[1]);
    }

    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData('a', 0, true)]
    [InlineData('b', 0, false)]
    [InlineData('a', 1, false)]
    [InlineData('c', 2, false)]
    public void ICollectionKeyValuePair_Contains_FilledReadOnlyBiDictionaryAndMissingPair_ReturnsExpectedResult(
        char key,
        int value,
        bool expectedResult)
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);
        var pair                 = new KeyValuePair<char, int>(key, value);

        var isExists = ((ICollection<KeyValuePair<char, int>>)readOnlyBiDictionary).Contains(pair);

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

        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char?, int?>(biDictionary);
#pragma warning restore CS8714

        var pair = new KeyValuePair<char?, int?>(key, value);

        Assert.Throws<ArgumentNullException>(() => ((ICollection<KeyValuePair<char?, int?>>)readOnlyBiDictionary).Contains(pair));
    }

    private static ReadOnlyBidirectionalDictionary<char, int> CreateReadOnlyBiDictionary()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        return new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);
    }
}
