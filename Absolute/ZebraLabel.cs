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
        public ZebraLabel(int density = 8, char quality = 'G', int width = 4, int height = 6, int padding = 0)
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

        //public void SetFont(char font = 'A', int height = 30, int width = 30)
        //    => AddField(new ZebraFont(font, height, width));

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
            }
            catch (WebException e)
            {
                Console.WriteLine("Error: {0}", e.Status);
            }
        }
    }
}
