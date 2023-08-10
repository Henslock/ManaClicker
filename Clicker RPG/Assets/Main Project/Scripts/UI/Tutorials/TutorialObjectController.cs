using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialObjectController : MonoBehaviour
{
    public GameObject descriptionObj;
    public GameObject previewImageObj;
    public CanvasGroup mainCanvasGroup;
    public GameObject blurObj;
    public GameObject backgroundObj;
    public List<GameObject> scrollNodes;
    public AudioClip appearSFX;
    public AudioClip waterDropSFX;

    [Serializable]
    public struct tutorialPanelStruct
    {
        public string bodyText;
        public Sprite image;
    }

    public tutorialPanelStruct[] tutorialPanel;

    private int sequenceState = 0;
    private int currentPage = 0;

    private void Start()
    {
        sequenceState = 0;
        currentPage = 0;
        blurObj.GetComponent<Image>().material.SetFloat("_Size", 0f);
        backgroundObj.GetComponent<Image>().material.SetFloat("_Alpha", 0f);
        backgroundObj.GetComponent<Image>().material.SetFloat("_MaskSlider", 0f);
        mainCanvasGroup.alpha = 0.001f;

        descriptionObj.GetComponent<TextMeshProUGUI>().text = tutorialPanel[currentPage].bodyText;
        previewImageObj.GetComponent<Image>().sprite = tutorialPanel[currentPage].image;

        if (tutorialPanel.Length-1 <= 1)
        {

        }

        StartCoroutine(StartBlur());

    }

    private void Update()
    {
        if (sequenceState >= 1)
        {
            blurObj.GetComponent<Image>().material.SetFloat("_Size", Mathf.Lerp(blurObj.GetComponent<Image>().material.GetFloat("_Size"), 2.5f, Time.deltaTime*3f));
        }
        if (sequenceState >= 2)
        {
            backgroundObj.GetComponent<Image>().material.SetFloat("_Alpha", Mathf.Lerp(backgroundObj.GetComponent<Image>().material.GetFloat("_Alpha"), 0.85f, Time.deltaTime));
            backgroundObj.GetComponent<Image>().material.SetFloat("_MaskSlider", Mathf.Lerp(backgroundObj.GetComponent<Image>().material.GetFloat("_MaskSlider"), 1.0f, Time.deltaTime*0.75f));
        }
        if (sequenceState >= 3)
        {
            mainCanvasGroup.alpha = Mathf.Lerp(mainCanvasGroup.alpha, 1.0f, Time.deltaTime);
        }
    }

    private IEnumerator StartBlur()
    {
        AudioManager.Instance.PlayUISound(appearSFX, 1.0f, 1.0f);
        yield return new WaitForSeconds(0.5f);
        sequenceState++;
        StartCoroutine(StartPaintedBG());
    }

    private IEnumerator StartPaintedBG()
    {

        yield return new WaitForSeconds(0.5f);
        sequenceState++;
        StartCoroutine(StartRevealContents());
    }

    private IEnumerator StartRevealContents()
    {
        AudioManager.Instance.PlayUISound(waterDropSFX, 0.3f, 0.6f);
        yield return new WaitForSeconds(1.0f);
        sequenceState++;
    }

    public void ClickLeft()
    {
        currentPage--;
        currentPage = Math.Max(currentPage, 0);

        descriptionObj.GetComponent<TextMeshProUGUI>().text = tutorialPanel[currentPage].bodyText;
        previewImageObj.GetComponent<Image>().sprite = tutorialPanel[currentPage].image;

        foreach(GameObject node in scrollNodes)
        {
            node.GetComponent<CanvasGroup>().alpha = 0.25f;
        }
        scrollNodes[currentPage].GetComponent<CanvasGroup>().alpha = 1.0f;

        AudioManager.Instance.PlayUISoundRandomPitch(AudioManager.Instance.genericClick, 0.4f, 0.8f, 1.2f);
    }

    public void ClickRight()
    {
        currentPage++;
        currentPage = Math.Min(currentPage, tutorialPanel.Length-1);

        descriptionObj.GetComponent<TextMeshProUGUI>().text = tutorialPanel[currentPage].bodyText;
        previewImageObj.GetComponent<Image>().sprite = tutorialPanel[currentPage].image;

        foreach (GameObject node in scrollNodes)
        {
            node.GetComponent<CanvasGroup>().alpha = 0.25f;
        }
        scrollNodes[currentPage].GetComponent<CanvasGroup>().alpha = 1.0f;
        AudioManager.Instance.PlayUISoundRandomPitch(AudioManager.Instance.genericClick, 0.4f, 0.8f, 1.2f);
    }

    public void CloseTutorial()
    {
        AudioManager.Instance.PlayUISound(AudioManager.Instance.promptClick, 0.2f, 1.0f);
        Destroy(this.gameObject);
    }
}
