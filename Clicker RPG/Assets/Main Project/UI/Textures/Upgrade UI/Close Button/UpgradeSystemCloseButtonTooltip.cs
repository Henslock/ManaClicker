using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSystemCloseButtonTooltip : BasicTooltip
{
    UpgradeSystemCloseButtonTooltip()
    {
        fadeDelay = 1.5f;
        verticalLock = true;
        horizontalLock = true;
        maxTooltipWidth = 205;
    }

    public override void SetupTooltip()
    {
        base.SetupTooltip();

        headerText = string.Empty;
        descriptionText = "Return to the Crystal";
    }

    public override void CalculateLockOffset()
    {
        base.CalculateLockOffset();
        Vector2 screenPos = GameManager.Instance.uiCam.WorldToScreenPoint(gameObject.GetComponent<RectTransform>().position);
        verticalLockOffset = screenPos.y + 30;
        horizontalLockOffset = screenPos.x;
    }
}
