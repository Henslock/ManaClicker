using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreasuryTooltip : UnitTooltip
{
    public Sprite mpIcon;
    TreasuryTooltip()
    {
        fadeDelay = 0f;
        verticalLock = true;
        horizontalLock = false;
    }

    public override void SetupTooltip()
    {
        base.SetupTooltip();
        TreasuryUnit treasuryUnit = gameObject.GetComponent<TreasuryUnit>();
        if (treasuryUnit == null)
            return;
        double shiftModifier = GameManager.Instance.manaPerSecond.CalculatePercentShiftFromStatModifiers();

        unitIcon = mpIcon;
        headerText = treasuryUnit.UnitName.ToUpper();
        rankText = "<size=13>RANK</size>" + treasuryUnit.AmountOwned.ToString();
        descriptionText = "\"" + treasuryUnit.UnitDescription + "\"";
        bonusEffectText = "<color=#FFFFFF>CORE EFFECT:</color>\n";
        bonusEffectText += "Unlocks Mana Shards, special clusters that appear on screen that can be clicked for additional mana.";

        string totalMPS = AbbrevationUtility.AbbreviateNumber(treasuryUnit.ManaPerSecond.value * shiftModifier);
        if(treasuryUnit.ManaPerSecond.value <= 0)
        {
            totalMPS = "0";
        }
        string ratioMPS = (((treasuryUnit.ManaPerSecond.value * shiftModifier) / GameManager.Instance.manaPerSecond.value) * 100f).ToString("F1");
        statsText = "• Currently generating "+ totalMPS + " MPS <color=#9D9D9D>(" + ratioMPS + "%)</color>";
    }

    public override void CalculateLockOffset()
    {
        base.CalculateLockOffset();

        verticalLockOffset = 150;
    }
}
