using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsoleCommand : ScriptableObject, IConsoleCommand
{
    [SerializeField]
    public string CommandWord = string.Empty;

    public string commandWord => CommandWord;

    protected string Result = string.Empty;
    public string result => Result;

    public abstract bool Processs(string[] args);
    public virtual string GetProcessResult() { return result; }
}
