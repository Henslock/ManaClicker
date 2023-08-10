using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeveloperConsole
{
    private readonly string prefix;
    private readonly IEnumerable<IConsoleCommand> commands;

    public DeveloperConsole(string _prefix, IEnumerable<IConsoleCommand> _commands)
    {
        prefix = _prefix;
        commands = _commands;
    }

    public void ProcessCommand(string input)
    {
        if(!input.StartsWith(prefix)) { return; }

        input = input.Remove(0, prefix.Length);

        string[] inputSplit = input.Split(' ');

        string commandInput = inputSplit[0];
        string[] args = inputSplit.Skip(1).ToArray();

        ProcessCommand(commandInput, args);
    }

    public void ProcessCommand(string commandInput, string[] args)
    {
        foreach(var command in commands)
        {
            if(!commandInput.Equals(command.commandWord, System.StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            if(command.Processs(args))
            {
                //Push the process result text to the console
                EventManager.Debugging.PushConsoleText(command.GetProcessResult());
                return;
            }
            else
            {
                //If nothing in the loop goes through then that just means its an invalid command.
                EventManager.Debugging.PushConsoleText(command.GetProcessResult());
                return;
            }
        }

        EventManager.Debugging.PushConsoleText("Error: Invalid console command.");
    }

}
