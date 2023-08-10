using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS NODE ACTIVATES YOUR CORE EFFECT FOR MANA PICKS

public class RuinsMajor_MasterOfStone : MajorUpgradeNode
{

    public List<GameObject> _connectedNodes = new List<GameObject>();
    public UpgradeController upgradeController;
    public bool _isActive;
    public int _requiredRank;

    public Button _nodeBtn;
    public Image _nodeShadow;

    RuinsMajor_MasterOfStone()
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
            return "<size=18>MASTER OF STONE <NYI>:\n<color=#00FF50>Increase your efficiency by <color=#FFFFFF>250%</color> multiplicatively. Every <color=#FFFFFF>200</color> ranks of Ruins, reduce your Research time by <color=#FFFFFF>3%</color>.</color>";


        return "<size=18>MASTER OF STONE <NYI>:\n<color=#5E5E5E>Increase your efficiency by 250% multiplicatively. Every 200 ranks of Ruins, reduce your Research time by 3%.</color>";
    }

    public override string FetchUpgradeListText()
    {
        return "<color=#00FF50>+ Increase efficiency by 200%. Every 200 ranks of Ruins, reduce research time by 3%.</color>\n";
    }

    public override void ApplyMajorBonus() 
    {
        if(upgradeController.ManaUnit is RuinsUnit)
        {
            //Implement
        }
    }
    public override void RemoveMajorBonus() 
    {
        if (upgradeController.ManaUnit is RuinsUnit)
        {
        }
    }
}
