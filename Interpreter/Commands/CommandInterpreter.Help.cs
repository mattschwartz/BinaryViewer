using System.Collections.Generic;
using System.Text;

namespace BinaryViewer.Interpreter
{
    public static partial class CommandInterpreter
    {
        private static Command Help => new Command
        {
            Aliases = new List<string>
            {
                "?"
            },
            Options = new List<string>
            {
                "",
                "<command>"
            },
            Usage = "help, ? <command-name?>",
            Description = "Displays the list of available commands, their usages and descriptions",
            Do = HelpCommand
        };

        private static Result HelpCommand(Arguments args)
        {
            var result = new Result();
            var command = args["command"];

            var message = new StringBuilder();

            if (!string.IsNullOrEmpty(command) && ValidCommand(ref command)) {
                message.AppendFormat("{0, -12} {1}\n{2, -12} {3}", command,
                       Commands[command].Usage, string.Empty,
                       Commands[command].Description);

                return new Result(message.ToString());
            }

            foreach (var key in Commands.Keys) {
                message.AppendFormat("{0, -12} {1}\n{2, -12} {3}\n\n", key,
                    Commands[key].Usage, string.Empty,
                    Commands[key].Description);
            }

            if (message.Length >= 2) {
                result.Message = message.ToString().Substring(0, message.Length - 2);
            } else {
                result.Message = message.ToString();
            }

            return result;
        }
    }
}
