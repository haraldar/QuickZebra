//using QuickZebra.Relative;

//ZebraLibrary lib = new();
//Console.WriteLine(lib.SetFont('0', null, 1));
//Console.WriteLine(lib.DrawGraphicalBox(50, 50));
//ZebraLabel label = new ZebraLabel(padding:3);
//Console.WriteLine(label);
//ZebraSection testSectionOne = new();

using QuickZebra.Absolute;

ZebraLabel label = new();

List<IZebraField> fields = new()
{
    new ZebraComment("Top section with logo, name, and address."),
    new ZebraFont(font: '0', height: 60),
    new ZebraGraphicalBox(width: 100, height: 100, thickness: 100) { X = 50, Y = 50 },
    new ZebraGraphicalBox(width: 100, height: 100, thickness: 100) { X = 75, Y = 75, invertOnOverlap = true },
    new ZebraGraphicalBox(width: 40, height: 40, thickness: 40) { X = 93, Y = 93 },
    new ZebraText("Intershipping, Inc.") { X = 220, Y = 50 },
    new ZebraGraphicalBox(width: 700, height: 3, thickness: 3) { X = 50, Y = 250 },
    new ZebraFont(height: 15),
    new ZebraGraphicalBox(width: 150, height: 150, thickness: 3) { X = 600, Y = 300 },
    new ZebraText("Permit") { X = 638, Y = 340 },
    new ZebraText("123456") { X = 638, Y = 390 },
    new ZebraGraphicalBox(width: 700, height: 3, thickness: 3) { X = 50, Y = 500 },
    // here comes the barcode section
    new ZebraBarcodeConfig(width: 5, height: 270),
    new ZebraBarcode("12345678") { X = 100, Y = 550 },
    new ZebraGraphicalBox(width: 700, height: 250, thickness: 3) { X = 50, Y = 900 },
    new ZebraGraphicalBox(width: 3, height: 250, thickness: 3) { X = 400, Y = 900 },
    new ZebraFont(font: '0', height: 190),
    new ZebraText("CA") { X = 470, Y = 955 }
};
label.AddFields(fields);

List<string> senderData = new()
{
    "1000 Shipping Lane",
    "Shelbyville TN 38102",
    "United States (USA)"
};
label.AddField(new ZebraFont(font: '0'));
label.AddFields(new ZebraText() { X = 220, Y = 115 }.ConvertMultilineText(senderData));

List<string> receiverData = new()
{
    "John Doe",
    "100 Main Street",
    "Springfield TN 39021",
    "United States (USA)"
};
label.AddField(new ZebraFont());
label.AddFields(new ZebraText() { X = 50, Y = 300}.ConvertMultilineText(receiverData));

List<string> miscData = new()
{
    "Ctr. X34B-1",
    "REF1 F00B47",
    "REF2 BL4H8"
};
label.AddField(new ZebraFont(font:'0', height: 40));
label.AddFields(new ZebraText() { X = 100, Y = 960}.ConvertMultilineText(miscData, yIncrement:50));


Console.WriteLine(label.GetLabelString(wrap: true, newlined: true));