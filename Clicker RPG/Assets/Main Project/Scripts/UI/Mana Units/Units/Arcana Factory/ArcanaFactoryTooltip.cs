using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ArcanaFactoryTooltip : UnitTooltip
{
    public Sprite mpIcon;
    ArcanaFactoryTooltip()
    {
        fadeDelay = 0f;
        verticalLock = true;
        horizontalLock = false;
    }

    public override void SetupTooltip()
    {
        base.SetupTooltip();
        ArcanaFactoryUnit arcanaFactoryUnit = gameObject.GetComponent<ArcanaFactoryUnit>();
        if (arcanaFactoryUnit == null)
            return;
        double shiftModifier = GameManager.Instance.manaPerSecond.CalculatePercentShiftFromStatModifiers();

        unitIcon = mpIcon;
        headerText = "<size=17>" + arcanaFactoryUnit.UnitName.ToUpper() + "</size>";
        rankText = "<size=13>RANK</size>" + arcanaFactoryUnit.AmountOwned.ToString();
        descriptionText = "\"" + arcanaFactoryUnit.UnitDescription + "\"";
        bonusEffectText = "<color=#FFFFFF>CORE EFFECT:</color>\n";
        bonusEffectText += "Every 30 ranks of Arcana Factory grants a <color=#FFFFFF>5% Efficiency Boost</color> to all mana units.";

        string totalMPS = AbbrevationUtility.AbbreviateNumber(arcanaFactoryUnit.ManaPerSecond.value * shiftModifier);
        if(arcanaFactoryUnit.ManaPerSecond.value <= 0)
        {
            totalMPS = "0";
        }
        string ratioMPS = (((arcanaFactoryUnit.ManaPerSecond.value * shiftModifier) / GameManager.Instance.manaPerSecond.value) * 100f).ToString("F1");
        statsText = "• Currently generating "+ totalMPS + " MPS <color=#9D9D9D>(" + ratioMPS + "%)</color>\n";

        if(arcanaFactoryUnit.AmountOwned >= 30)
        {
            double bonusAmnt = (Math.Floor((double)arcanaFactoryUnit.AmountOwned / 25) * 5);
            statsText += "• Increasing the effectiveness of all units by " + bonusAmnt.ToString() + "%\n";
        }
    }

    public override void CalculateLockOffset()
    {
        base.CalculateLockOffset();

        verticalLockOffset = 150;
    }
}
