using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static PInvoke;
[RequireComponent(typeof(Graphic))]
public class WindowMoveHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)
        {
            DragWindow();
        }
    }
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)
        {
            MouseButtonUp();
        }
    }


}

