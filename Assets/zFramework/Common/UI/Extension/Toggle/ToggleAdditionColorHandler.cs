﻿using Malee.List;
using System;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Toggle))]
public class ToggleAdditionColorHandler : MonoBehaviour,IToggleActionHandler
{
    [SerializeField, Reorderable] ConfigInfoArray config;
    private Toggle toggle;
    #region Configuration
    [Serializable]
    internal class ConfigInfo
    {
        public Image target;
        public Color on;
        public Color off;

        internal void SetColor(bool isOn)
        {
            target.color = isOn ? on : off;
        }
    }
    [Serializable]
    internal class ConfigInfoArray : ReorderableArray<ConfigInfo> { }
    #endregion


    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        UpdateComponents();
        toggle.onValueChanged.AddListener(v => OnValueChanged(v));

    }

    [EditorButton]
    public void UpdateComponents()
    {
        toggle = GetComponent<Toggle>();
        foreach (var item in config)
        {
            item.SetColor(toggle.isOn);
        }

#if UNITY_EDITOR

        if (!Application.isPlaying)
        {
            UnityEditor.EditorUtility.SetDirty(toggle);
        }
#endif
    }
    private void OnValueChanged(bool value)
    {
        UpdateComponents();
    }
}
