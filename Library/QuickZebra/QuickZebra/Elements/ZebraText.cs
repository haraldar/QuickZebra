using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickZebra.Options;

// NOTE: Give possibility to add a table like structure.

namespace QuickZebra.Elements
{
    public class ZebraText : ZebraField
    {
        private string _dataString;

        public ZebraText(string dataString = "")
            => _dataString = dataString;

        public override string Zebrify()
            => EncapsuleLine(ZebraLexicon.FD + _dataString);
    }
}
