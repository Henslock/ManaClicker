using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeTitleController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;
    [SerializeField]
    private TextMeshProUGUI rank;
    public void SetTitleText(string _title, int _rank)
    {
        title.text = _title;
        rank.text = "<size=20><alpha=#66>RANK " + _rank +"</size>";

        gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
        gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;
        FadeInText();
    }

    public void FadeInText()
    {
        title.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        rank.gameObject.GetComponent<CanvasGroup>().alpha = 0f;
    }

    private void Update()
    {
        title.gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(title.gameObject.GetComponent<CanvasGroup>().alpha, 1f, Time.deltaTime *8f);
        rank.gameObject.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(rank.gameObject.GetComponent<CanvasGroup>().alpha, 1f, Time.deltaTime*8f);
    }
}
