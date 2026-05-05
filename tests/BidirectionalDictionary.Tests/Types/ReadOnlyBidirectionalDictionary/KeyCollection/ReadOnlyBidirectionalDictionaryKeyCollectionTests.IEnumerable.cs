using System.Collections;
using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary.KeyCollection;

public partial class ReadOnlyBidirectionalDictionaryKeyCollectionTests
{
    [Fact]
    public void IEnumerableT_GetEnumerator_FilledReadOnlyBidirectionalDictionary_EnumeratesKeys()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var keys = ((IEnumerable<char>)readOnlyBidirectionalDictionary.Keys).ToArray();

        Assert.Equal(['a', 'b'], keys);
    }

    [Fact]
    public void IEnumerable_GetEnumerator_FilledReadOnlyBidirectionalDictionary_EnumeratesKeys()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var keys = ((IEnumerable)readOnlyBidirectionalDictionary.Keys).Cast<char>().ToArray();

        Assert.Equal(['a', 'b'], keys);
    }
}
