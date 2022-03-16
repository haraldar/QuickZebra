using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Absolute
{
    public class ZebraField
    {
        // Starting coordinates
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

        // Size values
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

        // Organization
        public int orientation = 0;

        private int _alignment;
        public int Alignment
        {
            get => _alignment;
            set => _alignment = ConfineInt(value, upper:2);
        }

        // Distance from other objects
        public int margin = 0;
        public int padding = 0;

        // Misc
        public bool invertOnOverlap = false;

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

        public static string WithComma(object? val)
        {
            try { return "," + val?.ToString(); }
            catch { return "," + (val ?? ""); }
        }
        public string GetPosition()
            => ZebraLexicon.FO + X.ToString() + WithComma(Y) + WithComma(Alignment);

        public string EncapsuleLine(string fieldContent)
            => GetPosition() + ((invertOnOverlap) ? ZebraLexicon.FR : "") + fieldContent + ZebraLexicon.FS;
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
    }
}
