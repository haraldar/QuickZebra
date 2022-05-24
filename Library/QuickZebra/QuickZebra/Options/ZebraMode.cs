using QuickZebra.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Options
{
    /// <summary>
    /// Contains the modes for each barcode.
    /// </summary>
    public class ZMode
    {
        public string Mode = "";
        public ZMode(string mode)
            => Mode = mode;

        public override string ToString()
            => Mode;

        /// <summary>
        /// The modes for the CODE128 barcode.
        /// </summary>
        public static class CODE128
        {
            /// <summary>
            /// No selected mode.
            /// </summary>
            public readonly static ZMode N = new("N");

            /// <summary>
            /// UCC Case Mode.
            /// </summary>
            public readonly static ZMode U = new("U");

            /// <summary>
            /// Automatic Mode.
            /// </summary>
            public readonly static ZMode A = new("A");

            /// <summary>
            /// UCC/ EAN Mode.
            /// </summary>
            public readonly static ZMode D = new("D");
        }
    }

    

}
