using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticTreeUnit : ManaUnit
{
    [HideInInspector]
    public Stat costReduction = new Stat();
    [HideInInspector]
    public Stat bonusRanks = new Stat();
    [HideInInspector]
    public Stat basePerSecond = new Stat();

    private void OnEnable()
    {
        EventManager.UnitEvents.OnUnitPurchased += UpdateBaseMPS;
    }

    private void OnDisable()
    {
        EventManager.UnitEvents.OnUnitPurchased -= UpdateBaseMPS;
    }

    public MysticTreeUnit()
    {
        manaPerClick = new Stat();
        manaPerSecond = new Stat();
        unitName = "Mystic Tree";
        unitDescription = "This otherwordly tree has brought unique creatures and flora with it.";
        baseCost = 15500;
        amountOwned = 0;
        manaPerSecond.baseValue = 0;
        manaPerClick.baseValue = 0;
    }

    public override double CalculateCost()
    {
        double costreducVal = 0;
        if (costReduction.value <= 0)
            costreducVal = 1;
        else
            costreducVal = costReduction.value;

        return Math.Floor(baseCost * (Math.Pow(1.15, amountOwned)) * costreducVal);
    }

    public void UpdateBaseMPS(GameObject manaUnit)
    {
        manaPerSecond.baseValue = (amountOwned * 82f);

        PlayerStats playerStats = GameManager.Instance.player.GetComponent<PlayerStats>();

        //Every 100 Mystic Trees increases your exp per click by 1
        playerStats.expPerClick.RemoveAllModifiersFromSource(this);
        playerStats.expPerClick.AddModifier(new StatModifier(GetBonusEffectAmount(), StatModType.FLAT, this));
    }

    public double GetBonusEffectAmount()
    {
        return (Mathf.Floor(amountOwned / 25));
    }

    public override double GetTrueTotalOwned()
    {
        return amountOwned + bonusRanks.value;
    }
}
