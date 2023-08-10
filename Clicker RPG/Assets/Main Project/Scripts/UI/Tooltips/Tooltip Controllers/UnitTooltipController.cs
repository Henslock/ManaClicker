using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitTooltipController : MonoBehaviour, ITooltipSetup
{
    private CanvasGroup canvasGroup;
    public GameObject Header;
    public GameObject Anchor; //We need to refresh the vertical layout group due to a bug with Unity
    public GameObject UnitIcon;
    public GameObject Rank;
    public GameObject Description;
    public GameObject BonusEffect;
    public GameObject Stats;
    public GameObject upgradePrompt;
    private float yOffset = 10;
    public bool isActive { get; set; }
    public float fadeTimer { get; set; }

    private float fadeTotalTime;

    private bool upgradePromptSwitch;

    private void Start()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        fadeTimer = 0;
        canvasGroup.alpha = 0;
        fadeTotalTime = 0;
        upgradePromptSwitch = false;
        upgradePrompt.SetActive(false); 
    }

    private void Update()
    {
        FadeController(isActive);

        if(upgradePromptSwitch == true) { return; } //This is so we don't keep spamming this check in Update

        if(GameManager.Instance.isUpgradesUnlocked)
        {
            upgradePrompt.SetActive(true);
            upgradePromptSwitch = true;
        }
    }
    public void SetupTooltipController(GameObject tooltip)
    {
        UnitTooltip unitTooltip = tooltip.GetComponent<UnitTooltip>();
        ManaUnitManager unitManager = tooltip.GetComponent<ManaUnitManager>();
        if (unitManager.isHidden)
            isActive = false;
        else
        {
            isActive = true;

            RectTransform controllerRect = gameObject.GetComponent<RectTransform>();
            Canvas.ForceUpdateCanvases();
            Header.GetComponent<TextMeshProUGUI>().text = unitTooltip.HeaderText;
            Rank.GetComponent<TextMeshProUGUI>().text = unitTooltip.RankText;

            //Make sure our image isn't null, if it is we have a fallback image already in place
            if (unitTooltip.UnitIcon != null)
            {
                UnitIcon.GetComponent<Image>().sprite = unitTooltip.UnitIcon;
            }

            Description.GetComponent<TextMeshProUGUI>().text = unitTooltip.DescriptionText;
            BonusEffect.GetComponent<TextMeshProUGUI>().text = unitTooltip.BonusEffectText;
            Stats.GetComponent<TextMeshProUGUI>().text = unitTooltip.StatsText;
            Anchor.GetComponent<VerticalLayoutGroup>().enabled = false;
            Anchor.GetComponent<VerticalLayoutGroup>().enabled = true;
            fadeTimer = unitTooltip.fadeDelay;
        }

        CalculateTooltipPos(gameObject.GetComponent<RectTransform>(), tooltip);
    }

    public void CalculateTooltipPos(RectTransform tooltipRect, GameObject tooltip)
    {

        UnitTooltip unitTooltip = tooltip.GetComponent<UnitTooltip>();
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
        }
        else if (_isActive == true)
        {
            fadeTotalTime += Time.deltaTime;
            if (fadeTotalTime >= fadeTimer)
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, Time.deltaTime * 10f);

            if(canvasGroup.alpha > 0 && fadeTotalTime < fadeTimer)
            {
                canvasGroup.alpha = 0;
            }
        }
    }
}
