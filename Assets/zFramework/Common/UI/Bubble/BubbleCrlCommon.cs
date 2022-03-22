using UnityEngine;
using UnityEngine.EventSystems;

public class BubbleCrlCommon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string Content = "";
    public BubbleDirection Direction;
    private void Awake()
    {
        if (string.IsNullOrEmpty(Content))
        {
            Debug.LogError($"{gameObject.name} BubbleCrlCommon: Content is null");
        }
    }

    private void Start()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(Content))
            return;
        BubbleItemManager.Instance.OnShow(Direction, this.transform, Content);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (string.IsNullOrEmpty(Content))
            return;
        BubbleItemManager.Instance.OnHide();
    }
}
