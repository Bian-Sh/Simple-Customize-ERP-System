using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class MoveImage : MonoBehaviour,IBeginDragHandler, IDragHandler,IEndDragHandler
{
    RectTransform rect;
    Vector3 offsetPos;
    private int fingerId = int.MinValue; //当前触发的 pointerId ，预设一个永远无法企及的值
    public bool IsDraging { get { return fingerId != int.MinValue; } } //拖拽状态

    private void Awake()
    {
        rect = transform as RectTransform;
    }
    public void OnDrag(PointerEventData eventData)
    {
        //拖拽移动图片
        if (fingerId != eventData.pointerId) return;
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rect.position = globalMousePos+offsetPos;
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerId < -1 || IsDraging) return; //适配 Touch：只响应一个Touch；适配鼠标：只响应左键
        fingerId = eventData.pointerId;
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            offsetPos =  rect.position -globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (fingerId != eventData.pointerId) return;//正确的手指抬起时才会重置；
        offsetPos = Vector3.zero;
        fingerId = int.MinValue;
    }


}

