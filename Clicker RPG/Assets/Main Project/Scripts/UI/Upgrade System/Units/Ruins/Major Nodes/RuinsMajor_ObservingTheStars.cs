using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS NODE ACTIVATES YOUR CORE EFFECT FOR MANA PICKS

public class RuinsMajor_ObservingTheStars : MajorUpgradeNode
{

    public List<GameObject> _connectedNodes = new List<GameObject>();
    public UpgradeController upgradeController;
    public bool _isActive;
    public int _requiredRank;

    public Button _nodeBtn;
    public Image _nodeShadow;

    RuinsMajor_ObservingTheStars()
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
        nodeMaster = UpgradeNodeMaster.RUINS;
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
        if (isPurchased)
            return "<size=18>OBSERVING THE STARS:\n<color=#00FF50>Grants <color=#FFFFFF>75%</color> unit cost reduction that stacks multiplicatively.</color>";


        return "<size=18>OBSERVING THE STARS:\n<color=#5E5E5E>Grants 75% unit cost reduction that stacks multiplicatively.</color>";
    }

    public override string FetchUpgradeListText()
    {
        return "<color=#00FF50>+ 75% unit cost reduction (multiplicative).</color>\n";
    }

    public override void ApplyMajorBonus() 
    {
        if(upgradeController.ManaUnit is RuinsUnit)
        {
            RuinsUnit ruins = (RuinsUnit)upgradeController.ManaUnit;
            ruins.PERK_ObservingTheStars = true;
        }
    }
    public override void RemoveMajorBonus() 
    {
        if (upgradeController.ManaUnit is RuinsUnit)
        {
            RuinsUnit ruins = (RuinsUnit)upgradeController.ManaUnit;
            ruins.PERK_ObservingTheStars = false;
        }
    }
}
