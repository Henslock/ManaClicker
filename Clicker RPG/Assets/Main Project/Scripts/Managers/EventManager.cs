using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//Hub for all of my events

namespace EventManager
{
    public static class GeneralEvents
    {
        public static Action OnClickCrystal;
        public static Action<double, bool> OnGainClickMana;
        public static Action UpdateManaStats;
    }

    public static class PlayerEvents
    {
        public static Action OnPlayerLevelUp;
        public static Action OnPlayerCrit;
    }

    public static class UnitEvents
    {
        public static Action<GameObject> OnUnitPurchased;
        public static Action<GameObject> OnUpgradePurchasedFromUnit;
        public static Action<GameObject> OnUpgradesResetFromUnit;
        public static Action OnResetUpgades;
        public static Action UpdateManaUnitManagers; //Call to update mana unit managers to keep stats up to date
        public static Action RecalculateManaUnitData; //Call to recalculate mana unit data on demand
    }

    public static class UIEvents
    {
        public static Action<GameObject> UpdateNodeAppearance;
        public static Action<GameObject> OnUpgradeUnitSelection;
        public static Action<GameObject> OnOpenUpgradePanelFromMainScreenUnits; //This event is for when you right click a unit to open the upgrade panel
    }

    public static class Debugging
    {
        public static Action<string> PushConsoleText;
    }
}
