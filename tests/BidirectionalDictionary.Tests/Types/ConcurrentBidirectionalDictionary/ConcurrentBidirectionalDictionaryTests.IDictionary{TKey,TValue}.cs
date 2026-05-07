using System.Collections.Concurrent;

namespace BidirectionalDictionary.Tests.Types.ConcurrentBidirectionalDictionary;

public partial class ConcurrentBidirectionalDictionaryTests
{
    [Fact]
    public void IDictionaryTKeyTValue_Add_ConcurrentBidirectionalDictionaryAndDuplicateValue_ThrowsArgumentException()
    {
        IDictionary<char, int> dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.Add('a', 1);

        Assert.Throws<ArgumentException>(() => dictionary.Add('b', 1));
        Assert.Single(dictionary, new KeyValuePair<char, int>('a', 1));
    }

    [Fact]
    public void IDictionaryTKeyTValue_KeysAndValues_FilledConcurrentBidirectionalDictionary_ReturnSnapshots()
    {
        var concrete = new ConcurrentBidirectionalDictionary<char, int>();
        IDictionary<char, int> dictionary = concrete;
        concrete.TryAdd('a', 1);

        ICollection<char> keys = dictionary.Keys;
        ICollection<int> values = dictionary.Values;
        concrete.Clear();

        Assert.Single(keys, 'a');
        Assert.Single(values, 1);
    }

    [Fact]
    public void IDictionaryTKeyTValue_Remove_FilledConcurrentBidirectionalDictionaryAndExistingKey_RemovesBothDirections()
    {
        var concrete = new ConcurrentBidirectionalDictionary<char, int>();
        Assert.True(concrete.TryAdd('a', 1));
        IDictionary<char, int> dictionary = concrete;

        var removed = dictionary.Remove('a');

        Assert.True(removed);
        Assert.Empty(concrete);
        Assert.Empty(concrete.Inverse);
    }
}
