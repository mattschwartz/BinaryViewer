using System;
using BinaryViewer.Interpreter;

namespace BinaryViewer
{
    public class Program
    {
        public static bool Shutdown { get; set; }

        public static void Main(string[] args)
        {
            try {
                if (args.Length > 0) {
                    Start(args);
                }
                
                Terminal();
            } catch (Exception ex) {
                Console.WriteLine("An exception occurred.\nMessage: {0}\nInner Exception: {1}\nTrace: {2}\n\nPress any key to continue...",
                    ex.Message,
                    ex.InnerException?.Message ?? "None",
                    ex.StackTrace);
                Console.ReadLine();
            }
        }

        private static void Start(string[] args)
        {
            var line = string.Join(" ", args);
            string response = CommandInterpreter.Interpret(line);

            Console.WriteLine(response);
        }

        private static void Terminal()
        {
            string command;
            string response;

            Console.WriteLine("Binary Viewer. Type \"help\" for a list of commands.");

            do {
                Console.Write("\n> ");
                command = Console.ReadLine();
                response = CommandInterpreter.Interpret(command);

                if (response.IsNotBlank()) {
                    Console.WriteLine(response);
                }
            } while (!Shutdown);
        }
    }
}
