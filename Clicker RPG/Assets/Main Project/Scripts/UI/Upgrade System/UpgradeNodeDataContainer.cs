using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade Node Data Container", menuName = "ClickerRPG/UpgradeNodeDataContainer", order = 1)]
public class UpgradeNodeDataContainer : ScriptableObject
{
    public Sprite minorNodeActiveSprite;
    public Sprite minorNodePurchasedSprite;
    public Sprite minorNodeInactiveSprite;
    public Sprite majorNodeActiveSprite;
    public Sprite majorNodePurchasedSprite;
    public Sprite majorNodeInactiveSprite;
    public GameObject majorNodeCelebratoryParticles;
    public GameObject minorNodeCelebratoryParticles;
    public GameObject majorNodeSparkle;
    public GameObject minorNodeSparkle;
}
