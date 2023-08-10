using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfusedSoilUnit : ManaUnit
{

    public enum ChosenBlessingEnum
    {
        NONE,
        SUN,
        MOON,
        AMBIVALENT
    }
    [HideInInspector]
    public Stat costReduction = new Stat();
    [HideInInspector]
    public Stat bonusRanks = new Stat();
    [HideInInspector]
    public Stat basePerSecond = new Stat();
    [HideInInspector]
    public Stat blessingBoost = new Stat();
    [HideInInspector]
    public ChosenBlessingEnum chosenBlessing = ChosenBlessingEnum.NONE;

    private void OnEnable()
    {
        EventManager.UnitEvents.OnUnitPurchased += UpdateBaseMPS;
        EventManager.UnitEvents.OnUpgradePurchasedFromUnit += UpdateBaseMPS;
        EventManager.UnitEvents.RecalculateManaUnitData += UpdateData;
    }

    private void OnDisable()
    {
        EventManager.UnitEvents.OnUnitPurchased -= UpdateBaseMPS;
        EventManager.UnitEvents.OnUpgradePurchasedFromUnit -= UpdateBaseMPS;
        EventManager.UnitEvents.RecalculateManaUnitData -= UpdateData;
    }

    public InfusedSoilUnit()
    {
        manaPerClick = new Stat();
        manaPerSecond = new Stat();
        unitName = "Infused Soil";
        unitDescription = "Natural mana now appears from the earth!";
        baseCost = 1200;
        amountOwned = 0;
        manaPerSecond.baseValue = 0;
        manaPerClick.baseValue = 0;
        isCoreEffectActive = false;
    }

    private void UpdateData()
    {
        UpdateBaseMPS(gameObject);
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
        manaPerSecond.baseValue = ((amountOwned + bonusRanks.value) * 8.5f) + ((amountOwned + bonusRanks.value) * basePerSecond.value);
    }

    public override double GetTrueTotalOwned()
    {
        return amountOwned + bonusRanks.value;
    }

}
