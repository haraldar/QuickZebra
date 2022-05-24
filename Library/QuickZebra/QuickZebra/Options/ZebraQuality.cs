using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Options
{
    public class ZQuality
    {
        /// <summary>
        /// Bitonal (B).
        /// </summary>
        public static readonly ZQuality B = new("B");

        /// <summary>
        /// Grayscale (G).
        /// </summary>
        public static readonly ZQuality G = new("G");
        public string Quality;
        public ZQuality(string literal)
            => Quality = literal;
    }
}
