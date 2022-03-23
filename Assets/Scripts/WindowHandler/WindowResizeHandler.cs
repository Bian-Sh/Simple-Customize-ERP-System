using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using static PInvoke;

public class WindowResizeHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    bool isDragging = false;
    bool isInsideOfHandler = false;
    public Vector2 hotspot = Vector2.zero;

    // Minimum and maximum values for window width/height in pixel.
    [SerializeField]
    private int minWidthPixel = 768;
    [SerializeField]
    private int minHeightPixel = 512;
    [SerializeField]
    private int maxWidthPixel = 2048;
    [SerializeField]
    private int maxHeightPixel = 2048;

    public Texture2D wnes;
    private float aspect = 16 / 9f;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
#if !UNITY_EDITOR
        WindowProcess(eventData.delta);
#endif
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        if (!isInsideOfHandler)
        {
            Cursor.SetCursor(default, default, CursorMode.Auto);
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        isInsideOfHandler = true;
        Cursor.SetCursor(wnes, hotspot, CursorMode.Auto);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        isInsideOfHandler = false;
        if (!isDragging)
        {
            Cursor.SetCursor(default, default, CursorMode.Auto);
        }
    }

    void WindowProcess(Vector2 delta)
    {
        RECT rc = default;
        GetWindowRect(UnityHWnd, ref rc);
        int newWidth = Mathf.Clamp(rc.Right - rc.Left + Mathf.RoundToInt(delta.x), minWidthPixel, maxWidthPixel);
        int newHeight = Mathf.RoundToInt(newWidth / aspect);
        SetWindowPos(UnityHWnd, 0, rc.Left, rc.Top, newWidth, newHeight, SWP_SHOWWINDOW);
    }
}
