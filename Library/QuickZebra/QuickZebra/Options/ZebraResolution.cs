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
    //public class ZResolution
    //{
    //    public static readonly ZResolution M6 = new(6);
    //    public static readonly ZResolution M8 = new(8);
    //    public static readonly ZResolution M12 = new(12);
    //    public static readonly ZResolution M24 = new(24);
    //    public static readonly ZResolution I150 = new(6);
    //    public static readonly ZResolution I203 = new(8);
    //    public static readonly ZResolution I300 = new(16);
    //    public static readonly ZResolution I600 = new(24);

    //    public int Dots = 0;

    //    public ZResolution(int dpmm)
    //        => Dots = dpmm;
    //}

    public enum ZResolution
    {
        /// <summary>
        /// 6 dots per mm.
        /// </summary>
        M6 = 6,

        /// <summary>
        /// 8 dots per mm.
        /// </summary>
        M8 = 8,

        /// <summary>
        /// 12 dots per mm.
        /// </summary>
        M12 = 12,

        /// <summary>
        /// 24 dots per mm.
        /// </summary>
        M24 = 24,

        /// <summary>
        /// 150 dots per inch (6dpmm).
        /// </summary>
        I150 = 6,

        /// <summary>
        /// 203 dots per inch (8dpmm).
        /// </summary>
        I203 = 8,

        /// <summary>
        /// 300 dots per inch (12dpmm).
        /// </summary>
        I300 = 12,

        /// <summary>
        /// 600 dots per inch (24dpmm).
        /// </summary>
        I600 = 24
    }
}
