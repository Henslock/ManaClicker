using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Set Unit Rank Command", menuName = "Utilities/Developer Console/Commands/Set Unit Rank Command")]
public class DeveloperCommand_SetUnitRank : ConsoleCommand
{
    public override bool Processs(string[] args)
    {
        if(args.Length < 1) 
        {
            Result = "Error: No arguments listed.";
            return false; 
        }

        if (!Database.Instance.units.ContainsKey(args[0]))
        {
            Result = "Error: Invalid unit name.";
            return false;
        }

        if (!int.TryParse(args[1], out int value))
        {
            Result = "Error: Amount invalid.";
            return false;
        }


        if(value < 0 || value > 9999)
        {
            Result = "Error: Value must be between 0 and 9999";
            return false;
        }

        GameObject mObj = Database.Instance.units[args[0]];
        mObj.GetComponent<ManaUnitManager>().SetUnitAmountOwned(value);
        Result = "Setting " + args[0].ToUpper() + " to rank " + value.ToString();
        return true;
    }
}
