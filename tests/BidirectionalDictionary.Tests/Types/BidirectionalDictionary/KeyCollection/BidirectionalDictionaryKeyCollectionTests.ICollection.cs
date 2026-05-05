using System.Collections;

namespace BidirectionalDictionary.Tests.Types.BidirectionalDictionary.KeyCollection;

public partial class BidirectionalDictionaryKeyCollectionTests
{
    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_IsReadOnly_FilledBiDictionary_ReturnsTrue()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isReadOnly = ((ICollection<char>)biDictionary.Keys).IsReadOnly;

        Assert.True(isReadOnly);
    }

    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_Add_FilledBiDictionary_ThrowsNotSupportedException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)biDictionary.Keys).Add('b'));
    }

    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_Add_EmptyBiDictionary_ThrowsNotSupportedException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)biDictionary.Keys).Add('a'));
    }

    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_Clear_FilledBiDictionary_ThrowsNotSupportedException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)biDictionary.Keys).Clear());
    }

    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_Clear_EmptyBiDictionary_ThrowsNotSupportedException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)biDictionary.Keys).Clear());
    }

    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_Remove_FilledBiDictionary_ThrowsNotSupportedException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)biDictionary.Keys).Remove('a'));
    }

    [Fact]
    [Trait("Method", "ICollection<TKey>")]
    public void ICollectionT_Remove_EmptyBiDictionary_ThrowsNotSupportedException()
    {
        var biDictionary = new BidirectionalDictionary<char, int>();

        Assert.Throws<NotSupportedException>(() => ((ICollection<char>)biDictionary.Keys).Remove('a'));
    }

    [Fact]
    [Trait("Method", "ICollection")]
    public void ICollection_IsSynchronized_FilledBiDictionary_ReturnsFalse()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
        };

        var isSynchronized = ((System.Collections.ICollection)biDictionary.Keys).IsSynchronized;

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

        var syncRoot = ((System.Collections.ICollection)biDictionary.Keys).SyncRoot;

        Assert.NotNull(syncRoot);
        Assert.Same(syncRoot, ((System.Collections.ICollection)biDictionary.Keys).SyncRoot);
    }

    [Fact]
    [Trait("Method", "ICollection")]
    public void ICollection_CopyTo_FilledBiDictionary_CopiesKeys()
    {
        var biDictionary = new BidirectionalDictionary<char, int>()
        {
            { 'a', 0 },
            { 'b', 1 },
        };

        var array = new char[3];

        ((ICollection)biDictionary.Keys).CopyTo(array, 1);

        Assert.Equal([default, 'a', 'b'], array);
    }
}
