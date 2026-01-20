using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIButtonBase : MonoBehaviour, 
    IPointerDownHandler, 
    IPointerUpHandler, 
    IPointerExitHandler, 
    IPointerClickHandler
{
    public event Action<bool> PressedChanged;
    public event Action Clicked;

    [Header("Mode")]
    [SerializeField] private bool m_UseHold = false;     // true면 PressedChanged 사용(홀드)
    [SerializeField] private bool m_UseClick = true;     // true면 Clicked 사용(탭)
    [SerializeField] private bool m_IsToggle = false;    // true면 Click 시 토글

    public bool IsPressed { get; private set; }
    public bool IsOn { get; private set; }  // 토글 상태

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!m_UseHold) return;
        SetPressed(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!m_UseHold) return;
        SetPressed(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!m_UseHold) return;
        SetPressed(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!m_UseClick) return;

        if (m_IsToggle)
        {
            IsOn = !IsOn;
            OnToggleChanged(IsOn);
        }

        Clicked?.Invoke();
        OnClicked();
    }

    public void Configure(bool useHold, bool useClick, bool isToggle)
    {
        m_UseHold = useHold;
        m_UseClick = useClick;
        m_IsToggle = isToggle;
    }

    private void SetPressed(bool pressed)
    {
        if (IsPressed == pressed) return;
        IsPressed = pressed;
        PressedChanged?.Invoke(IsPressed);
        OnPressedChanged(IsPressed);
    }

    protected virtual void OnPressedChanged(bool pressed) { }
    protected virtual void OnClicked() { }
    protected virtual void OnToggleChanged(bool isOn) { }
}