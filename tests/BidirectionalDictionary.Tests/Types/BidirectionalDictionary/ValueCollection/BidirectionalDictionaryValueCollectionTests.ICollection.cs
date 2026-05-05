using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.ValueCollection;

public partial class BidirectionalDictionaryValueCollectionTests
{
    [Fact]
    [Trait("Method", "ICollection<TValue>")]
    public void ICollectionT_IsReadOnly_FilledBiDictionary_ReturnsTrue()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isReadOnly = ((ICollection<int>)biDictionary.Values).IsReadOnly;

        Assert.True(isReadOnly);
    }

    [Fact]
    [Trait("Method", "ICollection<TValue>")]
    public void ICollectionT_Add_FilledBiDictionary_ThrowsNotSupportedException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)biDictionary.Values).Add(1));
    }

    [Fact]
    [Trait("Method", "ICollection<TValue>")]
    public void ICollectionT_Add_EmptyBiDictionary_ThrowsNotSupportedException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)biDictionary.Values).Add(0));
    }

    [Fact]
    [Trait("Method", "ICollection<TValue>")]
    public void ICollectionT_Clear_FilledBiDictionary_ThrowsNotSupportedException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)biDictionary.Values).Clear());
    }

    [Fact]
    [Trait("Method", "ICollection<TValue>")]
    public void ICollectionT_Clear_EmptyBiDictionary_ThrowsNotSupportedException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)biDictionary.Values).Clear());
    }

    [Fact]
    [Trait("Method", "ICollection<TValue>")]
    public void ICollectionT_Remove_FilledBiDictionary_ThrowsNotSupportedException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)biDictionary.Values).Remove(0));
    }

    [Fact]
    [Trait("Method", "ICollection<TValue>")]
    public void ICollectionT_Remove_EmptyBiDictionary_ThrowsNotSupportedException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<NotSupportedException>(() => ((ICollection<int>)biDictionary.Values).Remove(0));
    }

    [Fact]
    [Trait("Method", "ICollection")]
    public void ICollection_IsSynchronized_FilledBiDictionary_ReturnsFalse()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isSynchronized = ((System.Collections.ICollection)biDictionary.Values).IsSynchronized;

        Assert.False(isSynchronized);
    }

    [Fact]
    [Trait("Method", "ICollection")]
    public void ICollection_SyncRoot_FilledBiDictionary_ReturnsSyncRoot()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var syncRoot = ((System.Collections.ICollection)biDictionary.Values).SyncRoot;

        Assert.NotNull(syncRoot);
        Assert.Same(syncRoot, ((System.Collections.ICollection)biDictionary.Values).SyncRoot);
    }

    [Fact]
    [Trait("Method", "ICollection")]
    public void ICollection_CopyTo_FilledBiDictionary_CopiesValues()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var array = new int[3];

        ((ICollection)biDictionary.Values).CopyTo(array, 1);

        Assert.Equal([default, 0, 1], array);
    }
}
