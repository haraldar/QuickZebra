using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Absolute
{
    public class ZebraGraphicalBox : ZebraField, IZebraField
    {
        private int _thickness;
        private char _color;
        private int _rounding;

        public ZebraGraphicalBox(int width = 0, int height = 0, int thickness = 1,
            char color = 'B', int rounding = 0)
        {
            Width = width;
            Height = height;
            _thickness = thickness;
            _color = color;
            _rounding = rounding;
        }

        // function for multiple boxes / lines

        public string DrawGraphicalBox()
            => ZebraLexicon.GB + Width.ToString() + WithComma(Height) + WithComma(_thickness)
                + WithComma(_color) + WithComma(_rounding);

        string IZebraField.Zebrify()
            => EncapsuleLine(DrawGraphicalBox());
    }
}
