using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfusedSoilTooltip : UnitTooltip
{
    public Sprite mpIcon;
    InfusedSoilTooltip()
    {
        fadeDelay = 0f;
        verticalLock = true;
        horizontalLock = false;
    }

    public override void SetupTooltip()
    {
        base.SetupTooltip();
        InfusedSoilUnit infusedSoilUnit = gameObject.GetComponent<InfusedSoilUnit>();
        if (infusedSoilUnit == null)
            return;
        double shiftModifier = GameManager.Instance.manaPerSecond.CalculatePercentShiftFromStatModifiers();

        unitIcon = mpIcon;
        headerText = infusedSoilUnit.UnitName.ToUpper();
        rankText = "<size=13>RANK</size>" + infusedSoilUnit.AmountOwned.ToString();
        descriptionText = "\"" + infusedSoilUnit.UnitDescription + "\"";

        if (infusedSoilUnit.IsCoreEffectActive)
        {
            bonusEffectText = "<color=#FFFFFF>CORE EFFECT:</color>\n";
            bonusEffectText += "An unstable rift has appeared, feeding it enough mana will cause a miracle to occur.";
        }
        else
            bonusEffectText = string.Empty;

        string totalMPS = AbbrevationUtility.AbbreviateNumber(infusedSoilUnit.ManaPerSecond.value * shiftModifier, AbbrevationUtility.FormatType.TYPE_DECIMAL);
        if(infusedSoilUnit.ManaPerSecond.value <= 0)
        {
            totalMPS = "0";
        }
        string ratioMPS = (((infusedSoilUnit.ManaPerSecond.value * shiftModifier) / GameManager.Instance.manaPerSecond.value) * 100f).ToString("F1");
        statsText = "• Currently generating "+ totalMPS + " MPS <color=#9D9D9D>(" + ratioMPS + "%)</color>\n";

        if (infusedSoilUnit.bonusRanks.value > 0)
        {
            statsText += "• You have +" + infusedSoilUnit.bonusRanks.value.ToString() + " bonus ranks!\n";
        }

        if (infusedSoilUnit.basePerSecond.value > 0)
        {
            statsText += "• +" + infusedSoilUnit.basePerSecond.value.ToString() + " Base MPS per rank\n";
        }

        if (infusedSoilUnit.blessingBoost.value > 0)
        {
            statsText += "• " + infusedSoilUnit.blessingBoost.value.ToString() + "% improved chosen blessing.\n";
        }
    }

    public override void CalculateLockOffset()
    {
        base.CalculateLockOffset();

        verticalLockOffset = 150;
    }
}
