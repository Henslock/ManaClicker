using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionButton : MonoBehaviour
{
    public void SelectButton()
    {
        EventManager.UIEvents.OnUpgradeUnitSelection?.Invoke(this.gameObject);
    }
}
