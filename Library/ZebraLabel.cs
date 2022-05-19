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

        private static int MmToNextInch(int mm)
            => (int) ((mm / 25.4) - (mm / 25.4) % 1 + 1);

        private static (int x, int y) GetMaxDots(int width, int height, int dpu, bool metric = false)
        {
            double sysConverter = metric ? 1 : 25.4;
            return ((int) (width * sysConverter * dpu), (int) (height * sysConverter * dpu));
        }

        // returns leftmost x, and lowest y
        // elementBased if the new cordinates are to be of the lowest y currently and
        // x of label start x or x of max leftmost x of any element
        private static (int x, int y) GetMaxLocation(List<IZebraField> fields, bool elementBased = true)
        {
            //foreach (var field in fields)
            //    field.
            return (0, 0);
        }

        /// <summary>
        /// Send the current CollectedLabel as 
        /// </summary>
        public void CallLabelary(string zplCode, string labelFormat = "pdf")
        {
            // Check if the elements in the label system are deeper than print size
            Console.WriteLine(GetMaxDots(4, 6, 8, false));
            Console.WriteLine(GetMaxDots(65, 100, 8, true));

            // Check if the format parameter is a valid
            var validFormats = new[] { "pdf", "png" };
            if (!validFormats.Contains(labelFormat))
                throw new Exception("Invalid Format. Should be one of " + validFormats.ToString());

            // Start the request building process.
            byte[] zpl = Encoding.UTF8.GetBytes(zplCode);

            var url = string.Format("http://api.labelary.com/v1/printers/{0}dpmm/labels/{1}x{2}/0/", _dpu, _dims.width, _dims.height);
            Console.WriteLine(url);
            var request = (HttpWebRequest)WebRequest.Create(url);
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
    }
}
