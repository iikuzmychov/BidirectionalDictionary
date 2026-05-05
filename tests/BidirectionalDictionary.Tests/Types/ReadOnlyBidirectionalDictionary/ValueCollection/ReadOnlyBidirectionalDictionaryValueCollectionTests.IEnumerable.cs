using System.Collections;
using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary.ValueCollection;

public partial class ReadOnlyBidirectionalDictionaryValueCollectionTests
{
    [Fact]
    public void IEnumerableT_GetEnumerator_FilledReadOnlyBidirectionalDictionary_EnumeratesValues()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var values = ((IEnumerable<int>)readOnlyBidirectionalDictionary.Values).ToArray();

        Assert.Equal([0, 1], values);
    }

    [Fact]
    public void IEnumerable_GetEnumerator_FilledReadOnlyBidirectionalDictionary_EnumeratesValues()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var values = ((IEnumerable)readOnlyBidirectionalDictionary.Values).Cast<int>().ToArray();

        Assert.Equal([0, 1], values);
    }
}
