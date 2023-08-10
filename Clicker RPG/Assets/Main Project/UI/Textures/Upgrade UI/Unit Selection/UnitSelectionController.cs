using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionController : MonoBehaviour
{
    [Serializable]
    public struct unitSelectionStruct
    {
        public GameObject unitButtonObj;
        public GameObject unitObj;
        public GameObject unitUpgradeControllerObj;
    }
    public unitSelectionStruct[] unitSelections;
    public Dictionary<unitSelectionStruct, bool> activeButtons = new Dictionary<unitSelectionStruct, bool>();

    public List<GameObject> buttonsForHighlighting = new List<GameObject>();
    public Dictionary<GameObject,bool> buttonsForHighlightingDict = new Dictionary<GameObject, bool>();

    private void Awake()
    {
        for (int i = 0; i < unitSelections.Length; i++)
        {
            activeButtons.Add(unitSelections[i], false);
        }

        foreach(GameObject obj in buttonsForHighlighting)
        {
            buttonsForHighlightingDict.Add(obj, false);
        }
    }
    private void Start()
    {
        InitButtons();
    }
    private void OnEnable()
    {
        EventManager.UIEvents.OnUpgradeUnitSelection += PlaySelectedClickVFX;
        EventManager.UIEvents.OnUpgradeUnitSelection += SetButton;
        EventManager.UIEvents.OnOpenUpgradePanelFromMainScreenUnits += SetButtonFromMainScreenUnit;

        InitButtons();
    }

    private void OnDisable()
    {
        EventManager.UIEvents.OnUpgradeUnitSelection -= PlaySelectedClickVFX;
        EventManager.UIEvents.OnUpgradeUnitSelection -= SetButton;
        EventManager.UIEvents.OnOpenUpgradePanelFromMainScreenUnits -= SetButtonFromMainScreenUnit;
    }

    private void Update()
    {
        //This is for a performance increase. Previously we were doing cursor raycast result checks on every single unit button script, this instead collects it all under one script and saves us around 60 frames.
        GameObject objKey = null;
        foreach (GameObject obj in CursorManager.Instance.CursorRaycastResults())
        {
            if (buttonsForHighlightingDict.ContainsKey(obj))
            {
                objKey = obj;
                buttonsForHighlightingDict[obj] = true;
            }
        }

        foreach(KeyValuePair<GameObject, bool> obj in buttonsForHighlightingDict)
        {
            if(obj.Value == false)
            {
                obj.Key.GetComponent<UnitSelectionButtonUIController>().isHighlighted = false;
            }
        }

        if (objKey != null)
        {
            objKey.GetComponent<UnitSelectionButtonUIController>().isHighlighted = true;
            buttonsForHighlightingDict[objKey] = false;
        }

    }

    public void InitButtons()
    {
        for (int i = 0; i < activeButtons.Count; i++)
        {
            if (unitSelections[i].unitObj.GetComponent<ManaUnit>().GetTrueTotalOwned() >= 25)
            {
                unitSelections[i].unitButtonObj.GetComponent<UnitSelectionButtonUIController>().SetActive();
            }
            else
                unitSelections[i].unitButtonObj.GetComponent<UnitSelectionButtonUIController>().SetInactive();
        }

        activeButtons[unitSelections[0]] = true;
        SetButton(unitSelections[0].unitButtonObj);
    }

    //This method will set buttons in their states (active, selected, inactive) based on if they meet the conditions
    //As well it will active the appropriate upgrade panel for each
    private void SetButton(GameObject buttonObj)
    {
        if (buttonObj.GetComponent<UnitSelectionButtonUIController>() == null) { return; }

        //Ignore inactive buttons that are clicked or already selected
        if(buttonObj.GetComponent<UnitSelectionButtonUIController>().buttonState == UnitSelectionButtonUIController.BUTTONSTATE.INACTIVE) { return; }
        if(buttonObj.GetComponent<UnitSelectionButtonUIController>().buttonState == UnitSelectionButtonUIController.BUTTONSTATE.SELECTED) { return; }

        //Start by making sure the clicked button is set to be the only true condition in the dictionary
        for (int i = 0; i < activeButtons.Count; i++)
        {
            if (unitSelections[i].unitButtonObj == buttonObj)
            {
                activeButtons[unitSelections[i]] = true;
            }
            else
            {
                activeButtons[unitSelections[i]] = false;
            }
        }

        //Set the visuals and information for the buttons
        for (int i = 0; i < activeButtons.Count; i++)
        {
            if(activeButtons[unitSelections[i]] == true)
            {
                unitSelections[i].unitButtonObj.GetComponent<UnitSelectionButtonUIController>().SetSelected();
                unitSelections[i].unitUpgradeControllerObj.SetActive(true);
                unitSelections[i].unitUpgradeControllerObj.GetComponent<UpgradeController>().UpdateTextList();
                unitSelections[i].unitUpgradeControllerObj.GetComponent<UpgradeController>().UpdateTitle();
            }
            else
            {
                unitSelections[i].unitUpgradeControllerObj.SetActive(false);

                //Change selected or active to the Active button state
                if (unitSelections[i].unitButtonObj.GetComponent<UnitSelectionButtonUIController>().buttonState == UnitSelectionButtonUIController.BUTTONSTATE.SELECTED)
                {
                    unitSelections[i].unitButtonObj.GetComponent<UnitSelectionButtonUIController>().SetActive();
                }

                if (unitSelections[i].unitButtonObj.GetComponent<UnitSelectionButtonUIController>().buttonState == UnitSelectionButtonUIController.BUTTONSTATE.ACTIVE)
                {
                    unitSelections[i].unitButtonObj.GetComponent<UnitSelectionButtonUIController>().SetActive();
                }

                //Do not change inactive buttons
            }
        }
    }

    private void PlaySelectedClickVFX(GameObject buttonObj)
    {
        if (buttonObj.GetComponent<UnitSelectionButtonUIController>() == null) { return; }

        //Ignore inactive buttons that are clicked or already selected
        if (buttonObj.GetComponent<UnitSelectionButtonUIController>().buttonState == UnitSelectionButtonUIController.BUTTONSTATE.INACTIVE) { return; }
        if (buttonObj.GetComponent<UnitSelectionButtonUIController>().buttonState == UnitSelectionButtonUIController.BUTTONSTATE.SELECTED) { return; }

        buttonObj.GetComponent<UnitSelectionButtonUIController>().PlaySelectedVFX();
    }

    //This function is for setting the active button/unit when we right click the unit from the main screen.
    private void SetButtonFromMainScreenUnit(GameObject mUnit)
    {
        for(int i = 0; i < activeButtons.Count; i++)
        {
            //If the unit we just right click matches the one in our struct, we have a match!
            if(mUnit == unitSelections[i].unitObj)
            {
                SetButton(unitSelections[i].unitButtonObj);
                return;
            }
        }

        //If you right click a unit that isn't registered in our struct, we'll just default to the first element in the struct array
        SetButton(unitSelections[0].unitButtonObj);
    }


    //Our reset button uses this to reset the current active unit
    public void ResetActiveUnit()
    {
        for (int i = 0; i < activeButtons.Count; i++)
        {
            if (activeButtons[unitSelections[i]] == true)
            {
                unitSelections[i].unitUpgradeControllerObj.GetComponent<UpgradeController>().ResetAllNodes();
            }
        }
    }

    public ManaUnit GetActiveUnit()
    {
        for (int i = 0; i < activeButtons.Count; i++)
        {
            if (activeButtons[unitSelections[i]] == true)
            {
                return unitSelections[i].unitUpgradeControllerObj.GetComponent<UpgradeController>().ManaUnit;
            }
        }

        return null;
    }

    public UpgradeController GetActiveUnitController()
    {
        for (int i = 0; i < activeButtons.Count; i++)
        {
            if (activeButtons[unitSelections[i]] == true)
            {
                return unitSelections[i].unitUpgradeControllerObj.GetComponent<UpgradeController>();
            }
        }

        return null;
    }

}
