using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeSelectionUnitTooltip : BasicTooltip
{
    [SerializeField]
    private UnitSelectionController uController;
    UpgradeSelectionUnitTooltip()
    {
        fadeDelay = 1f;
        verticalLock = true;
        horizontalLock = true;
        maxTooltipWidth = 205;
    }

    public override void SetupTooltip()
    {
        base.SetupTooltip();
        headerText = string.Empty;
        descriptionText = "<size=16>???</size>";

        ManaUnit mUnit = null;

        for (int i = 0; i < uController.activeButtons.Count; i++)
        {
            if (uController.unitSelections[i].unitButtonObj != this.gameObject)
                continue;

            mUnit = uController.unitSelections[i].unitObj.GetComponent<ManaUnit>();
        }

        if(mUnit == null) { return; }

        if (mUnit.GetTrueTotalOwned() >= 25)
        {
            descriptionText = "<size=16>" + mUnit.UnitName + "</size>";
        }
    }

    public override void CalculateLockOffset()
    {
        base.CalculateLockOffset();

        Vector2 screenPos = GameManager.Instance.uiCam.WorldToScreenPoint(gameObject.GetComponent<RectTransform>().position);
        verticalLockOffset = screenPos.y + 25;
        horizontalLockOffset = screenPos.x;
    }
}
