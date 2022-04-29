// This constellation of code creates the example piece on the labelary.com's Online Viewer.

using QuickZebra.Absolute;

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

// VERSION 3: Small, but effective changes to V2 using tuples and TODO for relative, from this i might use sections!
ZebraLabel labelV3 = new();

// Section 1: Sender Data
labelV3.SetFont(font: '0', dims: (60, null));
labelV3.DrawBox(loc: (50, 50), dims: (100, 100, 100));
labelV3.DrawBox(loc: (75, 75), dims: (100, 100, 100), invertIfOverlap: true);
labelV3.DrawBox(loc: (93, 93), dims: (40, 40, 40));
labelV3.AddText("Intershipping, Inc.", loc: (220, 50));
labelV3.SetFont(font: '0') ;
labelV3.AddMultipleText(senderData, loc: (220, 115), incr: (null, null));
labelV3.DrawBox(loc: (50, 250), dims: (700, 3, 3));

// Section 2: Recipient Data
labelV3.DrawBox(loc: (600, 300), dims: (150, 150, 3));
labelV3.SetFont();
labelV3.AddMultipleText(receiverData, loc: (50, 300), incr: (null, null));
labelV3.SetFont(dims: (15, null));
labelV3.AddText("Permit", loc: (638, 340));
labelV3.AddText("123456", loc: (638, 390));
labelV3.DrawBox(loc: (50, 500), dims: (700, 3, 3));

// Section 3: Barcode
labelV3.ConfigureBarcode(width: 5, height: 270);
labelV3.DrawBarCode("12345678", x: 100, y: 550);

// Section 4: Final Data
labelV3.DrawBox(loc: (50, 900), dims: (700, 250, 3));
labelV3.SetFont(font: '0', dims: (40, null));
labelV3.AddMultipleText(miscData, loc: (100, 960), incr: (0, 50));
labelV3.DrawBox(loc: (400, 900), dims: (3, 250, 3));
labelV3.SetFont(font: '0', dims: (190, null));
labelV3.AddText("CA", loc: (470, 955));

Console.WriteLine(labelV3.GetLabelString(newlined: true));
labelV3.CallLabelary(labelV3.GetLabelString());


//// DEPRECATED
//// VERSION 2: Absolute, hidden object management
//ZebraLabel labelV2 = new();

//// Section 1: Sender Data
//labelV2.SetFont(font:'0', height: 60);
//labelV2.DrawBox(x: 50, y: 50, width: 100, height: 100, thickness: 100);
//labelV2.DrawBox(x: 75, y: 75, width: 100, height: 100, thickness: 100,
//    invertIfOverlap: true);
//labelV2.DrawBox(x: 93, y: 93, width: 40, height: 40, thickness: 40);
//labelV2.AddText("Intershipping, Inc.", x: 220, y: 50);
//labelV2.SetFont('0');
//labelV2.AddMultipleText(senderData, x: 220, y: 115);
//labelV2.DrawBox(x: 50, y: 250, width: 700, height: 3, thickness: 3);

//// Section 2: Recipient Data
//labelV2.DrawBox(x: 600, y: 300, width: 150, height: 150, thickness: 3);
//labelV2.SetFont();
//labelV2.AddMultipleText(receiverData, x:50, y:300);
//labelV2.SetFont(height: 15);
//labelV2.AddText("Permit", x: 638, y: 340);
//labelV2.AddText("123456", x: 638, y: 390);
//labelV2.DrawBox(x: 50, y: 500, width: 700, height: 3, thickness: 3);

//// Section 3: Barcode
//labelV2.ConfigureBarcode(width: 5, height: 270);
//labelV2.DrawBarCode("12345678", x: 100, y: 550);

//// Section 4: Final Data
//labelV2.DrawBox(x: 50, y: 900, width: 700, height: 250, thickness: 3);
//labelV2.SetFont(font: '0', height: 40);
//labelV2.AddMultipleText(miscData, x: 100, y: 960, yIncrement: 50);
//labelV2.DrawBox(x: 400, y: 900, width: 3, height: 250, thickness: 3);
//labelV2.SetFont('0', height: 190);
//labelV2.AddText("CA", x: 470, y: 955);

//Console.WriteLine(labelV2.GetLabelString(newlined: true));
//labelV2.CallLabelary(labelV2.GetLabelString());


// DEPRECATED
// VERSION 1: Absolute, raw object management
List<IZebraField> fields = new()
{
    // Section 1: Sender Data
    new ZebraFont('0')
        { Height = 60 },
    new ZebraGraphicalBox(thickness: 100)
        { X = 50, Y = 50, Width = 100, Height = 100 },
    new ZebraGraphicalBox(thickness: 100)
        { X = 75, Y = 75, Width = 100, Height = 100, invertOnOverlap = true },
    new ZebraGraphicalBox(thickness: 40)
        { X = 93, Y = 93, Width = 40, Height = 40 },
    new ZebraText("Intershipping, Inc.")
        { X = 220, Y = 50 },
    new ZebraFont('0'),
    new ZebraPlaceHolder("sender"),
    new ZebraGraphicalBox(thickness: 3)
        { X = 50, Y = 250, Width = 700, Height = 3 },

    // Section 2: Recipient Data
    new ZebraGraphicalBox(thickness: 3)
        { X = 600, Y = 300, Width = 150, Height = 150 },
    new ZebraFont(),
    new ZebraPlaceHolder("recipient"),
    new ZebraFont()
        { Height = 15 },
    new ZebraText("Permit")
        { X = 638, Y = 340 },
    new ZebraText("123456")
        { X = 638, Y = 390 },
    new ZebraGraphicalBox(thickness: 3)
        { X = 50, Y = 500, Width = 700, Height = 3 },

    // Section 3: Barcode
    new ZebraBarcodeConfig(width: 5, height: 270),
    new ZebraBarcode("12345678")
        { X = 100, Y = 550 },

    // Section 4: Final Data
    new ZebraGraphicalBox(thickness: 3)
        { X = 50, Y = 900, Width = 700, Height = 250 },
    new ZebraFont('0')
        { Height = 40 },
    new ZebraPlaceHolder("misc"),
    new ZebraGraphicalBox(thickness: 3)
        { X = 400, Y = 900, Width = 3, Height = 250 },
    new ZebraFont(font: '0')
        { Height = 190 },
    new ZebraText("CA")
        { X = 470, Y = 955 }
};
var labelV1 = new ZebraLabel();
labelV1.AddFields(fields);

var senderFields = new ZebraText().FromList(senderData, x: 220, y: 115);
var recipientFields = new ZebraText().FromList(receiverData, x: 50, y: 300);
var miscFields = new ZebraText().FromList(miscData, x: 100, y: 960, yIncrement: 50);
labelV1.ReplaceHolder("sender", senderFields);
labelV1.ReplaceHolder("recipient", recipientFields);
labelV1.ReplaceHolder("misc", miscFields);

//Console.WriteLine(labelV1.GetLabelString(wrap: true, newlined: true));
//labelV1.CallLabelary(labelV1.GetLabelString(wrap: true));