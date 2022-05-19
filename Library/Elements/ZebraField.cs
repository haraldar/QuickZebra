using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickZebra.Options;

namespace QuickZebra.Elements
{
    public class ZebraField
    {
        #region properties

        private int _x;
        public int X
        {
            get => _x;
            set => _x = ConfineInt(value);
        }

        private int _y;
        public int Y
        {
            get => _y;
            set => _y = ConfineInt(value);
        }

        private int _height;
        public int Height
        {
            get => _height;
            set => _height = ConfineInt(value);
        }

        private int _width;
        public int Width
        {
            get => _width;
            set => _width = ConfineInt(value);
        }

        private int _alignment;
        public int Alignment
        {
            get => _alignment;
            set => _alignment = ConfineInt(value, upper:2);
        }

        public bool invertOnOverlap = false;

        #endregion properties


        /// <summary>
        /// Restricts a given value to a given lower and a given upper limit
        /// (both inclusive).
        /// </summary>
        /// <param name="val">The value to limit.</param>
        /// <param name="lower">The lower bound.</param>
        /// <param name="upper">The upper bound.</param>
        /// <returns>The confined value.</returns>
        public int ConfineInt(int val, int lower = 0, int upper = 32000)
            => (val >= upper) ? upper : ((val <= lower) ? lower : val);

        /// <summary>
        /// Adds a comma to the given object's string representation.
        /// </summary>
        /// <param name="val">The value to add a comma to.</param>
        /// <returns>The object with a prepended comma.</returns>
        public static string WithComma(object? val)
        {
            try { return "," + val?.ToString(); }
            catch { return "," + (val ?? ""); }
        }

        /// <summary>
        /// Collects the positional info into a ZPL command.
        /// </summary>
        /// <returns>The ZPL code setting the position.</returns>
        public string GetPosition()
            => ZebraLexicon.FO + X.ToString() + WithComma(Y) + WithComma(Alignment);

        /// <summary>
        /// Encapsule the inner field part in  between position, switches and end-of-field.
        /// </summary>
        /// <param name="fieldContent">The inner field.</param>
        /// <returns>The full string line.</returns>
        public string EncapsuleLine(string fieldContent)
            => GetPosition() + ((invertOnOverlap) ? ZebraLexicon.FR : "") + fieldContent + ZebraLexicon.FS;

        public static char ToAnswer(bool choice)
            => choice ? 'Y' : 'N';
    }

    /// <summary>
    /// Forces certain methods, that are dependant on the Zebra object, to be implemented.
    /// </summary>
    public interface IZebraField
    {
        /// <summary>
        /// Converts certain parameters of the Zebra object into ZPL Code strings.
        /// </summary>
        /// <returns>The ZPL code string.</returns>
        public string Zebrify();
        public (int x, int y, int w, int h) GetMaxDimensions();
    }
}
