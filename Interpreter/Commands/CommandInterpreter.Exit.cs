using System.Collections.Generic;
using System.Text;

namespace BinaryViewer.Interpreter
{
    public static partial class CommandInterpreter
    {
        private static Command Exit => new Command
        {
            Aliases = new List<string>
            {
                "quit",
                "q"
            },
            Do = ExitCommand
        };

        private static Result ExitCommand(Arguments args)
        {
            Program.Shutdown = true;
            return "";
        }
    }
}
