using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;


public class IntroTextHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI mainText;
    public AudioClip textSFX;
    public AudioClip clickSFX;
    public float timeBetweenLetters = 0.1f;

    public GameObject manaIcon;
    public GameObject mousePrompt;
    public float initialWaitTime = 5f;

    public string[] introText;
    private int introTextCurrIndex = 0;

    private bool isTextReady = false;
    private float idleTimeCounter = 0;
    private float idleWaitTimeBeforePrompt = 4f;
    void Start()
    {
        introTextCurrIndex = 0;
        mousePrompt.GetComponent<CanvasGroup>().alpha = 0;
        mainText.text = string.Empty;
        manaIcon.SetActive(false);
    }

    public void Begin()
    {
        StartCoroutine(StartSequence());
    }

    private void Update()
    {
        //Show mana icon
        if (isTextReady == true)
        {
            float angle = Mathf.Sin(Time.time * 3f);
            manaIcon.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, angle * 10f);
            idleTimeCounter += Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(0) && isTextReady)
        {
            idleTimeCounter = 0;
            mousePrompt.GetComponent<CanvasGroup>().alpha = 0f;
            AudioManager.Instance.PlayUISound(clickSFX, 1.0f, 1.0f);
            introTextCurrIndex++;
            introTextCurrIndex = Mathf.Min(introText.Length - 1, introTextCurrIndex);
            isTextReady = false;
            manaIcon.SetActive(false);
            if (introTextCurrIndex != introText.Length - 1)
            {
                mainText.text = string.Empty;
                StartCoroutine(BuildText());
            }
            else
            {
                mainText.text = string.Empty;
                //Do stuff
            }
        }

        //If the player is sitting around long enough, give them a mouse prompt
        if(idleTimeCounter >= idleWaitTimeBeforePrompt)
        {
            ShowMousePrompt();
        }

        manaIcon.GetComponent<RectTransform>().localScale = Vector3.Lerp(manaIcon.GetComponent<RectTransform>().localScale, Vector3.one, Time.deltaTime * 6f);
    }

    private IEnumerator StartSequence()
    {
        yield return new WaitForSeconds(initialWaitTime);
        StartCoroutine(BuildText());
    }

    private IEnumerator BuildText()
    {
        float textSpeed = timeBetweenLetters;
        float bonusPause = 0;
        yield return new WaitForSeconds(0.5f);
        mainText.text = introText[introTextCurrIndex];
        for (int i = 0; i <= mainText.textInfo.characterCount; i++)
        {
            if (mainText.textInfo.linkCount > 0)
            {
                foreach (TMP_LinkInfo link in mainText.textInfo.linkInfo)
                {
                    if (link.GetLinkID() == "slowspeed")
                    {
                        if (i >= link.linkTextfirstCharacterIndex && i <= link.linkTextfirstCharacterIndex + link.linkTextLength)
                        {
                            textSpeed = timeBetweenLetters * 4f;
                        }
                        else
                        {
                            textSpeed = timeBetweenLetters;
                        }
                    }

                    if (link.GetLinkID() == "extraPause")
                    {
                        bonusPause = 1f;
                    }
                    else
                    {
                        bonusPause = 0f;
                    }
                }
            }

            mainText.maxVisibleCharacters = i;
            AudioManager.Instance.PlayUISoundRandomPitch(textSFX, 0.5f, 0.7f, 1.7f);
            //Wait a certain amount of time, then continue with the for loop
            yield return new WaitForSeconds(textSpeed);
        }

        yield return new WaitForSeconds(0.5f + bonusPause);
        isTextReady = true;
        manaIcon.SetActive(true);
        manaIcon.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);
    }

    private void ShowMousePrompt()
    {
        mousePrompt.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(mousePrompt.GetComponent<CanvasGroup>().alpha, 0.5f, Time.deltaTime);
    }
}
