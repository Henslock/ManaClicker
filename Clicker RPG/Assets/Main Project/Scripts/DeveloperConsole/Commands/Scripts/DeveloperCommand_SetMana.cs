using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Set Mana Command", menuName = "Utilities/Developer Console/Commands/Set Mana Command")]
public class DeveloperCommand_SetMana : ConsoleCommand
{
    public override bool Processs(string[] args)
    {
        if(args.Length != 1) 
        {
            Result = "Error: No arguments listed.";
            return false; 
        }
        if (!float.TryParse(args[0], out float value))
        {
            Result = "Error: String parse failed.";
            return false;
        }

        GameManager.Instance.SetMana(value);
        Result = "Setting mana to " + AbbrevationUtility.AbbreviateNumber(value);
        return true;
    }
}
