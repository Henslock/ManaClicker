using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectionButtonUIController : MonoBehaviour
{
    public enum BUTTONSTATE
    {
        INACTIVE,
        ACTIVE,
        SELECTED
    }

    public UnitSelectionButtonData buttonData;
    public GameObject Button;
    public GameObject highlightGlowObj;
    public GameObject hoverHighlightObj;
    public GameObject unitIconAnchor;
    public GameObject unitIcon;
    public GameObject backgroundFireObj;

    public BUTTONSTATE buttonState;

    public GameObject buttonBorder;
    public GameObject buttonFlair;
    public GameObject buttonBackground;
    public GameObject buttonSoftGlow;
    public GameObject buttonSmallBackground;

    public GameObject clickVFXPrefab;

    [HideInInspector]
    public bool isHighlighted;
    // Start is called before the first frame update
    void Start()
    {
        InitializeUI();
        isHighlighted = false;
    }

    public void SetButtonState(BUTTONSTATE _btnState)
    {
        buttonState = _btnState;
    }

    private void InitializeUI()
    {
        gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

        Material mat = Instantiate(hoverHighlightObj.GetComponent<Image>().material);
        hoverHighlightObj.GetComponent<Image>().material = mat;

        Material mat2 = Instantiate(backgroundFireObj.GetComponent<Image>().material);
        backgroundFireObj.GetComponent<Image>().material = mat2;

        if (buttonState == BUTTONSTATE.INACTIVE)
        {
            highlightGlowObj.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
            backgroundFireObj.GetComponent<Image>().material.SetFloat("_Speed", 0.1f);
            hoverHighlightObj.GetComponent<Image>().material.SetFloat("_Alpha", 0f);

            SetInactive();
        }

        if (buttonState == BUTTONSTATE.ACTIVE)
        {
            highlightGlowObj.GetComponent<Image>().color = new Color32(0, 28, 48, 0);
            backgroundFireObj.GetComponent<Image>().material.SetFloat("_Speed", 0.1f);
            hoverHighlightObj.GetComponent<Image>().material.SetFloat("_Alpha", 0f);

            SetActive();
        }

        if(buttonState == BUTTONSTATE.SELECTED)
        {
            highlightGlowObj.GetComponent<Image>().color = new Color32(48, 34, 0, 0);
            backgroundFireObj.GetComponent<Image>().material.SetFloat("_Speed", 0.4f);
            hoverHighlightObj.GetComponent<Image>().material.SetFloat("_Alpha", 0f);

            SetSelected();
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Control visual changes for ACTIVE button state
        if (buttonState == BUTTONSTATE.ACTIVE)
        {
            if (isHighlighted)
            {
                gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(gameObject.GetComponent<RectTransform>().localScale, Vector3.one * 1.1f, Time.deltaTime * 8f);
                hoverHighlightObj.GetComponent<Image>().material.SetFloat("_Alpha", Mathf.Lerp(hoverHighlightObj.GetComponent<Image>().material.GetFloat("_Alpha"), 0.5f, Time.deltaTime * 10f));
                highlightGlowObj.GetComponent<Image>().color = Color32.Lerp(highlightGlowObj.GetComponent<Image>().color, new Color32(0, 28, 48, 200), Time.deltaTime * 2f);
            }
            else
            {
                gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(gameObject.GetComponent<RectTransform>().localScale, Vector3.one, Time.deltaTime * 12f);
                hoverHighlightObj.GetComponent<Image>().material.SetFloat("_Alpha", Mathf.Lerp(hoverHighlightObj.GetComponent<Image>().material.GetFloat("_Alpha"), 0f, Time.deltaTime * 10f));
                highlightGlowObj.GetComponent<Image>().color = Color32.Lerp(highlightGlowObj.GetComponent<Image>().color, new Color32(0, 28, 48, 0), Time.deltaTime * 8f);
            }
        }

        if (buttonState == BUTTONSTATE.SELECTED)
        {
            gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(gameObject.GetComponent<RectTransform>().localScale, Vector3.one * 1.1f, Time.deltaTime * 8f);

            if (isHighlighted)
            {
                hoverHighlightObj.GetComponent<Image>().material.SetFloat("_Alpha", Mathf.Lerp(hoverHighlightObj.GetComponent<Image>().material.GetFloat("_Alpha"), 0.5f, Time.deltaTime * 10f));
                highlightGlowObj.GetComponent<Image>().color = Color32.Lerp(highlightGlowObj.GetComponent<Image>().color, new Color32(48, 34, 0, 200), Time.deltaTime * 2f);
            }
            else
            {
                hoverHighlightObj.GetComponent<Image>().material.SetFloat("_Alpha", Mathf.Lerp(hoverHighlightObj.GetComponent<Image>().material.GetFloat("_Alpha"), 0f, Time.deltaTime * 10f));
                highlightGlowObj.GetComponent<Image>().color = Color32.Lerp(highlightGlowObj.GetComponent<Image>().color, new Color32(48, 34, 0, 0), Time.deltaTime * 8f);
            }
        }

        if (buttonState == BUTTONSTATE.INACTIVE)
        {

            if (isHighlighted)
            {
                gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(gameObject.GetComponent<RectTransform>().localScale, Vector3.one * 1.1f, Time.deltaTime * 8f);
            }
            else
            {
                gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(gameObject.GetComponent<RectTransform>().localScale, Vector3.one, Time.deltaTime * 12f);
            }
        }
    }


    public void SetInactive()
    {
        buttonBorder.GetComponent<Image>().sprite = buttonData.InactiveBorder;
        buttonFlair.GetComponent<Image>().sprite = buttonData.InactiveFlair;
        buttonBackground.GetComponent<Image>().sprite = buttonData.InactiveBackground;
        buttonSmallBackground.GetComponent<Image>().sprite = buttonData.InactiveSmallBackground;
        backgroundFireObj.GetComponent<Image>().color = new Color32(0, 0, 0, 0);
        buttonSmallBackground.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        buttonSoftGlow.GetComponent<Image>().color = new Color32(255, 255, 255, 25);
        unitIcon.GetComponent<Image>().color = new Color32(0, 0, 0, 255);

        gameObject.GetComponent<CanvasGroup>().alpha = 0.5f;

        buttonState = BUTTONSTATE.INACTIVE;
    }

    public void SetActive()
    {
        buttonBorder.GetComponent<Image>().sprite = buttonData.ActiveBorder;
        buttonFlair.GetComponent<Image>().sprite = buttonData.ActiveFlair;
        buttonBackground.GetComponent<Image>().sprite = buttonData.ActiveBackground;
        buttonSmallBackground.GetComponent<Image>().sprite = buttonData.ActiveSmallBackground;
        backgroundFireObj.GetComponent<Image>().color = new Color32(0, 43, 118, 255);
        buttonSmallBackground.GetComponent<Image>().color = new Color32(32, 140, 255, 255);
        buttonSoftGlow.GetComponent<Image>().color = new Color32(0, 78, 185, 165);
        unitIcon.GetComponent<Image>().color = new Color32(210, 235, 255, 255);

        gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
        backgroundFireObj.GetComponent<Image>().material.SetFloat("_Speed", 0.1f);

        buttonState = BUTTONSTATE.ACTIVE;
    }

    public void SetSelected()
    {
        buttonBorder.GetComponent<Image>().sprite = buttonData.SelectedBorder;
        buttonFlair.GetComponent<Image>().sprite = buttonData.SelectedFlair;
        buttonBackground.GetComponent<Image>().sprite = buttonData.SelectedBackground;
        buttonSmallBackground.GetComponent<Image>().sprite = buttonData.SelectedSmallBackground;
        backgroundFireObj.GetComponent<Image>().color = new Color32(118, 20, 0, 255);
        buttonSmallBackground.GetComponent<Image>().color = new Color32(255, 149, 105, 255);
        buttonSoftGlow.GetComponent<Image>().color = new Color32(185, 66, 0, 255);
        unitIcon.GetComponent<Image>().color = new Color32(255, 247, 210, 255);

        gameObject.GetComponent<CanvasGroup>().alpha = 1.0f;
        backgroundFireObj.GetComponent<Image>().material.SetFloat("_Speed", 0.45f);

        buttonState = BUTTONSTATE.SELECTED;
    }

    public void PlaySelectedVFX()
    {
        GameObject vfx = Instantiate(clickVFXPrefab, gameObject.transform);
        Destroy(vfx, 2f);
    }
}
