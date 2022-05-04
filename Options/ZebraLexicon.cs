using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Absolute
{
    /// <summary>
    /// Contains the most important ZPL tags.
    /// </summary>
    public static class ZebraLexicon
    {
        /// <summary>
        /// Configure current field as Code 128 barcode.
        /// </summary>
        public static readonly string BC = "^BC";

        /// <summary>
        /// Configure global barcode defaults.
        /// </summary>
        public static readonly string BY = "^BY";

        /// <summary>
        /// Change font.
        /// </summary>
        public static readonly string CF = "^CF";

        /// <summary>
        /// Set field data string.
        /// </summary>
        public static readonly string FD = "^FD";

        /// <summary>
        /// Set field position.
        /// </summary>
        public static readonly string FO = "^FO";

        /// <summary>
        /// Invert current field's color if overlaps with another field.
        /// </summary>
        public static readonly string FR = "^FR";

        /// <summary>
        /// End field.
        /// </summary>
        public static readonly string FS = "^FS";

        /// <summary>
        /// Comment.
        /// </summary>
        public static readonly string FX = "^FX";

        /// <summary>
        /// Build graphical box.
        /// </summary>
        public static readonly string GB = "^GB";

        /// <summary>
        /// Label format starter tag.
        /// </summary>
        public static readonly string XA = "^XA";

        /// <summary>
        /// Label format finisher tag.
        /// </summary>
        public static readonly string XZ = "^XZ";

    }
}
