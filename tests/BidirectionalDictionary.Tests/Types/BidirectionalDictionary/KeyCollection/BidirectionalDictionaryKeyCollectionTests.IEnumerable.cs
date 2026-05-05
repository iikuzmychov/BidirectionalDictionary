using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.KeyCollection;

public partial class BidirectionalDictionaryKeyCollectionTests
{
    [Fact]
    [Trait("Method", "IEnumerable<TKey>")]
    public void IEnumerableT_GetEnumerator_FilledBiDictionary_EnumeratesKeys()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var keys = ((IEnumerable<char>)biDictionary.Keys).ToArray();

        Assert.Equal(['a', 'b'], keys);
    }

    [Fact]
    [Trait("Method", "IEnumerable")]
    public void IEnumerable_GetEnumerator_FilledBiDictionary_EnumeratesKeys()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var keys = ((IEnumerable)biDictionary.Keys).Cast<char>().ToArray();

        Assert.Equal(['a', 'b'], keys);
    }
}
