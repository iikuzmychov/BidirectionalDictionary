# BidirectionalDictionary

The **bidirectional dictionary** is a dictionary with non-null unique values, that provide access to an inverse dictionary.

## Example

```cs
using System.Collections.Generic;

var countryCapitalDictionary = new BidirectionalDictionary<string, string>()
{
    ["Italy"] = "Rome",
    ["India"] = "Mumbai",
    ["USA"]   = "Washington, D.C.",
};

var capital = countryCapitalDictionary["Italy"]); // "Rome"
var country = countryCapitalDictionary.Inverse["Rome"]; // "Italy"
```
