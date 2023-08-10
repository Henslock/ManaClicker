using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ManaPickUnit : ManaUnit
{
    [HideInInspector]
    public Stat costReduction = new Stat();
    [HideInInspector]
    public Stat bonusRanks = new Stat();
    [HideInInspector]
    public Stat basePerClick = new Stat();
    [HideInInspector]
    public Stat coreEffectScalar = new Stat();
    [HideInInspector]
    public Stat coreEffectRankThreshold = new Stat();

    public bool isKnowledgeStrikesActive { get; private set; }

    private void OnEnable()
    {
        EventManager.UnitEvents.OnUnitPurchased += UpdateBaseMPC;
        EventManager.UnitEvents.OnUpgradePurchasedFromUnit += UpdateBaseMPC;
        EventManager.UnitEvents.RecalculateManaUnitData += UpdateData;
        EventManager.GeneralEvents.OnClickCrystal += KnowledgeStrikesRoll;
    }

    private void OnDisable()
    {
        EventManager.UnitEvents.OnUnitPurchased -= UpdateBaseMPC;
        EventManager.UnitEvents.OnUpgradePurchasedFromUnit -= UpdateBaseMPC;
        EventManager.UnitEvents.RecalculateManaUnitData -= UpdateData;
        EventManager.GeneralEvents.OnClickCrystal -= KnowledgeStrikesRoll;
    }

    public ManaPickUnit()
    {
        manaPerClick = new Stat();
        manaPerSecond = new Stat();
        unitName = "Mana Pick";
        unitDescription = "The earth is now ripe with mana crystals, let's go harvesting!";
        baseCost = 10;
        amountOwned = 0;
        manaPerSecond.baseValue = 0;
        manaPerClick.baseValue = 0;

        isCoreEffectActive = false;
        isKnowledgeStrikesActive = false;
    }

    private void Start()
    {
        coreEffectScalar.baseValue = 5;
        coreEffectRankThreshold.baseValue = 25;
    }

    private void UpdateData()
    {
        UpdateBaseMPC(gameObject);
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

    public override double GetTrueTotalOwned()
    {
        return amountOwned + bonusRanks.value;
    }

    public void UpdateBaseMPC(GameObject manaUnit)
    {
        manaPerClick.baseValue = ((amountOwned + bonusRanks.value) * 0.2f) + ((amountOwned + bonusRanks.value) * basePerClick.value);
    }

    public override void CalculateBonuses()
    {
        if (isCoreEffectActive)
        {
            manaPerClick.RemoveAllModifiersFromSource(this);
            manaPerClick.AddModifier(new StatModifier(GetBonusEffectAmount(), StatModType.FLAT, 3, this));
        }
        else
        {
            manaPerClick.RemoveAllModifiersFromSource(this);
        }
    }

    public double GetBonusEffectPercent()
    {
        //Core bonus is 5% of our MPS gets turned into MPC for every 25 ranks (starting off)
        return (Mathf.Floor((amountOwned + (float)bonusRanks.value) / (float)coreEffectRankThreshold.value) * coreEffectScalar.value);
    }

    public double GetBonusEffectAmount()
    {
        return GameManager.Instance.manaPerSecond.value * (GetBonusEffectPercent() / 100);
    }

    public void SetKnowledgeStrikesStatus(bool state)
    {
        isKnowledgeStrikesActive = state;
    }


    //KnowledgeStrikes
    private void KnowledgeStrikesRoll()
    {
        if (!isKnowledgeStrikesActive) { return; }

        float randomRoll = Random.Range(0f, 1f);
        if ((randomRoll * 100f) <= 25f)
        {
            PlayerStats plrStats = GameManager.Instance.player.GetComponent<PlayerStats>();
            plrStats.GrantExp(1);
        }
    }
}
