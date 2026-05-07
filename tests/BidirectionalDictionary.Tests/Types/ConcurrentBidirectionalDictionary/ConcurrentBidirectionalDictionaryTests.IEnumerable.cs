using System.Collections;
using System.Collections.Concurrent;

namespace BidirectionalDictionary.Tests.Types.ConcurrentBidirectionalDictionary;

public partial class ConcurrentBidirectionalDictionaryTests
{
    [Fact]
    public void IEnumerableKeyValuePairTKeyTValue_GetEnumerator_FilledConcurrentBidirectionalDictionary_ReturnsSnapshot()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        using var enumerator = dictionary.GetEnumerator();
        dictionary.Clear();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('a', 1), enumerator.Current);
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void IDictionaryEnumerator_Properties_StartedConcurrentBidirectionalDictionaryEnumerator_ReturnExpectedValues()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>
        {
            { 'a', 1 },
        };
        var enumerator = (IDictionaryEnumerator)dictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new DictionaryEntry('a', 1), enumerator.Entry);
        Assert.Equal('a', enumerator.Key);
        Assert.Equal(1, enumerator.Value);
        Assert.Equal(new KeyValuePair<char, int>('a', 1), enumerator.Current);
    }

    [Fact]
    public void IEnumerator_Reset_StartedConcurrentBidirectionalDictionaryEnumerator_RestartsEnumeration()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>
        {
            { 'a', 1 },
        };
        var enumerator = (IEnumerator)dictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        enumerator.Reset();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new KeyValuePair<char, int>('a', 1), enumerator.Current);
    }
}
