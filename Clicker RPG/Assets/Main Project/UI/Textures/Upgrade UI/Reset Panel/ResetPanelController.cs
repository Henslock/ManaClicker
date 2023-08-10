using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIEffects;
using TMPro;

public class ResetPanelController : MonoBehaviour
{
    public GameObject panelBGObj;
    public GameObject bgFilterObj;
    public GameObject bodyContentObj;
    public GameObject resetButtonObj;
    public GameObject closeButtonObj;
    public GameObject titleParticlesObj;
    public GameObject swipeFX;
    public CanvasGroup panelCanvasGroup;

    public UnitSelectionController uSelectController;
    public TextMeshProUGUI bodyText2;
    public TextMeshProUGUI unitCostText;

    private UITransitionEffect uiTransition;

    //Fading in bools, meh
    private bool fadeInGUIBool = false;
    private bool fadeInTransitionBool = false;
    private bool fadeInButtonsBool = false;

    //Fading out bools
    private bool fadeOutAllGUIBool = false;

    private ManaUnit manaUnit;

    private void Awake()
    {
        uiTransition = panelBGObj.GetComponent<UITransitionEffect>();
    }

    private void OnEnable()
    {
        StopCoroutine(IFadeGUIIn());
        StopCoroutine(IFadeGUIOut());
        FadeInGUI();

        SetupText();
    }


    public void ShowResetUpgradesPanel()
    {
        gameObject.SetActive(true);
    }

    public void HideResetUpgradesPanel()
    {
        FadeOutGUI();
    }

    private void SetupText()
    {
        if(uSelectController.GetActiveUnit() == null) { return; }

        manaUnit = uSelectController.GetActiveUnit();

        bodyText2.text = "Do you wish to reset your <color=#00deff>" + manaUnit.UnitName.ToUpper() + "</color> upgrades?";
        if (CanAffordCheck())
        {
            unitCostText.text = "<color=#2FFF91>" + AbbrevationUtility.AbbreviateNumber(manaUnit.CalculateCost()) + "</color>";
        }
        else
        {
            unitCostText.text = "<color=#FF4C62>" + AbbrevationUtility.AbbreviateNumber(manaUnit.CalculateCost()) + "</color>";
        }
    }

    //Updates the reset button UI depending on if you can afford to reset or not
    private void ResetButtonCheck()
    {
        if (uSelectController.GetActiveUnit() == null) { return; }
        if (manaUnit == null)
        {
            manaUnit = uSelectController.GetActiveUnit();
        }
        double cost = manaUnit.CalculateCost();

        if (CanAffordCheck())
        {
            resetButtonObj.GetComponent<GenericButtonAnimation>().SetButtonState(true);
        }
        else
        {
            resetButtonObj.GetComponent<GenericButtonAnimation>().SetButtonState(false);
        }
        SetupText();
    }

    //Checks if we can afford to reset based on our cost formula
    private double GetResetCost()
    {
        if (manaUnit == null)
        {
            manaUnit = uSelectController.GetActiveUnit();
        }
        double cost = manaUnit.CalculateCost();
        return cost;
    }
    private bool CanAffordCheck()
    {
        if (GameManager.Instance.manaAmount >= GetResetCost())
        {
            return true;
        }
        return false;
    }

    public void ResetUpgrades()
    {
        if (CanAffordCheck())
        {
            GameManager.Instance.IncrementMana(GetResetCost() * -1);
            swipeFX.SetActive(true);
            swipeFX.GetComponent<SimpleSwipeScript>().StartWipeSequence();
        }
    }

    private void FadeInGUI()
    {
        fadeOutAllGUIBool = false;

        fadeInButtonsBool = false;
        fadeInTransitionBool = true;
        bgFilterObj.GetComponent<Image>().color = new Color32(0, 8, 35, 65);
        uiTransition.effectFactor = 0f;
        resetButtonObj.SetActive(true);
        closeButtonObj.SetActive(true);
        titleParticlesObj.SetActive(true);
        resetButtonObj.GetComponent<CanvasGroup>().alpha = 0f;
        closeButtonObj.GetComponent<CanvasGroup>().alpha = 0f;
        bodyContentObj.GetComponent<CanvasGroup>().alpha = 0f;

        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        gameObject.GetComponent<CanvasGroup>().interactable = true;
        bgFilterObj.GetComponent<CanvasGroup>().blocksRaycasts = true;
        bgFilterObj.GetComponent<CanvasGroup>().interactable = true;

        StartCoroutine(IFadeGUIIn());
    }

    private void FadeOutGUI()
    {
        fadeInGUIBool = false;
        fadeInTransitionBool = false;
        fadeInButtonsBool = false;

        gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
        gameObject.GetComponent<CanvasGroup>().interactable = false;
        bgFilterObj.GetComponent<CanvasGroup>().blocksRaycasts = false;
        bgFilterObj.GetComponent<CanvasGroup>().interactable = false;

        fadeOutAllGUIBool = true;
        StopCoroutine(IFadeGUIIn());
        StartCoroutine(IFadeGUIOut());
    }

    private void Update()
    {
        if(uiTransition == null) { return; }

        ResetButtonCheck();
        //Bunch of UI animation crap
        if (fadeInTransitionBool)
        {
            uiTransition.effectFactor = Mathf.Lerp(uiTransition.effectFactor, 1f, Time.deltaTime);
            bgFilterObj.GetComponent<Image>().color = Color32.Lerp(bgFilterObj.GetComponent<Image>().color, new Color32(0, 8, 35, 65), Time.deltaTime *8f);
        }

        if(fadeInGUIBool)
        {
            bodyContentObj.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(bodyContentObj.GetComponent<CanvasGroup>().alpha, 1f, Time.deltaTime);
        }

        if (fadeInButtonsBool)
        {
            resetButtonObj.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(resetButtonObj.GetComponent<CanvasGroup>().alpha, 1f, Time.deltaTime*3f);
            closeButtonObj.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(closeButtonObj.GetComponent<CanvasGroup>().alpha, 1f, Time.deltaTime*3f);
        }

        if(fadeOutAllGUIBool)
        {
            resetButtonObj.SetActive(false);
            closeButtonObj.SetActive(false);
            titleParticlesObj.SetActive(false);
            bodyContentObj.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(bodyContentObj.GetComponent<CanvasGroup>().alpha, 0f, Time.deltaTime * 3f);
            uiTransition.effectFactor = Mathf.Lerp(uiTransition.effectFactor, 0f, Time.deltaTime *2.5f);
            bgFilterObj.GetComponent<Image>().color = Color32.Lerp(bgFilterObj.GetComponent<Image>().color, new Color32(0, 8, 35, 0), Time.deltaTime * 8f);
        }
    }


    //Animates in the UI with fixed timers because I am a bad programmer
    private IEnumerator IFadeGUIIn()
    {
        yield return new WaitForSeconds(0.5f);
        fadeInGUIBool = true;
        yield return new WaitForSeconds(0.3f);
        fadeInButtonsBool = true;
        yield return new WaitForSeconds(1.5f);
        fadeInButtonsBool = false;
        fadeInGUIBool = false;
        fadeInTransitionBool = false;
        resetButtonObj.GetComponent<CanvasGroup>().alpha = 1f;
        closeButtonObj.GetComponent<CanvasGroup>().alpha = 1f;
    }

    private IEnumerator IFadeGUIOut()
    {
        yield return new WaitForSeconds(1.5f);

        gameObject.SetActive(false);
    }
}
