using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfusedSoilMinorUpgradeNode : MinorUpgradeNode
{
    public enum Effect
    {
        EFFICIENCYBOOST,
        COSTREDUCTION,
        BLESSINGBOOST,
        BONUSRANKS,
        BASEMANAPERSECOND
    }

    public List<GameObject> _connectedNodes = new List<GameObject>();
    public UpgradeController upgradeController;
    public Effect effect;
    public double _upgradeAmount;
    public bool _isActive;
    public int _requiredRank;

    public Button _nodeBtn;
    public Image _nodeShadow;

    InfusedSoilMinorUpgradeNode()
    {
        //This needs to populate before Start()
        connectedNodes = _connectedNodes;
    }
    protected override void Awake()
    {
        upgradeAmount = _upgradeAmount;
        isActive = _isActive;
        isPurchased = false;
        requiredRank = _requiredRank;
        nodeMaster = UpgradeNodeMaster.INFUSED_SOIL;

        nodeBtn = _nodeBtn;
        nodeShadow = _nodeShadow;
        unitUpgradeController = upgradeController;


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

    public override Dictionary<string, double> GetEffectAndAmount()
    {
        Dictionary<string, double> dict = new Dictionary<string, double>();
        dict.Add(effect.ToString(), _upgradeAmount);
        return dict;
    }
}
