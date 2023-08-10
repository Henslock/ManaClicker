using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTooltip : MonoBehaviour, ITooltip
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

    protected string descriptionText;
    public string DescriptionText => descriptionText;

    protected int maxTooltipWidth;
    public int MaxTooltipWidth => maxTooltipWidth;
    //

    public virtual void SetupTooltip() 
    {
        tType = TooltipType.BASIC;
    }

    public virtual void CalculateLockOffset()
    {
        if (verticalLock == false && horizontalLock == false)
            return;
    }
}
