using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS NODE ACTIVATES YOUR CORE EFFECT FOR MANA PICKS

public class InfusedSoil_Zenith : MajorUpgradeNode
{

    public List<GameObject> _connectedNodes = new List<GameObject>();
    public UpgradeController upgradeController;
    public bool _isActive;
    public int _requiredRank;

    public Button _nodeBtn;
    public Image _nodeShadow;

    InfusedSoil_Zenith()
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
            return "<size=18>ZENITH <NYI>:\n<color=#00FF50>During the peak hours of the <color=#FFFFFF>Day</color> (12 ~ 2 PM), double the effectiveness of your <color=#FFFFFF>Blessing of the Sun</color>.</color>";


        return "<size=18>ZENITH <NYI>:\n<color=#5E5E5E>During the peak hours of the Day (12 ~ 2 PM), double the effectiveness of your Blessing of the Sun.</color>";
    }

    public override string FetchUpgradeListText()
    {
        return "<color=#00FF50>+ Double the effectiveness of your Blessing of the Sun during the peak hours of the Day (12 ~ 2 PM).</color>\n";
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
