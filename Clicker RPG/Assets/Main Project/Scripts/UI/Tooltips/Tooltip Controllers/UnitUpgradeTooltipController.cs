using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUpgradeTooltipController : MonoBehaviour, ITooltipSetup
{
    private CanvasGroup canvasGroup;
    public GameObject Header;
    public GameObject Anchor; //We need to refresh the vertical layout group due to a bug with Unity
    public GameObject Description;
    public GameObject Rank;
    public GameObject Cost;
    public GameObject CostIcon;
    public GameObject HeaderBG;
    public GameObject BorderBG;
    private float yOffset = 10;
    public bool isActive { get; set; }
    public float fadeTimer { get; set; }

    private float fadeTotalTime;

    private void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        fadeTimer = 0;
        canvasGroup.alpha = 0;
        fadeTotalTime = 0;
    }

    private void Update()
    {
        FadeController(isActive);
    }
    public void SetupTooltipController(GameObject tooltip)
    {
        UnitUpgradeTooltip unitUpgradeTooltip = tooltip.GetComponent<UnitUpgradeTooltip>();

        isActive = true;

        RectTransform controllerRect = gameObject.GetComponent<RectTransform>();
        Canvas.ForceUpdateCanvases();
        Header.GetComponent<TextMeshProUGUI>().text = unitUpgradeTooltip.HeaderText;
        Rank.GetComponent<TextMeshProUGUI>().text = unitUpgradeTooltip.RankText;
        Description.GetComponent<TextMeshProUGUI>().text = unitUpgradeTooltip.DescriptionText;
        Cost.GetComponent<TextMeshProUGUI>().text = unitUpgradeTooltip.CostText;


        BorderBG.GetComponent<Image>().material.SetFloat("_PannerSpeed", 0.02f);
        BorderBG.GetComponent<Image>().material.SetFloat("_AlphaMult", 1.0f);
        if (unitUpgradeTooltip.UpgradeNodeType == UpgradeNodeType.MINOR)
        {
            BorderBG.GetComponent<Image>().material.SetColor("_MainColor", new Color32(0, 135, 255, 255));
            HeaderBG.GetComponent<Image>().color = new Color32(0, 135, 255, 55);
            Header.GetComponent<TextMeshProUGUI>().color = new Color32(0, 205, 255, 255);
        }
        else if (unitUpgradeTooltip.UpgradeNodeType == UpgradeNodeType.MAJOR)
        {
            BorderBG.GetComponent<Image>().material.SetColor("_MainColor", new Color32(0, 255, 80, 255));
            HeaderBG.GetComponent<Image>().color = new Color32(0, 255, 80, 55);
            Header.GetComponent<TextMeshProUGUI>().color = new Color32(88, 255, 109, 255);
        }

        if (tooltip.TryGetComponent<IUnitUpgrade>(out IUnitUpgrade iup))
        {
            if (iup.isActive == false || iup.isPurchased)
                CostIcon.SetActive(false);
            else
                CostIcon.SetActive(true);

            if(iup.isActive == false)
            {
                BorderBG.GetComponent<Image>().material.SetColor("_MainColor", new Color32(180, 180, 180, 255));
                BorderBG.GetComponent<Image>().material.SetFloat("_PannerSpeed", 0f);
                HeaderBG.GetComponent<Image>().color = new Color32(180, 180, 180, 55);
                Header.GetComponent<TextMeshProUGUI>().color = new Color32(180, 180, 180, 255);
            }
        }

        Anchor.GetComponent<VerticalLayoutGroup>().enabled = false;
        Anchor.GetComponent<VerticalLayoutGroup>().enabled = true;
        fadeTimer = unitUpgradeTooltip.fadeDelay;

        CalculateTooltipPos(gameObject.GetComponent<RectTransform>(), tooltip);
    }

    public void CalculateTooltipPos(RectTransform tooltipRect, GameObject tooltip)
    {

        UnitUpgradeTooltip unitTooltip = tooltip.GetComponent<UnitUpgradeTooltip>();
        unitTooltip.CalculateLockOffset();
        float vPos = unitTooltip.verticalLockOffset;
        float hPos = unitTooltip.horizontalLockOffset;


        Vector2 basePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y + yOffset + tooltipRect.sizeDelta.y / 2);

        if (unitTooltip.verticalLock)
            basePosition.y = vPos + yOffset + (tooltipRect.sizeDelta.y / 2);

        if (unitTooltip.horizontalLock)
            basePosition.x = hPos;

        float posX = basePosition.x;
        float posY = basePosition.y;

        //Clamp the position of the tooltip to the edges of the screen bounds so it doesn't clip out.
        posX = Mathf.Clamp(posX, 0 + (tooltipRect.sizeDelta.x / 2), Screen.width - (tooltipRect.sizeDelta.x / 2));
        posY = Mathf.Clamp(posY, 0 + (tooltipRect.sizeDelta.y / 2), Screen.height - (tooltipRect.sizeDelta.y / 2 + yOffset));

        tooltipRect.transform.position = new Vector2(posX, posY);
    }

    //Controls the tooltip fading
    public void FadeController(bool _isActive)
    {
        if (_isActive == false)
        {
            fadeTotalTime = 0;
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, Time.deltaTime * 10f);
            BorderBG.GetComponent<Image>().material.SetFloat("_AlphaMult", 0f);
        }
        else if (_isActive == true)
        {
            fadeTotalTime += Time.deltaTime;
            if (fadeTotalTime >= fadeTimer)
            {
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, Time.deltaTime * 10f);
                BorderBG.GetComponent<Image>().material.SetFloat("_AlphaMult", 1f);
            }

            if(canvasGroup.alpha > 0 && fadeTotalTime < fadeTimer)
            {
                canvasGroup.alpha = 0;
                BorderBG.GetComponent<Image>().material.SetFloat("_AlphaMult", 0f);
            }
        }
    }
}
