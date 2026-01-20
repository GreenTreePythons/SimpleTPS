using UnityEngine;

public sealed class UITapButton : UIButtonBase
{
    private void Reset() => Configure(useHold: false, useClick: true, isToggle: false);
    private void Awake() => Configure(useHold: false, useClick: true, isToggle: false);
}