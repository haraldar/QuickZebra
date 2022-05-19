using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickZebra.Options
{
    ///// <summary>
    ///// Contains all the different types of ZPL-supported barcodes.
    ///// </summary>
    //public class ZBarcodeType
    //{
    //    //public static readonly char AZTEC = '0';
    //    //public static readonly char CODE11 = '1';
    //    //public static readonly char INTERLEAVED2O5 = '2';
    //    //public static readonly char CODE39 = '3';
    //    //public static readonly char CODE49 = '4';
    //    //public static readonly char PLANETCODE = '5';
    //    //public static readonly char PDF417 = '7';
    //    //public static readonly char EAN8 = '8';
    //    //public static readonly char UPCE = '9';
    //    //public static readonly char CODE93 = 'A';
    //    //public static readonly char CODABLOCK = 'B';
    //    //public static readonly char UPSMAXICODE = 'D';
    //    //public static readonly char EAN13 = 'E';
    //    //public static readonly char MICROPDF417 = 'F';
    //    //public static readonly char STANDARD2O5 = 'J';
    //    //public static readonly char INDUSTRIAL2O5 = 'I';
    //    //public static readonly char ANSICODABAR = 'K';
    //    //public static readonly char LOGMARS = 'L';
    //    //public static readonly char MSI = 'M';
    //    //public static readonly char PLESSEY = 'P';
    //    //public static readonly char QR = 'Q';
    //    //public static readonly char RSS = 'R';
    //    //public static readonly char TLC39 = 'T';
    //    //public static readonly char UPCA = 'U';
    //    //public static readonly char DATAMATRIX = 'X';
    //    //public static readonly char POSTAL = 'Z';

    //    public static readonly ZBarcodeType CODE128 = new("C");

    //    public string Type = "";
    //    public ZBarcodeType(string initial)
    //        => Type = initial;

    //    public override string ToString()
    //        => Type;
    //}


    //public interface IBarcodeType
    //{
    //    public string Initial { get; set; }
    //    public ZOrientation Orientation { get; set; }
    //    public string ToString();
    //}



    /// <summary>
    /// Contains all the different types of ZPL-supported barcodes.
    /// </summary>
    public class ZBarcodeType
    {
        public string Options = "";

        public ZBarcodeType(List<string> options)
            => Options = string.Join(",",options);
        public static string ToAnswer(bool option)
            => option ? "Y" : "N";

        public static string TryToString(object? elem)
            => elem?.ToString() ?? "";

        public override string ToString()
            => Options;


        public struct CODE128
        {

            private string _initial = "C";

            /// <summary>
            /// The orientation of the barcode.^FO100,550,0^BC,N,,Y,N,N,N^FD12345678^FS
            /// </summary>
            public ZOrientation Orientation = ZOrientation.N;

            /// <summary>
            /// The height of the barcode. Defaults to the height set by the BY-tags.
            /// </summary>
            public int? Height = null;

            /// <summary>
            /// Print the interpretation line.
            /// </summary>
            public bool InterpretLine = true;

            /// <summary>
            /// Print a line above the barcode.
            /// </summary>
            public bool LineAbove = false;

            /// <summary>
            /// UCC check digit.
            /// </summary>
            public bool CheckDigit = false;

            /// <summary>
            /// Sets a custom mode.
            /// </summary>
            public ZMode Mode = ZMode.CODE128.N;

            private ZBarcodeType _type;
            public ZBarcodeType Type {
                get {
                    _type = new(new List<string>()
                    {
                        _initial + Orientation.Orientation,
                        TryToString(Height),
                        ToAnswer(InterpretLine),
                        ToAnswer(LineAbove),
                        ToAnswer(CheckDigit),
                        Mode.Mode
                    });
                    return _type;
                }
                set => _type = value;
            }

            public CODE128()
                => _type = new( new List<string>()
                    {
                        _initial + Orientation.Orientation,
                        TryToString(Height),
                        ToAnswer(InterpretLine),
                        ToAnswer(LineAbove),
                        ToAnswer(CheckDigit),
                        Mode.Mode
                    }
                );
        }
    }
}
    

