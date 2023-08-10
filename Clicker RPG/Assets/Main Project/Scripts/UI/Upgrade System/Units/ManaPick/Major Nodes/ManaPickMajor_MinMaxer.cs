using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS NODE REDUCES THE RANKS REQUIRED TO BOOST YOUR CORE EFFECT

public class ManaPickMajor_MinMaxer : MajorUpgradeNode
{

    public List<GameObject> _connectedNodes = new List<GameObject>();
    public UpgradeController upgradeController;
    public bool _isActive;
    public int _requiredRank;

    public Button _nodeBtn;
    public Image _nodeShadow;

    ManaPickMajor_MinMaxer()
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
            return "<size=18>MIN-MAXER:\n<color=#00FF50>Reduces the required amount for your <color=#FFFFFF>Core Effect</color> to improve by <color=#FFFFFF>5</color> ranks.</color>";


        return "<size=18>MIN-MAXER:\n<color=#5E5E5E>Reduces the required amount for your Core Effect to improve by 5 ranks.</color>";
    }

    public override string FetchUpgradeListText()
    {
        return "<color=#00FF50>+ Reduces the required amount for your Core Effect to improve by 5 ranks.</color>\n";
    }

    public override void ApplyMajorBonus() 
    {
        if(upgradeController.ManaUnit is ManaPickUnit)
        {
            ManaPickUnit mPick = (ManaPickUnit)upgradeController.ManaUnit;
            mPick.coreEffectRankThreshold.RemoveAllModifiersFromSource(this);
            mPick.coreEffectRankThreshold.AddModifier(new StatModifier(-5, StatModType.FLAT, this)); //Reduce amount needed by 5 ranks
        }
    }
    public override void RemoveMajorBonus() 
    {
        if (upgradeController.ManaUnit is ManaPickUnit)
        {
            ManaPickUnit mPick = (ManaPickUnit)upgradeController.ManaUnit;
            mPick.coreEffectRankThreshold.RemoveAllModifiersFromSource(this);
        }
    }
}
