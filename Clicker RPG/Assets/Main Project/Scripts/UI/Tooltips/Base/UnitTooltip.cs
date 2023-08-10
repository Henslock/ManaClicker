using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitTooltip : MonoBehaviour, ITooltip
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

    protected Sprite unitIcon;
    public Sprite UnitIcon => unitIcon;

    protected string descriptionText;
    public string DescriptionText => descriptionText;

    protected string bonusEffectText;
    public string BonusEffectText => bonusEffectText;

    protected string statsText;
    public string StatsText => statsText;
    //

    public virtual void SetupTooltip()
    {
        tType = TooltipType.UNIT;
    }

    public virtual void CalculateLockOffset()
    {
        if (verticalLock == false && horizontalLock == false)
            return;
    }
}
