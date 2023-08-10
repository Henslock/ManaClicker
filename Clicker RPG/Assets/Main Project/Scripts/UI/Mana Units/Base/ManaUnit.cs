using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaUnit : MonoBehaviour
{
    protected string unitName;
    protected string unitDescription;
    protected double baseCost;
    protected int amountOwned;
    protected Stat manaPerClick = new Stat();
    protected Stat manaPerSecond = new Stat();
    protected bool isCoreEffectActive { get; set; }

    public Stat ManaPerClick => manaPerClick;
    public Stat ManaPerSecond => manaPerSecond;
    public string UnitName => unitName;
    public string UnitDescription => unitDescription;
    public int AmountOwned => amountOwned;
    public bool IsCoreEffectActive => isCoreEffectActive;

    public virtual double CalculateCost()
    {
        return baseCost;
    }

    public virtual void SetCoreEffectState(bool state)
    {
        isCoreEffectActive = state;
    }

    public void SetAmountOwned(int val)
    {
        amountOwned = val;
    }

    public virtual void CalculateBonuses()
    {

    }

    public virtual double GetTrueTotalOwned()
    {
        return amountOwned;
    }
}
