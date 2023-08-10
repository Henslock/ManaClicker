using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS NODE ACTIVATES YOUR CORE EFFECT FOR MANA PICKS

public class ManaPickMajor_CoreEffect : MajorUpgradeNode
{

    public List<GameObject> _connectedNodes = new List<GameObject>();
    public UpgradeController upgradeController;
    public bool _isActive;
    public int _requiredRank;

    public Button _nodeBtn;
    public Image _nodeShadow;

    ManaPickMajor_CoreEffect()
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
            return "<color=#00FF50>Activates your Core Effect.</color>\n\n<size=18><color=#FFFFFF>CORE EFFECT:</color>\n<color=#00FF50>Every 25 ranks of your Mana Pick increases your Mana Per Click by <color=#FFFFFF>5% (Base)</color> of your total Mana Per Second.</color>";


        return "<color=#FFFFFF>Activates your Core Effect.\n\n<color=#5E5E5E><size=18>CORE EFFECT:\nEvery 25 ranks of your Mana Pick increases your Mana Per Click by 5% (Base) of your total Mana Per Second.</color>";
    }

    public override string FetchUpgradeListText()
    {
        return "<color=#00FF50>+ Core Effect activated.</color>\n";
    }

    public override void ApplyMajorBonus() 
    {
        if(upgradeController.ManaUnit is ManaPickUnit)
        {
            ManaPickUnit mPick = (ManaPickUnit)upgradeController.ManaUnit;
            mPick.SetCoreEffectState(true);
        }
    }
    public override void RemoveMajorBonus() 
    {
        if (upgradeController.ManaUnit is ManaPickUnit)
        {
            ManaPickUnit mPick = (ManaPickUnit)upgradeController.ManaUnit;
            mPick.SetCoreEffectState(false);
        }
    }
}
