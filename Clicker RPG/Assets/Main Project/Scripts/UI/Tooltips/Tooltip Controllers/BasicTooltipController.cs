using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BasicTooltipController : MonoBehaviour, ITooltipSetup
{
    private CanvasGroup canvasGroup;
    public GameObject Header;
    public GameObject Description;
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
        BasicTooltip basicTooltip = tooltip.GetComponent<BasicTooltip>();

        RectTransform controllerRect = gameObject.GetComponent<RectTransform>();

        controllerRect.sizeDelta = new Vector2(basicTooltip.MaxTooltipWidth, controllerRect.sizeDelta.y);
        Header.GetComponent<TextMeshProUGUI>().text = basicTooltip.HeaderText;
        Description.GetComponent<TextMeshProUGUI>().text = basicTooltip.DescriptionText;
        fadeTimer = basicTooltip.fadeDelay;

        CalculateTooltipPos(gameObject.GetComponent<RectTransform>(), tooltip);
    }

    public void CalculateTooltipPos(RectTransform tooltipRect, GameObject tooltip)
    {

        BasicTooltip basicTooltip = tooltip.GetComponent<BasicTooltip>();
        basicTooltip.CalculateLockOffset();
        float vPos = basicTooltip.verticalLockOffset;
        float hPos = basicTooltip.horizontalLockOffset;


        Vector2 basePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y + yOffset + tooltipRect.sizeDelta.y/2);

        if (basicTooltip.verticalLock)
            basePosition.y = vPos + yOffset + (tooltipRect.sizeDelta.y / 2);

        if (basicTooltip.horizontalLock)
            basePosition.x = hPos;

        float posX = basePosition.x;
        float posY = basePosition.y;

        //Clamp the position of the tooltip to the edges of the screen bounds so it doesn't clip out.
        posX = Mathf.Clamp(posX, 0 + (tooltipRect.sizeDelta.x/2), Screen.width - (tooltipRect.sizeDelta.x/2));
        posY = Mathf.Clamp(posY, 0 + (tooltipRect.sizeDelta.y/2), Screen.height - (tooltipRect.sizeDelta.y/2 + yOffset));

        tooltipRect.transform.position = new Vector2(posX, posY);
    }

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
