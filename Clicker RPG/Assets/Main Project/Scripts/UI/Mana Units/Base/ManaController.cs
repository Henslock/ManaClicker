using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : MonoBehaviour
{
    public GameObject[] unitObject;
    private List<ManaUnit> manaUnits = new List<ManaUnit>();
    private StatModifier mpcStatMod;
    private StatModifier mpsStatMod;

    private void Start()
    {
        for(int i=0; i<unitObject.Length; i++)
        {
            manaUnits.Add(unitObject[i].GetComponent<ManaUnit>());
        }


        for (int i = 0; i < unitObject.Length; i++)
        {
            if (unitObject[i].GetComponent<ManaUnitManager>().startRevealed == true) continue;
            unitObject[i].GetComponent<ManaUnitManager>().HideUnit();
        }
    }

    private void OnEnable()
    {
        EventManager.GeneralEvents.UpdateManaStats += UpdateTotalUnitMana;
        EventManager.UnitEvents.OnUnitPurchased += RevealUnit;
    }

    private void OnDisable()
    {
        EventManager.GeneralEvents.UpdateManaStats -= UpdateTotalUnitMana;
        EventManager.UnitEvents.OnUnitPurchased -= RevealUnit;
    }

    public void UpdateTotalUnitMana()
    {
        //First we calculate all of the mana from the units
        CalculateAllUnitMana();

        foreach (ManaUnit mUnit in manaUnits)
        {
            //Then we go through each unit again and calculate for any bonuses that might affect our total outcome
            mUnit.CalculateBonuses();
        }

        //And then do the final calculation
        CalculateAllUnitMana();
    }

    private void CalculateAllUnitMana()
    {
        //Gather all the mana per second and mana per click generated from our units and add it into our mpc and mps stat
        double unitMPC = 0;
        double unitMPS = 0;
        //Updates our MPC and MPS values
        foreach (ManaUnit mUnit in manaUnits)
        {
            unitMPC += mUnit.ManaPerClick.value;
            unitMPS += mUnit.ManaPerSecond.value;
        }

        mpcStatMod = new StatModifier(unitMPC, StatModType.FLAT, this);
        mpsStatMod = new StatModifier(unitMPS, StatModType.FLAT, this);

        GameManager.Instance.manaPerClick.RemoveAllModifiersFromSource(this); //Reset the mana given and then just re-add it
        GameManager.Instance.manaPerSecond.RemoveAllModifiersFromSource(this); //Reset the mana given and then just re-add it
        GameManager.Instance.manaPerClick.AddModifier(mpcStatMod);
        GameManager.Instance.manaPerSecond.AddModifier(mpsStatMod);
    }

    private void RevealUnit(GameObject unit)
    {
        int index = 0;

        for(int i = 0; i < unitObject.Length; i++)
        {
            if(unit == unitObject[i])
            {
                index = i;
            }
        }

        //By purchasing a unit, we reveal the one beside it. We need to make sure we aren't going out of array bounds with this!
        if(index < unitObject.Length - 1)
            unitObject[index + 1].GetComponent<ManaUnitManager>().RevealUnit();
    }
}
