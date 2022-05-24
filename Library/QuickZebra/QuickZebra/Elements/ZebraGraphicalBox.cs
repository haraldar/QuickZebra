using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickZebra.Options;

namespace QuickZebra.Elements
{
    public class ZebraGraphicalBox : ZebraField, IZebraField
    {
        private int _thickness;
        private char _color;
        private int _rounding;

        public ZebraGraphicalBox(int thickness = 1, char color = 'B', int rounding = 0)
        {
            _thickness = thickness;
            _color = color;
            _rounding = rounding;
        }

        // function for multiple boxes / lines

        public string DrawGraphicalBox()
            => ZebraLexicon.GB + Width.ToString() + WithComma(Height) + WithComma(_thickness)
                + WithComma(_color) + WithComma(_rounding);

        public (int x, int y, int w, int h) GetMaxDimensions()
            => (X, Y, Width, Height);

        string IZebraField.Zebrify()
            => EncapsuleLine(DrawGraphicalBox());
    }
}
