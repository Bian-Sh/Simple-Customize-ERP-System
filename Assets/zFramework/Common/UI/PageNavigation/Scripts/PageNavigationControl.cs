using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PageNavigationControl : MonoBehaviour
{
    /// <summary>
    /// 显示按钮的个数  奇数个
    /// </summary>
    [SerializeField, Range(3, 9), Header("展示的按钮个数：")]
    private int _showBtnCount = 9;
    [SerializeField, Header("页码按钮模板：")]
    private Transform template;
    [SerializeField] private Transform container;
    public int Total { get; private set; } = 0;
    public int Current { get; private set; } = 1;
    public int ShowBtnCount
    {
        get => (_showBtnCount % 2 == 0) ? _showBtnCount - 1 : _showBtnCount;
        set => _showBtnCount = (value % 2 == 0) ? value - 1 : value;
    }
    /// <summary>
    /// 页数按钮 存放list
    /// </summary>
    [SerializeField]
    private List<PageNavigationPageItem> pageItemList; //必须序列化到面板，直接写内存中各种毛病
    /// <summary>
    /// 上一页、下一页按钮
    /// </summary>
    private Button m_lastPageBtn, m_nextPageBtn;
    /// <summary>
    /// 页面跳转输入框
    /// </summary>
    private InputField m_jumpPageInputField;
    /// <summary>
    /// 两侧缩略点
    /// </summary>
    [SerializeField]
    private GameObject dot_lf, dot_rt;//必须序列化到面板，直接写内存中各种毛病
    [Space(10)]
    public PageNavigationEvent OnValueChanged = new PageNavigationEvent();
    [Serializable]
    public class PageNavigationEvent : UnityEvent<int> { }

    private void Awake()
    {
        m_jumpPageInputField = transform.Find("PageJumper/InputField").GetComponent<InputField>();
        m_jumpPageInputField.onEndEdit.AddListener(OnPageJumpRequired);

        m_lastPageBtn = container.Find("Button_LeftArrow").GetComponent<Button>();
        m_lastPageBtn.onClick.AddListener(OnPreviewPageRequired);

        m_nextPageBtn = container.Find("Button_RightArrow").GetComponent<Button>();
        m_nextPageBtn.onClick.AddListener(OnNextPageRequired);

        ConfigButtons();
    }

    private void OnValidate()
    {
        _showBtnCount = ShowBtnCount;
    }
    /// <summary>
    /// 创建忽略点
    /// </summary>
    private void InsertDots()
    {
        Func<int, GameObject> CreatDot = index =>
       {
           var go = Instantiate(template);
           //修剪模板冗余内容
           var item = go.GetComponent<PageNavigationPageItem>();
           var bt = go.GetComponent<Button>();
           var img = go.Find("Image");
           item.SetActive(false);
           if (Application.isPlaying)
           {
               Destroy(item);
               Destroy(bt);
               Destroy(img.gameObject);
           }
           else
           {
               DestroyImmediate(item);
               DestroyImmediate(bt);
               DestroyImmediate(img.gameObject);
           }
           //状态初始化
           go.name = "dot";
           var tx = go.GetComponentInChildren<Text>();
           tx.text = "...";
           go.SetParent(container, false);
           go.SetSiblingIndex(index);
           return go.gameObject;
       };
        if (!dot_lf) dot_lf = CreatDot(2);
        if (!dot_rt) dot_rt = CreatDot(container.childCount - 3);

        dot_lf.transform.SetSiblingIndex(2);
        dot_rt.transform.SetSiblingIndex(container.childCount - 3);
        dot_lf.SetActive(false);
        dot_rt.SetActive(false);
    }

    private void ConfigButtons()
    {
        //数据复位
        if (null == pageItemList)
        {
            pageItemList = new List<PageNavigationPageItem>();
        }
        else
        {
            foreach (var item in pageItemList)
            {
                if (!item) continue; //编辑器下手动删除会导致item被销毁，处理之
                DestroyImmediate(item.gameObject);
            }
            pageItemList.Clear();
        }
        //构建button
        for (int i = 0; i < ShowBtnCount; i++)
        {
            var go = Instantiate(template);
            go.name = i.ToString();
            go.SetParent(container, false);
            go.SetSiblingIndex(i + 1);
            var item = go.GetComponent<PageNavigationPageItem>();
            item.SetActive(false);
            item.OnClicked = OnPageItemSelected;
            pageItemList.Add(item);
            go.gameObject.SetActive(false);
        }
        //构建 dot

        InsertDots();

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    /// <summary>
    /// 下一页
    /// </summary>
    private void OnNextPageRequired()
    {
        if (Total <= 0 || Current >= Total) return;
        SubmitPageIndexChanges(Current + 1);
    }

    /// <summary>
    /// 上一页
    /// </summary>
    private void OnPreviewPageRequired()
    {
        if (Total <= 0 || Current <= 1) return;
        SubmitPageIndexChanges(Current - 1);
    }

    /// <summary>
    /// 跳转页面
    /// </summary>
    /// <param name="str"></param>
    private void OnPageJumpRequired(string str)
    {
        int index = 1;
        if (int.TryParse(str, out index))
        {
            index = Mathf.Clamp(index, 1, Total);
            m_jumpPageInputField.text = index.ToString();
            SubmitPageIndexChanges(index);
        }
    }

    private void SubmitPageIndexChanges(int index)
    {
        if (Current != index)
        {
            Current = index;
            UpdateFacade();
            Debug.Log($"用户选择了 {Current} 页！");
            OnValueChanged.Invoke(Current);
        }
        else
        {
            Debug.Log($"当前已经是 {Current} 页了！");
        }
    }

    /// <summary>
    /// 改变显示的状态
    /// </summary>
    private void UpdateFacade()
    {
        foreach (var item in pageItemList)
        {
            item.ResetStatus();
        }
        dot_lf.SetActive(false);
        dot_rt.SetActive(false);

        /*
            配置 ：showbutton = 7
            情况1： 1..  3  4  5  6  7  ..10
            情况2： 1  2  3  4  5  6  ..10
            情况3： 1..  5  6  7  8  9  10
            情况4： 1  2   3   4   5               （总页码小于showbutton数）

            配置 ：showbutton = 3
            情况1： 1  ..  2  ..   10                  (就不存在情况一了)
            情况2： 1  2  ..   10
            情况3： 1  ..  9  10
            情况4： 1  2   3                          （总页码小于showbutton数）
         */
        if (Total <= 0) return;
        if (Total <= ShowBtnCount)
        {
            for (int i = 0; i < Total; i++)
            {
                var item = pageItemList[i];
                item.gameObject.SetActive(true);
                item.Page = i + 1; //注意了：页码是以1开始，代码内所有索引以0开始
                pageItemList[i].SetActive(Current == i + 1);
            }
        }
        else
        {
            int center = ShowBtnCount / 2;
            for (int i = 0; i < pageItemList.Count; i++)
            {
                var item = pageItemList[i];
                item.gameObject.SetActive(true);
                if (Current > center + 1)
                {
                    if (Total - Current > center)
                    {
                        if (i == center)
                        {
                            item.Page = Current;
                        }
                        else if (i == ShowBtnCount - 1)
                        {
                            pageItemList[pageItemList.Count - 1].Page = Total;
                        }
                        else
                        {
                            pageItemList[i].Page = i == 0 ? 1 : Current + i - center;
                        }
                    }
                    else
                    {
                        item.Page = i == 0 ? 1 : Total + (i - (ShowBtnCount - 1));
                    }
                }
                else
                {
                    item.Page = (i == ShowBtnCount - 1 && Total - Current > center) ? Total : i + 1;
                }
                if (item.Page == Current)
                {
                    item.SetActive();
                }
            }
            dot_lf.gameObject.SetActive(Current > center + 1);
            dot_rt.gameObject.SetActive(Total - Current > center);
        }
    }

    [EditorButton]
    public void TestSet(int total, int current = 1)
    {
        ConfigButtons();
        Set(total, current);
    }

    /// <summary>
    /// 设置信息
    /// </summary>
    /// <param name="total">总页数</param>
    public void Set(int total, int current = 1)
    {
        this.Total = total;
        this.Current = Mathf.Clamp(current, 1, total);
        UpdateFacade();
    }
    private void OnPageItemSelected(PageNavigationPageItem target)
    {
        SubmitPageIndexChanges(target.Page);
    }
}
