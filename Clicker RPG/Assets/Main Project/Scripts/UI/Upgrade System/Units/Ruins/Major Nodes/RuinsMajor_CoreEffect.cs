using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS NODE ACTIVATES YOUR CORE EFFECT FOR MANA PICKS

public class RuinsMajor_CoreEffect : MajorUpgradeNode
{

    public List<GameObject> _connectedNodes = new List<GameObject>();
    public UpgradeController upgradeController;
    public bool _isActive;
    public int _requiredRank;

    public Button _nodeBtn;
    public Image _nodeShadow;

    RuinsMajor_CoreEffect()
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
        if(isPurchased)
            return "<color=#00FF50>Activates your Core Effect.</color>\n\n<size=18><color=#FFFFFF>CORE EFFECT:</color>\n<color=#00FF50>Unlocks <color=#FFFFFF>Research</color>. Researching allows you to improve the rank of a selected Unit after a period of time at no additional cost.</color>";


        return "<color=#FFFFFF>Activates your Core Effect.\n\n<color=#5E5E5E><size=18>CORE EFFECT:\nUnlocks Research. Research allows you to improve the rank of a selected Unit after a period of time at no additional cost.</color>";
    }

    public override string FetchUpgradeListText()
    {
        return "<color=#00FF50>+ Core Effect activated.</color>\n";
    }

    public override void ApplyMajorBonus() 
    {
        if(upgradeController.ManaUnit is RuinsUnit)
        {
            RuinsUnit ruins = (RuinsUnit)upgradeController.ManaUnit;
            ruins.SetCoreEffectState(true);
        }
    }
    public override void RemoveMajorBonus() 
    {
        if (upgradeController.ManaUnit is RuinsUnit)
        {
            RuinsUnit ruins = (RuinsUnit)upgradeController.ManaUnit;
            ruins.SetCoreEffectState(false);
        }
    }
}
