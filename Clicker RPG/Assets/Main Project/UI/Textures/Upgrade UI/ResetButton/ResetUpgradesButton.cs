using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetUpgradesButton : MonoBehaviour
{
    public GameObject magicObj;
    public GameObject highlightBorderObj;
    public GameObject highlightObj;
    public GameObject background;

    private float timeScalar = 0f;
    private float highlightBorderScalar = 0f;
    private float timeScalarSpeed;
    private float highlightBorderSpeed;

    private void Start()
    {
        magicObj.GetComponent<Image>().material.SetFloat("_Speed", 0.15f);
        highlightBorderObj.GetComponent<Image>().material.SetFloat("_Interpolate", 0f);
        highlightObj.GetComponent<Image>().color = new Color32(0, 17, 34, 0);
        background.GetComponent<RectTransform>().localScale = Vector3.one;
        timeScalarSpeed = 0.15f;
        highlightBorderSpeed = 0.25f;
    }

    private void Update()
    {
        timeScalar += Time.deltaTime * timeScalarSpeed;
        highlightBorderScalar += Time.deltaTime * highlightBorderSpeed;
        magicObj.GetComponent<Image>().material.SetFloat("_TimeScalar", timeScalar);
        if (CursorManager.Instance.CursorRaycastResults().Contains(background))
        {
            timeScalarSpeed = Mathf.Lerp(timeScalarSpeed, 0.35f, Time.deltaTime *4f);
            highlightBorderSpeed = Mathf.Lerp(highlightBorderSpeed, 1f, Time.deltaTime *4f);

            highlightObj.GetComponent<Image>().color = Color32.Lerp(highlightObj.GetComponent<Image>().color, new Color32(0, 17, 34, 155), Time.deltaTime*6f);
            highlightBorderObj.GetComponent<Image>().material.SetFloat("_Interpolate", highlightBorderScalar);

            background.GetComponent<RectTransform>().localScale = Vector3.Lerp(background.GetComponent<RectTransform>().localScale, Vector3.one*1.05f, Time.deltaTime*12f);
        }
        else
        {
            timeScalarSpeed = Mathf.Lerp(timeScalarSpeed, 0.15f, Time.deltaTime * 4f);
            highlightBorderSpeed = Mathf.Lerp(highlightBorderSpeed, 0.25f, Time.deltaTime * 4f);
            highlightBorderObj.GetComponent<Image>().material.SetFloat("_Interpolate", highlightBorderScalar);
            highlightObj.GetComponent<Image>().color = Color32.Lerp(highlightObj.GetComponent<Image>().color, new Color32(0, 17, 34, 0), Time.deltaTime * 10f);

            background.GetComponent<RectTransform>().localScale = Vector3.Lerp(background.GetComponent<RectTransform>().localScale, Vector3.one, Time.deltaTime * 12f);

        }
    }
}
