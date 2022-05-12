using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickZebra.Options;

namespace QuickZebra.Elements
{
    public class ZebraBarcode : ZebraField, IZebraField
    {
        private string _content;
        private string _type;
        private string? _bcOrientation;
        private int? _bcHeight;
        private char _line;
        private char _lineAbove;
        private char _checkDigit;
        private string? _mode;

        public ZebraBarcode(string content, ZBarcodeType? type = null, ZOrientation? orientation = null,
            int? height = null, bool line = true, bool lineAbove = false, bool checkDigit = false, ZMode? mode = null)
        {
            _content = content;
            _type = (type ?? ZBarcodeType.CODE128).Type;
            _bcOrientation = (orientation ?? ZOrientation.N).Orientation;
            _bcHeight = height;
            _line = ToAnswer(line);
            _lineAbove = ToAnswer(lineAbove);
            _checkDigit = ToAnswer(checkDigit);
            _mode = (mode ?? ZMode.CODE128.N).Mode;
        }

        public string? GetId()
            => Id;

        // TODO ZBarcodeType needs to return a custom type, that can be converted with e.g. toString to the correct param later
        string IZebraField.Zebrify()
            => EncapsuleLine(ZebraLexicon.B + _type + _bcOrientation?.ToString() + WithComma(_bcHeight)
                + WithComma(_line) + WithComma(_lineAbove) + WithComma(_checkDigit) + WithComma(_mode)
                + ZebraLexicon.FD + _content);
    }
}
