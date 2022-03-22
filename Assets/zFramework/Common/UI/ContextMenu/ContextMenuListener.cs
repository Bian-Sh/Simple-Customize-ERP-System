/*
 * ContextMenuHook 
 * 纯粹的事件监听组件，将鼠标事件与右键菜单解耦
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using zFrame.Events;
using System;

namespace zFrame.UI
{
    /// <summary>
    /// 右键菜单唤起组件
    /// </summary>
    public class ContextMenuListener : MonoBehaviour
    {
        private ContextMenu menu;
        private GameObject target; //右键事件发生时鼠标下的游戏对象
        private void Awake()
        {
            menu = GetComponent<ContextMenu>();
        }
        private void Start()
        {
            EventManager.AddListener(StylusEvent.Press ,OnStylusPress);
            EventManager.AddListener(StylusEvent.Release , OnStylusRelease);
        }


        private void OnStylusRelease(BaseEventArgs obj)
        {
            StylusEventArgs args = obj as StylusEventArgs;
            GameObject target = args.Selected;


        }

        private void OnStylusPress(BaseEventArgs obj)
        {
            StylusEventArgs args = obj as StylusEventArgs;
            target = args.Selected;
        }

        private void OnDestroy()
        {
            EventManager.DelListener(StylusEvent.Press, OnStylusPress);
            EventManager.DelListener(StylusEvent.Press, OnStylusRelease);
        }
    }
}
