using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasuryUnit : ManaUnit
{
    private void OnEnable()
    {
        EventManager.UnitEvents.OnUnitPurchased += UpdateBaseMPS;
    }

    private void OnDisable()
    {
        EventManager.UnitEvents.OnUnitPurchased -= UpdateBaseMPS;
    }

    public TreasuryUnit()
    {
        manaPerClick = new Stat();
        manaPerSecond = new Stat();
        unitName = "Treasury";
        unitDescription = "Mana has become increasingly more valuable, I smell a rich economy ahead!";
        baseCost = 1150000;
        amountOwned = 0;
        manaPerSecond.baseValue = 0;
        manaPerClick.baseValue = 0;
    }

    public override double CalculateCost()
    {
        return Math.Floor(baseCost * (Math.Pow(1.15, amountOwned)));
    }

    public void UpdateBaseMPS(GameObject manaUnit)
    {
        manaPerSecond.baseValue = (amountOwned * 9720);
    }

}
