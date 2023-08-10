using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTooltip : BasicTooltip
{
    TestTooltip()
    {
        fadeDelay = 0f;
        verticalLock = false;
        horizontalLock = false;
        maxTooltipWidth = 205;
    }

    public override void SetupTooltip()
    {
        base.SetupTooltip();

        headerText = "Testing This Tooltip";
        descriptionText = "Here is some random sample text to see how this tooltip is placed on the canvas. Hope you enjoy, bitch.";
    }

    public override void CalculateLockOffset()
    {
        base.CalculateLockOffset();
    }
}
