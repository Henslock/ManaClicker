using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS NODE ACTIVATES YOUR CORE EFFECT FOR MANA PICKS

public class InfusedSoil_BlessingOfTheSun : MajorUpgradeNode
{

    public List<GameObject> _connectedNodes = new List<GameObject>();
    public UpgradeController upgradeController;
    public bool _isActive;
    public int _requiredRank;

    public Button _nodeBtn;
    public Image _nodeShadow;

    public InfusedSoil_BlessingOfTheMoon majorMoonBlessing;

    InfusedSoil_BlessingOfTheSun()
    {
        //This needs to populate before Start()
        connectedNodes = _connectedNodes;
        unitUpgradeController = upgradeController;
    }
    protected override void Awake()
    {
        isActive = _isActive;
        isPurchased = false;
        requiredRank = _requiredRank;
        nodeMaster = UpgradeNodeMaster.INFUSED_SOIL;
        unitUpgradeController = upgradeController;

        nodeBtn = _nodeBtn;
        nodeShadow = _nodeShadow;

        if (isActive == false)
        {
            nodeBtn.interactable = false;
        }
   
    }

    public override void Purchase() 
    {
        InfusedSoilUnit mUnit = (InfusedSoilUnit)upgradeController.ManaUnit;
        if (mUnit.chosenBlessing != InfusedSoilUnit.ChosenBlessingEnum.NONE) { return; }

        if (isActive && !isPurchased)
        {
            if (GameManager.Instance.manaAmount >= unitUpgradeController.CalculateCost() && unitUpgradeController.GetUpgradeUnitRank() >= requiredRank)
            {
                GameManager.Instance.IncrementMana(-1 * unitUpgradeController.CalculateCost());
                isPurchased = true;
                ActivateNeighbourNodes();
                
                EventManager.UnitEvents.OnUpgradePurchasedFromUnit?.Invoke(gameObject);
                EventManager.UIEvents.UpdateNodeAppearance?.Invoke(gameObject);

                majorMoonBlessing.Deactivate();
            }
        }
    }


    public override void ResetNode() 
    {
        isActive = _isActive;
        isPurchased = false;

        if (isActive == false)
        {
            nodeBtn.interactable = false;
        }

        RemoveMajorBonus();
    }

    public override void ActivateNeighbourNodes()
    {
        base.ActivateNeighbourNodes();
    }

    //Activating a node is when a node can be unlocked because a neighboring node is connected to it
    //You will still need enough Mana and enough rank to actually purchase the unit
    public override void Activate()
    {
        if (majorMoonBlessing.isPurchased)
        {
            return;
        }
        else
        {
            isActive = true;
            nodeBtn.interactable = true;
        }
    }

    public void Deactivate()
    {
        isActive = false;
        nodeBtn.interactable = false;
        EventManager.UIEvents.UpdateNodeAppearance?.Invoke(gameObject);
        EventManager.UnitEvents.OnUpgradePurchasedFromUnit?.Invoke(gameObject);
    }

    public override string FetchUpgradeText()
    {
        if (isPurchased)
            return "<size=18>BLESSING OF THE SUN:\n<color=#00FF50>Your Infused Soil is <color=#FFFFFF>50%</color> more efficient during the <color=#FFFFFF>Day</color>. You may only have one blessing at a time.</color>";


        return "<size=18>BLESSING OF THE SUN:\n<color=#5E5E5E>Your Infused Soil is 50% more efficient during the Day. You may only have one blessing at a time.</color>";
    }

    public override string FetchUpgradeListText()
    {
        return "<color=#00FF50>+ Blessing of the Sun - your Infused Soil is 50% more efficient during the Day.</color>\n";
    }

    public override void ApplyMajorBonus() 
    {
        if (upgradeController.ManaUnit is InfusedSoilUnit)
        {
            InfusedSoilUnit mUnit = (InfusedSoilUnit)upgradeController.ManaUnit;

            mUnit.chosenBlessing = InfusedSoilUnit.ChosenBlessingEnum.SUN;
        }
    }
    public override void RemoveMajorBonus() 
    {
        if (upgradeController.ManaUnit is InfusedSoilUnit)
        {
            InfusedSoilUnit mUnit = (InfusedSoilUnit)upgradeController.ManaUnit;

            mUnit.chosenBlessing = InfusedSoilUnit.ChosenBlessingEnum.NONE;
        }
    }
}
