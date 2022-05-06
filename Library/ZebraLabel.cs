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
        private int _density;
        private char _quality;
        private int _width;
        private int _height;
        private int _padding;
        #endregion properties

        /// <summary>
        /// The constructor for a Format.
        /// </summary>
        /// <param name="density">The print density (in dpmm).</param>
        /// <param name="quality">The print quality. G for Grayscale, B for Bitonal.</param>
        /// <param name="width">The label width (in inches).</param>
        /// <param name="height">The label height (in inches).</param>
        /// <param name="padding">The inner padding (in inches).</param>
        public ZebraLabel(int dpi = 203, int density = 8, char quality = 'G', int width = 4,
            int height = 6, int padding = 0, bool useInches = true)
        {
            _density = density;
            _quality = quality;
            _width = width;
            _height = height;
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
            => fieldList.ForEach(field => Fields.Add(field));

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
        public ZebraLabel AddMultipleText(List<string> dataStrings, (int? x, int? y) loc, (int? x, int? y) incr,
            bool invertIfOverlap = false)
        {
            Fields.AddRange(new ZebraText()
                .FromList(dataStrings, loc.x ?? 0, loc.y ?? 0, incr.x ?? 0, incr.y ?? 40, invertIfOverlap));
            return this;
        }

        /// <summary>
        /// Sets the current font until it will be changed again.
        /// </summary>
        /// <param name="dims">The height and width dimensions.</param>
        /// <param name="font">The typical ZPL font selection.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel SetFont((int? height, int? width) dims, char font = 'A')
        {
            AddField(new ZebraFont(font)
            {
                Height = dims.height ?? 30,
                Width = dims.width ?? 0
            });
            return this;
        }

        /// <summary>
        /// Sets the current font until it will be changed again.
        /// </summary>
        /// <param name="font">The typical ZPL font selection.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel SetFont(int width = 0, int height = 30, char font = 'A')
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
        /// Sets a placeholder that can be replaced later.
        /// </summary>
        /// <param name="id">The given ID by the user.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel SetPlaceHolder(string id)
        {
            AddField(new ZebraPlaceHolder(id));
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
        /// <param name="orientation">The orientation of the barcode. (Options are: ...)</param>
        /// <param name="height">The height of the barcode.</param>
        /// <param name="line">TODO</param>
        /// <param name="lineAbove">TODO</param>
        /// <param name="checkDigit">TODO</param>
        /// <param name="mode">TODO</param>
        /// <param name="invertIfOverlap">Inverts the barcode where it overlaps.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel DrawBarCode(string content, (int? x, int? y) loc, char? type = null,
            char? orientation = null, int? height = null, bool line = true, bool lineAbove = false,
            bool checkDigit = false, char? mode = null, bool invertIfOverlap = false)
        {
            AddField(new ZebraBarcode(content, type, orientation, height, line, lineAbove, checkDigit, mode)
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
            this.Fields.AddRange(label.Fields);
            return this;
        }

        // offset = null means no offset, <0 means calculate automatically, >=0 means take the given offset
        public ZebraLabel MergeLabels(List<ZebraLabel> labels, int? offset = null)
        {
            labels.ForEach(label => this.Fields.AddRange(label.Fields));
            return this;
        }

        /// <summary>
        /// Replaces the placeholder with the matching id.
        /// </summary>
        /// <param name="id">The id to match.</param>
        /// <param name="insertFields">The Fields to insert.</param>
        /// <returns>The modified list of all Fields.</returns>
        public List<IZebraField> ReplaceHolder(string id, List<IZebraField> insertFields)
        {
            int index = Fields.FindIndex(f => f.GetId() == id);
            if (index != -1)
            {
                Fields.RemoveAt(index);
                Fields.InsertRange(index, insertFields);
            }
            return Fields;
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

        // returns leftmost x, and lowest y
        // elementBased if the new cordinates are to be of the lowest y currently and
        // x of label start x or x of max leftmost x of any element
        private (int x, int y) GetMaxLocation(List<IZebraField> fields, bool elementBased = true)
        {
            return (0, 0);
        }

        private double mmToInches(int mm, bool? round = null)
        {
            double inches = mm / 0.0393700874;
            if (round != null)
            {
                inches -= inches % 1;
                if (round == true) inches++;
            }
            return inches;
        }

        public (int x, int y) GetLabelDimensionsInDots()
        {
            return (0,0);
        }

        /// <summary>
        /// Send the current CollectedLabel as 
        /// </summary>
        public void CallLabelary(string zplCode, string labelFormat = "pdf")
        {
            // Check if the format parameter is a valid
            var validFormats = new[] { "pdf", "png" };
            if (!validFormats.Contains(labelFormat))
            {
                throw new Exception("Invalid Format. Should be one of " + validFormats.ToString());
            }

            byte[] zpl = Encoding.UTF8.GetBytes(zplCode);

            // adjust print density (8dpmm), label width (4 inches), label height (6 inches), and label index (0) as necessary
            var url = string.Format("http://api.labelary.com/v1/printers/{0}dpmm/labels/{1}x{2}/0/", _density, _width, _height);
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            if (labelFormat == "pdf")
            {
                request.Accept = "application/pdf";
            }
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
