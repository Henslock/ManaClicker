using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Give Mana Command", menuName = "Utilities/Developer Console/Commands/Give Mana Command")]
public class DeveloperCommand_GiveMana : ConsoleCommand
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

        GameManager.Instance.IncrementMana(value);
        Result = "Giving " + AbbrevationUtility.AbbreviateNumber(value) + " mana.";
        return true;
    }
}
