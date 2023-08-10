using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class IntroSequencer : MonoBehaviour
{
    public IntroTextHandler mainTextHandler;
    public AudioSource ambienceSource;
    public PostProcessVolume ppVol;
    public GameObject panningBackground;

    private bool isFadeInComplete = false;
    private void Start()
    {
        ambienceSource.volume = 0f;
        ppVol.weight = 0;
        panningBackground.GetComponent<Image>().material.SetFloat("_Alpha", 0f);
        StartCoroutine(StartTextCoroutine());
    }

    private void Update()
    {
        if (!isFadeInComplete)
        {
            ppVol.weight += Time.deltaTime;
            ppVol.weight = Mathf.Min(1.0f, ppVol.weight);
            ambienceSource.volume = Mathf.Lerp(ambienceSource.volume, 0.4f, Time.deltaTime);
            float matAlpha = panningBackground.GetComponent<Image>().material.GetFloat("_Alpha");
            panningBackground.GetComponent<Image>().material.SetFloat("_Alpha", Mathf.Lerp(matAlpha, 0.25f, Time.deltaTime));
        }
    }

    private IEnumerator StartTextCoroutine()
    {

        yield return new WaitForSeconds(0.5f);
        mainTextHandler.Begin();
    }
}
