using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS NODE GIVES 5% BASE CRIT STIKE

public class ManaPickMajor_ChipDamage : MajorUpgradeNode
{

    public List<GameObject> _connectedNodes = new List<GameObject>();
    public UpgradeController upgradeController;
    public bool _isActive;
    public int _requiredRank;

    public Button _nodeBtn;
    public Image _nodeShadow;

    ManaPickMajor_ChipDamage()
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
        nodeMaster = UpgradeNodeMaster.MANAPICK;
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
        base.Purchase();
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
        isActive = true;
        nodeBtn.interactable = true;
    }

    public override string FetchUpgradeText()
    {
        if(isPurchased)
            return "<size=18>CHIP DAMAGE <NYI>:\n<color=#00FF50>You have a <color=#FFFFFF>10%</color> chance per click to gain an additional amount of Mana.\nThe amount of Mana gained is anywhere between <color=#FFFFFF>1 ~ 20%</color> of the original amount gained from your click.</color>";


        return "<size=18>CHIP DAMAGE <NYI>:\n<color=#5E5E5E>You have a 10% chance per click to gain an additional amount of Mana.\nThe amount of Mana gained is anywhere between 1 ~ 20% of the original amount gained from your click.</color>";
    }

    public override string FetchUpgradeListText()
    {
        return "<color=#00FF50>+ 10% chance to gain between 1 ~ 20% of your original mana per click as bonus Mana.</color>\n";
    }

    public override void ApplyMajorBonus() 
    {
        if(upgradeController.ManaUnit is ManaPickUnit)
        {
            //Implement it
        }
    }
    public override void RemoveMajorBonus() 
    {
        if (upgradeController.ManaUnit is ManaPickUnit)
        {
        }
    }
}
