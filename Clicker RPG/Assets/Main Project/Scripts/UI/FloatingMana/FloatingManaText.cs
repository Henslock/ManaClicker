using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FloatingManaText : MonoBehaviour
{
    public float lifetime = 3;
    public float fadetime = 0.75f;
    public float scrollSpeed = 1.5f;
    private float lifeTimer = 0;
    private float fadeTimer = 0;

    public GameObject numberText;
    public AudioSource clickSFXSource;
    public AudioClip clickSFX;
    private RectTransform FMTRect;

    public bool isCrit;

    void Start()
    {
        lifeTimer = 0;
        fadeTimer = 0;
        FMTRect = gameObject.GetComponent<RectTransform>();
        if (isCrit)
        {
            AudioManager.Instance.PlayUISoundRandomPitch(clickSFX, clickSFXSource, 0.45f, 0.86f, 1.15f);
            FMTRect.localScale = Vector3.one * 2.0f;
        }
        else
        {
            AudioManager.Instance.PlayUISoundRandomPitch(clickSFX, clickSFXSource, 1.0f, 0.85f, 1.25f);
        }
    }

    void Update()
    {
        FMTRect.localPosition += Vector3.up * Time.deltaTime * scrollSpeed;

        lifeTimer += Time.deltaTime;
        if(lifeTimer >= lifetime)
        {
            fadeTimer += Time.deltaTime;

            gameObject.GetComponent<CanvasGroup>().alpha = (1.0f -(fadeTimer / fadetime));
        }

        if(lifeTimer >= (lifetime + fadetime))
        {
            Destroy(this.gameObject);
        }

        if(isCrit)
        {
            FMTRect.localScale = Vector3.Lerp(FMTRect.localScale, Vector3.one, Time.deltaTime*5f);
        }
    }

    public void SetText(double amnt)
    {
        string manaAmountString = AbbrevationUtility.AbbreviateNumber(amnt, AbbrevationUtility.FormatType.TYPE_DECIMAL);
        numberText.GetComponent<TextMeshProUGUI>().SetText("+" + manaAmountString);
    }
}
