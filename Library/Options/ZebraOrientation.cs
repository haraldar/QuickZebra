using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Options
{
    public static class ZOrientation
    {
        /// <summary> Default orientation. </summary>
        public static readonly char N = 'N';

        /// <summary> 90 degrees rotated clockwise. </summary>
        public static readonly char R = 'R';

        /// <summary> Inverted 180 degrees. </summary>
        public static readonly char I = 'I';

        /// <summary> Read from bottom up. 270 degrees. </summary>
        public static readonly char B = 'B';
    }
}
