using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Absolute
{
    public class ZebraLine : ZebraField, IZebraField
    {
        public ZebraLine()
        {

        }

        string IZebraField.Zebrify()
        {
            throw new NotImplementedException();
        }
    }
}
