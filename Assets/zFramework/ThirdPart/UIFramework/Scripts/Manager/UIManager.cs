using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using zFrame.UI;
using UnityEngine.Profiling;

namespace BenYuan.UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public UIPageConfigration uIPageConfigration;

        protected override void Awake()
        {
            base.Awake();

            uIPageConfigration.Init(); //非常需要清空这个数据，鬼知道什么时候加载了
            if (panelStack == null)
            {
                panelStack = new Stack<BasePage>();
            }
            billBoard = transform.Find("BillBoard");
            recycleBin = transform.Find("RecycleBin");
        }

        /// <summary>
        /// 注册已经存在的页面
        /// </summary>
        /// <param name="item"></param>
        public static void RegisterPage(BasePage item, bool recyclepage = false)
        {
            if (recyclepage)
            {
                Instance.uIPageConfigration.RecyclePage(item);
                item.transform.SetParent(Instance.recycleBin, false);
            }
            else
            {
                item.transform.SetParent(Instance.billBoard, false);
                CurrentPage = item;
            }
        }

        #region PanelRawData
        private Stack<BasePage> panelStack;
        /// <summary>
        /// 当前页面
        /// </summary>
        public static BasePage CurrentPage { get; private set; }
        #endregion

        #region UIFramework Component
        [HideInInspector]
        public Transform billBoard;
        private Transform recycleBin;
        #endregion


        #region  UIFrameBehaviours
        /// <summary>
        /// 显示指定类型的页面
        /// </summary>
        /// <param name="panelType">页面类型</param>
        public static T Show<T>() where T : BasePage
        {
            T page = Instance.uIPageConfigration.GetPage<T>();
            Instance.PushPanel(page);
            return page;
        }

        /// <summary>
        /// 关闭指定页面
        /// </summary>
        /// <param name="target">仅处理栈顶页面</param>
        public static void Close(BasePage target)
        {
            Instance.PopPanel(target);
        }
        /// <summary>
        /// 关闭当前栈顶页面
        /// </summary>
        public static void Close()
        {
            Instance.PopPanel();
        }
        #endregion

        /// <summary>
        /// 入栈 ,页面显示的第一步，就是数据的入栈
        /// </summary>
        private void PushPanel(BasePage target)
        {
            if (null == target)
            {
                Debug.LogError("入栈页面不得为空！");
                return;
            }

            if (panelStack.Count > 0)
            {
                BasePage topPanel = panelStack.Peek();//只取出栈顶，不删除
                if (target.isPopUpStyle) //如果是弹窗，则之前的页面可以共存 
                {                        //即：页面保持渲染，只是唤起 Pause() 
                    topPanel.OnPause();
                }
                else                     //否则页面互斥，只能显示最新压入的页面 
                {                        //即：页面移入回收区停止渲染，唤起 OnExit() 
                    topPanel.OnExit();
                    topPanel.transform.SetParent(recycleBin, false);
                }
            }
            CurrentPage = target;       //缓存当前游戏对象
            panelStack.Push(CurrentPage);    //将页面压栈
            CurrentPage.transform.SetParent(billBoard, false); //加载到画布渲染
            CurrentPage.transform.SetAsLastSibling(); //设为渲染的最上层
            CurrentPage.StartCoroutine(DelayAction(CurrentPage.OnEnter)); //延迟到渲染帧结束，这样避免Start中的初始化被跳过，也避免了页面绘制卡顿
            //CurrentPage.OnEnter();//调用页面显示
        }

        IEnumerator DelayAction(Action action)
        {
            yield return new WaitForEndOfFrame();
            action?.Invoke();
        }

        private void PopPanel()
        {
            this.PopPanel(CurrentPage);
        }

        /// <summary>
        /// 出栈
        /// </summary>
        private void PopPanel(BasePage target)
        {
            if (panelStack.Count <= 1 && null == target)
            {
                return;
            }
            if (target != CurrentPage)
            {
                Debug.LogWarning("只能移除栈顶的页面。");
                return;
            }

            CurrentPage.OnExit();
            CurrentPage.transform.SetParent(recycleBin, false); //移出画布避免渲染
            panelStack.Pop();               //将页面从栈顶移除

            BasePage topPage = panelStack.Peek(); //拿到当前栈顶对象

            if (CurrentPage.isPopUpStyle)
            {
                topPage.OnResume();
            }
            else
            {
                topPage.transform.SetParent(billBoard, false);
                topPage.transform.SetAsLastSibling();
                topPage.OnEnter();
            }
            CurrentPage = topPage;
        }
    }
}
