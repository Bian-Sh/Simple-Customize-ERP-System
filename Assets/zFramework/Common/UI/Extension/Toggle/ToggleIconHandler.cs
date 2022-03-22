using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Toggle))]
public class ToggleIconHandler : MonoBehaviour,IToggleActionHandler
{
    public Sprite onIcon;
    public Sprite offIcon;
    private Toggle toggle;
    private Image icon;
    public bool setNativeSize = true;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        icon = toggle.targetGraphic.gameObject.GetComponent<Image>();
        toggle.onValueChanged.AddListener(v => OnValueChanged(v));
    }

    public void UpdateIcon(Sprite onIcon, Sprite offIcon)
    {
        this.onIcon = onIcon;
        this.offIcon = offIcon;
        UpdateComponents();
    }

    [EditorButton]
    private void RevertIcon() 
    {
        var t = onIcon;
        onIcon = offIcon;
        offIcon = t;
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }

    [EditorButton]
    public void UpdateComponents()
    {
        if (!toggle) toggle = GetComponent<Toggle>();
        if (!icon) icon = toggle.targetGraphic.gameObject.GetComponent<Image>();
        if (toggle.isOn)
        {
            if (icon.sprite != onIcon)
            {
                icon.sprite = onIcon;
            }
        }
        else
        {
            if (icon.sprite != offIcon)
            {
                icon.sprite = offIcon;
            }
        }
        if (setNativeSize)
        {
            icon.SetNativeSize();
        }
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }

    public void OnValueChanged(bool value)
    {
        if (value)
        {
            icon.sprite = onIcon;
        }
        else
        {
            icon.sprite = offIcon;
        }
    }
}
