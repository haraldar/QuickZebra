using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Absolute
{
    public class ZebraComment : ZebraField, IZebraField
    {
        private string _comment;
        public ZebraComment(string comment)
            => _comment = comment;

        public string? GetId()
            => Id;

        string IZebraField.Zebrify()
            => ZebraLexicon.FX + " " + _comment;
    }
}
