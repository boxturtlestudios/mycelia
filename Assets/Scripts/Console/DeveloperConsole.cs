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

    public void ProcessCommand(string input)
    {
        if(!input.StartsWith(prefix)) { return; }

        input = input.Remove(0, prefix.Length);

        string[] inputSplit = input.Split(' ');

        string commandInput = inputSplit[0];
        string[] args = inputSplit.Skip(1).ToArray();

        RunCommand(commandInput, args);
    }

    public void RunCommand(string commandInput, string[] args)
    {
        foreach(IConsoleCommand command in commands)
        {
            if(!commandInput.Equals(command.CommandWord, System.StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if(command.Process(args))
            {
                return;
            }
        }
    }
}
