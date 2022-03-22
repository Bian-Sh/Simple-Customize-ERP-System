using System;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Toggle)),ExecuteInEditMode]
public class ToggleLabelHandler : MonoBehaviour,IToggleActionHandler
{
    public Color onColor = Color.white;
    public Color offColor = Color.black;
    Text label;
    private Toggle toggle;
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        label = transform.Find("Label").GetComponent<Text>();
    }
    private void OnEnable()
    {
        UpdateComponents();
    }
    void Start()
    {
        toggle.onValueChanged.AddListener(v => OnValueChanged(toggle));
    }

    [EditorButton]
    public void UpdateComponents()
    {
        label.color = toggle.isOn ? onColor : offColor;
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.SetDirty(toggle);
        }
#endif
    }
    private void OnValueChanged(Toggle toggle)
    {
        label.color = toggle.isOn ? onColor : offColor;
    }
}
