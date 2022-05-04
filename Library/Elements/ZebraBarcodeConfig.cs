using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickZebra.Options;

namespace QuickZebra.Elements
{
    public class ZebraBarcodeConfig : ZebraField, IZebraField
    {
        private double _ratio;
        public ZebraBarcodeConfig(int width = 2, double widthRatio = 3.0, int height = 10)
        {
            Width = ConfineInt(width, 1, 100);
            Height = ConfineInt(height, lower: 1);
            _ratio = widthRatio;
        }

        public string? GetId()
            => Id;

        string IZebraField.Zebrify()
            => ZebraLexicon.BY + Width.ToString() + WithComma(_ratio) + WithComma(Height);
    }
}
