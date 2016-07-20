using System.Collections.Generic;
using System.Text;

namespace BinaryViewer.Interpreter
{
    public static partial class CommandInterpreter
    {
        private static Command SetDisplayType => new Command
        {
            Aliases = new List<string>
            {
                "setd"
            },
            Options = new List<string>
            {
                "<display>"
            },
            Do = SetDisplayTypeCommand
        };

        private static Command SetSize => new Command
        {
            Aliases = new List<string>
            {
                "setz"
            },
            Options = new List<string>
            {
                "<size>"
            },
            Do = SetSizeCommand
        };

        private static Result SetDisplayTypeCommand(Arguments args)
        {
            DisplayType display;

            if (_bFile == null) {
                return new Result(false, "File has not been opened yet");
            }

            switch (args["display"]) {
                case "hex": 
                    display = DisplayType.Hexadecimal; 
                    break;

                case "char": 
                    display = DisplayType.Char; 
                    break;

                default:
                    return new Result(false, "Invalid value for display type {0}",
                        args["display"]);
            }

            _bFile.SetDisplayType(display);

            return "Display type set " + display;
        }

        private static Result SetSizeCommand(Arguments args)
        {
            int size;

            if (_bFile == null) {
                return new Result(false, "File has not been opened yet");
            }

            if (!int.TryParse(args["size"], out size)) {
                return new Result(null, "Invalid value for size {0}", 
                    args["size"]);
            }

            _bFile.SetSize(size);

            return "Default size set " + size;
        }
    }
}
