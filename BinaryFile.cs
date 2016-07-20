using System;
using System.IO;

namespace BinaryViewer 
{
    enum DisplayType 
    {
        Hexadecimal,
        Char
    };
    
    class BinaryFile
    {
        // http://www.dbf2002.com/dbf-file-format.html
        private DisplayType _defaultType;
        private byte[] _data;
        private int _segmentSize = 16;
        
        public int Size 
        {
            get
            {
                if (_data != null) return _data.Length;
                return -1;
            }
        }

        public static BinaryFile Open(string filepath)
        {
            if (!File.Exists(filepath)) {
                return null;
            }

            return new BinaryFile {
                _data = File.ReadAllBytes(filepath)
            };
        }

        public void SetDisplayType(DisplayType display) 
        {
            _defaultType = display;
        }

        public void SetSize(int size) 
        {
            _segmentSize = size;
        }

        public string GetBytes(int start, int end, DisplayType? display, int? segmentSize) 
        {
            if (_data == null)
                return "File has not been opened yet";
            if (start < 0 || start >= _data.Length) 
                return "Invalid value for start";
            if (end < 0 || end >= _data.Length)
                return "Invalid value for end. 0-" + Size;

            _segmentSize = segmentSize ?? _segmentSize;
            byte[] src = new byte[end - start + 1];

            Array.Copy(_data, start, src, 0, end - start);
            int length = (int)(Math.Ceiling(1m * src.Length / _segmentSize)) * _segmentSize;  
            byte[] bytes = new byte[length];
            Array.Copy(src, bytes, src.Length);  

            if (display == null) {
                display = _defaultType;
            }

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("{0,10}\t", "Offset");

            for (int i = 0; i < _segmentSize; ++i) {
                Console.Write("{0:D2} ", start + i);
            }

            Console.WriteLine();

            switch (display) {
                case DisplayType.Char:
                    PrintChar(bytes, start);
                    break;

                case DisplayType.Hexadecimal:
                default:
                    PrintHex(bytes, start);
                    break;
            }

            return "";
        }

        private void PrintHex(byte[] data, int start)
        {
            for (int i = 0; i < data.Length; i += _segmentSize) {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("{0:x10}\t", start + i);
                for (int j = 0; j < _segmentSize; ++j) {
                    if (data[i + j] == 0 || data[i + j] == 32) {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    } else {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write("{0:x2} ", data[i + j]);
                }
                Console.WriteLine();
            }
            
            Console.ResetColor();
        }

        private void PrintChar(byte[] data, int start)
        {
            for (int i = 0; i < data.Length; i += _segmentSize) {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("{0:x10}\t", start + i);
                for (int j = 0; j < _segmentSize; ++j) {
                    char ch = (char)data[i + j];
                    if (data[i + j] == 0 || data[i + j] == 32) {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                    } else {
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    if (ch == 0 || ch == 32) {
                        Console.Write(" . ");
                    } else if (char.IsControl(ch)) {
                        Console.Write("{0,2} ", "¿");
                    } else {
                        Console.Write("{0,2} ", ch);
                    }
                }
                Console.WriteLine();
            }
            
            Console.ResetColor();
        }
    }
}
