using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

[ExcludeFromCodeCoverage]
internal sealed class DictionaryKeyCollectionDebugView<TKey, TValue>
{
    private readonly ICollection<TKey> _collection;

    public DictionaryKeyCollectionDebugView(ICollection<TKey> collection)
    {
        _collection = collection ?? throw new ArgumentNullException(nameof(collection));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public TKey[] Items
    {
        get
        {
            var items = new TKey[_collection.Count];
            _collection.CopyTo(items, 0);

            return items;
        }
    }
}
