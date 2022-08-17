# BidirectionalDictionary

## Example

```cs
using System.Collections.Generic;

var capitalCountryDictionary = new BidirectionalDictionary<string, string>()
{
    ["Italy"]  = "Rome",
    ["Mumbai"] = "India",
    ["USA"]    = "Washington, D.C.",
};

var captial = capitalCountryDictionary["Italy"]); // "Rome"
var country = capitalCountryDictionary.Inverse["Rome"]; // "Italy"
```
