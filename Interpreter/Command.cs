using System.Collections.Generic;

namespace BinaryViewer.Interpreter
{
    public delegate Result Execute(Arguments args);

    public class Command
    {
        public List<string> Aliases { get; set; }
        public List<string> Options { get; set; }
        public List<string> Flags { get; set; }
        public string Description { get; set; }
        public string Usage { get; set; }
        public Execute Do { get; set; }

        public List<string> Manual { get; set; }

        public Command()
        {
            Aliases = new List<string>();
            Options = new List<string>();
            Flags = new List<string>();
            Manual = new List<string>();
            Description = string.Empty;
            Usage = string.Empty;
        }
    }
}
