using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeTextController : MonoBehaviour
{
    //Script's purpose is to pool text objects so that other controllers can set its text
    [SerializeField]
    private GameObject textListObject;
    [SerializeField]
    private GameObject noUpgradesObj;
    [SerializeField]
    private GameObject Anchor;
    private int startingObjectAmount = 20;
    private List<GameObject> textObjects = new List<GameObject>();

    public GameObject positionIndicator;

    private SortedDictionary<int, UpgradeTextList> priorRefDict = new SortedDictionary<int, UpgradeTextList>();
    private UpgradeController priorUpgradeController;
    private void Start()
    {
        InitializeListPool();
    }

    private void Update()
    {
        HighlightPointerCheck();
    }

    private void InitializeListPool()
    {
        for (int i = 0; i < startingObjectAmount; i++)
        {
            GameObject newTextObj = Instantiate(textListObject, Anchor.transform);
            newTextObj.SetActive(false);
            textObjects.Add(newTextObj);
        }
    }

    public void SetupTextList(SortedDictionary<int, UpgradeTextList> textDict, UpgradeController uController)
    {
        if (textObjects.Count == 0)
        {
            InitializeListPool();
        }

        //If our dictionary has more than one element in it, that means we have upgrades. So let's get rid of the "No upgrades" prompt object
        if(textDict.Count > 0)
        {
            noUpgradesObj.SetActive(false);
        }

        foreach (KeyValuePair<int, UpgradeTextList> dictData in textDict)
        {
            //If the data value in the struct is greater than 0 then we have an upgrade we can display.
            if(dictData.Value.val > 0)
            {
                textObjects[dictData.Key].SetActive(true);
                textObjects[dictData.Key].GetComponent<UpgradeListTextObject>().SetupText(dictData.Value.text);
                textObjects[dictData.Key].GetComponent<UpgradeListTextObject>().masterNode = dictData.Value.optionalNodeMaster;
                RefreshLayout();

                if(uController != priorUpgradeController) { continue; }
                //We have a dictionary that is a clone of the previously sent dictionary, we can use to see if values have changed between setups
                //This will allow us to handle cool animations if a new upgrade is introduced, or if its updated
                if(priorRefDict != null && priorRefDict.Count > 0)
                {
                    //If the values have changed
                    if(dictData.Value.val != priorRefDict[dictData.Key].val && priorRefDict[dictData.Key].val != 0)
                    {
                        textObjects[dictData.Key].GetComponent<UpgradeListTextObject>().SetUpdatePopAnim();
                    }

                    //If the values used to be 0 (AKA we didn't have the upgrade)
                    if(priorRefDict[dictData.Key].val == 0)
                    {
                        textObjects[dictData.Key].GetComponent<UpgradeListTextObject>().SetAppearAnim();
                    }
                }
            }
            else
                textObjects[dictData.Key].SetActive(false);
        }


        priorUpgradeController = uController;
        priorRefDict.Clear();
        //Re-populate the old reference dict
        foreach (KeyValuePair<int, UpgradeTextList> kvp in textDict)
        {
            UpgradeTextList uList = new UpgradeTextList();
            uList.text = kvp.Value.text;
            uList.val = kvp.Value.val;
            uList.optionalNodeMaster = kvp.Value.optionalNodeMaster;
            priorRefDict.Add(kvp.Key, uList);
        }

        RefreshLayout();
    }

    public void ResetTextList()
    {
        if (textObjects.Count == 0)
        {
            InitializeListPool();
        }
        EmptyTextList();
        noUpgradesObj.SetActive(true);
        noUpgradesObj.GetComponent<UpgradeListTextObject>().SetupText("<color=#747474><i>No upgrades active.</i></color>");

        RefreshLayout();
    }

    public void EmptyTextList()
    {
        foreach(GameObject textObject in textObjects)
        {
            textObject.GetComponent<UpgradeListTextObject>().SetupText("None");
            textObject.SetActive(false);
        }

        RefreshLayout();
    }

    public void HighlightPointerCheck()
    {
        GameObject masterNode = null;
        UpgradeListTextObject listTextObj = null;
        foreach (GameObject obj in CursorManager.Instance.CursorRaycastResults())
        {
            if(obj.TryGetComponent<UpgradeListTextObject>(out UpgradeListTextObject textObject))
            {
                listTextObj = textObject;
                if (textObject.masterNode != null)
                {
                    masterNode = textObject.masterNode;
                    break;
                }
            }
        }

        //This is to handle highlights on the individual text objects
        if (listTextObj == null)
        {
            foreach (GameObject textObj in textObjects)
            {
                textObj.GetComponent<UpgradeListTextObject>().SetHighlighted(false);
            }
        }
        else
        {
            listTextObj.SetHighlighted(true);
        }

        //

        if (masterNode == null)
        {
            positionIndicator.SetActive(false);
            return;
        }
        else
        {
            positionIndicator.SetActive(true);
            positionIndicator.transform.SetParent(masterNode.transform);
            positionIndicator.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
    }

    public void RefreshLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(Anchor.GetComponent<RectTransform>());
    }
}
