using UnityEngine;

//The interface needed for tooltip setups
public interface ITooltipSetup
{
    bool isActive { get; set; }
    float fadeTimer { get; set; }
    void SetupTooltipController(GameObject tooltipObj);
    void FadeController(bool _isActive);
    void CalculateTooltipPos(RectTransform rect, GameObject tooltip);
}

//Every type of tooltip will use this interface
public interface ITooltip
{
    TooltipType tType { get; }
    float fadeDelay { get; }
    bool verticalLock { get;}
    bool horizontalLock { get; }
    float verticalLockOffset { get; }
    float horizontalLockOffset { get; }

    void SetupTooltip();
    void CalculateLockOffset();
}