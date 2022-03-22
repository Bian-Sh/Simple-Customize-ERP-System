using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace zFrame.UI.Components
{
    public class SearchPanel : MonoBehaviour, IReusableScrollViewDataSource
    {
        [SerializeField]
        private InputField inputField;
        [SerializeField]
        private Button searchBt;
        [SerializeField]
        public ReusableScrollViewController scroll; //方便配置数据

        private List<ISearchable> data = null;
        public int Count => data?.Count ?? 0;
        private string lastSearchkeywords = null;//必须初始化非空值或者null
        CanvasGroup canvasGroup;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            inputField.onValueChanged.AddListener(OnSearchKeywordChanged);
            searchBt.onClick.AddListener(OnSearchButtonSubmit);
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
        }
        public void Show(bool animEnable = true)
        {
            if (animEnable)
            {
                canvasGroup.alpha = 0;
                canvasGroup.DOFade(1, 0.5f)
                    .SetDelay(1)
                    .Play()
                    .OnComplete(() =>
                {
                    canvasGroup.blocksRaycasts = true;
                });
            }
            else
            {
                canvasGroup.alpha = 1;
                canvasGroup.blocksRaycasts = true;
            }
        }

        public void Hide()
        {
            canvasGroup.DOKill();
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
            scroll.DataSource = null;
        }


        /// <summary>
        ///来自于用户的原始数据载体
        /// </summary>
        private ISearchDataSource mDataSource = null;
        public ISearchDataSource DataSource
        {
            set
            {
                if (null != value)
                {
                    mDataSource = value;
                    data = value.SearchableSourceData;
                    lastSearchkeywords = null;
                    OnSearchRequest(inputField.text);
                }
                else
                {
                    scroll.DataSource = null;
                    Debug.Log($"{GetType()} ：DataSource 指定为 null ");
                }
            }
        }

        private void OnSearchButtonSubmit()
        {
            OnSearchRequest(inputField.text);
        }
        private void OnSearchKeywordChanged(string arg0)
        {
            OnSearchRequest(arg0);
        }

        /// <summary>
        ///  刷新 searchpanel 的总接口，不管是填充源数据还是搜索结果
        /// </summary>
        /// <param name="arg0">从Inputfield获取</param>
        private void OnSearchRequest(string arg0)
        {
            if (null != mDataSource)
            {
                arg0 = arg0.ToLower().Trim();
                if (arg0 != lastSearchkeywords)
                {
                    lastSearchkeywords = arg0;
                    if (!string.IsNullOrEmpty(arg0))//输入框不变就不要搞事情了,比如输入空格的时候和被不停点击的时候
                    {
                        string[] keystr = arg0.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
                        data = mDataSource.SearchableSourceData; //每次拿源数据，避免输入框删除回退时没有数据
                        foreach (var item in keystr)
                        {
                            data = data.FindAll(v => v.SearchContext.ToLower().Contains(item)); //不分大小写
                        }
                    }
                    else
                    {
                        data = mDataSource.SearchableSourceData;
                    }
                    scroll.DataSource = this;
                }
            }
            else
            {
                Debug.LogWarning($"{GetType()} DataSource 不能为null，搜索不能进行 ！");
            }
        }

        public void UpdateCell(BaseCell cell)
        {
            if (null != mDataSource && cell.dataIndex >= 0 && cell.dataIndex < Count)
            {
                mDataSource.UpdateCell(cell, data[cell.dataIndex]);
            }
            else
            {
                Debug.LogError($"{GetType()} ：{(null == mDataSource ? "ISearchDataSource 似乎还没有赋值！" : $"Index 异常 ：{cell.dataIndex}")}");
            }
        }

        internal void Clear()
        {
            inputField.text = string.Empty;
        }
    }
}
