using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    protected ManaUnit mUnit;
    protected double baseCost;
    protected int amountUpgradesOwned;
    protected UpgradeNodeMaster upgradeNodeMaster;

    protected UpgradeTextController tController;
    protected UpgradeTitleController tTitle;

    public ManaUnit ManaUnit => mUnit;
    public double BaseCost => baseCost;
    public int AmountUpgradesOwned => amountUpgradesOwned;
    public UpgradeNodeMaster UpgradeNodeMaster => upgradeNodeMaster;

    public virtual void ApplyBonuses() { }
    public virtual void ResetAllNodes() { }
    public virtual void InitTextList() { }
    public virtual void UpdateTextList() { }
    public virtual void UpdateTitle() 
    {
        tTitle.SetTitleText(mUnit.name, (int)mUnit.GetTrueTotalOwned());
    }
    protected virtual void UpdateTotalUpgrades(GameObject obj) { }
    protected virtual void UpdateNodeVisuals(GameObject obj) { }

    public virtual int GetUpgradeUnitRank() { return (int)mUnit.GetTrueTotalOwned(); }

    protected virtual void IncreaseAmountOwned(GameObject obj)
    {
        //Check to make sure the unit upgrade is for this unit controller
        if (obj.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iupgrade))
        {
            if (iupgrade.nodeMaster == upgradeNodeMaster)
                amountUpgradesOwned++;
        }
    }

    public virtual double CalculateCost()
    {
        return Math.Floor(baseCost * (Math.Pow(1.75, amountUpgradesOwned)));
    }
}
