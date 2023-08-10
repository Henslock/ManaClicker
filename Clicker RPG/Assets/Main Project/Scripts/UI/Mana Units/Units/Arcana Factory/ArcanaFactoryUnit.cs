using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcanaFactoryUnit : ManaUnit
{
    public List<ManaUnit> manaUnits = new List<ManaUnit>();
    private void OnEnable()
    {
        EventManager.UnitEvents.OnUnitPurchased += UpdateBaseMPS;
    }

    private void OnDisable()
    {
        EventManager.UnitEvents.OnUnitPurchased -= UpdateBaseMPS;
    }

    public ArcanaFactoryUnit()
    {
        manaPerClick = new Stat();
        manaPerSecond = new Stat();
        unitName = "Arcana Factory";
        unitDescription = "We can use all of this mana to become more efficient.";
        baseCost = 192000;
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
        manaPerSecond.baseValue = (amountOwned * 660f);
    }

    public override void CalculateBonuses()
    {
        double bonusAmnt = (Math.Floor((double)amountOwned / 30) * 5); //Every 25 factories we gain 5% increased effectiveness on all units
        foreach(ManaUnit mUnit in manaUnits)
        {
            mUnit.ManaPerClick.RemoveAllModifiersFromSource(this);
            mUnit.ManaPerClick.AddModifier(new StatModifier(bonusAmnt, StatModType.ADDITIVEPERCENT, this));

            mUnit.ManaPerSecond.RemoveAllModifiersFromSource(this);
            mUnit.ManaPerSecond.AddModifier(new StatModifier(bonusAmnt, StatModType.ADDITIVEPERCENT, this));
        }
    }
}
