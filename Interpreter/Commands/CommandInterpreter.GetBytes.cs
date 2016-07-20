using System.Collections.Generic;

namespace BinaryViewer.Interpreter
{
    public static partial class CommandInterpreter
    {
        private static Command GetBytes => new Command
        {
            Options = new List<string>
            {
                "<start> <end> ?size ?display"
            },
            Usage = "getb <start-index> <end-index> <display-type> <size?>",
            Description = string.Format(
                "Reads bytes between start and end index and displays them, where display is one of:",
                "hex", "char"
            ),
            Do = GetBytesCommand
        };

        private static Result GetBytesCommand(Arguments args)
        {
            int start;
            int end;
            int sizeParse = 16;
            int? size = null;
            DisplayType? display = null;

            if (_bFile == null) {
                return new Result(false, "File has not been loaded yet");
            }

            if (!int.TryParse(args["start"], out start)) {
                return new Result(false, "Invalid value for start {0}", 
                    args["start"]);
            }
            if (!int.TryParse(args["end"], out end)) {
                return new Result(false, "Invalid value for end {0}", 
                    args["end"]);
            }
            if (args["size"].IsNotBlank() && !int.TryParse(args["size"], out sizeParse)) {
                return new Result(false, "Invalid value for size {0}",
                    args["size"]);
            } else if (args["size"].IsNotBlank()) {
                size = sizeParse;
            }

            switch (args["display"]) {
                case "hex": 
                    display = DisplayType.Hexadecimal; 
                    break;

                case "char": 
                    display = DisplayType.Char; 
                    break;

                default:
                    if (args["display"].IsNotBlank()) {
                        return new Result(false, "Invalid value for display type {0}",
                            args["display"]);
                    }
                    break;
            }

            return _bFile.GetBytes(start, end, display, size);
        }
    }
}
