using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static PInvoke;
[RequireComponent(typeof(Image))]
public class WindowMoveHandler : MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerExitHandler
{
    static bool isDrag = false;
    void IPointerDownHandler.OnPointerDown(PointerEventData eventData) => isDrag = true;
    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) => isDrag = false;
    void IPointerUpHandler.OnPointerUp(PointerEventData eventData) => isDrag = false;
#if !UNITY_EDITOR
    private void Update()
    {
        if (isDrag)
        {
            DragWindow();
        }
    }
#endif
}

