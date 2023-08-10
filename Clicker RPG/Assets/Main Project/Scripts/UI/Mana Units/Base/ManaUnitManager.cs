using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ManaUnitManager : MonoBehaviour, IPointerDownHandler
{
    public TextMeshProUGUI uName;
    public TextMeshProUGUI uCost;
    public GameObject uCostGroup;
    public TextMeshProUGUI uAmountOwned;
    public Image uLineAcross;
    public Image uOutlineBorder;
    public Image uGlow1;
    public Image uGlow2;
    public RawImage uImage;
    public CanvasGroup canvasGroup;
    public Canvas unitUpgradeCanvas;

    private ManaUnit mUnit;

    public bool isHidden { get; private set; }
    public bool startRevealed;
    void Start()
    {
        mUnit = this.GetComponent<ManaUnit>();
        if (mUnit == null) return;
        uAmountOwned.text = "<size=16>RANK</size>\n" + mUnit.GetTrueTotalOwned().ToString();

        if (!isHidden)
        {
            uName.text = mUnit.UnitName;
            uCost.text = AbbrevationUtility.AbbreviateNumber(mUnit.CalculateCost());
        }
    }

    private void OnEnable()
    {
        EventManager.UnitEvents.UpdateManaUnitManagers += UpdateUI;
    }

    private void OnDisable()
    {
        EventManager.UnitEvents.UpdateManaUnitManagers -= UpdateUI;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //If you right click on a unit it opens up the upgrade canvas
        if(isHidden) { return; }
        if(!GameManager.Instance.isUpgradesUnlocked) { return; }
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            unitUpgradeCanvas.gameObject.SetActive(true);
            EventManager.UIEvents.OnOpenUpgradePanelFromMainScreenUnits?.Invoke(gameObject); //Evoke an event that will allow us to open the specific upgrade panel for the right-clicked unit
        }
    }

    public void PurchaseUnit()
    {
        AudioManager.Instance.PlayUISoundRandomPitch(AudioManager.Instance.specialClick, 0.3f, 0.9f, 1.4f);
        ClickBounce();
        if (mUnit == null) return;

        if (GameManager.Instance.manaAmount >= mUnit.CalculateCost() || ExtensionMethods.IsApproximately(GameManager.Instance.manaAmount, mUnit.CalculateCost()))
        {
            GameManager.Instance.IncrementMana(-mUnit.CalculateCost()); //Subtract mana with unit cost
            mUnit.SetAmountOwned(mUnit.AmountOwned + 1);
            UpdateUI();
            EventManager.UnitEvents.OnUnitPurchased?.Invoke(this.gameObject);
            EventManager.GeneralEvents.UpdateManaStats?.Invoke();

        }
    }

    public void SetUnitAmountOwned(int amount)
    {
        if (mUnit == null) return;
        mUnit.SetAmountOwned(amount);
        UpdateUI();
        EventManager.UnitEvents.OnUnitPurchased?.Invoke(this.gameObject);
        EventManager.GeneralEvents.UpdateManaStats?.Invoke();

        if (isHidden)
            RevealUnit();
    }

    private void UpdateUI()
    {
        if (mUnit == null) return;

        if (isHidden)
            uName.text = "???";
        else
            uName.text = mUnit.UnitName;
        uCost.text = AbbrevationUtility.AbbreviateNumber(mUnit.CalculateCost());
        uAmountOwned.text = "<size=16>RANK</size>\n" + mUnit.GetTrueTotalOwned().ToString();
    }

    //Animation polish, when you click the UI button it'll bounce!
    public void ClickBounce()
    {
        if (canvasGroup.interactable)
        {
            RectTransform uiRect = gameObject.GetComponent<RectTransform>();
            uiRect.transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
        }
    }

    public void HideUnit()
    {
        isHidden = true;
        canvasGroup.interactable = false;
        uName.text = "???";
        uCostGroup.SetActive(false);
        uAmountOwned.enabled = false;
        //Greyscale colors
        uLineAcross.color = new Color32(63, 63, 63, 255);
        uOutlineBorder.color = new Color32(63, 63, 63, 255);
        uGlow1.color = new Color32(50, 50, 50, 255);
        uGlow2.color = new Color32(50, 50, 50, 255);
        uImage.color = new Color32(0, 0, 0, 255);
        canvasGroup.alpha = 0.45f;
    }

    public void RevealUnit()
    {
        isHidden = false;
        canvasGroup.interactable = true;
        uName.text = mUnit.UnitName;
        uCostGroup.SetActive(true);
        uAmountOwned.enabled = true;
        uAmountOwned.text = "<size=16>RANK</size>\n" + mUnit.GetTrueTotalOwned().ToString();
        //Restore colors
        uLineAcross.color = new Color32(140, 255, 255, 255);
        uOutlineBorder.color = new Color32(0, 180, 255, 255);
        uGlow1.color = new Color32(0, 180, 255, 255);
        uGlow2.color = new Color32(0, 180, 255, 255);
        uImage.color = new Color32(255, 255, 255, 255);
        canvasGroup.alpha = 0.45f;
    }

    private void Update()
    {
        if(isHidden) { return; }
        RectTransform uiRect = gameObject.GetComponent<RectTransform>();
        uiRect.transform.localScale = Vector3.Lerp(uiRect.transform.localScale, new Vector3(1, 1, 1), Time.deltaTime * 5f);

        if (GameManager.Instance.manaAmount >= mUnit.CalculateCost() || ExtensionMethods.IsApproximately(GameManager.Instance.manaAmount, mUnit.CalculateCost()))
        {
            canvasGroup.interactable = true;
            canvasGroup.alpha = 1.0f;
            uCost.text = "<color=#2FFF91>" + AbbrevationUtility.AbbreviateNumber(mUnit.CalculateCost()) + " </color>";
        }
        else
        {
            canvasGroup.interactable = false;
            canvasGroup.alpha = 0.45f;

            uCost.text = "<color=#FF4C63>" + AbbrevationUtility.AbbreviateNumber(mUnit.CalculateCost()) + " </color>";
        }
    }
}
