// This constellation of code creates the example piece on the labelary.com's Online Viewer.

using QuickZebra.Absolute;

ZebraLabel label = new();

List<IZebraField> fields = new()
{
    // Section 1: Sender Data
    new ZebraFont(font: '0', height: 60),
    new ZebraGraphicalBox(width: 100, height: 100, thickness: 100) { X = 50, Y = 50 },
    new ZebraGraphicalBox(width: 100, height: 100, thickness: 100) { X = 75, Y = 75, invertOnOverlap = true },
    new ZebraGraphicalBox(width: 40, height: 40, thickness: 40) { X = 93, Y = 93 },
    new ZebraText("Intershipping, Inc.") { X = 220, Y = 50 },
    new ZebraGraphicalBox(width: 700, height: 3, thickness: 3) { X = 50, Y = 250 },
    // Section 2: Recipient Data
    new ZebraFont(height: 15),
    new ZebraGraphicalBox(width: 150, height: 150, thickness: 3) { X = 600, Y = 300 },
    new ZebraText("Permit") { X = 638, Y = 340 },
    new ZebraText("123456") { X = 638, Y = 390 },
    new ZebraGraphicalBox(width: 700, height: 3, thickness: 3) { X = 50, Y = 500 },
    // Section 3: Barcode
    new ZebraBarcodeConfig(width: 5, height: 270),
    new ZebraBarcode("12345678") { X = 100, Y = 550 },
    // Section 4: Final Data
    new ZebraGraphicalBox(width: 700, height: 250, thickness: 3) { X = 50, Y = 900 },
    new ZebraGraphicalBox(width: 3, height: 250, thickness: 3) { X = 400, Y = 900 },
    new ZebraFont(font: '0', height: 190),
    new ZebraText("CA") { X = 470, Y = 955 }
};

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

label.AddFields(fields);
label.AddField(new ZebraFont(font: '0'));
label.AddFields(new ZebraText() { X = 220, Y = 115 }.ConvertMultilineText(senderData));
label.AddField(new ZebraFont());
label.AddFields(new ZebraText() { X = 50, Y = 300}.ConvertMultilineText(receiverData));
label.AddField(new ZebraFont(font:'0', height: 40));
label.AddFields(new ZebraText() { X = 100, Y = 960}.ConvertMultilineText(miscData, yIncrement:50));

Console.WriteLine(label.GetLabelString(wrap: true, newlined: true));
