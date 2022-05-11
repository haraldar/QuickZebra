// This constellation of code creates the example piece on the labelary.com's Online Viewer.

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
ZebraLabel section1 = new();
section1.SetFont(font: '0', height: 60)
    .DrawBox(loc: (50, 50), dims: 100)
    .DrawBox(loc: (75, 75), dims: 100, invertIfOverlap: true)
    .DrawBox(loc: (93, 93), dims: 40)
    .AddText("Intershipping, Inc.", loc: (220, 50))
    .SetFont(font: '0')
    .AddMultipleText(senderData, loc: (220, 115))
    .DrawBox(loc: (50, 250), dims: (700, 3, 3));

// Section 2: Recipient Data
ZebraLabel section2 = new();
section2.DrawBox(loc: (600, 300), dims: (150, 150, 3))
    .SetFont()
    .AddMultipleText(receiverData, loc: (50, 300))
    .SetFont(height: 15)
    .AddText("Permit", loc: (638, 340))
    .AddText("123456", loc: (638, 390))
    .DrawBox(loc: (50, 500), dims: (700, 3, 3));

// Section 3: Barcode
ZebraLabel section3 = new();
section3.ConfigureBarcode(dims: (5, 270))
    .DrawBarCode("12345678", loc: (100, 550));

// Section 4: Final Data
ZebraLabel section4 = new();
section4.DrawBox(loc: (50, 900), dims: (700, 250, 3))
    .SetFont(font: '0', height: 40)
    .AddMultipleText(miscData, loc: (100, 960), down: 50)
    .DrawBox(loc: (400, 900), dims: (3, 250, 3))
    .SetFont(font: '0', height: 190)
    .AddText("CA", loc: (470, 955));

// Build the full label.
var sections = new List<ZebraLabel>() { section1, section2, section3, section4 };
var labelV3 = new ZebraLabel(dims: (65, 100, true))
    .MergeLabels(sections);
Console.WriteLine(labelV3.GetLabelString(newlined: true));
labelV3.CallLabelary(labelV3.GetLabelString(), labelFormat: "png");