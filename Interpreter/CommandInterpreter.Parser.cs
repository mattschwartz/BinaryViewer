using System;
using System.Collections.Generic;
using System.Linq;

namespace BinaryViewer.Interpreter
{
    public static partial class CommandInterpreter
    {
        public static string Interpret(string line)
        {
            Arguments args;

            var name = line?.Split(null)[0];

            if (!ValidCommand(ref name)) {
                if (line == null) {

                    string result = GetLevenshteinClosest(name);

                    if (result == null) {
                        return string.Format("Command not found: \"{0}\". "
                            + "Type \"help\" for a list of available commands.",
                            name);
                    }

                    return string.Format("Command not found \"{0}\". Did you mean \"{1}\"?",
                        name, result);
                }

                if (line == string.Empty) {
                    return "";
                }

                return "Command not found.";
            }

            args = ParseLine(name, line);

            if (args != null) {
                return Commands[name].Do(args).ToString();
            }

            return "Usage: " + Commands[name].Usage;
        }

        private static string GetLevenshteinClosest(string name)
        {
            foreach (var command in Commands) {
                if (Levenshtein(name, command.Key) <= 2) {
                    return command.Key;
                }
            }

            return null;
        }

        private static bool ValidCommand(ref string command)
        {
            if (string.IsNullOrWhiteSpace(command)) return false;

            if (Commands.ContainsKey(command)) {
                return true;
            }

            foreach (var key in Commands.Keys) {
                if (Commands[key].Aliases.Contains(command)) {
                    command = key;
                    return true;
                }
            }

            return false;
        }

        private static Arguments ParseLine(string command, string line)
        {
            Arguments result = null;

            var options = Commands[command].Options;

            if (options == null || options.Count == 0) {
                result = new Arguments();

                return GetFlags(Commands[command], Split(line), result);
            }

            foreach (var option in options) {
                result = ParseOption(option, Split(line));

                if (result != null) {
                    return GetFlags(Commands[command], Split(line), result);
                }
            }

            return result;
        }

        private static Arguments GetFlags(Command command, string[] line, Arguments args)
        {
            if (command.Flags.Count == 0) {
                return args;
            }

            foreach (var flag in command.Flags) {
                string value;

                var flagIndex = line.ToList().IndexOf(flag);

                if (flagIndex < 0) {
                    continue;
                }

                value = (line.Length <= flagIndex + 1) ? "true" : line[flagIndex + 1];

                args.Flags[flag.Replace("-", "")] = value;
            }

            return args;
        }

        private static string[] Split(string line)
        {
            var firstPass = line.Split('"').Select((element, index) =>
                    index % 2 == 0  // If even index
                    ? element.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)  // Split the item
                    : new string[] { element })  // Keep the entire item
                .SelectMany(element => element).ToList();

            var result = new List<string>();

            firstPass.ForEach(element => {
                if (!element.Contains('=')) {
                    result.Add(element);
                } else {
                    string[] args = element.Split('=');
                    result.Add(args[0] + '=');
                    result.Add(args[1]);
                }
            });

            return result.ToArray();
        }

        private static Arguments ParseOption(string option, string[] line)
        {
            var result = new Arguments();
            var options = option.Split(' ');

            // This command can be executed with no arguments
            if (string.IsNullOrEmpty(option)) {
                if (line.Length == 1 || line[1][0] == '-') {
                    return new Arguments();
                } else {
                    return null;
                }
            }

            // Set subcommand
            if (options[0][0] != '<' && options[0][0] != '?') {
                result.Subcommand = options[0];
            }

            var i = 1;

            foreach (var seg in options) {
                if (line.Length <= i) {
                    if (seg[0] == '?') {
                        return result;
                    }

                    return null;
                }

                if (seg.Contains("=") && line[i].EndsWith("=")) {
                    while (line[i].EndsWith("=")) {
                        if (line.Length < i + 2) {
                            return null;
                        }

                        var key = line[i];
                        var value = line[i + 1];

                        result.UserSuppliedParameters[key.Replace("=", "")] = value;

                        i += 2;

                        if (line.Length <= i) {
                            break;
                        }
                    }

                    continue;
                }

                if (seg.EndsWith("*") && !line[i].EndsWith("=")) {
                    var j = 0;

                    while (line.Length > i) {
                        var value = line[i];

                        result.UserSuppliedParameters["arg" + j] = value;

                        ++i;
                        ++j;

                        if (line.Length <= i) {
                            break;
                        }
                    }

                    continue;
                }

                string arg;

                switch (seg[0]) {
                    case '<':
                        arg = seg.Replace("<", "").Replace(">", "");
                        result[arg] = line[i];
                        break;

                    case '?':
                        if (line[i].StartsWith("-")) {
                            break;
                        }

                        if (line.Length <= i) {
                            return result;
                        }

                        arg = seg.Replace("?", "");

                        result[arg] = line[i];

                        break;

                    default:
                        if (seg != line[i]) {
                            return null;
                        }

                        break;
                }

                ++i;
            }

            if (line.Length > i && line[i][0] == '-') {
                return result;
            }

            if (line.Length > i) {
                return null;
            }

            return result;
        }

        private static int Levenshtein(string s, string t)
        {
            var n = s.Length;
            var m = t.Length;
            var d = new int[n + 1, m + 1];

            if (n == 0) {
                return m;
            }

            if (m == 0) {
                return n;
            }

            for (var i = 1; i <= n; i++) {
                for (var j = 1; j <= m; j++) {
                    var cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }
    }
}
