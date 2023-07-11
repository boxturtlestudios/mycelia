public interface IConsoleCommand
{
    string CommandWord { get; }
    CommandReturn Process(string[] args);
}

public class CommandReturn 
{
    public bool successful;
    public string message;

    public CommandReturn(bool _successful)
    {
        successful = _successful;
    }

    public CommandReturn(bool _successful, string _message)
    {
        successful = _successful;
        message = _message;
    }
}