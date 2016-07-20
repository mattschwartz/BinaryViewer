using System.Collections.Generic;

namespace BinaryViewer.Interpreter
{
    public static partial class CommandInterpreter
    {
        private static BinaryFile _bFile = null;

        private static Dictionary<string, Command> Commands = new Dictionary<string, Command>
        {
            ["getb"] = GetBytes,
            ["read"] = Read,
            ["setd"] = SetDisplayType,
            ["setz"] = SetSize,
            ["exit"] = Exit,
            ["help"] = Help
        };
    }
}
