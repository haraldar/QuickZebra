using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Absolute
{
    public class ZebraFont : ZebraField, IZebraField
    {
        private char _font;
        private int? _fontW;

        public ZebraFont(char font = 'A', int height = 30, int? width = null)
            => (_font, Height, _fontW) = (font, height, width);

        string IZebraField.Zebrify()
            => ZebraLexicon.CF + _font.ToString() + WithComma(Height) + WithComma(_fontW);
    }
}
