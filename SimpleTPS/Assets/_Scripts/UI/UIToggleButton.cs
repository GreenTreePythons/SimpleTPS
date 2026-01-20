using UnityEngine;
using UnityEngine.UI;

public sealed class UIToggleButton : UIButtonBase
{
    [SerializeField] private Image m_TargetImage;   // Image / Text / TMP 등
    [SerializeField] private Color m_OffColor = Color.white;
    [SerializeField] private Color m_OnColor  = Color.green;
    
    private void Reset()
    {
        Configure(useHold: false, useClick: true, isToggle: true);   
    }

    private void Awake()
    {
        Configure(useHold: false, useClick: true, isToggle: true);
        ApplyColor(IsOn);
    }
    
    protected override void OnToggleChanged(bool isOn)
    {
        ApplyColor(isOn);
    }
    
    private void ApplyColor(bool isOn)
    {
        if (m_TargetImage == null) return;
        m_TargetImage.color = isOn ? m_OnColor : m_OffColor;
    }
}