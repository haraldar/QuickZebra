using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Options
{
    public class ZOrientation
    {
        /// <summary> Default orientation. </summary>
        public static readonly ZOrientation N = new("N");

        /// <summary> 90 degrees rotated clockwise. </summary>
        public static readonly ZOrientation R = new("R");

        /// <summary> Inverted 180 degrees. </summary>
        public static readonly ZOrientation I = new("I");

        /// <summary> Read from bottom up. 270 degrees. </summary>
        public static readonly ZOrientation B = new("B");

        public string Orientation = "";

        public ZOrientation(string orientation)
            => Orientation = orientation;

        public override string ToString()
            => Orientation;
    }
}
