using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//THIS NODE GIVES YOUR CLICKS A CHANCE TO GRANT XP

public class ManaPickMajor_KnowledgeStrikes : MajorUpgradeNode
{

    public List<GameObject> _connectedNodes = new List<GameObject>();
    public UpgradeController upgradeController;
    public bool _isActive;
    public int _requiredRank;

    public Button _nodeBtn;
    public Image _nodeShadow;

    ManaPickMajor_KnowledgeStrikes()
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
            return "<size=18>KNOWLEDGE STRIKES:\n<color=#00FF50>Clicking the Crystal has a <color=#FFFFFF>25%</color> chance to grant you an additional 1 XP.</color>";


        return "<size=18>KNOWLEDGE STRIKES:\n<color=#5E5E5E>Clicking the Crystal has a 25% chance to grant you an additional 1 XP.</color>";
    }

    public override string FetchUpgradeListText()
    {
        return "<color=#00FF50>+ 25% chance on click to grant an additional XP.</color>\n";
    }

    public override void ApplyMajorBonus() 
    {
        if(upgradeController.ManaUnit is ManaPickUnit)
        {
            ManaPickUnit mPick = (ManaPickUnit)upgradeController.ManaUnit;
            mPick.SetKnowledgeStrikesStatus(true);
        }
    }
    public override void RemoveMajorBonus() 
    {
        if (upgradeController.ManaUnit is ManaPickUnit)
        {
            ManaPickUnit mPick = (ManaPickUnit)upgradeController.ManaUnit;
            mPick.SetKnowledgeStrikesStatus(false);
        }
    }
}
