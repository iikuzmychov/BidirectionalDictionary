using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary;

public partial class BidirectionalDictionaryTests
{
    [Fact]
    public void IDictionary_Properties_FilledBidirectionalDictionary_ReturnsExpectedValues()
    {
        var bidirectionalDictionary = CreateBidirectionalDictionaryForDictionary();
        var dictionary = (IDictionary)bidirectionalDictionary;

        Assert.False(dictionary.IsFixedSize);
        Assert.False(dictionary.IsReadOnly);
        Assert.False(((ICollection)dictionary).IsSynchronized);
        Assert.Same(((ICollection)dictionary.Keys).SyncRoot, ((ICollection)dictionary).SyncRoot);
        Assert.Same(((ICollection)dictionary.Values).SyncRoot, ((ICollection)dictionary).SyncRoot);
        Assert.Equal(['a'], dictionary.Keys.Cast<char>());
        Assert.Equal([0], dictionary.Values.Cast<int>());
    }

    [Fact]
    public void IDictionary_Add_EmptyBidirectionalDictionary_AddsEntryAndInverseEntry()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();
        var dictionary = (IDictionary)bidirectionalDictionary;

        dictionary.Add('a', 0);

        Assert.Single(bidirectionalDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
    }

    [Fact]
    public void IDictionary_Add_WrongKeyType_ThrowsArgumentException()
    {
        var dictionary = (IDictionary)new BidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentException>(() => dictionary.Add("a", 0));
    }

    [Fact]
    public void IDictionary_Add_NullKey_ThrowsArgumentNullException()
    {
        var dictionary = (IDictionary)new BidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary.Add(null!, 0));
    }

    [Fact]
    public void IDictionary_Add_WrongValueType_ThrowsArgumentException()
    {
        var dictionary = (IDictionary)new BidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentException>(() => dictionary.Add('a', "0"));
    }

    [Fact]
    public void IDictionary_Add_NullValue_ThrowsArgumentNullException()
    {
        var dictionary = (IDictionary)new BidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary.Add('a', null));
    }

    [Fact]
    public void IDictionary_Contains_FilledBidirectionalDictionary_ReturnsExpectedResult()
    {
        var dictionary = (IDictionary)CreateBidirectionalDictionaryForDictionary();

        Assert.True(dictionary.Contains('a'));
        Assert.False(dictionary.Contains('b'));
        Assert.False(dictionary.Contains("a"));
        Assert.Throws<ArgumentNullException>(() => dictionary.Contains(null!));
    }

    [Fact]
    public void IDictionary_IndexerGet_FilledBidirectionalDictionary_ReturnsExpectedValue()
    {
        var dictionary = (IDictionary)CreateBidirectionalDictionaryForDictionary();

        Assert.Equal(0, dictionary['a']);
        Assert.Null(dictionary['b']);
        Assert.Null(dictionary["a"]);
        Assert.Throws<ArgumentNullException>(() => dictionary[null!]);
    }

    [Fact]
    public void IDictionary_IndexerSet_FilledBidirectionalDictionary_UpdatesEntryAndInverseEntry()
    {
        var bidirectionalDictionary = CreateBidirectionalDictionaryForDictionary();
        var dictionary = (IDictionary)bidirectionalDictionary;

        dictionary['a'] = 1;

        Assert.Single(bidirectionalDictionary, new KeyValuePair<char, int>('a', 1));
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(1, 'a'));
    }

    [Fact]
    public void IDictionary_IndexerSet_FilledBidirectionalDictionaryAndNewKey_AddsEntryAndInverseEntry()
    {
        var bidirectionalDictionary = CreateBidirectionalDictionaryForDictionary();
        var dictionary = (IDictionary)bidirectionalDictionary;

        dictionary['b'] = 1;

        Assert.Equal(
            [new KeyValuePair<char, int>('a', 0), new KeyValuePair<char, int>('b', 1)],
            bidirectionalDictionary);
        Assert.Equal(
            [new KeyValuePair<int, char>(0, 'a'), new KeyValuePair<int, char>(1, 'b')],
            bidirectionalDictionary.Inverse);
    }

    [Fact]
    public void IDictionary_IndexerSet_NullKey_ThrowsArgumentNullException()
    {
        var dictionary = (IDictionary)new BidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary[null!] = 0);
    }

    [Fact]
    public void IDictionary_IndexerSet_NullValue_ThrowsArgumentNullException()
    {
        var dictionary = (IDictionary)new BidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary['a'] = null);
    }

    [Fact]
    public void IDictionary_IndexerSet_WrongKeyType_ThrowsArgumentException()
    {
        var dictionary = (IDictionary)new BidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentException>(() => dictionary["a"] = 0);
    }

    [Fact]
    public void IDictionary_IndexerSet_WrongValueType_ThrowsArgumentException()
    {
        var dictionary = (IDictionary)new BidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentException>(() => dictionary['a'] = "0");
    }

    [Fact]
    public void IDictionary_Remove_FilledBidirectionalDictionaryAndExistingKey_RemovesEntryAndInverseEntry()
    {
        var bidirectionalDictionary = CreateBidirectionalDictionaryForDictionary();
        var dictionary = (IDictionary)bidirectionalDictionary;

        dictionary.Remove('a');

        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Inverse);
    }

    [Fact]
    public void IDictionary_Remove_FilledBidirectionalDictionaryAndWrongKeyType_DoesNotChangeDictionary()
    {
        var bidirectionalDictionary = CreateBidirectionalDictionaryForDictionary();
        var dictionary = (IDictionary)bidirectionalDictionary;

        dictionary.Remove("a");

        Assert.Single(bidirectionalDictionary, new KeyValuePair<char, int>('a', 0));
        Assert.Single(bidirectionalDictionary.Inverse, new KeyValuePair<int, char>(0, 'a'));
    }

    [Fact]
    public void IDictionary_Remove_NullKey_ThrowsArgumentNullException()
    {
        var dictionary = (IDictionary)new BidirectionalDictionary<char, int>();

        Assert.Throws<ArgumentNullException>(() => dictionary.Remove(null!));
    }

    [Fact]
    public void IDictionary_Clear_FilledBidirectionalDictionary_ClearsDictionaryAndInverseDictionary()
    {
        var bidirectionalDictionary = CreateBidirectionalDictionaryForDictionary();
        var dictionary = (IDictionary)bidirectionalDictionary;

        dictionary.Clear();

        Assert.Empty(bidirectionalDictionary);
        Assert.Empty(bidirectionalDictionary.Inverse);
    }

    [Fact]
    public void IDictionary_GetEnumerator_FilledBidirectionalDictionary_EnumeratesDictionaryEntries()
    {
        var dictionary = (IDictionary)CreateBidirectionalDictionaryForDictionary();

        var enumerator = dictionary.GetEnumerator();

        Assert.True(enumerator.MoveNext());
        Assert.Equal(new DictionaryEntry('a', 0), enumerator.Entry);
        Assert.Equal(new DictionaryEntry('a', 0), enumerator.Current);
    }

    [Fact]
    public void ICollection_CopyTo_FilledBidirectionalDictionaryAndDictionaryEntryArray_CopiesEntries()
    {
        var dictionary = (ICollection)CreateBidirectionalDictionaryForDictionary();
        var entries = new DictionaryEntry[2];

        dictionary.CopyTo(entries, 1);

        Assert.Equal(default, entries[0]);
        Assert.Equal(new DictionaryEntry('a', 0), entries[1]);
    }

    [Fact]
    public void ICollection_CopyTo_FilledBidirectionalDictionaryAndObjectArray_CopiesPairs()
    {
        var dictionary = (ICollection)CreateBidirectionalDictionaryForDictionary();
        var entries = new object[2];

        dictionary.CopyTo(entries, 1);

        Assert.Null(entries[0]);
        Assert.Equal(new KeyValuePair<char, int>('a', 0), entries[1]);
    }

    [Fact]
    public void ICollection_CopyTo_FilledBidirectionalDictionaryAndKeyValuePairArray_CopiesPairs()
    {
        var dictionary = (ICollection)CreateBidirectionalDictionaryForDictionary();
        var entries = new KeyValuePair<char, int>[2];

        dictionary.CopyTo(entries, 1);

        Assert.Equal(default, entries[0]);
        Assert.Equal(new KeyValuePair<char, int>('a', 0), entries[1]);
    }

    [Fact]
    public void ICollection_CopyTo_FilledBidirectionalDictionaryAndNullArray_ThrowsArgumentNullException()
    {
        var dictionary = (ICollection)CreateBidirectionalDictionaryForDictionary();

        Assert.Throws<ArgumentNullException>(() => dictionary.CopyTo(null!, 0));
    }

    [Fact]
    public void ICollection_CopyTo_FilledBidirectionalDictionaryAndMultidimensionalArray_ThrowsArgumentException()
    {
        var dictionary = (ICollection)CreateBidirectionalDictionaryForDictionary();

        Assert.Throws<ArgumentException>(() => dictionary.CopyTo(new DictionaryEntry[1, 1], 0));
    }

    [Fact]
    public void ICollection_CopyTo_FilledBidirectionalDictionaryAndNonZeroLowerBoundArray_ThrowsArgumentException()
    {
        var dictionary = (ICollection)CreateBidirectionalDictionaryForDictionary();
        var entries = Array.CreateInstance(typeof(DictionaryEntry), [1], [1]);

        Assert.Throws<ArgumentException>(() => dictionary.CopyTo(entries, 0));
    }

    [Fact]
    public void ICollection_CopyTo_FilledBidirectionalDictionaryAndOutOfRangeIndex_ThrowsArgumentOutOfRangeException()
    {
        var dictionary = (ICollection)CreateBidirectionalDictionaryForDictionary();
        var entries = new DictionaryEntry[1];

        Assert.Throws<ArgumentOutOfRangeException>(() => dictionary.CopyTo(entries, 2));
    }

    [Fact]
    public void ICollection_CopyTo_FilledBidirectionalDictionaryAndSmallArray_ThrowsArgumentException()
    {
        var dictionary = (ICollection)CreateBidirectionalDictionaryForDictionary();
        var entries = Array.Empty<DictionaryEntry>();

        Assert.Throws<ArgumentException>(() => dictionary.CopyTo(entries, 0));
    }

    [Fact]
    public void ICollection_CopyTo_FilledBidirectionalDictionaryAndIncompatibleArray_ThrowsArgumentException()
    {
        var dictionary = (ICollection)CreateBidirectionalDictionaryForDictionary();
        var entries = new int[2];

        Assert.Throws<ArgumentException>(() => dictionary.CopyTo(entries, 1));
    }

    [Fact]
    public void ICollection_CopyTo_FilledBidirectionalDictionaryAndMismatchedObjectArray_ThrowsArgumentException()
    {
        var dictionary = (ICollection)CreateBidirectionalDictionaryForDictionary();
        var entries = new string[2];

        Assert.Throws<ArgumentException>(() => dictionary.CopyTo(entries, 1));
    }

    private static BidirectionalDictionary<char, int> CreateBidirectionalDictionaryForDictionary() =>
        new()
        {
            { 'a', 0 },
        };
}
