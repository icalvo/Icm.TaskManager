namespace Icm.ChoreManager.CommandLine.Commands
{
    internal interface IParameter
    {
        string Name { get; }
        ICommandParseResult Parse(string token);
    }
}