using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Absolute
{
    public class ZebraBarcode : ZebraField, IZebraField
    {
        private char? _bcOrientation;
        private int? _bcHeight;
        private char? _line;
        private char? _lineAbove;
        private char? _checkDigit;
        private char? _mode;
        private string _content;

        public ZebraBarcode(string content, char? orientation = null, int? height = null, char? line = null,
            char? lineAbove = null, char? checkDigit = null, char? mode = null)
        {
            _content = content;
            _bcOrientation = orientation;
            _bcHeight = height;
            _line = line;
            _lineAbove = lineAbove;
            _checkDigit = checkDigit;
            _mode = mode;
        }

        public string? GetId()
            => Id;

        string IZebraField.Zebrify()
            => EncapsuleLine(ZebraLexicon.BC + _bcOrientation?.ToString() + WithComma(_bcHeight)
                + WithComma(_line) + WithComma(_lineAbove) + WithComma(_checkDigit) + WithComma(_mode)
                + ZebraLexicon.FD + _content);
    }
}
