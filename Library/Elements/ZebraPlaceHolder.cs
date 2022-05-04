using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Absolute
{
    public class ZebraPlaceHolder : ZebraField, IZebraField
    {
        public ZebraPlaceHolder(string id)
            => Id = id;

        public string? GetId()
            => Id;

        string IZebraField.Zebrify()
            => EncapsuleLine(ZebraLexicon.FD + "Placeholder: " + Id);
    }
}
