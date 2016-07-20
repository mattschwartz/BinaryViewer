namespace BinaryViewer.Interpreter
{
    public class Result
    {
        public bool Success;
        public string Message;

        public Result(string message = "")
        {
            Success = true;
            Message = message;
        }

        public Result(string message, params object[] args)
        {
            Success = true;
            Message = string.Format(message, args);
        }

        public Result(bool success, string message = "")
        {
            Success = success;
            Message = message;
        }

        public Result(bool success, string message, params object[] args) 
        {
            Success = success;
            Message = string.Format(message, args);
        }

        public override string ToString()
        {
            if (Success) {
                return Message;
            } else {
                return "The command failed: " + Message;
            }
        }

        public static implicit operator Result(string message)
        {
            return new Result(message);
        }
    }
}
