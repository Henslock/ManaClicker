using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

// MANA PICK UPGRADE CONTROLLER
public class ManaPickUpgradeController : UpgradeController
{
    //Unit Data
    public ManaPickUnit manaPick;
    public UpgradeNodeDataContainer nodeData;
    public UpgradeNodeMaster _upgradeNodeMaster;
    public double _baseCost = 1000;

    public GameObject connectingLine;
    public GameObject lineContainer;
    public UpgradeTextController textController;
    public UpgradeTitleController titleController;
    private Dictionary<GameObject, bool> dirtyLineDict = new Dictionary<GameObject, bool>(); //We sort through this dict and check if a node has created a line or not by using the bool
    private Dictionary<IUnitUpgrade, bool> dirtyPurchasedNodeDict = new Dictionary<IUnitUpgrade, bool>(); //We sort through this dict and check if a node has created a line or not by using the bool
    private List<GameObject> upgradeNodes = new List<GameObject>();
    private List<GameObject> disposeParticlesList = new List<GameObject>();

    private Dictionary<ManaPickMinorUpgradeNode.Effect, double> totalMinorUpgradesDict = new Dictionary<ManaPickMinorUpgradeNode.Effect, double>();

    private void OnEnable()
    {
        EventManager.UnitEvents.OnUpgradePurchasedFromUnit += UpdateTotalUpgrades;
        EventManager.UIEvents.UpdateNodeAppearance += UpdateNodeVisuals;
    }

    private void OnDisable()
    {
        EventManager.UnitEvents.OnUpgradePurchasedFromUnit -= UpdateTotalUpgrades;
        EventManager.UIEvents.UpdateNodeAppearance -= UpdateNodeVisuals;
    }

    private void Awake()
    {
        mUnit = manaPick;
        baseCost = _baseCost;
        amountUpgradesOwned = 0;
        tTitle = titleController;
        tController = textController;
        upgradeNodeMaster = _upgradeNodeMaster;

        //Populate a list of upgrade nodes based on our contained children
        Transform[] childObj = GetComponentsInChildren<Transform>();
        foreach(Transform obj in childObj)
        {
            if(obj.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iupgrade))
            {
                if (iupgrade.isPurchased) { amountUpgradesOwned++; }
                upgradeNodes.Add(obj.gameObject);
            }
        }

        //Add these nodes to a dirty list
        foreach(GameObject node in upgradeNodes)
        {
            dirtyLineDict.Add(node, false);
        }

        //Create lines, referencing our dirty list
        foreach (GameObject node in upgradeNodes)
        {
            if (node.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iupgrade))
            {
                foreach(GameObject connectedNode in iupgrade.connectedNodes)
                {
                    if(connectedNode == null) 
                    {
                        //print("Warning: Null connected node under " + node);
                        continue; 
                    }
                    //Check if that node isn't dirty - nasty nasty boy
                    if(dirtyLineDict[connectedNode] == false)
                    {
                        GameObject newLine = Instantiate(connectingLine);
                        newLine.transform.SetParent(lineContainer.transform);
                        newLine.GetComponent<ConnectingLine>().node1 = node;
                        newLine.GetComponent<ConnectingLine>().node2 = connectedNode;

                        newLine.GetComponent<ConnectingLine>().InitializeLine();
                    }
                }
                dirtyLineDict[node] = true; //Set our current gameobject to dirty so that we prevent creating duplicate nodes

                //UpdateNodeVisuals(node);
            }
        }


        //Initialize minor upgrade dict
        foreach(ManaPickMinorUpgradeNode.Effect effect in Enum.GetValues(typeof(ManaPickMinorUpgradeNode.Effect)))
        {
            totalMinorUpgradesDict.Add(effect, 0);
        }
    }

    private void Start()
    {
        foreach (GameObject node in upgradeNodes)
        {
            if (node.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iupgrade))
            {
                UpdateNodeVisuals(node);
            }
        }

        InitTextList();
        UpdateTextList();
        UpdateTitle();
    }

    public override int GetUpgradeUnitRank()
    {
        return base.GetUpgradeUnitRank();
    }

    protected override void UpdateTotalUpgrades(GameObject obj)
    {
        IncreaseAmountOwned(obj);

        //Check if minor upgrade node
        if (obj.GetComponent<ManaPickMinorUpgradeNode>())
        {
            ManaPickMinorUpgradeNode minorUpgradeNode = obj.GetComponent<ManaPickMinorUpgradeNode>();
            //If the dictionary doesn't have a key yet with that minor node upgrade type, then we need to add it
            if (totalMinorUpgradesDict.ContainsKey(minorUpgradeNode.effect) == false)
            {
                totalMinorUpgradesDict.Add(minorUpgradeNode.effect, minorUpgradeNode.upgradeAmount);
            }
            else
            {
                //If we do have the key, just add to the value
                if (minorUpgradeNode.effect == ManaPickMinorUpgradeNode.Effect.EFFICIENCYBOOST)
                {
                    if(totalMinorUpgradesDict[minorUpgradeNode.effect] == 0)
                    {
                        totalMinorUpgradesDict[minorUpgradeNode.effect] = 1;
                    }
                    totalMinorUpgradesDict[minorUpgradeNode.effect] *= 1 + (minorUpgradeNode.upgradeAmount/100);
                }
                else if (minorUpgradeNode.effect == ManaPickMinorUpgradeNode.Effect.COSTREDUCTION)
                {
                    if (totalMinorUpgradesDict[minorUpgradeNode.effect] == 0)
                    {
                        totalMinorUpgradesDict[minorUpgradeNode.effect] = 1;
                    }
                    totalMinorUpgradesDict[minorUpgradeNode.effect] *= (1 - (minorUpgradeNode.upgradeAmount / 100));
                }
                else
                    totalMinorUpgradesDict[minorUpgradeNode.effect] += minorUpgradeNode.upgradeAmount;

            }
        }

        ApplyBonuses();

        UpdateTextList();
    }


    //This function is called when a node is purchased, connected/activated, or reset so that we can visually update it.    
    protected override void UpdateNodeVisuals(GameObject obj)
    {
        if (obj.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iupgrade))
        {
            if(iupgrade == null) { return; }

            if(iupgrade.nodeType == UpgradeNodeType.MINOR)
            {
                if(!iupgrade.isActive)
                {
                    iupgrade.nodeBtn.gameObject.GetComponent<Image>().sprite = nodeData.minorNodeInactiveSprite;
                    iupgrade.nodeShadow.color = new Color32(0, 0, 0, 115);
                }
                else if(iupgrade.isActive && !iupgrade.isPurchased)
                {
                    iupgrade.nodeBtn.gameObject.GetComponent<Image>().sprite = nodeData.minorNodeActiveSprite;
                    iupgrade.nodeShadow.color = new Color32(0, 0, 0, 115);
                }
                else if (iupgrade.isActive && iupgrade.isPurchased)
                {
                    iupgrade.nodeBtn.gameObject.GetComponent<Image>().sprite = nodeData.minorNodePurchasedSprite;
                    iupgrade.nodeShadow.color = new Color32(0, 187, 255, 115);

                    if (!dirtyPurchasedNodeDict.ContainsKey(iupgrade) || dirtyPurchasedNodeDict[iupgrade] == false)
                    {
                        //Celebratory particles for purchase!
                        GameObject celebratoryParticles = Instantiate(nodeData.minorNodeCelebratoryParticles);
                        celebratoryParticles.transform.SetParent(iupgrade.nodeBtn.gameObject.transform);
                        celebratoryParticles.transform.position = iupgrade.nodeBtn.gameObject.transform.position;
                        celebratoryParticles.transform.localScale = Vector3.one;
                        Destroy(celebratoryParticles, 3);

                        GameObject sparkleParticles = Instantiate(nodeData.minorNodeSparkle);
                        sparkleParticles.transform.SetParent(iupgrade.nodeBtn.gameObject.transform);
                        sparkleParticles.transform.position = iupgrade.nodeBtn.gameObject.transform.position;
                        sparkleParticles.transform.localScale = Vector3.one;
                        disposeParticlesList.Add(sparkleParticles);
                        dirtyPurchasedNodeDict.Add(iupgrade, true);
                    }
                }
            }
            else if (iupgrade.nodeType == UpgradeNodeType.MAJOR)
            {
                if (!iupgrade.isActive)
                {
                    iupgrade.nodeBtn.gameObject.GetComponent<Image>().sprite = nodeData.majorNodeInactiveSprite;
                    iupgrade.nodeShadow.color = new Color32(0, 0, 0, 175);
                }
                else if (iupgrade.isActive && !iupgrade.isPurchased)
                {
                    iupgrade.nodeBtn.gameObject.GetComponent<Image>().sprite = nodeData.majorNodeActiveSprite;
                    iupgrade.nodeShadow.color = new Color32(0, 0, 0, 175);
                }
                else if (iupgrade.isActive && iupgrade.isPurchased)
                {
                    iupgrade.nodeBtn.gameObject.GetComponent<Image>().sprite = nodeData.majorNodePurchasedSprite;
                    iupgrade.nodeShadow.color = new Color32(0, 255, 80, 175);

                    if (!dirtyPurchasedNodeDict.ContainsKey(iupgrade) || dirtyPurchasedNodeDict[iupgrade] == false)
                    {
                        //Celebratory particles for purchase!
                        GameObject celebratoryParticles = Instantiate(nodeData.majorNodeCelebratoryParticles);
                        celebratoryParticles.transform.SetParent(iupgrade.nodeBtn.gameObject.transform);
                        celebratoryParticles.transform.position = iupgrade.nodeBtn.gameObject.transform.position;
                        celebratoryParticles.transform.localScale = Vector3.one;
                        Destroy(celebratoryParticles, 3);

                        GameObject sparkleParticles = Instantiate(nodeData.majorNodeSparkle);
                        sparkleParticles.transform.SetParent(iupgrade.nodeBtn.gameObject.transform);
                        sparkleParticles.transform.position = iupgrade.nodeBtn.gameObject.transform.position;
                        sparkleParticles.transform.localScale = Vector3.one;
                        disposeParticlesList.Add(sparkleParticles);
                        dirtyPurchasedNodeDict.Add(iupgrade, true);
                    }

                }
            }
        }
    }

    public override void ApplyBonuses()
    {
        //APPLY EFFICIENCY BOOST
        manaPick.ManaPerClick.RemoveAllModifiersFromSource(this);
        double boostVal = totalMinorUpgradesDict[ManaPickMinorUpgradeNode.Effect.EFFICIENCYBOOST];
        if (boostVal == 0)
            boostVal = 1;
        manaPick.ManaPerClick.AddModifier(new StatModifier((boostVal * 100f) - 100f, StatModType.MULTIPLYPERCENTAGE, this));

        //COST REDUCTION
        manaPick.costReduction.RemoveAllModifiersFromSource(this);
        double costReductionVal = totalMinorUpgradesDict[ManaPickMinorUpgradeNode.Effect.COSTREDUCTION];
        manaPick.costReduction.AddModifier(new StatModifier(costReductionVal, StatModType.FLAT, this));

        //APPLY FREE BONUS RANKS
        manaPick.bonusRanks.RemoveAllModifiersFromSource(this);
        double freePicksVal = totalMinorUpgradesDict[ManaPickMinorUpgradeNode.Effect.BONUSPICKS];
        manaPick.bonusRanks.AddModifier(new StatModifier(freePicksVal, StatModType.FLAT, this));

        //APPLY BASE MANA PER CLICK
        manaPick.basePerClick.RemoveAllModifiersFromSource(this);
        double bpcVal = totalMinorUpgradesDict[ManaPickMinorUpgradeNode.Effect.BASEMANAPERCLICK];
        manaPick.basePerClick.AddModifier(new StatModifier(bpcVal, StatModType.FLAT, this));

        foreach (GameObject node in upgradeNodes)
        {
            if (node.TryGetComponent<MajorUpgradeNode>(out MajorUpgradeNode majorNode))
            {
                if (majorNode.isPurchased)
                {
                    majorNode.ApplyMajorBonus();
                }
            }
        }

        EventManager.UnitEvents.UpdateManaUnitManagers?.Invoke(); //Update unit UI display data
        EventManager.UnitEvents.RecalculateManaUnitData?.Invoke(); //Recalculate unit data
        EventManager.GeneralEvents.UpdateManaStats?.Invoke(); //Push updated data to finalize mana

    }

    protected override void IncreaseAmountOwned(GameObject obj)
    {
        base.IncreaseAmountOwned(obj);
    }

    public override void ResetAllNodes()
    {
        EventManager.UnitEvents.OnResetUpgades?.Invoke();  
        //Iterate through each node and reset them, then update the visuals
        foreach(GameObject node in upgradeNodes)
        {
            if(node.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iupgrade))
            {
                iupgrade.ResetNode();
                UpdateNodeVisuals(node);
            }
        }
        amountUpgradesOwned = 0;

        //Remove bonuses
        manaPick.ManaPerClick.RemoveAllModifiersFromSource(this);
        manaPick.costReduction.RemoveAllModifiersFromSource(this);
        manaPick.bonusRanks.RemoveAllModifiersFromSource(this);
        manaPick.basePerClick.RemoveAllModifiersFromSource(this);

        //Update our mana stats/unit data after we've wiped everything
        EventManager.UnitEvents.UpdateManaUnitManagers?.Invoke();
        EventManager.UnitEvents.RecalculateManaUnitData?.Invoke(); //Recalculate unit data
        EventManager.GeneralEvents.UpdateManaStats?.Invoke();

        totalMinorUpgradesDict.Clear();
        //Re-initialize minor upgrade dict
        foreach (ManaPickMinorUpgradeNode.Effect effect in Enum.GetValues(typeof(ManaPickMinorUpgradeNode.Effect)))
        {
            totalMinorUpgradesDict.Add(effect, 0);
        }

        //Empty particles if there are any
        foreach(GameObject particle in disposeParticlesList)
        {
            Destroy(particle);
        }
        disposeParticlesList.Clear();

        dirtyPurchasedNodeDict.Clear();
        InitTextList();
        UpdateTextList();
    }

    public override double CalculateCost()
    {
        return base.CalculateCost();
    }


    public SortedDictionary<int, UpgradeTextList> upgradeTextListDict = new SortedDictionary<int, UpgradeTextList>();
    public override void InitTextList()
    {
        upgradeTextListDict.Clear();
        UpgradeTextList textStructInfo = new UpgradeTextList();

        //Efficiency Boost
        textStructInfo.val = Math.Max((totalMinorUpgradesDict[ManaPickMinorUpgradeNode.Effect.EFFICIENCYBOOST] * 100f) - 100f, 0);
        textStructInfo.text = "+ " + AbbrevationUtility.AbbreviateNumber(textStructInfo.val) + "% Efficiency Boost";
        textStructInfo.optionalNodeMaster = null;
        upgradeTextListDict.Add(0, textStructInfo);

        //Bonus Picks
        textStructInfo.val = totalMinorUpgradesDict[ManaPickMinorUpgradeNode.Effect.BONUSPICKS];
        textStructInfo.text = "+ " + AbbrevationUtility.AbbreviateNumber(textStructInfo.val) + " Free Mana Pick Rank(s)";
        textStructInfo.optionalNodeMaster = null;
        upgradeTextListDict.Add(1, textStructInfo);

        //Cost Reduction
        textStructInfo.val = totalMinorUpgradesDict[ManaPickMinorUpgradeNode.Effect.COSTREDUCTION];
        textStructInfo.text = "+ " + AbbrevationUtility.AbbreviateNumber(textStructInfo.val) + "% Reduced Unit Cost";
        textStructInfo.optionalNodeMaster = null;
        upgradeTextListDict.Add(2, textStructInfo);

        //Base Mana Per Click
        textStructInfo.val = totalMinorUpgradesDict[ManaPickMinorUpgradeNode.Effect.BASEMANAPERCLICK];
        textStructInfo.text = "+ " + AbbrevationUtility.AbbreviateNumber(textStructInfo.val, AbbrevationUtility.FormatType.TYPE_DECIMAL) + " base MPC per Rank";
        textStructInfo.optionalNodeMaster = null;
        upgradeTextListDict.Add(3, textStructInfo);

        foreach (GameObject node in upgradeNodes)
        {
            if (node.TryGetComponent<MajorUpgradeNode>(out MajorUpgradeNode majorNode))
            {
                textStructInfo.text = majorNode.FetchUpgradeListText();
                textStructInfo.optionalNodeMaster = node;

                if (majorNode.isPurchased)
                    textStructInfo.val = 1;
                else
                    textStructInfo.val = 0;

                upgradeTextListDict.Add(upgradeTextListDict.Count, textStructInfo);
            }
        }

        textController.SetupTextList(upgradeTextListDict, this);
    }

    //Our text list shows what upgrades we currently have for our unit, so let's make sure that's up to date
    public override void UpdateTextList()
    {

        if(amountUpgradesOwned <= 0)
        {
            textController.ResetTextList();
        }
        else
        {
            if(upgradeTextListDict.Count == 0)
            {
                InitTextList();
            }

            UpgradeTextList textStructInfo = new UpgradeTextList();

            double boostVal = totalMinorUpgradesDict[ManaPickMinorUpgradeNode.Effect.EFFICIENCYBOOST];
            //Only update on changes
            if (upgradeTextListDict[0].val != boostVal && boostVal > 0)
            {
                textStructInfo.val = Math.Floor((boostVal * 100f) - 100f);
                textStructInfo.text = "+ " + AbbrevationUtility.AbbreviateNumber(textStructInfo.val) + "% Efficiency Boost";
                textStructInfo.optionalNodeMaster = null;
                //Store updated values and pass them into our dict for reference
                upgradeTextListDict[0] = textStructInfo;
            }

            double bonusPicksVal = totalMinorUpgradesDict[ManaPickMinorUpgradeNode.Effect.BONUSPICKS];
            if (upgradeTextListDict[1].val != boostVal && bonusPicksVal > 0)
            {
                textStructInfo.val = bonusPicksVal;
                textStructInfo.text = "+ " + textStructInfo.val + " Free Mana Pick Rank(s)";
                textStructInfo.optionalNodeMaster = null;

                upgradeTextListDict[1] = textStructInfo;
            }

            double costReductionVal = totalMinorUpgradesDict[ManaPickMinorUpgradeNode.Effect.COSTREDUCTION];
            if (upgradeTextListDict[2].val != costReductionVal && costReductionVal < 1)
            {
                textStructInfo.val = (1 - costReductionVal) * 100f;
                textStructInfo.text = "+ " + textStructInfo.val.ToString("F2") + "% Reduced Unit Cost";
                textStructInfo.optionalNodeMaster = null;

                upgradeTextListDict[2] = textStructInfo;
            }

            double baseClickVal = totalMinorUpgradesDict[ManaPickMinorUpgradeNode.Effect.BASEMANAPERCLICK];
            if (upgradeTextListDict[3].val != baseClickVal && baseClickVal > 0)
            {
                textStructInfo.val = baseClickVal;
                textStructInfo.text = "+ " + AbbrevationUtility.AbbreviateNumber(textStructInfo.val, AbbrevationUtility.FormatType.TYPE_DECIMAL) + " base MPC per Rank";
                textStructInfo.optionalNodeMaster = null;

                upgradeTextListDict[3] = textStructInfo;
            }

            //Ghetto way to iterate through a sorted dict
            int incIndex = 4;

            foreach(GameObject node in upgradeNodes)
            {
                if(node.TryGetComponent<MajorUpgradeNode>(out MajorUpgradeNode majorNode))
                {
                    if (majorNode.isPurchased)
                        textStructInfo.val = 1;
                    else
                        textStructInfo.val = 0;

                    textStructInfo.text = majorNode.FetchUpgradeListText();
                    textStructInfo.optionalNodeMaster = node;

                    upgradeTextListDict[incIndex] = textStructInfo;

                    incIndex++;
                }
            }

            textController.RefreshLayout();
            textController.SetupTextList(upgradeTextListDict, this);
        }
    }

    public override void UpdateTitle()
    {
        base.UpdateTitle();
    }
}
