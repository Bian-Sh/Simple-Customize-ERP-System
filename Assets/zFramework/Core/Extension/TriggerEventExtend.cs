using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace zFrame.Extension
{
    public static class TriggerEventExtend
    {
        public static GameObject OnTriggerEnter(this GameObject go, Action<Collider> action)
        {
            if (null == go)
            {
                Debug.LogError("指定对象为空引用！");
                return null;
            }
            if (null == go.GetComponent<Collider>())
            {
                Debug.LogError("指定对象不存在Collider组件！");
                return go;
            }
            TriggerEventListener eventListener = go.GetComponent<TriggerEventListener>();
            if (null == eventListener)
            {
                eventListener = go.AddComponent<TriggerEventListener>();
            }
            if (null != action)
            {
                if (null != eventListener.OnTriggerEntered)
                {
                    Delegate[] delegates = eventListener.OnTriggerEntered.GetInvocationList();
                    Delegate _delegate = action;
                    Debug.Log("要找的这个：" + _delegate.GetType().ToString() + ":" + _delegate.Target.ToString() + ":" + _delegate.Method.Name);
                    if (!Array.Exists(delegates, (v) =>
                    {
                        Debug.Log("已经存在的：" + v.GetType().ToString() + ":" + v.Target.ToString() + ":" + v.Method.Name);
                        return v == (Delegate)action;
                    }))
                    {
                        eventListener.OnTriggerEntered += action;
                    }
                    else
                    {
                        Debug.LogWarning("已添加相同的事件，请确认！");
                    }
                }
                else
                {
                    eventListener.OnTriggerEntered = action;
                }
            }
            else
            {
                Debug.LogError("给定的事件为空值！");
            }
            return go;
        }


    }

    public class TriggerEventListener : MonoBehaviour
    {
        public Action<Collider> OnTriggerEntered;
        private void OnTriggerEnter(Collider other)
        {
            if (null != OnTriggerEntered)
            {
                OnTriggerEntered(other);
            }
        }
    }

}
