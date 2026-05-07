using System.Collections.Concurrent;

namespace BidirectionalDictionary.Tests.Types.ConcurrentBidirectionalDictionary;

public partial class ConcurrentBidirectionalDictionaryTests
{
    [Fact]
    public void ICollectionKeyValuePairTKeyTValue_Remove_FilledConcurrentBidirectionalDictionaryAndMatchingPair_RemovesBothDirections()
    {
        var dictionary = new ConcurrentBidirectionalDictionary<char, int>();
        dictionary.TryAdd('a', 1);

        var removed = ((ICollection<KeyValuePair<char, int>>)dictionary).Remove(new KeyValuePair<char, int>('a', 1));

        Assert.True(removed);
        Assert.Empty(dictionary);
        Assert.Empty(dictionary.Inverse);
    }

    [Fact]
    public void ICollectionKeyValuePairTKeyTValue_Add_EmptyConcurrentBidirectionalDictionary_AddsEntryAndInverseEntry()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>();
        var collection = (ICollection<KeyValuePair<char, int>>)concurrentBidirectionalDictionary;

        collection.Add(new KeyValuePair<char, int>('a', 0));

        Assert.Single(concurrentBidirectionalDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(concurrentBidirectionalDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
    }

    [Fact]
    public void ICollectionKeyValuePairTKeyTValue_Properties_FilledConcurrentBidirectionalDictionary_ReturnsExpectedValues()
    {
        var collection = (ICollection<KeyValuePair<char, int>>)new ConcurrentBidirectionalDictionary<char, int>
        {
            { 'a', 0 },
        };

        Assert.False(collection.IsReadOnly);
        Assert.True(collection.Contains(new KeyValuePair<char, int>('a', 0)));
        Assert.False(collection.Contains(new KeyValuePair<char, int>('a', 1)));
        Assert.False(collection.Contains(new KeyValuePair<char, int>('b', 0)));
    }

    [Fact]
    public void ICollectionKeyValuePairTKeyTValue_CopyTo_FilledConcurrentBidirectionalDictionary_CopiesPairs()
    {
        var collection = (ICollection<KeyValuePair<char, int>>)new ConcurrentBidirectionalDictionary<char, int>
        {
            { 'a', 0 },
        };
        var entries = new KeyValuePair<char, int>[2];

        collection.CopyTo(entries, 1);

        Assert.Equal(default, entries[0]);
        Assert.Equal(new KeyValuePair<char, int>('a', 0), entries[1]);
    }

    [Fact]
    public void ICollectionKeyValuePairTKeyTValue_CopyTo_NullArray_ThrowsArgumentNullException()
    {
        var collection = (ICollection<KeyValuePair<char, int>>)new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => collection.CopyTo(null!, 0));
    }

    [Fact]
    public void ICollectionKeyValuePairTKeyTValue_Remove_FilledConcurrentBidirectionalDictionaryAndMismatchedPair_ReturnsFalseAndDoesNotChangeDictionary()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>
        {
            { 'a', 0 },
        };
        var collection = (ICollection<KeyValuePair<char, int>>)concurrentBidirectionalDictionary;

        var removed = collection.Remove(new KeyValuePair<char, int>('a', 1));

        Assert.False(removed);
        Assert.Single(concurrentBidirectionalDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(concurrentBidirectionalDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
    }
}
