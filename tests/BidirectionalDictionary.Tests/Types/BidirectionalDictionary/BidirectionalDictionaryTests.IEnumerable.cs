using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary;

public partial class BidirectionalDictionaryTests
{
    [Fact]
    public void IEnumerableKeyValuePair_GetEnumerator_FilledBidirectionalDictionary_EnumeratesEntries()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var entries = ((IEnumerable<KeyValuePair<char, int>>)bidirectionalDictionary).ToArray();

        Assert.Equal(
            [
                new KeyValuePair<char, int>('a', 0),
                new KeyValuePair<char, int>('b', 1),
            ],
            entries);
    }

    [Fact]
    public void IEnumerable_GetEnumerator_FilledBidirectionalDictionary_EnumeratesEntries()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var entries = ((IEnumerable)bidirectionalDictionary).Cast<KeyValuePair<char, int>>().ToArray();

        Assert.Equal(
            [
                new KeyValuePair<char, int>('a', 0),
                new KeyValuePair<char, int>('b', 1),
            ],
            entries);
    }
}
