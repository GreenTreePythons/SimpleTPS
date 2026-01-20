using UnityEngine;

public sealed class UIHoldButton : UIButtonBase
{
    private void Reset() => Configure(useHold: true, useClick: false, isToggle: false);
    private void Awake() => Configure(useHold: true, useClick: false, isToggle: false);
}