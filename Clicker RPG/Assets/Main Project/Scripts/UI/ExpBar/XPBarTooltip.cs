using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPBarTooltip : BasicTooltip
{
    XPBarTooltip()
    {
        fadeDelay = 0.5f;
        verticalLock = false;
        horizontalLock = false;
        maxTooltipWidth = 205;
    }

    public override void SetupTooltip()
    {
        base.SetupTooltip();

        PlayerStats playerStats = GameManager.Instance.player.GetComponent<PlayerStats>();


        string requiredXp = (playerStats.playerExp).ToString() + "/" + (LevelingTable.GetRequiredExpForLevel(playerStats.playerLevel)).ToString();
        string percentageXp = "<size=14><color=#9D9D9D>(" + (playerStats.GetLevelingPercentage() * 100).ToString("F1") + "%)</color></size>";

        headerText = string.Empty;
        descriptionText = requiredXp + "\n" + percentageXp;
    }

    public override void CalculateLockOffset()
    {
        base.CalculateLockOffset();
    }
}
