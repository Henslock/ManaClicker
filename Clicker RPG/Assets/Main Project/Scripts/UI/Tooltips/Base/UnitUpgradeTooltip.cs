using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitUpgradeTooltip : MonoBehaviour, ITooltip
{
    //Implemented interface variables
    public TooltipType tType { get; private set; }
    public float fadeDelay { get; protected set; }
    public bool verticalLock { get; protected set; }
    public bool horizontalLock { get; protected set; }
    public float verticalLockOffset { get; protected set; }
    public float horizontalLockOffset { get; protected set; }
    //

    //Basic Tooltip specific variables
    protected string headerText;
    public string HeaderText => headerText;

    protected string rankText;
    public string RankText => rankText;

    protected UpgradeNodeType upgradeNodeType;
    public UpgradeNodeType UpgradeNodeType => upgradeNodeType;

    protected Image headerBG;
    public Image HeaderBG => headerBG;

    protected Image borderBG;
    public Image BorderBG => borderBG;

    protected GameObject costIcon;
    public GameObject CostIcon => costIcon;

    protected string descriptionText;
    public string DescriptionText => descriptionText;

    protected string costText;
    public string CostText => costText;
    //

    public virtual void SetupTooltip()
    {
        tType = TooltipType.UNITUPGRADE;
    }

    public virtual void CalculateLockOffset()
    {
        if (verticalLock == false && horizontalLock == false)
            return;
    }
}
