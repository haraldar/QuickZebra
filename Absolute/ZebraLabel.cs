using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Absolute
{
    public class ZebraLabel
    {
        #region properties
        private List<IZebraField> fields = new();
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
        public ZebraLabel(int density = 8, char quality = 'G', int width = 4,
            int height = 6, int padding = 0)
        {
            _density = density;
            _quality = quality;
            _width = width;
            _height = height;
            _padding = padding;
        }

        /// <summary>
        /// Add on ZebraField to the labels list.
        /// </summary>
        /// <param name="field">The field to add.</param>
        public void AddField(IZebraField field)
            => fields.Add(field);

        /// <summary>
        /// Adds all the fields from a list to the labels list.
        /// </summary>
        /// <param name="fieldList">The list containing the fields to add.</param>
        public void AddFields(List<IZebraField> fieldList)
            => fieldList.ForEach(field => fields.Add(field));

        //// DEPRECATED
        //public void AddText(string text, int x = 0, int y = 0,
        //    bool invertIfOverlap = false)
        //    => fields.Add(new ZebraText(text) { X = x, Y = y,
        //        invertOnOverlap=invertIfOverlap});

        // NEW
        public List<IZebraField> AddText(string text, (int x, int y) loc, bool invertIfOverlap = false)
            => new(fields.Append(new ZebraText(text)
            {
                X = loc.x,
                Y = loc.y,
                invertOnOverlap=invertIfOverlap
            }));

        //// DEPRECATED
        //public void AddMultipleText(List<string> dataStrings, int x = 0, int y = 0,
        //    int xIncrement = 0, int yIncrement = 40, bool invertIfOverlap = false)
        //    => fields.AddRange(new ZebraText().FromList(dataStrings, x, y,
        //        xIncrement, yIncrement, invertIfOverlap));

        // NEW
        public List<IZebraField> AddMultipleText(List<string> dataStrings, (int? x, int? y) loc, (int? x, int? y) incr,
            bool invertIfOverlap = false)
        {
            fields.AddRange(new ZebraText()
                .FromList(dataStrings, loc.x ?? 0, loc.y ?? 0, incr.x ?? 0, incr.y ?? 40, invertIfOverlap));
            return fields;
        }

        //// DEPRECATED
        //public void SetFont(char font = 'A', int height = 30, int width = 0)
        //    => fields.Add(new ZebraFont(font)
        //    {
        //        Height = height,
        //        Width = width
        //    });

        // NEW
        public List<IZebraField> SetFont((int? height, int? width) dims, char font = 'A')
            => new(fields.Append(new ZebraFont(font)
            {
                Height = dims.height ?? 30,
                Width = dims.width ?? 0
            }));

        // NEW
        public List<IZebraField> SetFont(char font = 'A')
            => new(fields.Append(new ZebraFont(font)
            {
                Height = 30,
                Width = 0
            }));

        // NEW
        public List<IZebraField> AddComment(string comment)
            => new(fields.Append(new ZebraComment(comment)));

        //// DEPRECATED
        //public void DrawBox(int x = 0, int y = 0, int height = 1, int width = 1,
        //    int thickness = 1, char color = 'B', int rounding = 0,
        //    bool invertIfOverlap = false, int? xy = null)
        //    => fields.Add(new ZebraGraphicalBox(thickness, color, rounding)
        //    {
        //        X = x,
        //        Y = y,
        //        Width = xy ?? width,
        //        Height = xy ?? height,
        //        invertOnOverlap = invertIfOverlap
        //    });

        // NEW
        public void DrawBox((int x, int y) loc, (int width, int height, int thickness) dims,
            char color = 'B', int rounding = 0, bool invertIfOverlap = false)
            => fields.Add(new ZebraGraphicalBox(dims.thickness, color, rounding)
            {
                X = loc.x,
                Y = loc.y,
                Width = dims.width,
                Height = dims.height,
                invertOnOverlap = invertIfOverlap
            });

        // USELESS
        // DEPRECATED
        public void SetPlaceHolder(string id)
            => fields.Add(new ZebraPlaceHolder(id));

        public void ConfigureBarcode(int width = 2, double widthRatio = 3.0, int height = 10)
            => fields.Add(new ZebraBarcodeConfig(width, widthRatio, height));

        public void DrawBarCode(string content, int x = 0, int y = 0, char? orientation = null,
            int? height = null, char? line = null, char? lineAbove = null,
            char? checkDigit = null, char? mode = null, bool invertIfOverlap = false)
            => fields.Add(new ZebraBarcode(content, orientation, height, line, lineAbove, checkDigit, mode)
            {
                X = x,
                Y = y,
                invertOnOverlap = invertIfOverlap
            });

        /// <summary>
        /// Replaces the placeholder with the matching id.
        /// </summary>
        /// <param name="id">The id to match.</param>
        /// <param name="insertFields">The fields to insert.</param>
        /// <returns>The modified list of all fields.</returns>
        public List<IZebraField> ReplaceHolder(string id, List<IZebraField> insertFields)
        {
            int index = fields.FindIndex(f => f.GetId() == id);
            if (index != -1)
            {
                fields.RemoveAt(index);
                fields.InsertRange(index, insertFields);
            }
            return fields;
        }

        /// <summary>
        /// Converts the format into a ZPL code string.
        /// </summary>
        /// <param name="wrap">Wrap return string into XA and XZ. Defaults to true.</param>
        /// <param name="newlined">Newline the return string. Defaults to false.</param>
        /// <returns></returns>
        public string GetLabelString(bool wrap = true, bool newlined = false)
        {
            List<string> zebras = fields.Select(field => field.Zebrify()).ToList();
            if (wrap) zebras = zebras.Prepend(ZebraLexicon.XA).Append(ZebraLexicon.XZ).ToList();
            return string.Join((newlined) ? "\n" : "", zebras);
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
            var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/");
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
                Console.WriteLine("hi");
            }
            catch (WebException e)
            {
                Console.WriteLine("Error: {0}", e.Status);
            }
        }
    }
}
