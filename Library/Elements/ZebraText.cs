using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickZebra.Options;

// NOTE: Give possibility to add a table like structure.

namespace QuickZebra.Elements
{
    public class ZebraText : ZebraField, IZebraField
    {
        private string _dataString;

        public ZebraText(string dataString = "")
            => _dataString = dataString;

        public List<IZebraField> FromList(List<string> dataStrings, int x = 0,
             int y = 0, int xIncrement = 0, int yIncrement = 40,
             bool invertIfOverlap = false)
        {
            (var xPos, var yPos) = (x, y);
            List<IZebraField> textFields = new();
            foreach (var dataString in dataStrings)
            {
                textFields.Add(new ZebraText(dataString)
                {
                    X = xPos,
                    Y = yPos,
                    invertOnOverlap = invertIfOverlap
                });
                xPos += xIncrement;
                yPos += yIncrement;
            }
            return textFields;
        }

        public (int x, int y, int w, int h) GetMaxDimensions()
            => (X, Y, Width, Height);

        string IZebraField.Zebrify()
            => EncapsuleLine(ZebraLexicon.FD + _dataString);
    }
}
