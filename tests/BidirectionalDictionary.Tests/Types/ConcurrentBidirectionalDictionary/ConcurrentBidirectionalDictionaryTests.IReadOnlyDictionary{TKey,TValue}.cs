using System.Collections.Concurrent;

namespace BidirectionalDictionary.Tests.Types.ConcurrentBidirectionalDictionary;

public partial class ConcurrentBidirectionalDictionaryTests
{
    [Fact]
    public void IReadOnlyDictionaryTKeyTValue_KeysAndValues_FilledConcurrentBidirectionalDictionary_ReturnSnapshots()
    {
        var concrete = new ConcurrentBidirectionalDictionary<char, int>();
        IReadOnlyDictionary<char, int> dictionary = concrete;
        concrete.TryAdd('a', 1);

        var keys = dictionary.Keys.ToArray();
        var values = dictionary.Values.ToArray();

        Assert.Single(keys, 'a');
        Assert.Single(values, 1);
    }
}
