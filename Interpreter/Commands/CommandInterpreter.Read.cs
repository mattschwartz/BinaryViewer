using System.Collections.Generic;

namespace BinaryViewer.Interpreter
{
    public static partial class CommandInterpreter
    {
        private static Command Read => new Command
        {
            Options = new List<string>
            {
                "<filepath>"
            },
            Usage = "read <filepath>",
            Description = "Read in the specified file to begin performing commands on it",
            Do = ReadCommand
        };

        private static Result ReadCommand(Arguments args)
        {
            string filepath = args["filepath"];

            _bFile = BinaryFile.Open(filepath);

            if (_bFile == null) {
                return new Result(false, "File not found {0}", filepath);
            }

            return "File ready, size: " + _bFile.Size;
        }
    }
}
