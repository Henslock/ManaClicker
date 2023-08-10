using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfusedSoilBlessingUpgradeTooltip : UnitUpgradeTooltip
{
    private MajorUpgradeNode majorUpgradeNode;

    public InfusedSoil_BlessingOfTheMoon moonBlessing;
    public InfusedSoil_BlessingOfTheSun sunBlessing;
    InfusedSoilBlessingUpgradeTooltip()
    {
        fadeDelay = 0f;
        verticalLock = true;
        horizontalLock = true;
    }

    private void Start()
    {
        majorUpgradeNode = gameObject.GetComponent<MajorUpgradeNode>();
    }

    public override void SetupTooltip()
    {
        base.SetupTooltip();
        if (majorUpgradeNode == null)
            return;

        headerText = "MAJOR NODE";
        upgradeNodeType = UpgradeNodeType.MAJOR;

        rankText = "<size=10><color=#8A8A8A>Requires</color></size> <color=#FF4C62>RANK " + majorUpgradeNode.requiredRank + "</color>";
        if (majorUpgradeNode.unitUpgradeController.GetUpgradeUnitRank() >= majorUpgradeNode.requiredRank)
        {
            rankText = "<size=10><color=#8A8A8A>Requires</color></size> RANK " + majorUpgradeNode.requiredRank;
        }
        if(majorUpgradeNode.requiredRank <= 0)
        {
            rankText = string.Empty;
        }
        
        costText = AbbrevationUtility.AbbreviateNumber(majorUpgradeNode.unitUpgradeController.CalculateCost());

        //Cant Afford the node
        if(majorUpgradeNode.unitUpgradeController.CalculateCost() > GameManager.Instance.manaAmount)
        {
            costText = "<color=#FF4C62>" + AbbrevationUtility.AbbreviateNumber(majorUpgradeNode.unitUpgradeController.CalculateCost()) + "</color>";
        }

        //Node is inactive
        if(majorUpgradeNode.isActive == false)
        {
            costText = "<color=#333333>INACTIVE</color>";
            descriptionText = "<color=#808080>";
        }

        //Node is purchased
        if (majorUpgradeNode.isPurchased == true)
        {
            costText = "<color=#2FFF91>ACTIVE</color>";
        }

        if(majorUpgradeNode.unitUpgradeController.ManaUnit is InfusedSoilUnit)
        {
            if(gameObject.GetComponent<InfusedSoil_BlessingOfTheMoon>() != null)
            {
                if (sunBlessing.isPurchased)
                    descriptionText = "<size=18>BLESSING OF THE MOON:\n<color=#5E5E5E>Your Infused Soil is 50% more efficient during the Night. You may only have one blessing at a time.</color>\n You have already chosen a blessing.";
                else
                    descriptionText = majorUpgradeNode.FetchUpgradeText();
            }

            if (gameObject.GetComponent<InfusedSoil_BlessingOfTheSun>() != null)
            {
                if (moonBlessing.isPurchased)
                    descriptionText = "<size=18>BLESSING OF THE SUN:\n<color=#5E5E5E>Your Infused Soil is 50% more efficient during the Day. You may only have one blessing at a time.</color>\n You have already chosen a blessing.";
                else
                    descriptionText = majorUpgradeNode.FetchUpgradeText();
            }
        }
        else
            descriptionText = majorUpgradeNode.FetchUpgradeText();
    }

    public override void CalculateLockOffset()
    {
        base.CalculateLockOffset();

        Vector2 screenPos = GameManager.Instance.uiCam.WorldToScreenPoint(gameObject.GetComponent<RectTransform>().position);
        verticalLockOffset = screenPos.y + 25;
        horizontalLockOffset = screenPos.x;
    }
}
