using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

[ExcludeFromCodeCoverage]
internal sealed class DictionaryValueCollectionDebugView<TKey, TValue>
{
    private readonly ICollection<TValue> _collection;

    public DictionaryValueCollectionDebugView(ICollection<TValue> collection)
    {
        _collection = collection ?? throw new ArgumentNullException(nameof(collection));
    }

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    public TValue[] Items
    {
        get
        {
            var items = new TValue[_collection.Count];
            _collection.CopyTo(items, 0);

            return items;
        }
    }
}
