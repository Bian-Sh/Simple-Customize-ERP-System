using UnityEngine;
using UnityEngine.EventSystems;
using static PInvoke;

public class WindowResizeHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    bool isDragging = false;
    public Vector2 hotspot = Vector2.zero;
    public Vector2 aspect = new Vector2(16, 9);

    // Minimum and maximum values for window width/height in pixel.
    [SerializeField]
    private int minWidthPixel = 1366;
    [SerializeField]
    private int maxWidthPixel = 2048;

    public Texture2D wnes;
    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)
        {
            isDragging = true;
            Cursor.SetCursor(wnes, hotspot, CursorMode.Auto);
        }
    }

    void IDragHandler.OnDrag(PointerEventData eventData) => WindowProcess(eventData);
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerId == -1)
        {
            isDragging = false;
            Cursor.SetCursor(default, default, CursorMode.Auto);
        }
    }

    private void WindowProcess(PointerEventData eventData)
    {
        if (Application.isEditor || !isDragging) return;
        RECT rc = default;
        GetWindowRect(UnityHWnd, ref rc);
        Debug.Log($"{nameof(WindowResizeHandler)}: RECT = {rc}");
        int newWidth = Mathf.Clamp(rc.Right - rc.Left + Mathf.RoundToInt(eventData.delta.x), minWidthPixel, maxWidthPixel);
        int newHeight = Mathf.RoundToInt(newWidth / (aspect.x / aspect.y));
        SetWindowPos(UnityHWnd, 0, rc.Left, rc.Top, newWidth, newHeight, SWP_SHOWWINDOW);
        //锁定鼠标光标位置？


    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(wnes, hotspot, CursorMode.Auto);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (!isDragging)
        {
            Cursor.SetCursor(default, default, CursorMode.Auto);
        }
    }
}
