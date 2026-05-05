using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.ValueCollection;

public partial class BidirectionalDictionaryValueCollectionTests
{
    [Fact]
    public void ICollectionT_IsReadOnly_FilledBidirectionalDictionary_ReturnsTrue()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isReadOnly = ((ICollection<int>)bidirectionalDictionary.Values).IsReadOnly;

        Assert.True(isReadOnly);
    }

    [Fact]
    public void ICollectionT_Add_FilledBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)bidirectionalDictionary.Values).Add(1));
    }

    [Fact]
    public void ICollectionT_Add_EmptyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)bidirectionalDictionary.Values).Add(0));
    }

    [Fact]
    public void ICollectionT_Clear_FilledBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)bidirectionalDictionary.Values).Clear());
    }

    [Fact]
    public void ICollectionT_Clear_EmptyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)bidirectionalDictionary.Values).Clear());
    }

    [Fact]
    public void ICollectionT_Remove_FilledBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)bidirectionalDictionary.Values).Remove(0));
    }

    [Fact]
    public void ICollectionT_Remove_EmptyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)bidirectionalDictionary.Values).Remove(0));
    }

    [Fact]
    public void ICollection_IsSynchronized_FilledBidirectionalDictionary_ReturnsFalse()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isSynchronized = ((System.Collections.ICollection)bidirectionalDictionary.Values).IsSynchronized;

        Assert.False(isSynchronized);
    }

    [Fact]
    public void ICollection_SyncRoot_FilledBidirectionalDictionary_ReturnsSyncRoot()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var syncRoot = ((System.Collections.ICollection)bidirectionalDictionary.Values).SyncRoot;

        Assert.NotNull(syncRoot);
        Assert.Same(syncRoot, ((System.Collections.ICollection)bidirectionalDictionary.Values).SyncRoot);
    }

    [Fact]
    public void ICollection_CopyTo_FilledBidirectionalDictionary_CopiesValues()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var array = new int[3];

        ((ICollection)bidirectionalDictionary.Values).CopyTo(array, 1);

        Assert.Equal([default, 0, 1], array);
    }
}
