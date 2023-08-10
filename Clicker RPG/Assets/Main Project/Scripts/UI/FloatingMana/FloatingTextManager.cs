using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject FloatingManaTextPrefab;
    public GameObject CritFloatingManaTextPrefab;
    public GameObject clickEffect;
    public AudioClip critSFX;

    private Canvas ftCanvas;

    private void Start()
    {
        ftCanvas = gameObject.GetComponent<Canvas>();
    }
    private void OnEnable()
    {
        EventManager.GeneralEvents.OnGainClickMana += CreateFloatingManaText;
    }

    private void OnDisable()
    {
        EventManager.GeneralEvents.OnGainClickMana -= CreateFloatingManaText;
    }

    private void CreateFloatingManaText(double amnt, bool isCrit)
    {
        var newClickEffect = Instantiate(clickEffect, ftCanvas.transform);
        Vector2 mousePos = Input.mousePosition;
        newClickEffect.GetComponent<RectTransform>().anchoredPosition = mousePos;
        newClickEffect.GetComponentInChildren<ParticleSystem>().Play();
        Destroy(newClickEffect, 3f);


        if (isCrit)
        {
            var newText = Instantiate(CritFloatingManaTextPrefab, this.transform);
            newText.GetComponent<RectTransform>().anchoredPosition = mousePos + (Vector2.up * 15);
            newText.GetComponent<FloatingManaText>().SetText(amnt);
        }
        else
        {
            var newText = Instantiate(FloatingManaTextPrefab, this.transform);
            newText.GetComponent<RectTransform>().anchoredPosition = mousePos + (Vector2.up * 15);
            newText.GetComponent<FloatingManaText>().SetText(amnt);
        }
    }
}
