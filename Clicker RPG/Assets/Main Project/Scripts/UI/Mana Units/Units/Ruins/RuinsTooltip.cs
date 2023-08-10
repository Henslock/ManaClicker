using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuinsTooltip : UnitTooltip
{
    public Sprite mpIcon;
    RuinsTooltip()
    {
        fadeDelay = 0f;
        verticalLock = true;
        horizontalLock = false;
    }

    public override void SetupTooltip()
    {
        base.SetupTooltip();
        RuinsUnit ruinUnit = gameObject.GetComponent<RuinsUnit>();
        if (ruinUnit == null)
            return;

        double shiftModifier = GameManager.Instance.manaPerSecond.CalculatePercentShiftFromStatModifiers();

        unitIcon = mpIcon;
        headerText = ruinUnit.UnitName.ToUpper();
        rankText = "<size=13>RANK</size>" + ruinUnit.AmountOwned.ToString();
        descriptionText = "\"" + ruinUnit.UnitDescription + "\"";

        if (ruinUnit.IsCoreEffectActive)
        {
            bonusEffectText = "<color=#FFFFFF>CORE EFFECT:</color>\n";
            bonusEffectText += "Unlocks Researching. Every 30 minutes, you gain a rank for free in the Mana Unit of your choice.";
        }
        else
            bonusEffectText = string.Empty;

        string totalMPS = AbbrevationUtility.AbbreviateNumber(ruinUnit.ManaPerSecond.value * shiftModifier, AbbrevationUtility.FormatType.TYPE_DECIMAL);
        if(ruinUnit.ManaPerSecond.value <= 0)
        {
            totalMPS = "0";
        }
        string ratioMPS = (((ruinUnit.ManaPerSecond.value* shiftModifier) / GameManager.Instance.manaPerSecond.value) * 100f).ToString("F1");
        statsText = "• Currently generating "+ totalMPS + " MPS <color=#9D9D9D>(" + ratioMPS + "%)</color>\n";

        if (ruinUnit.bonusRanks.value > 0)
        {
            statsText += "• You have +" + ruinUnit.bonusRanks.value.ToString() + " bonus ranks!\n";
        }

        if (ruinUnit.basePerSecond.value > 0)
        {
            statsText += "• +" + ruinUnit.basePerSecond.value.ToString() + " Base MPS per rank\n";
        }

        if (ruinUnit.researchTimeReduction.value > 0)
        {
            statsText += "• " + ruinUnit.researchTimeReduction.value.ToString() + "% research time reduction.\n";
        }
    }

    public override void CalculateLockOffset()
    {
        base.CalculateLockOffset();

        verticalLockOffset = 150;
    }
}
