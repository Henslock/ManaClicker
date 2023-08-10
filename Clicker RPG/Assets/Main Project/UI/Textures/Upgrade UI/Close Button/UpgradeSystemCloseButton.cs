using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSystemCloseButton : MonoBehaviour
{
    public Canvas upgradeSystemCanvas;

    public GameObject crystalRenderObj;
    public GameObject crystalIcon;
    public GameObject highlightObj;
    public GameObject cornerGlowObj;
    public GameObject sparklesParticleObj;
    public GameObject arrowObj;

    float crystalRotateSpeed = 6.5f;

    private void Start()
    {
        gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
        highlightObj.GetComponent<Image>().material.SetFloat("_Alpha", 0);
        arrowObj.GetComponent<Image>().material.SetFloat("_Slider", 0);
        sparklesParticleObj.SetActive(false);
    }
    public void ClosePanel()
    {
        if (upgradeSystemCanvas != null)
            upgradeSystemCanvas.gameObject.SetActive(false);
    }

    private void Update()
    {
        crystalRenderObj.transform.Rotate(Vector3.up * Time.deltaTime * crystalRotateSpeed);

        if(CursorManager.Instance.CursorRaycastResults().Contains(crystalIcon))
        {
            gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(gameObject.GetComponent<RectTransform>().localScale, Vector3.one * 1.15f, Time.deltaTime *8f);
            highlightObj.GetComponent<Image>().material.SetFloat("_Alpha", 0.375f);
            arrowObj.GetComponent<Image>().material.SetFloat("_Slider", Mathf.Lerp(arrowObj.GetComponent<Image>().material.GetFloat("_Slider"), 1.0f, Time.deltaTime*4f));
            sparklesParticleObj.SetActive(true);
            crystalRenderObj.transform.Rotate(Vector3.up * Time.deltaTime * crystalRotateSpeed * 3f);
        }
        else
        {
            gameObject.GetComponent<RectTransform>().localScale = Vector3.Lerp(gameObject.GetComponent<RectTransform>().localScale, Vector3.one, Time.deltaTime*8f);
            highlightObj.GetComponent<Image>().material.SetFloat("_Alpha", 0);
            arrowObj.GetComponent<Image>().material.SetFloat("_Slider", Mathf.Lerp(arrowObj.GetComponent<Image>().material.GetFloat("_Slider"), 0f, Time.deltaTime *4f));
            sparklesParticleObj.SetActive(false);
            crystalRenderObj.transform.Rotate(Vector3.up * Time.deltaTime * crystalRotateSpeed);
        }

        float dist = Vector2.Distance(Input.mousePosition, GameManager.Instance.uiCam.WorldToScreenPoint(gameObject.GetComponent<RectTransform>().position));
        float lerpVal = Mathf.Max(0, (Extensions.RemapNumber(dist, 0, 250, 1, 0)));

        cornerGlowObj.GetComponent<Image>().color = Color32.Lerp(new Color32(0, 170, 255, 0), new Color32(0, 170, 255, 66), lerpVal);
    }
}
