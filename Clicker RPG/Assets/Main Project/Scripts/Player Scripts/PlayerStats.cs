using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    public int playerLevel { get; private set; }
    public double playerExp { get; private set; }
    public Stat expPerClick { get; set; }

    //Player attributes
    public int currentAttributePoints { get; set; }
    public Stat bonusAttributePointsPerLevel { get; set; }
    private int baseAttributePointsPerLevel = 5;
    public Stat Strength { get; private set; }
    public Stat Intelligence { get; private set; }
    public Stat Agility { get; private set; }
    public Stat Charisma { get; private set; }
    public Stat Power { get; private set; }


    public Stat CriticalStrikeChance { get; private set; }

    private void Start()
    {
        playerLevel = 1;
        expPerClick = new Stat();
        expPerClick.baseValue = 0;

        currentAttributePoints = 0;
        bonusAttributePointsPerLevel = new Stat(0);
        Strength = new Stat(0);
        Intelligence = new Stat(0);
        Agility = new Stat(0);
        Charisma = new Stat(0);
        Power = new Stat(0);

        CriticalStrikeChance = new Stat();
        CriticalStrikeChance.baseValue = 0;
    }


    //Grant exp with a specified value
    public void GrantExp(double amount)
    {
        playerExp += amount;
        LevelCheck();
    }

    //General grant exp that will factor in the defautl exp per click
    public void GrantExp()
    {
        playerExp += expPerClick.value;
        LevelCheck();
    }

    public float GetLevelingPercentage()
    {
        double requiredExp = LevelingTable.GetRequiredExpForLevel(playerLevel);
        return (float)(playerExp / requiredExp);
    }

    private void LevelCheck()
    {
        double requiredExp = LevelingTable.GetRequiredExpForLevel(playerLevel);

        if(playerExp >= requiredExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        playerExp = 0;
        playerLevel += 1;
        currentAttributePoints += baseAttributePointsPerLevel + (int)bonusAttributePointsPerLevel.value;
        EventManager.PlayerEvents.OnPlayerLevelUp?.Invoke();
    }
}
