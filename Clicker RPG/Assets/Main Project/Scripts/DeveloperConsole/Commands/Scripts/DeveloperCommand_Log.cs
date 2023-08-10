using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Log Command", menuName = "Utilities/Developer Console/Commands/Log Command")]
public class DeveloperCommand_Log : ConsoleCommand
{
    public override bool Processs(string[] args)
    {
        string logText = string.Join(" ", args);
        Debug.Log(logText);
        Result = logText;
        return true;
    }
}
