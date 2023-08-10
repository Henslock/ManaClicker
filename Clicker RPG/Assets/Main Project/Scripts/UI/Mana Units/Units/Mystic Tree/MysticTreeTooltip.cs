using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MysticTreeTooltip : UnitTooltip
{
    public Sprite mpIcon;
    MysticTreeTooltip()
    {
        fadeDelay = 0f;
        verticalLock = true;
        horizontalLock = false;
    }

    public override void SetupTooltip()
    {
        base.SetupTooltip();
        MysticTreeUnit mysticTreeUnit = gameObject.GetComponent<MysticTreeUnit>();
        if (mysticTreeUnit == null)
            return;
        double shiftModifier = GameManager.Instance.manaPerSecond.CalculatePercentShiftFromStatModifiers();
        unitIcon = mpIcon;
        headerText = mysticTreeUnit.UnitName.ToUpper();
        rankText = "<size=13>RANK</size>" + mysticTreeUnit.AmountOwned.ToString();
        descriptionText = "\"" + mysticTreeUnit.UnitDescription + "\"";
        bonusEffectText = "<color=#FFFFFF>CORE EFFECT:</color>\n";
        bonusEffectText += "Every 25 ranks of your Mystic Tree grants the player an additional <color=#FFFFFF>1 XP</color> per click.";

        string totalMPS = AbbrevationUtility.AbbreviateNumber(mysticTreeUnit.ManaPerSecond.value * shiftModifier);
        if(mysticTreeUnit.ManaPerSecond.value <= 0)
        {
            totalMPS = "0";
        }
        string ratioMPS = (((mysticTreeUnit.ManaPerSecond.value * shiftModifier) / GameManager.Instance.manaPerSecond.value) * 100f).ToString("F1");
        statsText = "• Currently generating "+ totalMPS + " MPS <color=#9D9D9D>(" + ratioMPS + "%)</color>\n";

        if(mysticTreeUnit.GetBonusEffectAmount() >= 1)
        {
            statsText += "• Your Mystic Tree is granting you an additional " + mysticTreeUnit.GetBonusEffectAmount().ToString() + " XP per click\n";
        }
    }

    public override void CalculateLockOffset()
    {
        base.CalculateLockOffset();

        verticalLockOffset = 150;
    }
}
