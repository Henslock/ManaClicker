using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//THIS NODE GIVES 25% CORE EFFICIENCY BUT REDUCES MPS BY 20%

public class ManaPickMajor_LaborCommitted : MajorUpgradeNode
{

    public List<GameObject> _connectedNodes = new List<GameObject>();
    public UpgradeController upgradeController;
    public bool _isActive;
    public int _requiredRank;

    public Button _nodeBtn;
    public Image _nodeShadow;

    ManaPickMajor_LaborCommitted()
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
            return "<size=18>LABOR COMMITTED:\n<color=#00FF50>Reduce your overall <color=#FFFFFF>Mana Per Second</color> by <color=#FFFFFF>20%</color> but increase the base scaling of your <color=#FFFFFF>Core Effect</color> by an additional <color=#FFFFFF>25%</color>.</color>";


        return "<size=18>LABOR COMMITTED:\n<color=#5E5E5E>Reduce your overall Mana Per Second by 20% but increase the base scaling of your Core Effect by an additional 25%.</color>";
    }

    public override string FetchUpgradeListText()
    {
        return "<color=#00FF50>+ Core Effect base scaling increased by an additional 25%. Reduced overall Mana Per Second by 20%.</color>\n";
    }

    public override void ApplyMajorBonus() 
    {
        if(upgradeController.ManaUnit is ManaPickUnit)
        {
            ManaPickUnit mPick = (ManaPickUnit)upgradeController.ManaUnit;
            mPick.coreEffectScalar.RemoveAllModifiersFromSource(this);
            mPick.coreEffectScalar.AddModifier(new StatModifier(25, StatModType.FLAT, this)); //Add 25% core efficiency base

            GameManager.Instance.manaPerSecond.RemoveAllModifiersFromSource(this);
            GameManager.Instance.manaPerSecond.AddModifier(new StatModifier(-20, StatModType.MULTIPLYPERCENTAGE, this)); //Reduce MPS by 20%
        }
    }
    public override void RemoveMajorBonus() 
    {
        if (upgradeController.ManaUnit is ManaPickUnit)
        {
            ManaPickUnit mPick = (ManaPickUnit)upgradeController.ManaUnit;
            mPick.coreEffectScalar.RemoveAllModifiersFromSource(this);
            GameManager.Instance.manaPerSecond.RemoveAllModifiersFromSource(this);
        }
    }
}
