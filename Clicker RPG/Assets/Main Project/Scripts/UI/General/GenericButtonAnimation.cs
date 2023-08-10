using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericButtonAnimation : MonoBehaviour
{
    public GenericButtonUIDataContainer buttonData;
    public Canvas referenceCanvas; //We need to operate differently based on if we are using screen space or camera canvases

    public GameObject buttonHighlightObj;
    public GameObject buttonBG;
    public GameObject buttonLineBreak;
    public GameObject buttonBorder;
    public GameObject highlightBar;
    
    private Material buttonHighlightMat;
    private Color32 highlightBarCol;

    public bool isButtonActive = true;
    private RenderMode rendMode;

    private void Awake()
    {
        Material mat = Instantiate(buttonHighlightObj.GetComponent<Image>().material);
        buttonHighlightObj.GetComponent<Image>().material = mat;
        buttonHighlightMat = buttonHighlightObj.GetComponent<Image>().material;

        buttonHighlightMat.SetFloat("_Alpha", 0f);
        buttonBG.GetComponent<Button>().interactable = true;
        highlightBarCol = highlightBar.GetComponent<Image>().color;

        if(referenceCanvas == null)
        {
            rendMode = RenderMode.ScreenSpaceOverlay;
        }
        else
            rendMode = referenceCanvas.renderMode;
    }

    public void SetButtonState(bool _state)
    {
        if(isButtonActive == _state) { return; } //Don't do anything if we are just setting it to the exact same state

        isButtonActive = _state;
        if (isButtonActive)
            ActivateButton();
        else
            DeactivateButton();
    }

    private void ActivateButton()
    {
        buttonBG.GetComponent<Image>().sprite = buttonData.activeBG;
        buttonLineBreak.GetComponent<Image>().sprite = buttonData.activeLineBreak;
        buttonBorder.GetComponent<Image>().sprite = buttonData.activeBorder;
        highlightBar.GetComponent<Image>().color = highlightBarCol;

        buttonBG.GetComponent<Button>().interactable = true;
    }

    private void DeactivateButton()
    {
        buttonBG.GetComponent<Image>().sprite = buttonData.inactiveBG;
        buttonLineBreak.GetComponent<Image>().sprite = buttonData.inactiveLineBreak;
        buttonBorder.GetComponent<Image>().sprite = buttonData.inactiveBorder;
        highlightBar.GetComponent<Image>().color = new Color32(0, 0, 0, 65);
        buttonHighlightMat.SetFloat("_Alpha", 0f);
        gameObject.GetComponent<RectTransform>().localScale = Vector3.one;

        buttonBG.GetComponent<Button>().interactable = false;
    }

    private void Update()
    {
        //This button doesn't animate if its not active
        if(!isButtonActive) { return; }
        float dist = 1000;
        if (rendMode == RenderMode.ScreenSpaceCamera)
        {
            dist = Vector2.Distance(Input.mousePosition, GameManager.Instance.uiCam.WorldToScreenPoint(gameObject.GetComponent<RectTransform>().position));
        }
        else
        {
            dist = Vector2.Distance(Input.mousePosition, gameObject.GetComponent<RectTransform>().position);
        }

        //This distance check is hopefully a performance increase
        if(dist <= 100f)
        {
            //Check is cursor is hovering over button
            if (CursorManager.Instance.CursorRaycastResults().Contains(buttonBG))
            {
                buttonHighlightMat.SetFloat("_Alpha", Mathf.Lerp(buttonHighlightMat.GetFloat("_Alpha"), 0.45f, Time.deltaTime * 8f));

                //Lerp highlight bar;
                Color32 tarCol = highlightBarCol;
                tarCol.a += 30;
                highlightBar.GetComponent<Image>().color = Color32.Lerp(highlightBar.GetComponent<Image>().color, tarCol, Time.deltaTime * 8f);

                //Lerp size
                Vector3 size = gameObject.GetComponent<RectTransform>().localScale;
                gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(size, Vector3.one * 1.05f, Time.deltaTime*8f);
            }
            else
            {
                buttonHighlightMat.SetFloat("_Alpha", Mathf.Lerp(buttonHighlightMat.GetFloat("_Alpha"), 0, Time.deltaTime * 8f));
                highlightBar.GetComponent<Image>().color = highlightBarCol;

                //Lerp size
                Vector3 size = gameObject.GetComponent<RectTransform>().localScale;
                gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(size, Vector3.one, Time.deltaTime * 8f);
            }
        }
        else
        {
            buttonHighlightMat.SetFloat("_Alpha", Mathf.Lerp(buttonHighlightMat.GetFloat("_Alpha"), 0, Time.deltaTime * 8f));
            highlightBar.GetComponent<Image>().color = highlightBarCol;

            //Lerp size
            Vector3 size = gameObject.GetComponent<RectTransform>().localScale;
            gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(size, Vector3.one, Time.deltaTime * 8f);
        }
    }
}
