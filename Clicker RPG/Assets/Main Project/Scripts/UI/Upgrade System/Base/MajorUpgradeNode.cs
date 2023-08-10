using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MajorUpgradeNode : MonoBehaviour, IUnitUpgrade
{
    public bool isActive { get; protected set; }
    public bool isPurchased { get; protected set; }
    public UpgradeNodeType nodeType => UpgradeNodeType.MAJOR;
    public UpgradeNodeMaster nodeMaster { get; protected set; }
    public List<GameObject> connectedNodes { get; protected set; }
    public Button nodeBtn { get; protected set; }
    public Image nodeShadow { get; protected set; }
    public int requiredRank { get; protected set; }
    public UpgradeController unitUpgradeController { get; protected set; }


    public virtual void Purchase() 
    {
        if (isActive && !isPurchased)
        {
            if (GameManager.Instance.manaAmount >= unitUpgradeController.CalculateCost() && unitUpgradeController.GetUpgradeUnitRank() >= requiredRank)
            {
                GameManager.Instance.IncrementMana(-1 * unitUpgradeController.CalculateCost());
                isPurchased = true;
                ActivateNeighbourNodes();

                EventManager.UnitEvents.OnUpgradePurchasedFromUnit?.Invoke(gameObject);
                EventManager.UIEvents.UpdateNodeAppearance?.Invoke(gameObject);
            }
        }
    }
    public virtual void ResetNode() { }

    public virtual void Activate() { }

    public virtual void ActivateNeighbourNodes()
    {
        foreach (GameObject node in connectedNodes)
        {
            if (node.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iupgrade))
            {
                iupgrade.Activate();
                EventManager.UIEvents.UpdateNodeAppearance(iupgrade.nodeBtn.transform.parent.gameObject);
            }
        }
    }

    public virtual string FetchUpgradeText()
    {
        return string.Empty;
    }

    public virtual string FetchUpgradeListText()
    {
        return string.Empty;
    }

    public virtual void ApplyMajorBonus() { }
    public virtual void RemoveMajorBonus() { }

    protected virtual void Update()
    { }

    protected virtual void Awake()
    { }

    protected virtual void Start()
    { }
}