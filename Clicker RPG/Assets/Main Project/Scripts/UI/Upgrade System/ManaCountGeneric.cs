using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ManaCountGeneric : MonoBehaviour
{
    private TextMeshProUGUI amnt;
    void Start()
    {
        amnt = gameObject.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        amnt.text = AbbrevationUtility.AbbreviateNumber(GameManager.Instance.manaAmount);
    }
}
