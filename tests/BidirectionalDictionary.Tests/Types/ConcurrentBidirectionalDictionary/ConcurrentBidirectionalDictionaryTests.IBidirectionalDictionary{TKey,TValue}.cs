using System.Collections.Concurrent;

namespace BidirectionalDictionary.Tests.Types.ConcurrentBidirectionalDictionary;

public partial class ConcurrentBidirectionalDictionaryTests
{
    [Fact]
    public void IBidirectionalDictionaryTKeyTValue_Inverse_FilledConcurrentBidirectionalDictionary_ReturnsInverse()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>();
        concurrentBidirectionalDictionary.TryAdd('a', 0);

        var inverse = ((IBidirectionalDictionary<char, int>)concurrentBidirectionalDictionary).Inverse;

        Assert.Same(concurrentBidirectionalDictionary.Inverse, inverse);
        Assert.Single(inverse, new KeyValuePair<int, char>(0, 'a'));
    }
}
