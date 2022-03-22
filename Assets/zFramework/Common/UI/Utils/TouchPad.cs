using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace zFrame.UI
{
    /// <summary>
    /// 用于控制相机旋转
    /// </summary>
    public class TouchPad : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public TouchPadEvent OnValueChanged = new TouchPadEvent(); //事件
        public bool IsTouched { get; private set; } = false;

        [System.Serializable] public class TouchPadEvent : UnityEvent<Touch> { }

        private void OnEnable() //每次被唤醒时自检
        {
            enabled = Input.touchSupported; //如果不支持多点触控这个Pad不需要
        }

        void Update()
        {
        }


        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            OnValueChanged.Invoke(Input.GetTouch(Mathf.Clamp(eventData.pointerId, 0, Input.touchCount - 1)));
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
        }
    }
}