using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary;

public partial class BidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "IEnumerable<KeyValuePair<TKey, TValue>>")]
    public void IEnumerableKeyValuePair_GetEnumerator_FilledBiDictionary_EnumeratesEntries()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var entries = ((IEnumerable<KeyValuePair<char, int>>)biDictionary).ToArray();

        Assert.Equal(
            [
                new KeyValuePair<char, int>('a', 0),
                new KeyValuePair<char, int>('b', 1),
            ],
            entries);
    }

    [Fact]
    [Trait("Method", "IEnumerable")]
    public void IEnumerable_GetEnumerator_FilledBiDictionary_EnumeratesEntries()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var entries = ((IEnumerable)biDictionary).Cast<KeyValuePair<char, int>>().ToArray();

        Assert.Equal(
            [
                new KeyValuePair<char, int>('a', 0),
                new KeyValuePair<char, int>('b', 1),
            ],
            entries);
    }
}
