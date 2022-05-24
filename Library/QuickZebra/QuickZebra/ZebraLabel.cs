using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using QuickZebra.Elements;
using QuickZebra.Options;

namespace QuickZebra
{
    public class ZebraLabel
    {
        #region properties
        public List<IZebraField> Fields = new();
        private int _dpu;
        private string _quality;
        private (int width, int height, bool metric) _dims;
        private int _padding;
        #endregion properties

        /// <summary>
        /// The constructor for a Format.
        /// </summary>
        /// <param name="dpu">The print density (in dpmm).</param>
        /// <param name="quality">The print quality. G for Grayscale, B for Bitonal.</param>
        /// <param name="dims">The labels' dimensions width, height and if metric.</param>
        /// <param name="padding">The inner padding (in inches).</param>
        public ZebraLabel(ZResolution? dpu = null, ZQuality? quality = null,
            (int width, int height, bool metric)? dims = null, int padding = 0)
        {
            _dims = dims ?? (4, 6, false);
            if (_dims.metric)
                (_dims.width, _dims.height) = (MmToNextInch(_dims.width), MmToNextInch(_dims.height));
            _dpu = (int)(dpu ?? ZResolution.M8);
            _quality = (quality ?? ZQuality.G).Quality;
            _padding = padding;
        }

        /// <summary>
        /// Add one ZebraField to the labels list.
        /// </summary>
        /// <param name="field">The field to add.</param>
        public void AddField(IZebraField field)
            => Fields.Add(field);

        /// <summary>
        /// Adds all the Fields from a list to the labels list.
        /// </summary>
        /// <param name="fieldList">The list containing the Fields to add.</param>
        public void AddFields(List<IZebraField> fieldList)
            => Fields.AddRange(fieldList);

        /// <summary>
        /// Create a text field.
        /// </summary>
        /// <param name="text">The text to create.</param>
        /// <param name="loc">The starting coordinates of the field.</param>
        /// <param name="invertIfOverlap">A flag to invert on overlap.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel AddText(string text, (int x, int y) loc, bool invertIfOverlap = false)
        {
            AddField(new ZebraText(text)
            {
                X = loc.x,
                Y = loc.y,
                invertOnOverlap = invertIfOverlap
            });
            return this;
        }

        /// <summary>
        /// Create multiple text Fields from a list of strings.
        /// </summary>
        /// <param name="dataStrings">The list of strings to add.</param>
        /// <param name="loc">The starting coordinates of the first string.</param>
        /// <param name="incr">The vertical and horizontal incrementation per string.</param>
        /// <param name="invertIfOverlap">A flag to invert on overlap.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel AddMultipleText(List<string> dataStrings, (int x, int y) loc,
            (int? x, int? y)? incr = null, bool invertIfOverlap = false)
        {
            Fields.AddRange(new ZebraText()
                .FromList(dataStrings, loc.x, loc.y, incr?.x ?? 0, incr?.y ?? 40, invertIfOverlap));
            return this;
        }

        /// <summary>
        /// Create multiple text Fields from a list of strings.
        /// </summary>
        /// <param name="dataStrings">The list of strings to add.</param>
        /// <param name="loc">The starting coordinates of the first string.</param>
        /// <param name="incr">The vertical and horizontal incrementation per string.</param>
        /// <param name="invertIfOverlap">A flag to invert on overlap.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel AddMultipleText(List<string> dataStrings, (int x, int y) loc, int down,
            bool invertIfOverlap = false)
        {
            Fields.AddRange(new ZebraText()
                .FromList(dataStrings, loc.x, loc.y, 0, down, invertIfOverlap));
            return this;
        }

        /// <summary>
        /// Sets the current font until it will be changed again.
        /// </summary>
        /// <param name="font">The typical ZPL font selection.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel SetFont(char font = 'A', int width = 0, int height = 30)
        {
            AddField(new ZebraFont(font)
            {
                Height = height,
                Width = width
            });
            return this;
        }

        /// <summary>
        /// Adds a comment field.
        /// </summary>
        /// <param name="comment">The comment string to add.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel AddComment(string comment)
        {
            AddField(new ZebraComment(comment));
            return this;
        }

        /// <summary>
        /// Draws a rectangular box.
        /// </summary>
        /// <param name="loc">The starting coordinates of the box.</param>
        /// <param name="dims">The width and height dimensions.</param>
        /// <param name="color">Sets the color.</param>
        /// <param name="rounding">Rounds the corners of the box.</param>
        /// <param name="invertIfOverlap">A flag to invert on overlap.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel DrawBox((int x, int y) loc, (int width, int height, int thickness) dims,
            char color = 'B', int rounding = 0, bool invertIfOverlap = false)
        {
            AddField(new ZebraGraphicalBox(dims.thickness, color, rounding)
            {
                X = loc.x,
                Y = loc.y,
                Width = dims.width,
                Height = dims.height,
                invertOnOverlap = invertIfOverlap
            });
            return this;
        }

        /// <summary>
        /// Draws a rectangular box.
        /// </summary>
        /// <param name="loc">The starting coordinates of the box.</param>
        /// <param name="dims">The width and height dimensions.</param>
        /// <param name="color">Sets the color.</param>
        /// <param name="rounding">Rounds the corners of the box.</param>
        /// <param name="invertIfOverlap">A flag to invert on overlap.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel DrawBox((int x, int y) loc, int dims,
            char color = 'B', int rounding = 0, bool invertIfOverlap = false)
        {
            AddField(new ZebraGraphicalBox(dims, color, rounding)
            {
                X = loc.x,
                Y = loc.y,
                Width = dims,
                Height = dims,
                invertOnOverlap = invertIfOverlap
            });
            return this;
        }

        /// <summary>
        /// Sets the barcode configuration.
        /// </summary>
        /// <param name="dims">Sets the height and width dimensions.</param>
        /// <param name="widthRatio">Wide bar to narrom bar ratio.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel ConfigureBarcode((int? width, int? height) dims, double widthRatio = 3.0)
        {
            AddField(new ZebraBarcodeConfig(dims.width ?? 2, widthRatio, dims.height ?? 10));
            return this;
        }

        /// <summary>
        /// Adds a barcode field.
        /// </summary>
        /// <param name="content">The content of the barcode.</param>
        /// <param name="loc">The starting coordinates of the barcode.</param>
        /// <param name="invertIfOverlap">Inverts the barcode where it overlaps.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel DrawBarCode(string content, (int? x, int? y) loc,
            (int? width, int? height, double? ratio)? dims = null, ZBarcodeType? type = null,
            bool invertIfOverlap = false)
        {
            if (dims != null)
                AddField(new ZebraBarcodeConfig(dims?.width ?? 2, dims?.ratio ?? 3.0, dims?.height ?? 10));

            AddField(new ZebraBarcode(content, type)
            {
                X = loc.x ?? 0,
                Y = loc.y ?? 0,
                invertOnOverlap = invertIfOverlap
            });
            return this;
        }

        // offset = null means no offset, <0 means calculate automatically, >=0 means take the given offset
        public ZebraLabel MergeLabels(ZebraLabel label, int? offset = null)
        {
            AddFields(label.Fields);
            return this;
        }

        // offset = null means no offset, <0 means calculate automatically, >=0 means take the given offset
        public ZebraLabel MergeLabels(List<ZebraLabel> labels, int? offset = null)
        {
            labels.ForEach(label => AddFields(label.Fields));
            return this;
        }

        /// <summary>
        /// Converts the format into a ZPL code string.
        /// </summary>
        /// <param name="wrap">Wrap return string into XA and XZ. Defaults to true.</param>
        /// <param name="newlined">Newline the return string. Defaults to false.</param>
        /// <returns>The ZPL code string that defines the label contents.</returns>
        public string GetLabelString(bool wrap = true, bool newlined = false)
        {
            List<string> zebras = Fields.Select(field => field.Zebrify()).ToList();
            if (wrap) zebras = zebras.Prepend(ZebraLexicon.XA).Append(ZebraLexicon.XZ).ToList();
            return string.Join((newlined) ? "\n" : "", zebras);
        }

        /// <summary>
        /// Rounds up an amount of mm's to the next bigger inch.
        /// </summary>
        /// <param name="mm">The amount millimeters.</param>
        /// <returns>The next bigger inch.</returns>
        private static int MmToNextInch(int mm)
            => (int) ((mm / 25.4) - (mm / 25.4) % 1 + 1);

        /// <summary>
        /// Converts a given number from either metric or US system to dots.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="dpu">The dots per unit.</param>
        /// <param name="metric">true if metric system else false.</param>
        /// <returns>A tuple that represents the max possible x and y dots.</returns>
        private static (int x, int y) GetMaxDots(int width, int height, int dpu, bool metric = false)
            => ((int)(width * (metric ? 1 : 25.4) * dpu), (int)(height * (metric ? 1 : 25.4) * dpu));

        /// <summary>
        /// Gets the approximately max coordinates used in the label.
        /// TODO width calculation for ZebraText is not yet entirely correct.
        /// TODO I might use recursion here.
        /// </summary>
        /// <param name="fields">The list of the Zebra elements.</param>
        /// <returns>A tuple that contains the max used coordinates in x and y.</returns>
        private static (int x, int y) GetMaxLocation(List<IZebraField> fields)
        {
            (int xMax, int yMax) = (0, 0);
            (int w, int h) font = (0, 0);

            foreach (var field in fields)
            {
                var (x, y, w, h) = field.GetMaxDimensions();
                if (field.GetType() == typeof(ZebraFont))
                {
                    font = (w, h);
                }
                else if (field.GetType() == typeof(ZebraText))
                {
                    if (x + font.w >= xMax) xMax = x + font.w;
                    if (y + font.h >= yMax) yMax = y + font.h;
                }
                else
                {
                    if (x + w >= xMax) xMax = x + w;
                    if (y + h >= yMax) yMax = y + h;
                }
            }
            return (xMax, yMax);
        }

        /// <summary>
        /// Checks if the inner bounds required exceed the given outer bounds.
        /// </summary>
        /// <param name="outer">The outer bounds.</param>
        /// <param name="inner">The inner bounds.</param>
        /// <returns>true if fully inside else false.</returns>
        private static bool CheckWithinBounds((int x, int y) outer, (int x, int y) inner)
            => outer.x >= inner.x && outer.y >= inner.y;

        /// <summary>
        /// Sends the ZPL code to the labelary API and retrieves the rendered label as either png or pdf. 
        /// </summary>
        /// <param name="zplCode">The full ZPL code string.</param>
        /// <param name="labelFormat">The rendered label's type (pdf / png).</param>
        /// <param name="borderCheck">Option to check using an exception wether the elements exceed the borders of the label.</param>
        /// <exception cref="Exception">Thrown if label borders are exceeded by the elements inside.</exception>
        public void CallLabelary(string zplCode, string labelFormat = "pdf", bool borderCheck = false)
        {
            // Check if the elements in the label system are exceeding the bounds of the label.
            (int xDots, int yDots) = GetMaxDots(_dims.width, _dims.height, _dpu);
            (int xMax, int yMax) = GetMaxLocation(Fields);
            var within = CheckWithinBounds((xDots, yDots), (xMax, yMax));

            if (within || !borderCheck)
            {
                // Check if the format parameter is valid.
                var validFormats = new[] { "pdf", "png" };
                if (!validFormats.Contains(labelFormat))
                    throw new Exception("Invalid Format. Should be one of " + validFormats.ToString());

                // Start the request building process.
                byte[] zpl = Encoding.UTF8.GetBytes(zplCode);
                var url = string
                    .Format("http://api.labelary.com/v1/printers/{0}dpmm/labels/{1}x{2}/0/", 
                    _dpu, _dims.width, _dims.height);
                var request = (HttpWebRequest) WebRequest.Create(url);
                if (labelFormat == "pdf")
                    request.Accept = "application/pdf";
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = zpl.Length;

                var requestStream = request.GetRequestStream();
                requestStream.Write(zpl, 0, zpl.Length);
                requestStream.Close();

                try
                {
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseStream = response.GetResponseStream();
                    var fileStream = File.Create("label." + labelFormat);
                    responseStream.CopyTo(fileStream);
                    responseStream.Close();
                    fileStream.Close();
                }
                catch (WebException e)
                {
                    Console.WriteLine("Error: {0}", e.Status);
                }
            }
            else if (!within && borderCheck)
            {
                throw new Exception("The elements on the label exceed the bounds of the set label borders.\n" +
                    "Max used coordinate of elements: X=" + xMax + ", Y=" + yMax + ".\n" +
                    "Max possible label cordinates: X=" + xDots + ", Y=" + yDots + ".");
            }
            else
            {
                Console.WriteLine("The elements on the label exceed the bounds of the set label borders.\n" +
                    "Max used coordinate of elements: X=" + xMax + ", Y=" + yMax + ".\n" +
                    "Max possible label cordinates: X=" + xDots + ", Y=" + yDots + ".");
            }
            
        }
        public static void Main(string[] args)
        {
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
        }
    }
}
