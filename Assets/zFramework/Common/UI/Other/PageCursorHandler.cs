using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace zFrame.UI
{
    public class PageCursorHandler : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {

        [SerializeField]
        private Texture2D hotAreaCursor;

        public void OnPointerEnter(PointerEventData eventData)
        {
            Cursor.SetCursor(hotAreaCursor, Vector2.zero, CursorMode.Auto);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetDefaultCursor();
        }
        private static void SetDefaultCursor()
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        private void OnDisable()
        {
            SetDefaultCursor();
        }
        private void OnDestroy()
        {
            SetDefaultCursor();
        }

    }

}
