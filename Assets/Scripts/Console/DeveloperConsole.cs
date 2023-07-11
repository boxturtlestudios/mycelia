using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class DeveloperConsole
{
    private readonly string prefix;
    private readonly IEnumerable<IConsoleCommand> commands;

    public DeveloperConsole(string prefix, IEnumerable<IConsoleCommand> commands)
    {
        this.prefix = prefix;
        this.commands = commands;
    }

    public CommandReturn ProcessCommand(string input)
    {
        if(!input.StartsWith(prefix)) { return null; }

        input = input.Remove(0, prefix.Length);

        string[] inputSplit = input.Split(' ');

        string commandInput = inputSplit[0];
        string[] args = inputSplit.Skip(1).ToArray();

        return RunCommand(commandInput, args);
    }

    public CommandReturn RunCommand(string commandInput, string[] args)
    {
        CommandReturn result = new CommandReturn(false, $"No command found with the name of '{commandInput}'!");

        foreach(IConsoleCommand command in commands)
        {
            if(!commandInput.Equals(command.CommandWord, System.StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            result = command.Process(args);
        }

        return result;
    }
}
