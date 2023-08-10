using UnityEngine;

[CreateAssetMenu(fileName = "Generic Button Data Container", menuName = "ClickerRPG/GenericButtonDataContainer", order = 1)]
public class GenericButtonUIDataContainer : ScriptableObject
{
    public Sprite activeBG;
    public Sprite activeBorder;
    public Sprite activeLineBreak;

    public Sprite inactiveBG;
    public Sprite inactiveBorder;
    public Sprite inactiveLineBreak;
}
