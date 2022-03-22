using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityFramework.Extentions
{
    public class UISelectable : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public Action<PointerEventData> OnClick;
        public Action<PointerEventData> OnLongPress;
        public Action<PointerEventData> OnPointUp;
        public Action<PointerEventData> OnPointDown;
        public Action<PointerEventData> OnPointEnter;
        public Action<PointerEventData> OnPointExit;
        public Action<PointerEventData> OnBeginDrag;
        public Action<PointerEventData> OnEndDrag;
        public Action<PointerEventData> OnDrag;
        public void OnPointerClick(PointerEventData eventData)
        {
            if((eventData.pointerId == -1 || eventData.pointerId == 0) && !eventData.dragging && null != OnClick)
            {
                OnClick(eventData);
            }
        }

        private bool isDown = false;
        private float beginTime = 0f;
        public void OnPointerDown(PointerEventData eventData)
        {
            if((eventData.pointerId == -1 || eventData.pointerId == 0) && !eventData.dragging)
            {
                isDown = true;
                beginTime = Time.time;
                OnPointDown?.Invoke(eventData);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if((eventData.pointerId == -1 || eventData.pointerId == 0) && !eventData.dragging)
            {
                if (isDown && (Time.time - beginTime) > 1.0f)
                {
                    isDown = false;
                    if (null != OnLongPress)
                    {
                        OnLongPress(eventData);
                    }
                }
                OnPointUp?.Invoke(eventData);
            }
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            OnPointEnter?.Invoke(eventData);
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            OnPointExit?.Invoke(eventData);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDrag?.Invoke(eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            OnEndDrag?.Invoke(eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            OnDrag?.Invoke(eventData);
        }
    }

    public static class GameObjectUIExtentions
    {
        public static void AddClick(this GameObject go, Action click)
        {
            UISelectable selectable = go.GetComponent<UISelectable>();
            if(null == selectable)
            {
                selectable = go.AddComponent<UISelectable>();
            }
            if(null != selectable)
            {
                selectable.OnClick = (PointerEventData data) => 
                {
                    click();
                };
            }
        }

        public static void AddClick(this GameObject go, Action<object> click, object param)
        {
            UISelectable selectable = go.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = go.AddComponent<UISelectable>();
            }
            if (null != selectable)
            {
                selectable.OnClick = (PointerEventData data) =>
                {
                    click(param);
                };
            }
        }

        public static void RemoveClick(this GameObject go)
        {
            UISelectable selectable = go.GetComponent<UISelectable>();
            if(null != selectable)
            {
                selectable.OnClick = null;
                GameObject.Destroy(selectable);
            }
        }

        public static void AddPointEnter(this GameObject img, Action onPointEnter)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnPointEnter = (PointerEventData data) => {
                    onPointEnter?.Invoke();
                };
            }
        }

        public static void AddPointExit(this GameObject img, Action onPointExit)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnPointExit = (PointerEventData data) => {
                    onPointExit?.Invoke();
                };
            }
        }

        public static void AddPointEnter(this GameObject img, Action<PointerEventData> onPointEnter)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnPointEnter = (PointerEventData data) => {
                    onPointEnter?.Invoke(data);
                };
            }
        }

        public static void AddPointExit(this GameObject img, Action<PointerEventData> onPointExit)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnPointExit = (PointerEventData data) => {
                    onPointExit?.Invoke(data);
                };
            }
        }

        public static void AddPointEnter(this GameObject img, Action<object> onPointEnter, object parm)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnPointEnter = (PointerEventData data) => {
                    onPointEnter?.Invoke(parm);
                };
            }
        }

        public static void AddPointExit(this GameObject img, Action<object> onPointExit, object parm)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnPointExit = (PointerEventData data) => {
                    onPointExit?.Invoke(parm);
                };
            }
        }
    }

    public static class ButtonExtentions
    {
        public static void AddClick(this Button button, Action click)
        {
            button.onClick.AddListener(()=> {
                if(null != click)
                {
                    click();
                }
            });
        }

        public static void RemoveClick(this Button button)
        {
            button.onClick.RemoveAllListeners();
        }

        public static void AddPointerDown(this Button button, Action down)
        {
            UISelectable selectable = button.gameObject.GetComponent<UISelectable>();
            if(null == selectable)
            {
                selectable = button.gameObject.AddComponent<UISelectable>();
            }

            if(null != selectable)
            {
                selectable.OnPointDown = (PointerEventData data) => {
                    down?.Invoke();
                };
            }
        }

        public static void AddPointerUp(this Button button, Action up)
        {
            UISelectable selectable = button.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = button.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnPointUp = (PointerEventData data) => {
                    up?.Invoke();
                };
            }
        }
    }

    public static class ImageExtentions
    {
        public static void AddClick(this Image img, Action click)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnClick = (PointerEventData data) =>
                {
                    if (null != click)
                    {
                        click();
                    }
                };
            }
            img.raycastTarget = true;
        }

        public static void AddLongPress(this Image img, Action onLongPress)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnLongPress = (PointerEventData data) =>
                {
                    if (null != onLongPress)
                    {
                        onLongPress();
                    }
                };
            }
            img.raycastTarget = true;
        }

        public static void AddClick(this Image img, Action<object> click, object parmar)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnClick = (PointerEventData data) =>
                {
                    if (null != click)
                    {
                        click(parmar);
                    }
                };
            }
            img.raycastTarget = true;
        }

        public static void RemoveClick(this Image img)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnClick = null;
                GameObject.Destroy(selectable);
            }
        }

        public static void AddPointEnter(this Image img, Action onPointEnter)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnPointEnter = (PointerEventData data) => {
                    onPointEnter?.Invoke();
                };
            }
        }

        public static void AddPointExit(this Image img, Action onPointExit)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnPointExit = (PointerEventData data) => {
                    onPointExit?.Invoke();
                };
            }
        }

        public static void AddPointEnter(this Image img, Action<object> onPointEnter, object parm)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnPointEnter = (PointerEventData data) => {
                    onPointEnter?.Invoke(parm);
                };
            }
        }

        public static void AddPointExit(this Image img, Action<object> onPointExit, object parm)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnPointExit = (PointerEventData data) => {
                    onPointExit?.Invoke(parm);
                };
            }
        }

        public static void AddBeginDrag(this Image img, Action<PointerEventData> OnBeginDrag)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnBeginDrag = (PointerEventData data) => {
                    OnBeginDrag?.Invoke(data);
                };
            }
        }

        public static void AddEndDrag(this Image img, Action<PointerEventData> OnEndDrag)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnEndDrag = (PointerEventData data) => {
                    OnEndDrag?.Invoke(data);
                };
            }
        }

        public static void AddDrag(this Image img, Action<PointerEventData> OnDrag)
        {
            UISelectable selectable = img.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = img.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnDrag = (PointerEventData data) => {
                    OnDrag?.Invoke(data);
                };
            }
        }

        public static void SetActive(this Image img, bool isActive)
        {
            img.gameObject.SetActive(isActive);
        }

    }

    public static class TextExtentions
    {
        public static void AddClick(this Text text, Action click)
        {
            UISelectable selectable = text.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = text.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnClick = (PointerEventData data) => {
                    if (null != click)
                    {
                        click();
                    }
                };
            }
            text.raycastTarget = true;
        }

        public static void AddClick(this Text text, Action<object> click, object param)
        {
            UISelectable selectable = text.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = text.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnClick = (PointerEventData data) => {
                    if (null != click)
                    {
                        click(param);
                    }
                };
            }
            text.raycastTarget = true;
        }

        public static void RemoveClick(this Text text)
        {
            UISelectable selectable = text.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = text.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnClick = null;
                GameObject.Destroy(selectable);
            }
        }
    }

    public static class SliderExtentions
    {
        public static void AddPointUp(this Slider slider, Action clickComplete)
        {
            UISelectable selectable = slider.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = slider.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnPointUp = (PointerEventData data) =>
                {
                    clickComplete?.Invoke();
                };
            }
            slider.interactable = true;
        }

        public static void AddPointDown(this Slider slider, Action clickDown)
        {
            UISelectable selectable = slider.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = slider.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnPointDown = (PointerEventData data) =>
                {
                    clickDown?.Invoke();
                };
            }
            slider.interactable = true;
        }

        public static void AddEndDrag(this Slider slider, Action OnEndDrag)
        {
            UISelectable selectable = slider.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = slider.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnEndDrag = (PointerEventData data) => {
                    OnEndDrag?.Invoke();
                };
            }
        } 
    }

    public static class ToggleExtentions
    {
        public static void AddClick(this Toggle toggle, Action click)
        {
            UISelectable selectable = toggle.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = toggle.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnClick = (PointerEventData data) => {
                    if (null != click)
                    {
                        click();
                    }
                };
            }
            toggle.image.raycastTarget = true;
        }

        public static void RemoveClick(this Toggle toggle)
        {
            UISelectable selectable = toggle.gameObject.GetComponent<UISelectable>();
            if (null == selectable)
            {
                selectable = toggle.gameObject.AddComponent<UISelectable>();
            }

            if (null != selectable)
            {
                selectable.OnClick = null;
                GameObject.Destroy(selectable);
            }
        }
    }
}
