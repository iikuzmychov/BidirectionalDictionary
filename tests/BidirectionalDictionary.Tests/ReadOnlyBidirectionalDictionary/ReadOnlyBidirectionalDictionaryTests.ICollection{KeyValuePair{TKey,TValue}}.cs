using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Theory]
    [Trait("Method", "ICollection<KeyValuePair<TKey, TValue>>")]
    [InlineData('a', 0, true)]
    [InlineData('b', 0, false)]
    [InlineData('a', 1, false)]
    [InlineData('c', 2, false)]
    public void ICollectionKeyValuePair_Contains_FilledReadOnlyBiDictionaryAndMissingPair_ReturnsExpectedResult(
        char key, int value, bool expectedResult)
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
}