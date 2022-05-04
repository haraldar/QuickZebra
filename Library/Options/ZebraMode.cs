using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Options
{
    public static class ZMode
    {
        public static class CODE128
        {
            /// <summary>
            /// No selected mode.
            /// </summary>
            public static char N = 'N';

            /// <summary>
            /// UCC Case Mode.
            /// </summary>
            public static char U = 'U';

            /// <summary>
            /// Automatic Mode.
            /// </summary>
            public static char A = 'A';

            /// <summary>
            /// UCC/ EAN Mode.
            /// </summary>
            public static char D = 'D';
        }
    }
}
