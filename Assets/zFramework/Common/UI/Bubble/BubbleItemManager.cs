using UnityEngine;
using UnityEngine.UI;

public enum BubbleDirection
{
    Down,
    Left,
    Right
}

public class BubbleItemManager : MonoSingleton<BubbleItemManager>
{
    //private float m_xAxisOffset = -20f;
    private GameObject m_bubbleGo = null;

    private GameObject m_bubbleGo_Down = null;
    private Text m_bubbleText_Down = null;

    private GameObject m_bubbleGo_Left = null;
    private Text m_bubbleText_Left = null;
    private RectTransform m_bubbleRect_Left = null;
    private ContentSizeFitter m_bubbleSizeFitter_Left = null;

    private GameObject m_bubbleGo_Right = null;
    private Text m_bubbleText_Right = null;
    private RectTransform m_bubbleRect_Right = null;
    private ContentSizeFitter m_bubbleSizeFitter_Right = null;

    private BubbleDirection curType;

    private bool m_isInit = false;

    public override void OnInit()
    {
        if (m_isInit)
            return;
        m_bubbleGo = gameObject;
        if (!m_bubbleGo)
            Debug.LogError($"BubbleItemManager: {m_bubbleGo.name} is null");
        Transform m_bubbleT = m_bubbleGo.transform;
        m_bubbleGo_Down = m_bubbleT.Find("Bubble_Down").gameObject;
        m_bubbleText_Down = m_bubbleGo_Down.transform.Find("Text").GetComponent<Text>();

        m_bubbleGo_Left = m_bubbleT.Find("Bubble_Left").gameObject;
        m_bubbleText_Left = m_bubbleGo_Left.transform.Find("Text").GetComponent<Text>();
        m_bubbleRect_Left = m_bubbleGo_Left.transform as RectTransform;
        m_bubbleSizeFitter_Left = m_bubbleGo_Left.GetComponent<ContentSizeFitter>();

        m_bubbleGo_Right = m_bubbleT.Find("Bubble_Right").gameObject;
        m_bubbleText_Right = m_bubbleGo_Right.transform.Find("Text").GetComponent<Text>();
        m_bubbleRect_Right = m_bubbleGo_Right.transform as RectTransform;
        m_bubbleSizeFitter_Right = m_bubbleGo_Right.GetComponent<ContentSizeFitter>();
        HideAllGo();
        m_isInit = true;
    }

    private float m_temp = 0f;
    public void OnShow(BubbleDirection directionType, Transform parent, string str)
    {
        curType = directionType;
        HideAllGo();
        switch (directionType)
        {
            case BubbleDirection.Down:
                if (!m_bubbleGo_Down.activeInHierarchy)
                    m_bubbleGo_Down.SetActive(true);
                m_bubbleText_Down.text = str;
                m_bubbleGo_Down.transform.position = parent.position;
                m_bubbleGo_Down.transform.localPosition += Vector3.down * ((parent.transform as RectTransform).sizeDelta.y / 2 + (m_bubbleGo_Down.transform as RectTransform).sizeDelta.y / 2 + 20);
                m_bubbleGo_Down.transform.localPosition += Vector3.right * 10;//图片并不是对称的，右侧有阴影，所以需要偏移一点
                break;
            case BubbleDirection.Left:
                if (!m_bubbleGo_Left.activeInHierarchy)
                    m_bubbleGo_Left.SetActive(true);
                m_bubbleText_Left.text = str;
                m_temp = HandleSelfFittingAlongAxis(0, m_bubbleSizeFitter_Left, m_bubbleRect_Left);
                m_bubbleGo_Left.transform.position = parent.position;
                m_bubbleGo_Left.transform.localPosition += Vector3.left * ((parent.transform as RectTransform).sizeDelta.x / 2 + m_temp / 2 + 20);
                m_bubbleGo_Left.transform.localPosition += Vector3.down * 10;
                break;
            case BubbleDirection.Right:
                if (!m_bubbleGo_Right.activeInHierarchy)
                    m_bubbleGo_Right.SetActive(true);
                m_bubbleText_Right.text = str;
                m_temp = HandleSelfFittingAlongAxis(0, m_bubbleSizeFitter_Right, m_bubbleRect_Right);
                m_bubbleGo_Right.transform.position = parent.position;
                m_bubbleGo_Right.transform.localPosition += Vector3.right * ((parent.transform as RectTransform).sizeDelta.x / 2 + m_temp / 2 + 20);
                m_bubbleGo_Right.transform.localPosition += Vector3.down * 10;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 实时获取自适应宽度或高度
    /// 解决ContentSizeFitter在同步操作时不能及时刷新的问题
    /// </summary>
    /// <param name="axis"></param>
    /// <param name="contentSizeFitter"></param>
    /// <param name="rectTransform"></param>
    /// <returns></returns>
    private float HandleSelfFittingAlongAxis(int axis, ContentSizeFitter contentSizeFitter, RectTransform rectTransform)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        ContentSizeFitter.FitMode fitting = (axis == 0 ? contentSizeFitter.horizontalFit : contentSizeFitter.verticalFit);
        if (fitting == ContentSizeFitter.FitMode.MinSize)
        {
            return LayoutUtility.GetMinSize(rectTransform, axis);
        }
        else
        {
            return LayoutUtility.GetPreferredSize(rectTransform, axis);
        }
    }

    public void OnHide()
    {
        switch (curType)
        {
            case BubbleDirection.Down:
                m_bubbleGo_Down.transform.position = Vector3.zero;
                m_bubbleText_Down.text = string.Empty;
                if (m_bubbleGo_Down.activeInHierarchy)
                    m_bubbleGo_Down.SetActive(false);
                break;
            case BubbleDirection.Left:
                m_bubbleGo_Left.transform.position = Vector3.zero;
                m_bubbleText_Left.text = string.Empty;
                if (m_bubbleGo_Left.activeInHierarchy)
                    m_bubbleGo_Left.SetActive(false);
                break;
            case BubbleDirection.Right:
                m_bubbleGo_Right.transform.position = Vector3.zero;
                m_bubbleText_Right.text = string.Empty;
                if (m_bubbleGo_Right.activeInHierarchy)
                    m_bubbleGo_Right.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void ResetStatus()
    {
        OnHide();
        m_isInit = false;
    }

    private void HideAllGo()
    {
        if (m_bubbleGo_Down.activeInHierarchy)
            m_bubbleGo_Down.SetActive(false);

        if (m_bubbleGo_Left.activeInHierarchy)
            m_bubbleGo_Left.SetActive(false);

        if (m_bubbleGo_Right.activeInHierarchy)
            m_bubbleGo_Right.SetActive(false);
    }
}
