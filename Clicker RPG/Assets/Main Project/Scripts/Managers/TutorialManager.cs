using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum Tutorial
{
    UNITUPGRADE
}

public class TutorialManager : MonoBehaviour
{
    [Serializable]
    public struct TutorialStruct
    {
        public Tutorial tutorialType;
        public GameObject tutorialObj;
    }

    public TutorialStruct[] tutorialStruct;

    public static TutorialManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(TutorialManager)) as TutorialManager;

            return instance;
        }
        set
        {
            instance = value;
        }
    }

    private static TutorialManager instance;


    //Call this function with the tutorial object to make it appear
    public void CreateTutorial(Tutorial _tutorialType)
    {
        for(int i = 0; i < tutorialStruct.Length; i++)
        {
            if(tutorialStruct[i].tutorialType == _tutorialType)
            {
                GameObject newTutorial = Instantiate(tutorialStruct[i].tutorialObj);
                newTutorial.transform.parent = this.gameObject.transform;
                RectTransform newTutRect = newTutorial.GetComponent<RectTransform>();
                newTutRect.offsetMin = new Vector2(0, 0);
                newTutRect.offsetMax = new Vector2(0, 0);
            }
        }
    }
}
