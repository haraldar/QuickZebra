using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickZebra.Options;


// NOTE This class's params are too much focused on CODE128.
// Move the parameters to a custom class struct.

namespace QuickZebra.Elements
{
    public class ZebraBarcode : ZebraField
    {
        private string _content;
        private ZBarcodeType _barcode;

        public ZebraBarcode(string content, ZBarcodeType? barcode = null)
        {
            _content = content;
            _barcode = barcode ?? new ZBarcodeType.CODE128().Type;
        }

        // TODO ZBarcodeType needs to return a custom type, that can be converted with e.g. toString to the correct param later
        public override string Zebrify()
            => EncapsuleLine(ZebraLexicon.B + _barcode + ZebraLexicon.FD + _content);
    }
}
