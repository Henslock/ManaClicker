using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarScript : MonoBehaviour
{

    public Material xpBarMat;

    void Start()
    {
        xpBarMat.SetFloat("_Swipe", 0);
    }

    private void OnEnable()
    {
        EventManager.PlayerEvents.OnPlayerLevelUp += ResetBar;
    }

    private void OnDisable()
    {
        EventManager.PlayerEvents.OnPlayerLevelUp -= ResetBar;
    }

    void Update()
    {
        float targetAmount = GameManager.Instance.player.GetComponent<PlayerStats>().GetLevelingPercentage();
        float currXPAmnt = xpBarMat.GetFloat("_Swipe");
        xpBarMat.SetFloat("_Swipe", Mathf.Lerp(currXPAmnt, targetAmount, Time.deltaTime * 5f));
    }

    private void ResetBar()
    {
        xpBarMat.SetFloat("_Swipe", 0);
    }
}
