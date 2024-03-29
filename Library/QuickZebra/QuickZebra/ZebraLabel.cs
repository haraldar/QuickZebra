﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using QuickZebra.Elements;
using QuickZebra.Options;

namespace QuickZebra
{
    public class ZebraLabel
    {
        #region Properties

        public List<ZebraField> Fields = new();
        public ZResolution DPU { get; set; } = ZResolution.M8; 
        public ZQuality Quality { get; set; } = ZQuality.G;
        public (int width, int height) Dims { get; set; } = (4, 6);
        public int Padding { get; set; } = 0;

        #endregion Properties


        #region Constructor

        /// <summary>
        /// The constructor for a Format.
        /// </summary>
        public ZebraLabel() {}

        public ZebraLabel(params ZebraLabel[] labels)
            => labels.ToList().ForEach(label => AddFields(label.Fields));

        #endregion Constructor


        #region Helpers

        private static IEnumerable<ZebraField> InLocrement(
            IEnumerable<ZebraField> elems, Func<int, (int h, int v)> behave, (int x, int y)? loc = null)
            => Enumerable
                .Range(0, elems.Count())
                .Select(
                    i =>
                    {
                        var (h, v) = behave(i);
                        var (x, y) = loc ?? (0, 0);
                        var field = elems.ElementAt(i);
                        (field.X, field.Y) = (x + h, y + v);
                        return field;
                    }
                );

        /// <summary>
        /// Add one ZebraField to the labels list.
        /// </summary>
        /// <param name="field">The field to add.</param>
        public void AddField(ZebraField field)
            => Fields.Add(field);

        /// <summary>
        /// Adds all the Fields from a list to the labels list.
        /// </summary>
        /// <param name="fieldList">The list containing the Fields to add.</param>
        public void AddFields(IEnumerable<ZebraField> fieldList)
            => Fields.AddRange(fieldList);


        public ZebraLabel InvertOverlap(bool invert = true, int index = 0)
        {
            ((index > 0)? Fields[index] : Fields[Fields.Count + index]).invertOnOverlap = invert;
            return this;
        }

        #endregion Helpers


        #region ZebraFieldActions


        #region TextActions

        /// <summary>
        /// Create a single text field.
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
        /// Create multiple text fields along a linear path.
        /// </summary>
        /// <param name="texts">The texts to display.</param>
        /// <param name="loc">The starting location.</param>
        /// <param name="incr">The increments for the x and y coordinates.</param>
        /// <param name="invertIfOverlap">Invert if an overlap happened.</param>
        /// <returns></returns>
        public ZebraLabel AddTexts((int x, int y) loc, (int h, int v) incr, bool invertIfOverlap = false,
            params string[] texts)
        {
            var textObjs = texts.Select(text => new ZebraText(text) { invertOnOverlap = invertIfOverlap });
            var movedTextObjs = InLocrement(textObjs, i => (i*incr.h, i*incr.v), loc);
            Fields.AddRange(movedTextObjs);
            return this;
        }

        /// <summary>
        /// Create multiple text fields with a custom behaviour function that depends on the numbered position of the
        /// element in the list.
        /// </summary>
        /// <param name="texts">The list of strings to add.</param>
        /// <param name="loc">The starting coordinates of the first string.</param>
        /// <param name="incr">A delegate function depending on the numbered position of the current text element.</param>
        /// <param name="invertIfOverlap">A flag to invert on overlap.</param>
        /// <returns>The current ZebraLabel.</returns>
        public ZebraLabel AddTexts((int x, int y) loc, Func<int, (int x, int y)> incr, bool invertIfOverlap = false,
            params string[] texts)
        {
            var textObjs = texts.Select(text => new ZebraText(text) { invertOnOverlap = invertIfOverlap });
            var movedTextObjs = InLocrement(textObjs, incr);
            Fields.AddRange(movedTextObjs);
            return this;
        }

        #endregion TextActions

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


        #endregion ZebraFieldActions


        #region ZebraLabelActions

        ///// <summary>
        ///// Merges a list of labels with this label.
        ///// </summary>
        ///// <param name="labels">A list of label objects.</param>
        ///// <returns>The merged label.</returns>
        //public ZebraLabel MergeLabels(params ZebraLabel[] labels)
        //{
        //    labels.ToList().ForEach(label => AddFields(label.Fields));
        //    return this;
        //}

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
        /// Converts the format into a ZPL code string.
        /// </summary>
        /// <param name="newlined">Newline the return string.</param>
        /// <returns>The ZPL code string that defines the label contents.</returns>
        public string ToString(bool newlined)
            => GetLabelString(newlined: newlined);

        /// <summary>
        /// Converts the format into a ZPL code string.
        /// </summary>
        /// <returns>The ZPL code string that defines the label contents.</returns>
        public override string ToString()
            => GetLabelString();

        #endregion ZebraLabelActions


        #region LabelaryActions

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
        private static (int x, int y) GetMaxLocation(List<ZebraField> fields)
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
            (int xDots, int yDots) = GetMaxDots(Dims.width, Dims.height, (int) DPU);
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
                var url = $"http://api.labelary.com/v1/printers/{(int) DPU}dpmm/labels/{Dims.width}x{Dims.height}/0/";
                var request = (HttpWebRequest) WebRequest.Create(url);
                if (labelFormat == "pdf")
                    request.Accept = "application/pdf";
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = zpl.Length;

                var requestStream = request.GetRequestStream();
                requestStream.Write(zpl, 0, zpl.Length);
                requestStream.Close();

                Console.WriteLine(url);
                Console.WriteLine(request.ToString());

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
                    Console.WriteLine(e.StackTrace);
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

        /// <summary>
        /// Sends the ZPL code to the labelary API and retrieves the rendered label as either png or pdf. 
        /// </summary>
        /// <param name="labelFormat">The rendered label's type (pdf / png).</param>
        /// <param name="borderCheck">Option to check using an exception wether the elements exceed the borders of the label.</param>
        public void CallLabelary(string labelFormat = "pdf", bool borderCheck = false)
            => CallLabelary(this.ToString(), labelFormat: labelFormat, borderCheck: borderCheck);

        #endregion LabelaryActions
    }
}
