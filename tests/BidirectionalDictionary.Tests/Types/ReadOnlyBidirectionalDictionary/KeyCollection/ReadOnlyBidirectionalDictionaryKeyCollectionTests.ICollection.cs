using System.Collections;
using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary.KeyCollection;

public partial class ReadOnlyBidirectionalDictionaryKeyCollectionTests
{
    [Fact]
    public void ICollectionT_IsReadOnly_FilledReadOnlyBidirectionalDictionary_ReturnsTrue()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var isReadOnly = ((ICollection<char>)readOnlyBidirectionalDictionary.Keys).IsReadOnly;

        Assert.True(isReadOnly);
    }

    [Fact]
    public void ICollectionT_Add_FilledReadOnlyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)readOnlyBidirectionalDictionary.Keys).Add('b'));
    }

    [Fact]
    public void ICollectionT_Clear_FilledReadOnlyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)readOnlyBidirectionalDictionary.Keys).Clear());
    }

    [Fact]
    public void ICollectionT_Remove_FilledReadOnlyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)readOnlyBidirectionalDictionary.Keys).Remove('a'));
    }

    [Fact]
    public void ICollection_IsSynchronized_FilledReadOnlyBidirectionalDictionary_ReturnsFalse()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var isSynchronized = ((ICollection)readOnlyBidirectionalDictionary.Keys).IsSynchronized;

        Assert.False(isSynchronized);
    }

    [Fact]
    public void ICollection_SyncRoot_FilledReadOnlyBidirectionalDictionary_ReturnsSyncRoot()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var syncRoot = ((ICollection)readOnlyBidirectionalDictionary.Keys).SyncRoot;

        Assert.NotNull(syncRoot);
        Assert.Same(syncRoot, ((ICollection)readOnlyBidirectionalDictionary.Keys).SyncRoot);
    }

    [Fact]
    public void ICollection_SyncRoot_WrappedCollectionDoesNotImplementICollection_ReturnsSelf()
    {
        ICollection<char> collection = new HashSet<char>(['a']);
        var keys = new ReadOnlyBidirectionalDictionary<char, int>.KeyCollection(collection);

        var syncRoot = ((ICollection)keys).SyncRoot;

        Assert.Same(keys, syncRoot);
    }

    [Fact]
    public void ICollection_CopyTo_FilledReadOnlyBidirectionalDictionary_CopiesKeys()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);
        var array = new char[3];

        ((ICollection)readOnlyBidirectionalDictionary.Keys).CopyTo(array, 1);

        Assert.Equal([default, 'a', 'b'], array);
    }
}
