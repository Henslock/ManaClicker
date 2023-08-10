using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelText : MonoBehaviour
{
    private TextMeshProUGUI levelText;

    private void Start()
    {
        levelText = gameObject.GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        EventManager.PlayerEvents.OnPlayerLevelUp += UpdateLevelText;
    }

    private void OnDisable()
    {
        EventManager.PlayerEvents.OnPlayerLevelUp -= UpdateLevelText;
    }

    private void UpdateLevelText()
    {
        levelText.text = "Lvl. " + GameManager.Instance.player.GetComponent<PlayerStats>().playerLevel;
    }
}
