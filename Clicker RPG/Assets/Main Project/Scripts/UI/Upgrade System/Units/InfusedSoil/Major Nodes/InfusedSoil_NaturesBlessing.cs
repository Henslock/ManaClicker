using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS NODE ACTIVATES YOUR CORE EFFECT FOR MANA PICKS

public class InfusedSoil_NaturesBlessing : MajorUpgradeNode
{

    public List<GameObject> _connectedNodes = new List<GameObject>();
    public UpgradeController upgradeController;
    public bool _isActive;
    public int _requiredRank;

    public Button _nodeBtn;
    public Image _nodeShadow;

    InfusedSoil_NaturesBlessing()
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
            return "<size=18>NATURE'S BLESSING <NYI>:\n<color=#00FF50>In addition to <color=#FFFFFF>Infused Soil</color>, <color=#FFFFFF>Mystic Trees</color> and <color=#FFFFFF>Ancient Treants</color> also receive the benefits of your chosen blessing.</color>";


        return "<size=18>NATURE'S BLESSING <NYI>:\n<color=#5E5E5E>In addition to Infused Soil, Mystic Trees and Ancient Treants also receive the benefits of your chosen blessing.</color>";
    }

    public override string FetchUpgradeListText()
    {
        return "<color=#00FF50>+ In addition to Infused Soil, Mystic Trees and Ancient Treants also receive the benefits of your chosen blessing.</color>\n";
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
