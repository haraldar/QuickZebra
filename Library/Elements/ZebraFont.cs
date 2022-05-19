using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickZebra.Options;

namespace QuickZebra.Elements
{
    public class ZebraFont : ZebraField, IZebraField
    {
        private char _font;
        private int? _fontW;

        public ZebraFont(char font = 'A')
        {
            _font = font;
            if (Height == 0) Height = 30;
            if (Width == 0) _fontW = null;
        }

        public (int x, int y, int w, int h) GetMaxDimensions()
            => (X, Y, Width, Height);

        string IZebraField.Zebrify()
            => ZebraLexicon.CF + _font.ToString() + WithComma(Height)
                + WithComma(_fontW);
    }
}
