using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPointingChevronIndicator : MonoBehaviour
{
    public GameObject chevronObj;
    private RectTransform chevronRect;

    private float yOffset = 65f;

    private float waitTimerBeforeShowing = 0.5f;

    private float timer = 0f;
    private void Start()
    {
        chevronRect = chevronObj.GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        chevronObj.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= waitTimerBeforeShowing)
        {
            float bounceOffset = Mathf.Sin(Time.time * 5f) * 8f;
            chevronRect.localPosition = new Vector2(0, yOffset + bounceOffset);

            chevronObj.GetComponent<Image>().color = Color32.Lerp(chevronObj.GetComponent<Image>().color, new Color32(255, 255, 255, 255), Time.deltaTime * 5f);
        }
    }
}
