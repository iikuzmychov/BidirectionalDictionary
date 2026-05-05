using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.ValueCollection;

public partial class BidirectionalDictionaryValueCollectionTests
{
    [Fact]
    [Trait("Method", "IEnumerable<TValue>")]
    public void IEnumerableT_GetEnumerator_FilledBiDictionary_EnumeratesValues()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var values = ((IEnumerable<int>)biDictionary.Values).ToArray();

        Assert.Equal([0, 1], values);
    }

    [Fact]
    [Trait("Method", "IEnumerable")]
    public void IEnumerable_GetEnumerator_FilledBiDictionary_EnumeratesValues()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var values = ((IEnumerable)biDictionary.Values).Cast<int>().ToArray();

        Assert.Equal([0, 1], values);
    }
}
