using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UnityFramework.Utils
{
    public class CheckUI : MonoSingleton<CheckUI>
    {
        bool isUI = false;
        public bool CheckIsUI()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) || Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                isUI = HasCastOnUI();
            }
            return isUI;
        }

       public  bool HasCastOnUI()
        {
            if (!EventSystem.current||!EventSystem.current.IsPointerOverGameObject()) return false;
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.pressPosition = Input.mousePosition;
            data.position = Input.mousePosition;
            List<RaycastResult> list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, list);
            return list.Count > 0 && list[0].module is GraphicRaycaster;
        }
        public bool CheckIsTouchOnUI(Touch touch)
        {
            if (touch.phase == TouchPhase.Began)
            {
                isUI = HasCastOnUI(); //用在 Touch 上可能有问题
            }
            return isUI;
        }
    }
}
