using System;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Toggle)),ExecuteInEditMode]
public class ToggleEventHandler : MonoBehaviour
{
    private Toggle toggle;
    public Toggle.ToggleEvent OnToggleOnEvent= new Toggle.ToggleEvent();
    public Toggle.ToggleEvent OnToggleOffEvent= new Toggle.ToggleEvent();
    private void Awake()
    {
        toggle = GetComponent<Toggle>();
    }
    void Start()
    {
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool value)
    {
        if (value)
        {
            OnToggleOnEvent.Invoke(value);
        }
        else
        {
            OnToggleOffEvent.Invoke(value);
        }
    }
}
