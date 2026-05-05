using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Fact]
    public void ICollectionKeyValuePair_IsReadOnly_FilledReadOnlyBidirectionalDictionary_ReturnsTrue()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionary();

        var isReadOnly = ((ICollection<KeyValuePair<char, int>>)readOnlyBidirectionalDictionary).IsReadOnly;

        Assert.True(isReadOnly);
    }

    [Fact]
    public void ICollectionKeyValuePair_Add_FilledReadOnlyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionary();

        Assert.Throws<NotSupportedException>(
            () => ((ICollection<KeyValuePair<char, int>>)readOnlyBidirectionalDictionary).Add(new KeyValuePair<char, int>('b', 1)));
    }

    [Fact]
    public void ICollectionKeyValuePair_Remove_FilledReadOnlyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionary();

        Assert.Throws<NotSupportedException>(
            () => ((ICollection<KeyValuePair<char, int>>)readOnlyBidirectionalDictionary).Remove(new KeyValuePair<char, int>('a', 0)));
    }

    [Fact]
    public void ICollectionKeyValuePair_Clear_FilledReadOnlyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionary();

        Assert.Throws<NotSupportedException>(() => ((ICollection<KeyValuePair<char, int>>)readOnlyBidirectionalDictionary).Clear());
    }

    [Fact]
    public void ICollectionKeyValuePair_CopyTo_FilledReadOnlyBidirectionalDictionary_CopiesEntries()
    {
        var readOnlyBidirectionalDictionary = CreateReadOnlyBidirectionalDictionary();
        var entries = new KeyValuePair<char, int>[2];

        ((ICollection<KeyValuePair<char, int>>)readOnlyBidirectionalDictionary).CopyTo(entries, 1);

        Assert.Equal(default, entries[0]);
        Assert.Equal(new KeyValuePair<char, int>('a', 0), entries[1]);
    }

    [Theory]
    [InlineData('a', 0, true)]
    [InlineData('b', 0, false)]
    [InlineData('a', 1, false)]
    [InlineData('c', 2, false)]
    public void ICollectionKeyValuePair_Contains_FilledReadOnlyBidirectionalDictionaryAndMissingPair_ReturnsExpectedResult(
        char key,
        int value,
        bool expectedResult)
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);
        var pair = new KeyValuePair<char, int>(key, value);

        var isExists = ((ICollection<KeyValuePair<char, int>>)readOnlyBidirectionalDictionary).Contains(pair);

        Assert.Equal(expectedResult, isExists);
    }

    [Theory]
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

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char?, int?>(bidirectionalDictionary);
#pragma warning restore CS8714

        var pair = new KeyValuePair<char?, int?>(key, value);

        Assert.Throws<ArgumentNullException>(() => ((ICollection<KeyValuePair<char?, int?>>)readOnlyBidirectionalDictionary).Contains(pair));
    }

    private static ReadOnlyBidirectionalDictionary<char, int> CreateReadOnlyBidirectionalDictionary()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        return new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);
    }
}
