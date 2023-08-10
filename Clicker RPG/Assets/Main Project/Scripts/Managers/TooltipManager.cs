using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public enum TooltipType
{
    BASIC,
    ADVANCED,
    UNIT,
    UNITUPGRADE
}

public class TooltipManager : MonoBehaviour
{
    [Serializable]
    public struct tooltipLookupStruct
    {
        public TooltipType tType;
        public GameObject tooltip;
    }

    public tooltipLookupStruct[] tooltipLookup;

    private Dictionary<TooltipType, GameObject> tooltipLookupDict = new Dictionary<TooltipType, GameObject>();

    private void Start()
    {
        for (int i = 0; i < tooltipLookup.Length; i++)
        {
            tooltipLookupDict.Add(tooltipLookup[i].tType, tooltipLookup[i].tooltip);
        }
    }

    private void Update()
    {
        GameObject TooltipObj = TooltipRaycast();
        if (TooltipObj == null)
        {
            FadeTooltips();
            return;
        }
        SetupTooltip(TooltipObj);
    }

    //Sets a tooltip to active or not, which will allow it to fade/appear
    private void FadeTooltips()
    {
        foreach(KeyValuePair<TooltipType, GameObject> tooltipLookup in tooltipLookupDict)
        {
            GameObject tooltipController = tooltipLookup.Value;
            if(tooltipController.TryGetComponent<ITooltipSetup>(out ITooltipSetup itooltipsetup))
            {
                itooltipsetup.isActive = false;
            }
        }
    }

    private void SetupTooltip(GameObject tooltipObj)
    {
        //Get the tooltip interface from the hovered over GUI element (if it has a tooltip interface)
        TooltipType tType = new TooltipType();
        if (tooltipObj.TryGetComponent<ITooltip>(out ITooltip _itooltip))
        {
            _itooltip.SetupTooltip();
            tType = _itooltip.tType;
        }

        //Check to make sure our lookup table has the tooltip type as a possible key
        if (tooltipLookupDict.ContainsKey(tType))
        {
            //Lets grab the correct tooltip from our tooltip canvas by referencing our lookup dictionary. Based on the tooltip type, we can grab the correct tooltip game object and run its setup function.
            if (tooltipLookupDict[tType].TryGetComponent<ITooltipSetup>(out ITooltipSetup itooltipsetup))
            {
                itooltipsetup.isActive = true;
                itooltipsetup.SetupTooltipController(tooltipObj);
            }
        }
    }

    private GameObject TooltipRaycast()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            List<RaycastResult> UIObj = HoveringUIElement;
            if(UIObj == null) { return null; }
            int maxSort = 0;
            for (int i = 0; i < UIObj.Count; i++)
            {
                //Were going to see if the tooltip object is on the highest sorted canvas
                if (UIObj[i].gameObject.GetComponentInParent<Canvas>().sortingOrder > maxSort)
                {
                    maxSort = UIObj[i].gameObject.GetComponentInParent<Canvas>().sortingOrder;
                }

                if (UIObj[i].gameObject.TryGetComponent<ITooltip>(out ITooltip _itooltip))
                {
                    if(UIObj[i].gameObject.GetComponentInParent<Canvas>().sortingOrder >= maxSort)
                        return UIObj[i].gameObject;
                }
            }
        }
        return null;
    }

    private static List<RaycastResult> HoveringUIElement
    {
        get
        {
            //Construct a default PointerEventData, we'll use this to grab raycast data from our pointer
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                pointerId = -1,
            };

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                //If the first element is a UI element, we CAN have a tooltip.
                if (results[0].gameObject.layer == 5) // 5 is Unity's UI layer
                { return results; }

            }

            return null;
        }
    }
}
