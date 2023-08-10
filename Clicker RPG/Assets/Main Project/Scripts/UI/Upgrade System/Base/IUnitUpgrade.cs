using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public enum UpgradeNodeType
{
    MINOR,
    MAJOR
}

public enum UpgradeNodeMaster
{
    MANAPICK,
    RUINS,
    INFUSED_SOIL,
    MYSTIC_TREE
}
public interface IUnitUpgrade
{
    bool isActive { get; }
    bool isPurchased { get; }
    UpgradeNodeType nodeType { get; }
    UpgradeNodeMaster nodeMaster { get; }
    List<GameObject> connectedNodes { get; }
    Button nodeBtn { get; }
    Image nodeShadow { get; }
    int requiredRank { get; }

    void Activate();
    void ResetNode();
}
