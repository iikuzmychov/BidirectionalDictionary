[![NuGet](https://img.shields.io/nuget/v/BidirectionalDictionary)](https://www.nuget.org/packages/BidirectionalDictionary/)
[![Downloads](https://img.shields.io/nuget/dt/BidirectionalDictionary)](https://www.nuget.org/packages/BidirectionalDictionary/)
[![License](https://img.shields.io/github/license/iiKuzmychov/BidirectionalDictionary)](https://github.com/iiKuzmychov/BidirectionalDictionary/blob/master/LICENSE.md)

# BidirectionalDictionary

Proper implementation of a bidirectional dictionary, also known as "BiMap" or "two-way dictionary", for [.NET Standard 2.0](https://learn.microsoft.com/dotnet/standard/net-standard?tabs=net-standard-2-0#select-net-standard-version) and higher.

## Quick sample

```cs
using System.Collections.Generic;

var countryCapitals = new BidirectionalDictionary<string, string>()
{
    ["Italy"] = "Rome",
    ["India"] = "New Delhi",
    ["USA"]   = "Washington, D.C.",
};

Console.WriteLine(countryCapitals["Italy"]); // "Rome"
Console.WriteLine(countryCapitals.Inverse["Rome"]); // "Italy"
```

## Read-only support

You can expose a read-only view over an existing bidirectional dictionary, keeping inversion capabilities intact.
The wrapper uses the same underlying data and blocks modifications through the read-only API.

From `BidirectionalDictionary`:

```cs
using System.Collections.Generic;

BidirectionalDictionary<Key, Value> bidirectionalDictionary = ...; 
var readOnly = bidirectionalDictionary.AsReadOnly();
```

From `IBidirectionalDictionary`:

```cs
using System.Collections.Generic;
using System.Collections.ObjectModel;

IBidirectionalDictionary<Key, Value> bidirectionalDictionary = ...;
var readOnly = new ReadOnlyBidirectionalDictionary<Key, Value>(dictionary);
```

## Interfaces

To support abstraction-friendly code, the package exposes two interfaces:

- `IBidirectionalDictionary`
- `IReadOnlyBidirectionalDictionary`

Both `BidirectionalDictionary` and `ReadOnlyBidirectionalDictionary` implement these interfaces,
so you can depend on contracts instead of concrete types when needed.

## LINQ extensions

The package includes LINQ-extensions to create a `BidirectionalDictionary` directly from sequences.

From `KeyValuePair<TKey, TValue>`:

```cs
using System.Linq;

IEnumerable<KeyValuePair<int, string>> source = new[]
{
    new KeyValuePair<int, string>(1, "one"),
    new KeyValuePair<int, string>(2, "two")
};

var bidirectionalDictionary = source.ToBidirectionalDictionary();
```

From tuple sequence:

```cs
using System.Linq;

IEnumerable<(string Key, string Value)> source = new[]
{
    (Key: "US", Value: "United States"),
    (Key: "IT", Value: "Italy")
};

var bidirectionalDictionary = source.ToBidirectionalDictionary();
```

From arbitrary source with selectors:

```cs
using System.Linq;

var users = new[]
{
    new { Id = 10, Email = "a@example.com" },
    new { Id = 20, Email = "b@example.com" }
};

var bidirectionalDictionary = users.ToBidirectionalDictionary(user => user.Id, user => user.Email);
```

You can also pass custom comparers via overloads with `keyComparer` and `valueComparer`.

## License

The library is licensed under the
[MIT](https://github.com/iiKuzmychov/BidirectionalDictionary/blob/master/LICENSE.md)
license.
