using QuickZebra;

// Section 1: Sender Data
var section1 = new ZebraLabel()
    .SetFont(font: '0', height: 60)
    .DrawBox(loc: (50, 50), dims: 100)
    .DrawBox(loc: (75, 75), dims: 100, invertIfOverlap: true)
    .DrawBox(loc: (93, 93), dims: 40)
    .AddText("Intershipping, Inc.", loc: (220, 50))
    .SetFont(font: '0')
    .AddTexts(loc: (220, 115), incr: (0, 40), invertIfOverlap: false,
        "1000 Shipping Lane",
        "Shelbyville TN 38102",
        "United States (USA)")
    .DrawBox(loc: (50, 250), dims: (700, 3, 3));

// Section 2: Recipient Data
var section2 = new ZebraLabel()
    .DrawBox(loc: (600, 300), dims: (150, 150, 3))
    .SetFont()
    .AddTexts(loc: (50, 300), incr: (0, 40), invertIfOverlap: false,
        "John Doe",
        "100 Main Street",
        "Springfield TN 39021",
        "United States (USA)")
    .SetFont(height: 15)
    .AddTexts(loc: (638, 340), incr: (0, 50), invertIfOverlap: false,
        "Permit",
        "123456")
    .DrawBox(loc: (50, 500), dims: (700, 3, 3));

// Section 3: Barcode
var section3 = new ZebraLabel()
    .DrawBarCode("12345678", loc: (100, 550), dims: (5, 270, 2.0));

// Section 4: Final Data
var section4 = new ZebraLabel()
    .DrawBox(loc: (50, 900), dims: (700, 250, 3))
    .SetFont(font: '0', height: 40)
    .AddTexts(loc: (100, 960), incr: (0, 50), invertIfOverlap: false,
        "Ctr. X34B-1",
        "REF1 F00B47",
        "REF2 BL4H8")
    .DrawBox(loc: (400, 900), dims: (3, 250, 3))
    .SetFont(font: '0', height: 190)
    .AddText("CA", loc: (470, 955));


//Build the full label.
var labelV4 = new ZebraLabel(section1, section2, section3, section4);
Console.WriteLine(labelV4.GetLabelString(newlined: true));
labelV4.CallLabelary(labelV4.GetLabelString(), labelFormat: "png");