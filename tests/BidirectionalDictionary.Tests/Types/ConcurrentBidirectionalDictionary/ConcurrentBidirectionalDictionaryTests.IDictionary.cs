using System.Collections;
using System.Collections.Concurrent;

namespace BidirectionalDictionary.Tests.Types.ConcurrentBidirectionalDictionary;

public partial class ConcurrentBidirectionalDictionaryTests
{
    [Fact]
    public void IDictionary_Properties_FilledConcurrentBidirectionalDictionary_ReturnsExpectedValues()
    {
        var concurrentBidirectionalDictionary = CreateConcurrentBidirectionalDictionaryForDictionary();
        var dictionary = (IDictionary)concurrentBidirectionalDictionary;

        Assert.False(dictionary.IsFixedSize);
        Assert.False(dictionary.IsReadOnly);
        Assert.False(((ICollection)dictionary).IsSynchronized);
        Assert.Throws<NotSupportedException>(() => ((ICollection)dictionary).SyncRoot);
        Assert.Equal(['a'], dictionary.Keys.Cast<char>());
        Assert.Equal([0], dictionary.Values.Cast<int>());
    }

    [Fact]
    public void IDictionary_Add_EmptyConcurrentBidirectionalDictionary_AddsEntryAndInverseEntry()
    {
        var concurrentBidirectionalDictionary = new ConcurrentBidirectionalDictionary<char, int>();
        var dictionary = (IDictionary)concurrentBidirectionalDictionary;

        dictionary.Add('a', 0);

        Assert.Single(concurrentBidirectionalDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(concurrentBidirectionalDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
    }

    [Fact]
    public void IDictionary_Add_WrongKeyType_ThrowsArgumentException()
    {
        var dictionary = (IDictionary)new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentException>(() => dictionary.Add("a", 0));
    }

    [Fact]
    public void IDictionary_Add_NullKey_ThrowsArgumentNullException()
    {
        var dictionary = (IDictionary)new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary.Add(null!, 0));
    }

    [Fact]
    public void IDictionary_Add_WrongValueType_ThrowsArgumentException()
    {
        var dictionary = (IDictionary)new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentException>(() => dictionary.Add('a', "0"));
    }

    [Fact]
    public void IDictionary_Add_NullValue_ThrowsArgumentException()
    {
        var dictionary = (IDictionary)new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentException>(() => dictionary.Add('a', null));
    }

    [Fact]
    public void IDictionary_Contains_FilledConcurrentBidirectionalDictionary_ReturnsExpectedResult()
    {
        var dictionary = (IDictionary)CreateConcurrentBidirectionalDictionaryForDictionary();

        Assert.True(dictionary.Contains('a'));
        Assert.False(dictionary.Contains('b'));
        Assert.False(dictionary.Contains("a"));
        Assert.Throws<ArgumentNullException>(() => dictionary.Contains(null!));
    }

    [Fact]
    public void IDictionary_IndexerGet_FilledConcurrentBidirectionalDictionary_ReturnsExpectedValue()
    {
        var dictionary = (IDictionary)CreateConcurrentBidirectionalDictionaryForDictionary();

        Assert.Equal(0, dictionary['a']);
        Assert.Null(dictionary['b']);
        Assert.Null(dictionary["a"]);
        Assert.Throws<ArgumentNullException>(() => dictionary[null!]);
    }

    [Fact]
    public void IDictionary_IndexerSet_FilledConcurrentBidirectionalDictionary_UpdatesEntryAndInverseEntry()
    {
        var concurrentBidirectionalDictionary = CreateConcurrentBidirectionalDictionaryForDictionary();
        var dictionary = (IDictionary)concurrentBidirectionalDictionary;

        dictionary['a'] = 1;

        Assert.Single(concurrentBidirectionalDictionary, new KeyValuePair<char, int>('a', 1));
        Assert.Single(concurrentBidirectionalDictionary.Inverse, new KeyValuePair<int, char>(1, 'a'));
    }

    [Fact]
    public void IDictionary_IndexerSet_FilledConcurrentBidirectionalDictionaryAndNewKey_AddsEntryAndInverseEntry()
    {
        var concurrentBidirectionalDictionary = CreateConcurrentBidirectionalDictionaryForDictionary();
        var dictionary = (IDictionary)concurrentBidirectionalDictionary;

        dictionary['b'] = 1;

        Assert.Equal(2, concurrentBidirectionalDictionary.Count);
        Assert.Contains(new KeyValuePair<char, int>('a', 0), concurrentBidirectionalDictionary);
        Assert.Contains(new KeyValuePair<char, int>('b', 1), concurrentBidirectionalDictionary);
        Assert.Equal(2, concurrentBidirectionalDictionary.Inverse.Count);
        Assert.Contains(new KeyValuePair<int, char>(0, 'a'), concurrentBidirectionalDictionary.Inverse);
        Assert.Contains(new KeyValuePair<int, char>(1, 'b'), concurrentBidirectionalDictionary.Inverse);
    }

    [Fact]
    public void IDictionary_IndexerSet_NullKey_ThrowsArgumentNullException()
    {
        var dictionary = (IDictionary)new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary[null!] = 0);
    }

    [Fact]
    public void IDictionary_IndexerSet_NullValue_ThrowsArgumentException()
    {
        var dictionary = (IDictionary)new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentException>(() => dictionary['a'] = null);
    }

    [Fact]
    public void IDictionary_IndexerSet_WrongKeyType_ThrowsArgumentException()
    {
        var dictionary = (IDictionary)new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentException>(() => dictionary["a"] = 0);
    }

    [Fact]
    public void IDictionary_IndexerSet_WrongValueType_ThrowsArgumentException()
    {
        var dictionary = (IDictionary)new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentException>(() => dictionary['a'] = "0");
    }

    [Fact]
    public void IDictionary_Remove_FilledConcurrentBidirectionalDictionaryAndExistingKey_RemovesEntryAndInverseEntry()
    {
        var concurrentBidirectionalDictionary = CreateConcurrentBidirectionalDictionaryForDictionary();
        var dictionary = (IDictionary)concurrentBidirectionalDictionary;

        dictionary.Remove('a');

        Assert.Empty(concurrentBidirectionalDictionary);
        Assert.Empty(concurrentBidirectionalDictionary.Inverse);
    }

    [Fact]
    public void IDictionary_Remove_FilledConcurrentBidirectionalDictionaryAndWrongKeyType_DoesNotChangeDictionary()
    {
        var concurrentBidirectionalDictionary = CreateConcurrentBidirectionalDictionaryForDictionary();
        var dictionary = (IDictionary)concurrentBidirectionalDictionary;

        dictionary.Remove("a");

        Assert.Single(concurrentBidirectionalDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(concurrentBidirectionalDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
    }

    [Fact]
    public void IDictionary_Remove_NullKey_ThrowsArgumentNullException()
    {
        var dictionary = (IDictionary)new ConcurrentBidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary.Remove(null!));
    }

    [Fact]
    public void IDictionary_Clear_FilledConcurrentBidirectionalDictionary_ClearsDictionaryAndInverseDictionary()
    {
        var concurrentBidirectionalDictionary = CreateConcurrentBidirectionalDictionaryForDictionary();
        var dictionary = (IDictionary)concurrentBidirectionalDictionary;

        dictionary.Clear();

        Assert.Empty(concurrentBidirectionalDictionary);
        Assert.Empty(concurrentBidirectionalDictionary.Inverse);
    }

    [Fact]
    public void IDictionary_GetEnumerator_FilledConcurrentBidirectionalDictionary_EnumeratesDictionaryEntries()
    {
        var dictionary = (IDictionary)CreateConcurrentBidirectionalDictionaryForDictionary();

        var enumerator = dictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new DictionaryEntry('a', 0), enumerator.Entry);
        Assert.Equal('a', enumerator.Key);
        Assert.Equal(0, enumerator.Value);
        Assert.Equal(new DictionaryEntry('a', 0), enumerator.Current);
        enumerator.Reset();
        Assert.True(enumerator.MoveNext());
    }

    [Fact]
    public void ICollection_CopyTo_FilledConcurrentBidirectionalDictionaryAndDictionaryEntryArray_CopiesEntries()
    {
        var dictionary = (ICollection)CreateConcurrentBidirectionalDictionaryForDictionary();
        var entries = new DictionaryEntry[2];

        dictionary.CopyTo(entries, 1);

        Assert.Equal(default, entries[0]);
        Assert.Equal(new DictionaryEntry('a', 0), entries[1]);
    }

    [Fact]
    public void ICollection_CopyTo_FilledConcurrentBidirectionalDictionaryAndObjectArray_CopiesPairs()
    {
        var dictionary = (ICollection)CreateConcurrentBidirectionalDictionaryForDictionary();
        var entries = new object[2];

        dictionary.CopyTo(entries, 1);

        Assert.Null(entries[0]);
        Assert.Equal(new KeyValuePair<char, int>('a', 0), entries[1]);
    }

    [Fact]
    public void ICollection_CopyTo_FilledConcurrentBidirectionalDictionaryAndKeyValuePairArray_CopiesPairs()
    {
        var dictionary = (ICollection)CreateConcurrentBidirectionalDictionaryForDictionary();
        var entries = new KeyValuePair<char, int>[2];

        dictionary.CopyTo(entries, 1);

        Assert.Equal(default, entries[0]);
        Assert.Equal(new KeyValuePair<char, int>('a', 0), entries[1]);
    }

    [Fact]
    public void ICollection_CopyTo_FilledConcurrentBidirectionalDictionaryAndNullArray_ThrowsArgumentNullException()
    {
        var dictionary = (ICollection)CreateConcurrentBidirectionalDictionaryForDictionary();

        Assert.Throws<ArgumentNullException>(() => dictionary.CopyTo(null!, 0));
    }

    [Fact]
    public void ICollection_CopyTo_FilledConcurrentBidirectionalDictionaryAndOutOfRangeIndex_ThrowsArgumentOutOfRangeException()
    {
        var dictionary = (ICollection)CreateConcurrentBidirectionalDictionaryForDictionary();
        var entries = new DictionaryEntry[1];

        Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.CopyTo(entries, -1));
    }

    [Fact]
    public void ICollection_CopyTo_FilledConcurrentBidirectionalDictionaryAndSmallArray_ThrowsArgumentException()
    {
        var dictionary = (ICollection)CreateConcurrentBidirectionalDictionaryForDictionary();
        var entries = Array.Empty<DictionaryEntry>();

        Assert.Throws<ArgumentException>(() => dictionary.CopyTo(entries, 0));
    }

    [Fact]
    public void ICollection_CopyTo_FilledConcurrentBidirectionalDictionaryAndIncompatibleArray_ThrowsArgumentException()
    {
        var dictionary = (ICollection)CreateConcurrentBidirectionalDictionaryForDictionary();
        var entries = new int[2];

        Assert.Throws<ArgumentException>(() => dictionary.CopyTo(entries, 1));
    }

    [Fact]
    public void ICollection_CopyTo_FilledConcurrentBidirectionalDictionaryAndCovariantObjectArray_ThrowsArgumentException()
    {
        var dictionary = (ICollection)CreateConcurrentBidirectionalDictionaryForDictionary();
        var entries = new string[2];

        Assert.Throws<ArrayTypeMismatchException>(() => dictionary.CopyTo(entries, 0));
    }

    private static ConcurrentBidirectionalDictionary<char, int> CreateConcurrentBidirectionalDictionaryForDictionary() =>
        new(
        [
            new KeyValuePair<char, int>('a', 0),
        ]);
}
