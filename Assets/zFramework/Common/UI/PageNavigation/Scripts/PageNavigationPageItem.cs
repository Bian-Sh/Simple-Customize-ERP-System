using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PageNavigationPageItem : MonoBehaviour
{
    int mPage = 0;
    public int Page
    {
        get => mPage;
        set
        {
            Init();
            mPage = value;
            m_selfText.text = value.ToString();
        }
    }//可见，必须初始化到Awake之后才能使用

    private GameObject m_imgGo;
    private Text m_selfText;
    private Button button;
    public Action<PageNavigationPageItem> OnClicked;
    bool hasInit = false;
    private void Init()
    {
        if (hasInit) return;
        hasInit = true;
        m_imgGo = transform.Find("Image").gameObject;
        m_imgGo.SetActive(false);
        m_selfText = GetComponentInChildren<Text>();
        m_selfText.ChangeTextColor(E_TextColorType.Gray);
        button = GetComponent<Button>();
        if (button)
        {
            button.onClick.AddListener(() =>
            {
                SetActive();
                OnClicked?.Invoke(this);
            });
        }
    }

    /// <summary>
    /// 是否选中
    /// </summary>
    public void SetActive(bool isSelect = true)
    {
        Init();
        m_imgGo.SetActive(isSelect);
        m_selfText.ChangeTextColor(isSelect ? E_TextColorType.Black : E_TextColorType.Gray);
    }

    internal void ResetStatus()
    {
        Page = 0;
        SetActive(false);
        gameObject.SetActive(false);
    }
}
public enum E_TextColorType
{
    White,
    Black,
    /// <summary>
    /// 天蓝色#78F4CE
    /// </summary>
    SkyBlue,
    /// <summary>
    /// 灰色#5B5B5B
    /// </summary>
    Gray,
    /// <summary>
    /// 字体淡蓝色#ADD2DC
    /// </summary>
    Text_LightBlue,
    /// <summary>
    /// 字体天蓝色#63EDC5
    /// </summary>
    Text_Blue,
    /// <summary>
    /// 字体灰色#AAAAAA
    /// </summary>
    Text_Gray,
    /// <summary>
    /// 字体淡绿色#A3E2CF
    /// </summary>
    Text_LightGreen,
    /// <summary>
    /// 字体半透明
    /// </summary>
    Text_Translucent
}
public static class TextEx
{
    public static void ChangeTextColor(this Text txt, E_TextColorType col = E_TextColorType.White)
    {
        Color m_colorTemp = Color.white;
        switch (col)
        {
            case E_TextColorType.White:
                txt.color = Color.white;
                break;
            case E_TextColorType.Black:
                txt.color = Color.black;
                break;
            case E_TextColorType.SkyBlue://78F4CE
                if (ColorUtility.TryParseHtmlString("#78F4CE", out m_colorTemp))
                    txt.color = m_colorTemp;
                break;
            case E_TextColorType.Gray://5B5B5B
                if (ColorUtility.TryParseHtmlString("#5B5B5B", out m_colorTemp))
                    txt.color = m_colorTemp;
                break;
            case E_TextColorType.Text_LightBlue://ADD2DC
                if (ColorUtility.TryParseHtmlString("#ADD2DC", out m_colorTemp))
                    txt.color = m_colorTemp;
                break;
            case E_TextColorType.Text_Blue://63EDC5
                if (ColorUtility.TryParseHtmlString("#63EDC5", out m_colorTemp))
                    txt.color = m_colorTemp;
                break;
            case E_TextColorType.Text_Gray://AAAAAA
                if (ColorUtility.TryParseHtmlString("#AAAAAA", out m_colorTemp))
                    txt.color = m_colorTemp;
                break;
            case E_TextColorType.Text_LightGreen://A3E2CF
                if (ColorUtility.TryParseHtmlString("#A3E2CF", out m_colorTemp))
                    txt.color = m_colorTemp;
                break;
            case E_TextColorType.Text_Translucent:
                m_colorTemp = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a / 2);
                txt.color = m_colorTemp;
                break;
            default:
                break;
        }
    }
}