using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS NODE GIVES 5% BASE CRIT STIKE

public class ManaPickMajor_BuildingMuscle : MajorUpgradeNode
{

    public List<GameObject> _connectedNodes = new List<GameObject>();
    public UpgradeController upgradeController;
    public bool _isActive;
    public int _requiredRank;

    public Button _nodeBtn;
    public Image _nodeShadow;

    ManaPickMajor_BuildingMuscle()
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
            return "<size=18>BUILDING MUSCLE:\n<color=#00FF50>Increases your <color=#FFFFFF>Strength</color> by <color=#FFFFFF>10%</color>.</color>";


        return "<size=18>BUILDING MUSCLE:\n<color=#5E5E5E>Increases your Strength by 10%.</color>";
    }

    public override string FetchUpgradeListText()
    {
        return "<color=#00FF50>+ 10% Strength.</color>\n";
    }

    public override void ApplyMajorBonus() 
    {
        if(upgradeController.ManaUnit is ManaPickUnit)
        {
            PlayerStats plrStats = GameManager.Instance.player.GetComponent<PlayerStats>();
            plrStats.Strength.RemoveAllModifiersFromSource(this);
            plrStats.Strength.AddModifier(new StatModifier(10, StatModType.FLAT, this)); //4% Strength
        }
    }
    public override void RemoveMajorBonus() 
    {
        if (upgradeController.ManaUnit is ManaPickUnit)
        {
            PlayerStats plrStats = GameManager.Instance.player.GetComponent<PlayerStats>();
            plrStats.Strength.RemoveAllModifiersFromSource(this);
        }
    }
}
