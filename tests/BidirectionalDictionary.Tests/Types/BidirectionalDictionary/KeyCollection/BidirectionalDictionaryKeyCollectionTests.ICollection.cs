using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.KeyCollection;

public partial class BidirectionalDictionaryKeyCollectionTests
{
    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_IsReadOnly_FilledBidirectionalDictionary_ReturnsTrue()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isReadOnly = ((ICollection<char>)bidirectionalDictionary.Keys).IsReadOnly;

        Assert.True(isReadOnly);
    }

    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_Add_FilledBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)bidirectionalDictionary.Keys).Add('b'));
    }

    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_Add_EmptyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)bidirectionalDictionary.Keys).Add('a'));
    }

    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_Clear_FilledBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)bidirectionalDictionary.Keys).Clear());
    }

    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_Clear_EmptyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)bidirectionalDictionary.Keys).Clear());
    }

    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_Remove_FilledBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)bidirectionalDictionary.Keys).Remove('a'));
    }

    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_Remove_EmptyBidirectionalDictionary_ThrowsNotSupportedException()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)bidirectionalDictionary.Keys).Remove('a'));
    }

    [Fact]
    [Trait("Method", "ICollection")]
    public void ICollection_IsSynchronized_FilledBidirectionalDictionary_ReturnsFalse()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isSynchronized = ((System.Collections.ICollection)bidirectionalDictionary.Keys).IsSynchronized;

        Assert.False(isSynchronized);
    }

    [Fact]
    [Trait("Method", "ICollection")]
    public void ICollection_SyncRoot_FilledBidirectionalDictionary_ReturnsSyncRoot()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var syncRoot = ((System.Collections.ICollection)bidirectionalDictionary.Keys).SyncRoot;

        Assert.NotNull(syncRoot);
        Assert.Same(syncRoot, ((System.Collections.ICollection)bidirectionalDictionary.Keys).SyncRoot);
    }

    [Fact]
    [Trait("Method", "ICollection")]
    public void ICollection_CopyTo_FilledBidirectionalDictionary_CopiesKeys()
    {
        var bidirectionalDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var array = new char[3];

        ((ICollection)bidirectionalDictionary.Keys).CopyTo(array, 1);

        Assert.Equal([default, 'a', 'b'], array);
    }
}
