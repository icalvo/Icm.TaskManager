namespace Icm.ChoreManager.CommandLine.Commands
{
    internal class CommandParseResult : ICommandParseResult
    {
        private CommandParseResult(string token, object value, bool isValid, string errorMessage)
        {
            Token = token;
            Value = value;
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        public static CommandParseResult GeneralError(string errorMessage)
        {
            return new CommandParseResult(null, null, false, errorMessage);
        }

        public static CommandParseResult<T> ParameterError<T>(string token, string errorMessage)
        {
            return new CommandParseResult<T>(token, default(T), false, errorMessage);
        }
        public static CommandParseResult<T> ParameterSuccess<T>(string token, T value)
        {
            return new CommandParseResult<T>(token, value, true, null);
        }

        public string Token { get; }

        public object Value { get; }

        public bool IsValid { get; }
        public string ErrorMessage { get; }
    }

    internal class CommandParseResult<T> : ICommandParseResult
    {
        public CommandParseResult(string token, T value, bool isValid, string errorMessage)
        {
            Token = token;
            Value = value;
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }


        public string Token { get; }

        public object Value { get; }

        public bool IsValid { get; }
        public string ErrorMessage { get; }
    }
}