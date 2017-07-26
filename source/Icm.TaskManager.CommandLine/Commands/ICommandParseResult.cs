namespace Icm.ChoreManager.CommandLine.Commands
{
    internal interface ICommandParseResult
    {
        string ErrorMessage { get; }
        bool IsValid { get; }
        string Token { get; }
        object Value { get; }
    }
}