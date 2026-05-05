using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "IEnumerable<KeyValuePair<TKey, TValue>>")]
    public void IEnumerableKeyValuePair_GetEnumerator_FilledReadOnlyBidirectionalDictionary_EnumeratesEntries()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var entries = ((IEnumerable<KeyValuePair<char, int>>)readOnlyBidirectionalDictionary).ToArray();

        Assert.Single(entries, new KeyValuePair<char, int>('a', 0));
    }
}
