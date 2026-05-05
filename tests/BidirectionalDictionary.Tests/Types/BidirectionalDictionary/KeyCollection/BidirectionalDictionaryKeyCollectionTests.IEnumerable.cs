using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.KeyCollection;

public partial class BidirectionalDictionaryKeyCollectionTests
{
    [Fact]
    [Trait("Method", "IEnumerable<TKey>")]
    public void IEnumerableT_GetEnumerator_FilledBidirectionalDictionary_EnumeratesKeys()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var keys = ((IEnumerable<char>)bidirectionalDictionary.Keys).ToArray();

        Assert.Equal(['a', 'b'], keys);
    }

    [Fact]
    [Trait("Method", "IEnumerable")]
    public void IEnumerable_GetEnumerator_FilledBidirectionalDictionary_EnumeratesKeys()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var keys = ((IEnumerable)bidirectionalDictionary.Keys).Cast<char>().ToArray();

        Assert.Equal(['a', 'b'], keys);
    }
}
