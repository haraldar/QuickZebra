using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Options
{
    /// <summary>
    /// Contains the Zebra printer supported printer head resolutions.
    /// </summary>
    public class ZResolution
    {
        public static readonly ZResolution M6 = new(6);
        public static readonly ZResolution M8 = new(8);
        public static readonly ZResolution M12 = new(12);
        public static readonly ZResolution M24 = new(24);
        public static readonly ZResolution I150 = new(6);
        public static readonly ZResolution I203 = new(8);
        public static readonly ZResolution I300 = new(16);
        public static readonly ZResolution I600 = new(24);

        public int Dots = 0;

        public ZResolution(int dpmm)
            => Dots = dpmm;
    }
}
