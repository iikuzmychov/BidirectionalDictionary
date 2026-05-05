using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.ValueCollection;

public partial class BidirectionalDictionaryValueCollectionTests
{
    [Fact]
    public void IEnumerableT_GetEnumerator_FilledBidirectionalDictionary_EnumeratesValues()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var values = ((IEnumerable<int>)bidirectionalDictionary.Values).ToArray();

        Assert.Equal([0, 1], values);
    }

    [Fact]
    public void IEnumerable_GetEnumerator_FilledBidirectionalDictionary_EnumeratesValues()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var values = ((IEnumerable)bidirectionalDictionary.Values).Cast<int>().ToArray();

        Assert.Equal([0, 1], values);
    }
}
