using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class PressedButton : Button
{
    public event Action<bool> OnPressedChanged;

    public bool IsPressed { get; private set; }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        IsPressed = true;
        OnPressedChanged?.Invoke(true);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        IsPressed = false;
        OnPressedChanged?.Invoke(false);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (!IsPressed) return;

        IsPressed = false;
        OnPressedChanged?.Invoke(false);
    }
}