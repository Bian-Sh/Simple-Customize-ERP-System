using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Text 水平跑马灯效果
/// </summary>
public class TextBillboardEffect : MonoBehaviour
{
    [SerializeField]
    float speed = 0.1f;
    ScrollRect rect;
    RectTransform rectSelf;
    bool canScroll = false;
    Text text = null;
    void Start()
    {
        rect = this.GetComponent<ScrollRect>();
        rectSelf = transform as RectTransform;
        text = rect.content.GetComponent<Text>();
    }

    void Update()
    {
        if (!canScroll && rect.content.sizeDelta.x > rectSelf.sizeDelta.x)
        {
            string context = text.text;
            text.text = $"{context}    {new string (' ', context.Length)} ";
            canScroll = true;
        }

        if (canScroll && rect.content.sizeDelta.x <= rectSelf.sizeDelta.x)
        {
            canScroll = false;
        }
        if (canScroll) ScrollValue();
    }
    private void ScrollValue()
    {
        if (rect.horizontalNormalizedPosition > 1.0f)
        {
            rect.horizontalNormalizedPosition = 0;
        }
        //逐渐递增 ScrollRect 水平方向上的值
        rect.horizontalNormalizedPosition = rect.horizontalNormalizedPosition + speed * Time.deltaTime;
    }
}
