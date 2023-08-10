using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UpgradeListTextObject : MonoBehaviour
{
    public GameObject textBG;
    public TextMeshProUGUI listText;
    public GameObject masterNode;

    private bool isHighlighted = false;
    private bool isAppearing = false;
    private bool isPopping = false;
    private Material fadeMat;

    private Color32 fadeMinorCol = new Color32(0, 212, 233, 255);
    private Color32 fadeMajorCol = new Color32(0, 222, 92, 255);

    private void Awake()
    {
        Material mat = Instantiate(textBG.GetComponent<Image>().material);
        textBG.GetComponent<Image>().material = mat;
        fadeMat = mat;

        textBG.GetComponent<Image>().color = new Color32(0, 0, 0, 85);
        fadeMat.SetFloat("_Slider", 1f);
        listText.text = "Nothing";

    }

    public void SetupText(string text)
    {
        listText.text = text;
    }

    public void SetHighlighted(bool _state)
    {
        isHighlighted = _state;
    }

    public void SetAppearAnim()
    {
        isAppearing = true;
        fadeMat.SetFloat("_Slider", 0f);

        if(masterNode == null)
            fadeMat.SetColor("_FadeColor", fadeMinorCol);
        else
            fadeMat.SetColor("_FadeColor", fadeMajorCol);

        listText.color = new Color32(255, 255, 255, 0);
        StartCoroutine(FadeInCancelCoroutine());
    }

    public void SetUpdatePopAnim()
    {
        isPopping = true;
        listText.gameObject.GetComponent<RectTransform>().localScale = Vector3.one * 1.1f;
        StartCoroutine(UpdatePoppingCoroutine());
    }

    private void Update()
    {
        //Controls UI when we highlight over List Text Object
        if(isHighlighted)
            textBG.GetComponent<Image>().color = new Color32(0, 0, 0, 155);
        else
            textBG.GetComponent<Image>().color = Color32.Lerp(textBG.GetComponent<Image>().color, new Color32(0, 0, 0, 85), Time.deltaTime);

        if(isAppearing)
        {
            fadeMat.SetFloat("_Slider", Mathf.Lerp(fadeMat.GetFloat("_Slider"), 1f, Time.deltaTime*3f));
            listText.color = Color32.Lerp(listText.color, new Color32(255, 255, 255, 255), Time.deltaTime*4f);
        }

        if (isPopping)
        {
            listText.gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(listText.gameObject.GetComponent<RectTransform>().localScale, Vector3.one, Time.deltaTime*4f);
        }
    }

    IEnumerator FadeInCancelCoroutine()
    {
        yield return new WaitForSeconds(2);

        fadeMat.SetFloat("_Slider", 1f);
        listText.color = new Color32(255, 255, 255, 255);
        isAppearing = false;

    }

    IEnumerator UpdatePoppingCoroutine()
    {
        yield return new WaitForSeconds(2);
        listText.gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        isPopping = false;

    }
}
