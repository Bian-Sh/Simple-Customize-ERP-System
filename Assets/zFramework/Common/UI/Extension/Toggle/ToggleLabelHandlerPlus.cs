using System;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Toggle)), ExecuteInEditMode]
public class ToggleLabelHandlerPlus : MonoBehaviour,IToggleActionHandler
{
    [SerializeField] Config on  = new Config { color =Color.white , fontSize = 22, fontStyle = FontStyle.Normal};
    [SerializeField] Config off =new Config { color =Color.white , fontSize = 22, fontStyle = FontStyle.Normal};

[SerializeField] Text label;
    private Toggle toggle;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        if (!label)
        {
            label = transform.Find("Label").GetComponent<Text>();
        }
        toggle.onValueChanged.AddListener(v => OnValueChanged(toggle));
    }
    private void OnEnable()
    {
        UpdateComponents();
    }
    [EditorButton]
    public void UpdateComponents()
    {
        label.color = toggle.isOn ? on.color : off.color;
        label.fontStyle = toggle.isOn ? on.fontStyle : off.fontStyle;
        label.fontSize = toggle.isOn ? on.fontSize : off.fontSize;
        label.font = toggle.isOn ? on.font : off.font;

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.SetDirty(toggle);
        }
#endif
    }
    private void OnValueChanged(Toggle toggle)
    {
        UpdateComponents();
    }
    public void SetFontSize(int factor) 
    {
        on.fontSize *= factor;
        off.fontSize *= factor;
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(toggle);
#endif
    }
    [Serializable]
    class Config
    {
        public FontStyle fontStyle;
        public int fontSize;
        public Color color;
        public Font font;
    }

}
