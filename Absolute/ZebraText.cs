using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// NOTE: Give possibility to add a table like structure.

namespace QuickZebra.Absolute
{
    public class ZebraText : ZebraField, IZebraField
    {
        private string _dataString;

        public ZebraText(string dataString = "")
            => _dataString = dataString;

        public List<IZebraField> ConvertMultilineText(List<string> dataStrings, int xIncrement = 0,
            int yIncrement = 40)
        {
            var xPos = X;
            var yPos = Y;
            var textFields = new List<IZebraField>();
            foreach (var dataString in dataStrings)
            {
                textFields.Add(new ZebraText(dataString) { X = xPos, Y = yPos, Alignment = Alignment});
                xPos += xIncrement;
                yPos += yIncrement;
            }
            return textFields;
        }

        string IZebraField.Zebrify()
            => EncapsuleLine(ZebraLexicon.FD + _dataString);
    }
}
