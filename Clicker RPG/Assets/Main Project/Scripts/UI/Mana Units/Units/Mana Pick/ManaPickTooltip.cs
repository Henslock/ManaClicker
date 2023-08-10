using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaPickTooltip : UnitTooltip
{
    public Sprite mpIcon;
    ManaPickTooltip()
    {
        fadeDelay = 0f;
        verticalLock = true;
        horizontalLock = false;
    }

    public override void SetupTooltip()
    {
        base.SetupTooltip();
        ManaPickUnit manaPick = gameObject.GetComponent<ManaPickUnit>();
        if (manaPick == null)
            return;

        unitIcon = mpIcon;
        headerText = manaPick.UnitName.ToUpper();
        rankText = "<size=13>RANK</size>" + manaPick.GetTrueTotalOwned().ToString();
        descriptionText = "\"" + manaPick.UnitDescription + "\"";
        if (manaPick.IsCoreEffectActive)
        {
            bonusEffectText = "<color=#FFFFFF>CORE EFFECT:</color>\n";
            bonusEffectText += "Every <color=#FFFFFF>" + manaPick.coreEffectRankThreshold.value + "</color> ranks of your Mana Pick increases your Mana Per Click by <color=#FFFFFF>"+ manaPick.coreEffectScalar.value.ToString() +"%</color> of your total Mana Per Second.";
        }
        else
            bonusEffectText = string.Empty;

        string totalMPC = AbbrevationUtility.AbbreviateNumber(manaPick.ManaPerClick.value, AbbrevationUtility.FormatType.TYPE_DECIMAL);
        if(manaPick.ManaPerClick.value <= 0)
        {
            totalMPC = "0";
        }

        string ratioMPC = ((manaPick.ManaPerClick.value / GameManager.Instance.manaPerClick.value) * 100f).ToString("F1");
        statsText = "• Currently generating "+ totalMPC + " MPC <color=#9D9D9D>(" + ratioMPC + "%)</color>\n";

        if(manaPick.GetTrueTotalOwned() >=manaPick.coreEffectRankThreshold.value && manaPick.IsCoreEffectActive)
        {
            statsText += "• Your current bonus effect is +"+manaPick.GetBonusEffectPercent().ToString()+"% <color=#9D9D9D>(+"+ AbbrevationUtility.AbbreviateNumber(manaPick.GetBonusEffectAmount())+")</color>\n";
        }

        if(manaPick.bonusRanks.value > 0)
        {
            statsText += "• You have +" + manaPick.bonusRanks.value.ToString() + " bonus ranks!\n";
        }

        if (manaPick.basePerClick.value > 0)
        {
            statsText += "• +"+ manaPick.basePerClick.value.ToString() +" Base MPC per rank\n";
        }
    }

    public override void CalculateLockOffset()
    {
        base.CalculateLockOffset();

        verticalLockOffset = 150;
    }
}
