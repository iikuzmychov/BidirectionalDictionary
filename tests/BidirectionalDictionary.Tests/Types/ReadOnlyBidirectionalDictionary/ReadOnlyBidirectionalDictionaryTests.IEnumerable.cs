using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary;

public partial class ReadOnlyBidirectionalDictionaryTests
{
    [Fact]
    [Trait("Method", "IEnumerable<KeyValuePair<TKey, TValue>>")]
    public void IEnumerableKeyValuePair_GetEnumerator_FilledReadOnlyBiDictionary_EnumeratesEntries()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBiDictionary = new ReadOnlyBidirectionalDictionary<char, int>(biDictionary);

        var entries = ((IEnumerable<KeyValuePair<char, int>>)readOnlyBiDictionary).ToArray();

        Assert.Single(entries, new KeyValuePair<char, int>('a', 0));
    }
}
