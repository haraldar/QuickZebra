using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickZebra.Options;

namespace QuickZebra.Elements
{
    public class ZebraComment : ZebraField
    {
        private string _comment = "";
        public ZebraComment() { }
        public ZebraComment(string comment)
            => _comment = comment;

        public override string Zebrify()
            => ZebraLexicon.FX + " " + _comment;
    }
}
