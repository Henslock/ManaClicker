using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType(typeof(GameManager)) as GameManager;

            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private static GameManager instance;


    public GameObject player;
    public Camera mainCam;
    public Camera uiCam;

    public bool isUpgradesUnlocked;
    public double manaAmount { get; private set; } = 0;
    public Stat manaPerSecond { get; set; }
    public Stat manaPerClick { get; set; }

    private void Start()
    {
        isUpgradesUnlocked = false;
        InitializeManaStats();
        InitializeCanvases();
        RecalculateMana();
    }

    private void InitializeManaStats()
    {
        manaPerSecond = new Stat();
        manaPerClick = new Stat();

        manaPerSecond.baseValue = 0;
        manaPerClick.baseValue = 0;
    }

    private void OnEnable()
    {
        EventManager.GeneralEvents.OnClickCrystal += MainCrystalClick;
        EventManager.UnitEvents.OnUnitPurchased += CheckUpgradeUnlockCondition;
    }

    private void OnDisable()
    {
        EventManager.GeneralEvents.OnClickCrystal -= MainCrystalClick;
        EventManager.UnitEvents.OnUnitPurchased -= CheckUpgradeUnlockCondition;
    }

    private void MainCrystalClick()
    {
        float randomRoll = Random.Range(0f, 1f);
        if ((randomRoll * 100f) <= player.GetComponent<PlayerStats>().CriticalStrikeChance.value)
        {
            //CRIT!
            IncrementMana(manaPerClick.value * 2);

            player.GetComponent<PlayerStats>().GrantExp();
            EventManager.GeneralEvents.OnGainClickMana?.Invoke(manaPerClick.value * 2, true);
            EventManager.PlayerEvents.OnPlayerCrit?.Invoke();
        }
        else
        {
            IncrementMana(manaPerClick.value);

            player.GetComponent<PlayerStats>().GrantExp();
            EventManager.GeneralEvents.OnGainClickMana?.Invoke(manaPerClick.value, false);
        }

    }

    public void RecalculateMana()
    {
        manaPerClick.baseValue = 0.2f;
    }

    public void IncrementMana(double amnt)
    {
        manaAmount += amnt;
    }

    public void SetMana(double amnt)
    {
        manaAmount = amnt;
    }

    private void Update()
    {
        IncrementMana(manaPerSecond.value * Time.deltaTime);
    }

    private void InitializeCanvases()
    { }

    //We unlock unit upgrades when any purchased unit reaches rank 25
    private void CheckUpgradeUnlockCondition(GameObject unitObj)
    {
        if(isUpgradesUnlocked) { return; }
        if(unitObj.TryGetComponent<ManaUnit>(out ManaUnit mUnit))
        {
            if(mUnit.AmountOwned >= 25)
            {
                UnlockUpgradeSystem();
            }
        }
    }

    private void UnlockUpgradeSystem()
    {
        isUpgradesUnlocked = true;
        TutorialManager.Instance.CreateTutorial(Tutorial.UNITUPGRADE);
    }
}
