using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Options
{
    public class ZQuality
    {
        public ZQuality B { get => Make("B"); }
        public ZQuality G { get => Make("G"); }
        public ZQuality()
        {

        }

        public ZQuality Make(string literal)
            => ZQuality
    }
}
