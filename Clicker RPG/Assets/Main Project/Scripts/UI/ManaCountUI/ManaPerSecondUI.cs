using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ManaPerSecondUI : MonoBehaviour
{
    private TextMeshProUGUI mpsText;
    private void Start()
    {
        mpsText = gameObject.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        string mpsVal = AbbrevationUtility.AbbreviateNumber(GameManager.Instance.manaPerSecond.value);
        mpsText.text = mpsVal + " <size=22><color=#00F1FF>M</color>/S</size>";
    }
}
