using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttributesPointsAvailable : MonoBehaviour
{
    private TextMeshProUGUI amntText;
    private void Start()
    {
        amntText = gameObject.GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        amntText.text = "Available Points: " + GameManager.Instance.player.GetComponent<PlayerStats>().currentAttributePoints;


    }
}
