using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuinsUnit : ManaUnit
{
    [HideInInspector]
    public Stat costReduction = new Stat();
    [HideInInspector]
    public Stat bonusRanks = new Stat();
    [HideInInspector]
    public Stat basePerSecond = new Stat();
    [HideInInspector]
    public Stat researchTimeReduction = new Stat();
    [HideInInspector]
    public bool PERK_ObservingTheStars = false;

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

    public RuinsUnit()
    {
        manaPerClick = new Stat();
        manaPerSecond = new Stat();
        unitName = "Ruins";
        unitDescription = "An ancient structure that appeared when Mana was first discovered.";
        baseCost = 100;
        amountOwned = 0;
        manaPerSecond.baseValue = 0;
        manaPerClick.baseValue = 0;
        isCoreEffectActive = false;
        PERK_ObservingTheStars = false;
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

        return Math.Floor(baseCost * (Math.Pow(1.15, amountOwned)) * costreducVal * ObservingTheStarsCheck());
    }

    private double ObservingTheStarsCheck()
    {
        return PERK_ObservingTheStars ? 0.25f : 1f; //75% Unit Cost reduction with this perk.
    }

    public void UpdateBaseMPS(GameObject manaUnit)
    {
        manaPerSecond.baseValue = ((amountOwned + bonusRanks.value) * 1f) + ((amountOwned + bonusRanks.value) * basePerSecond.value);
    }

    public override double GetTrueTotalOwned()
    {
        return amountOwned + bonusRanks.value;
    }

}
