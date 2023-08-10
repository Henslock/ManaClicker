using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttributeTooltip : BasicTooltip
{
    public enum AttributeTypes
    {
        STRENGTH,
        INTELLIGENCE,
        AGILITY,
        CHARISMA,
        POWER

    }

    public AttributeTypes attributeType;

    AttributeTooltip()
    {
        fadeDelay = 0f;
        verticalLock = false;
        horizontalLock = true;
        maxTooltipWidth = 200;
    }

    public override void SetupTooltip()
    {
        base.SetupTooltip();

        PlayerStats playerStats = GameManager.Instance.player.GetComponent<PlayerStats>();

        headerText = "";
        descriptionText = "Tooltip.";

        switch(attributeType)
        {
            case AttributeTypes.STRENGTH:
                {
                    headerText = "<color=#FF6161><size=20>STR</size></color>";
                    descriptionText = "<size=14>Strength empowers your clicks, causing them to grant more mana. Additionally, increases the chance that a click will critically strike to acquire double mana!</size>";
                    break;
                }
            case AttributeTypes.INTELLIGENCE:
                {
                    headerText = "<color=#61C3FF><size=20>INT</size></color>";
                    descriptionText = "<size=14>Intelligence improves your passive mana generation, as well as granting cooldown reduction for your class abilities.</size>";
                    break;
                }
            case AttributeTypes.AGILITY:
                {
                    headerText = "<color=#62FF7E><size=20>AGI</size></color>";
                    descriptionText = "<size=14>Agility will improve your luck, causing things to swing more in your favour. Additionally, it will improve the base effectiveness of all your units.</size>";
                    break;
                }
            case AttributeTypes.CHARISMA:
                {
                    headerText = "<color=#FFC461><size=20>CHR</size></color>";
                    descriptionText = "<size=14>Charisma allows you to purchase all upgrades and new units at a percent discounted cost.</size>";
                    break;
                }
            case AttributeTypes.POWER:
                {
                    headerText = "<color=#FF61C3><size=20>PWR</size></color>";
                    descriptionText = "<size=14>Power will increase the effectiveness of your abilities, as well as slightly increasing the effectiveness of all mana generation.</size>";
                    break;
                }
        }
    }

    public override void CalculateLockOffset()
    {
        base.CalculateLockOffset();

        horizontalLockOffset = 400f;
    }
}
