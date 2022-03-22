using System;
using UnityEngine;
using UnityEngine.UI;
namespace zFrame.Events.Example
{
    public class ColorEventReceive : MonoBehaviour
    {
        void Awake()
        {
            EventManager.AddListener(ColorEvent.ChangeTo, OnColorChangeRequired);
        }
        private void OnColorChangeRequired(BaseEventArgs obj)
        {
            ColorEventArgs args = obj as ColorEventArgs;
            GetComponent<MeshRenderer>().material.color = args.Color;
        }
        void OnDestroy()
        {
            EventManager.DelListener(ColorEvent.ChangeTo, OnColorChangeRequired);
        }
    }
}