using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaPickMinorUpgradeTooltip : UnitUpgradeTooltip
{
    private ManaPickMinorUpgradeNode minorUpgradeNode;
    ManaPickMinorUpgradeTooltip()
    {
        fadeDelay = 0f;
        verticalLock = true;
        horizontalLock = true;
    }

    private void Start()
    {
        minorUpgradeNode = gameObject.GetComponent<ManaPickMinorUpgradeNode>();
    }

    public override void SetupTooltip()
    {
        base.SetupTooltip();
        if (minorUpgradeNode == null)
            return;

        headerText = "MINOR NODE";
        upgradeNodeType = UpgradeNodeType.MINOR;
        descriptionText = string.Empty;

        rankText = "<size=10><color=#8A8A8A>Requires</color></size> <color=#FF4C62>RANK " + minorUpgradeNode.requiredRank + "</color>";
        if (minorUpgradeNode.upgradeController.GetUpgradeUnitRank() >= minorUpgradeNode.requiredRank)
        {
            rankText = "<size=10><color=#8A8A8A>Requires</color></size> RANK " + minorUpgradeNode.requiredRank;
        }
        if(minorUpgradeNode.requiredRank <= 0)
        {
            rankText = string.Empty;
        }
        
        costText = AbbrevationUtility.AbbreviateNumber(minorUpgradeNode.upgradeController.CalculateCost());

        //Cant Afford the node
        if(minorUpgradeNode.upgradeController.CalculateCost() > GameManager.Instance.manaAmount)
        {
            costText = "<color=#FF4C62>" + AbbrevationUtility.AbbreviateNumber(minorUpgradeNode.upgradeController.CalculateCost()) + "</color>";
        }

        //Node is inactive
        if(minorUpgradeNode.isActive == false)
        {
            costText = "<color=#333333>INACTIVE</color>";
            descriptionText = "<color=#808080>";
        }

        //Node is purchased
        if (minorUpgradeNode.isPurchased == true)
        {
            costText = "<color=#2FFF91>ACTIVE</color>";
            descriptionText = "<color=#00A5FF>";
        }

        //Set our description text;
        switch (minorUpgradeNode.effect)
        {
            case ManaPickMinorUpgradeNode.Effect.BASEMANAPERCLICK:
                descriptionText += "+" + minorUpgradeNode.upgradeAmount + " Base Mana Per Click";
                break;
            case ManaPickMinorUpgradeNode.Effect.BONUSPICKS:
                descriptionText += "+" + minorUpgradeNode.upgradeAmount + " Free Mana Pick Rank(s)";
                break;
            case ManaPickMinorUpgradeNode.Effect.COSTREDUCTION:
                descriptionText += "+" + minorUpgradeNode.upgradeAmount + "% Unit Cost Reduction";
                break;
            case ManaPickMinorUpgradeNode.Effect.EFFICIENCYBOOST:
                descriptionText += "+" + minorUpgradeNode.upgradeAmount + "% Efficiency Boost";
                break;
            default:
                descriptionText += "Upgrade type not chosen.";
                break;
        }
    }

    public override void CalculateLockOffset()
    {
        base.CalculateLockOffset();

        Vector2 screenPos = GameManager.Instance.uiCam.WorldToScreenPoint(gameObject.GetComponent<RectTransform>().position);
        verticalLockOffset = screenPos.y + 25;
        horizontalLockOffset = screenPos.x;
    }
}