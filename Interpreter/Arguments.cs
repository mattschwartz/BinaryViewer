using System.Collections.Generic;

namespace BinaryViewer.Interpreter
{
    public class Arguments
    {
        public string Subcommand;
        public Dictionary<string, string> Flags;
        public Dictionary<string, string> Args;
        public Dictionary<string, string> UserSuppliedParameters;

        public string this[string key]
        {
            get
            {
                if (!Args.ContainsKey(key)) {
                    return null;
                }

                return Args[key];
            }
            set
            {
                Args[key] = value;
            }
        }

        public Arguments()
        {
            Subcommand = string.Empty;
            Flags = new Dictionary<string, string>();
            Args = new Dictionary<string, string>();
            UserSuppliedParameters = new Dictionary<string, string>();
        }

        public string GetFlag(string key)
        {
            string value;

            if (!Flags.TryGetValue(key, out value)) {
                return null;
            }

            return value;
        }

        public string GetUserSuppliedParameters(string key)
        {
            string value;

            if (!UserSuppliedParameters.TryGetValue(key, out value)) {
                return null;
            }

            return value;
        }
    }
}
