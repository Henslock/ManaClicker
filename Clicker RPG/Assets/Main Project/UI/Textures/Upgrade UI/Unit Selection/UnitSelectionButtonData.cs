using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade Selection Button Data", menuName = "ClickerRPG/UpgradeSelectionButtonData", order = 1)]
public class UnitSelectionButtonData : ScriptableObject
{
    //Inactive
    public Sprite InactiveFlair;
    public Sprite InactiveBackground;
    public Sprite InactiveSmallBackground;
    public Sprite InactiveBorder;

    //Active
    public Sprite ActiveFlair;
    public Sprite ActiveBackground;
    public Sprite ActiveSmallBackground;
    public Sprite ActiveBorder;

    //Selected
    public Sprite SelectedFlair;
    public Sprite SelectedBackground;
    public Sprite SelectedSmallBackground;
    public Sprite SelectedBorder;
}
