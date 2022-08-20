# BidirectionalDictionary

## Example

```cs
using System.Collections.Generic;

var countryCapitalDictionary = new BidirectionalDictionary<string, string>()
{
    ["Italy"] = "Rome",
    ["India"] = "Mumbai",
    ["USA"]   = "Washington, D.C.",
};

var captial = countryCapitalDictionary["Italy"]); // "Rome"
var country = countryCapitalDictionary.Inverse["Rome"]; // "Italy"
```
