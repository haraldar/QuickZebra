# QuickZebra

A C# class library for generating ZPL/ ZPL2 code from C# code. Supports rendering with labelary's API.

---

## Chapters

1. [ZebraLabel](#zebralabel)
2. [Full Example](#full-example)

---

![QuickZebra Banner (Created with Wombo AI)](https://github.com/haraldar/QuickZebra/blob/main/quickzebra_banner.jpg)

---

## ZebraLabel

The main class to use is the ZebraLabel class.

A ZebraLabel can be seen as a full label:
```csharp

var label = new ZebraLabel(dims: (4, 6, false))
    .SetFont(font: '0', height: 60)
    .DrawBox(loc: (50, 50), dims: 100)
    ...
    .AddText("Intershipping, Inc.", loc: (220, 50))
    .AddMultipleText(senderData, loc: (220, 115));
```

Or it can be used in sections that are more variable:
```csharp
// Section 1: Sender Data
var section1 = new ZebraLabel()
    .SetFont(font: '0', height: 60)
    ...
    .DrawBox(loc: (50, 250), dims: (700, 3, 3));

// Section 2: Recipient Data
var section2 = new ZebraLabel()
    .AddText("123456", loc: (638, 390))
    ...
    .DrawBox(loc: (50, 500), dims: (700, 3, 3));

// Section 3: Barcode
var section3 = new ZebraLabel()
    .DrawBarCode("12345678", loc: (100, 550), dims: (5, 270, 2.0));

// Section 4: Final Data
var section4 = new ZebraLabel()
    .DrawBox(loc: (50, 900), dims: (700, 250, 3))
    ...
    .AddText("CA", loc: (470, 955));
```

Everything can be merged later on:
```csharp
//Build the full label.
var sections = new List<ZebraLabel>() { section1, section2, section3, section4 };
var label = new ZebraLabel(dims: (4, 6, false))
    .MergeLabels(sections);
```

And finally it can be simply converted to a ZPL string using ```GetLabelString()``` and sent to labelary's API using ```CallLabelary()```:
```csharp
var labelStr = label.GetLabelString(newlined: true);
labelV3.CallLabelary(labelV3.GetLabelString(), labelFormat: "png");
```

---

## Full Example

The following example and result is the equivalent of the one on [Labelary's page](http://labelary.com/viewer.html).

```csharp
using QuickZebra;
List<string> senderData = new()
{
    "1000 Shipping Lane",
    "Shelbyville TN 38102",
    "United States (USA)"
};

List<string> receiverData = new()
{
    "John Doe",
    "100 Main Street",
    "Springfield TN 39021",
    "United States (USA)"
};

List<string> miscData = new()
{
    "Ctr. X34B-1",
    "REF1 F00B47",
    "REF2 BL4H8"
};

// Section 1: Sender Data
var section1 = new ZebraLabel()
    .SetFont(font: '0', height: 60)
    .DrawBox(loc: (50, 50), dims: 100)
    .DrawBox(loc: (75, 75), dims: 100, invertIfOverlap: true)
    .DrawBox(loc: (93, 93), dims: 40)
    .AddText("Intershipping, Inc.", loc: (220, 50))
    .SetFont(font: '0')
    .AddMultipleText(senderData, loc: (220, 115))
    .DrawBox(loc: (50, 250), dims: (700, 3, 3));

// Section 2: Recipient Data
var section2 = new ZebraLabel()
    .DrawBox(loc: (600, 300), dims: (150, 150, 3))
    .SetFont()
    .AddMultipleText(receiverData, loc: (50, 300))
    .SetFont(height: 15)
    .AddText("Permit", loc: (638, 340))
    .AddText("123456", loc: (638, 390))
    .DrawBox(loc: (50, 500), dims: (700, 3, 3));

// Section 3: Barcode
var section3 = new ZebraLabel()
    .DrawBarCode("12345678", loc: (100, 550), dims: (5, 270, 2.0));

// Section 4: Final Data
var section4 = new ZebraLabel()
    .DrawBox(loc: (50, 900), dims: (700, 250, 3))
    .SetFont(font: '0', height: 40)
    .AddMultipleText(miscData, loc: (100, 960), down: 50)
    .DrawBox(loc: (400, 900), dims: (3, 250, 3))
    .SetFont(font: '0', height: 190)
    .AddText("CA", loc: (470, 955));

//Build the full label.
var sections = new List<ZebraLabel>() { section1, section2, section3, section4 };
var labelV3 = new ZebraLabel(dims: (4, 6, false))
    .MergeLabels(sections);
Console.WriteLine(labelV3.GetLabelString(newlined: true));
labelV3.CallLabelary(labelV3.GetLabelString(), labelFormat: "png");
```
