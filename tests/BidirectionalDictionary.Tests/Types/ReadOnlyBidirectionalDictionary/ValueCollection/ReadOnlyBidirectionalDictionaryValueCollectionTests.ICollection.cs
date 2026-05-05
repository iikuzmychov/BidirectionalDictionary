using System.Collections;
using System.Collections.ObjectModel;

namespace BidirectionalDictionary.Tests.Types.ReadOnlyBidirectionalDictionary.ValueCollection;

public partial class ReadOnlyBidirectionalDictionaryValueCollectionTests
{
    [Fact]
    public void ICollectionT_IsReadOnly_FilledReadOnlyBidirectionalDictionary_ReturnsTrue()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var isReadOnly = ((ICollection<int>)readOnlyBidirectionalDictionary.Values).IsReadOnly;

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

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)readOnlyBidirectionalDictionary.Values).Add(1));
    }

    [Fact]
    public void ICollectionT_Clear_FilledReadOnlyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)readOnlyBidirectionalDictionary.Values).Clear());
    }

    [Fact]
    public void ICollectionT_Remove_FilledReadOnlyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)readOnlyBidirectionalDictionary.Values).Remove(0));
    }

    [Fact]
    public void ICollection_IsSynchronized_FilledReadOnlyBidirectionalDictionary_ReturnsFalse()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);

        var isSynchronized = ((ICollection)readOnlyBidirectionalDictionary.Values).IsSynchronized;

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

        var syncRoot = ((ICollection)readOnlyBidirectionalDictionary.Values).SyncRoot;

        Assert.NotNull(syncRoot);
        Assert.Same(syncRoot, ((ICollection)readOnlyBidirectionalDictionary.Values).SyncRoot);
    }

    [Fact]
    public void ICollection_SyncRoot_WrappedCollectionDoesNotImplementICollection_ReturnsSelf()
    {
        ICollection<int> collection = new HashSet<int>([0]);
        var values = new ReadOnlyBidirectionalDictionary<char, int>.ValueCollection(collection);

        var syncRoot = ((ICollection)values).SyncRoot;

        Assert.Same(values, syncRoot);
    }

    [Fact]
    public void ICollection_CopyTo_FilledReadOnlyBidirectionalDictionary_CopiesValues()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var readOnlyBidirectionalDictionary = new ReadOnlyBidirectionalDictionary<char, int>(bidirectionalDictionary);
        var array = new int[3];

        ((ICollection)readOnlyBidirectionalDictionary.Values).CopyTo(array, 1);

        Assert.Equal([default, 0, 1], array);
    }
}
