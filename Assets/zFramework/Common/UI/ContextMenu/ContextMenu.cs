/*
 * 
 * 作用： 响应鼠标右键事件 ，根据场景名称和游戏对象 唤起上下文菜单
 * 备注： 作为单例类使用。
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace zFrame.UI
{
    public class ContextMenu : MonoBehaviour, IPointerDownHandler
    {
        private GameObject target; //唤起该右键菜单的游戏对象
        public ContextItemConfigration itemConfigration;
        private GameObject template; //菜单子项模板
        private Transform container; //菜单容器
        private Transform recycleBin; //菜单回收容器
        private CanvasGroup canvasGroup;// 画布组
        public string defaultMode = "Building.Empty"; //根据场景变化，及时更新
        public float maxDistance = 100;
        public LayerMask layerMask = -1;
        public ContextMenuEvents OnItemSelected = new ContextMenuEvents();
        [System.Serializable]
        public class ContextMenuEvents : UnityEvent<string> { };

        public static ContextMenu Instance { get; private set; }
        private bool IsShow { get { return canvasGroup.alpha > 0; } }
        public bool canShow = true;
        private Vector2 v2;
        /// <summary>
        /// Root RectTransform of context menu.
        /// </summary>
        protected RectTransform rootRect;
        protected Transform canvas;
        private void Awake()
        {
            Instance = this;
            InitComponent();
        }
        private void OnDestroy()
        {
            Instance = null; //断引用，所以调用改组件不建议缓存引用，而是直接用 ： ContextMenu.Instance 调用 即可。
        }

       
        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                v2 = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(1)&&Vector2.Distance(v2,Input.mousePosition)<0.1f) //将模型的交互（射线检测）固化在该组件
            {
                if (!canShow ||( null != EventSystem.current
                    &&
                    EventSystem.current.IsPointerOverGameObject())) //当用户打到UI时不搞事情。(当然，如果3D游戏对象也实现了接口，同样不理他。)
                {
                    return;  //Todo UIManager 的栈顶页面如果为
                }
                RaycastHit hit;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, maxDistance, layerMask))
                {
                    GameObject cached = hit.collider.gameObject;
                    if (cached)
                    {
                        ContextMenuAgent agent = cached.GetComponent<ContextMenuAgent>();
                        if (agent)
                        {
                            Debug.Log("Show agent.mode :" + agent.mode);
                            Show(agent.mode);
                            return;
                        }
                    }
                }
                Debug.Log("Show defaultMode :" + defaultMode);
                Show(defaultMode);
            }
        }



        /// <summary>
        /// 显示指定情景模式的上下文菜单
        /// </summary>
        /// <param name="mode">指定一个情景模式</param>
        public void Show(string mode)
        {
            ContextItemConfigration.ContextItemConfig configration = itemConfigration.configList.Find(v => v.model.Trim() == mode.Trim());
            if (null != configration)//如果配置存在且数据不为空
            {
                if (configration.items.Count > 0)
                {
                    //1. 回收子项
                    RecycleAll();
                    //2. 根据场景配置新的子项
                    ConfigMenu(configration.items);
                    Canvas.ForceUpdateCanvases();
                    CalculateContainerPositon();
                    //3. 显示画布
                    canvasGroup.alpha = 1;
                    canvasGroup.blocksRaycasts = true;
                    return;
                }
                else
                {
                    Debug.LogWarningFormat("  指定配置【{0}】数据为空！", mode);
                }
            }
            else
            {
                Debug.LogErrorFormat("  指定配置【{0}】不存在！", mode);
            }
            //3. 显示画布
            Hide(); //如果有必要就设置为隐藏状态，主要是状态同步啦
        }

        private void CalculateContainerPositon()
        {
            var width = rootRect.rect.width * canvas.localScale.x;
            var height = rootRect.rect.height * canvas.localScale.y;
            var newY = Input.mousePosition.y < height ? Input.mousePosition.y + height : Input.mousePosition.y ; //编辑器下有边界换算错误，打包后无异常。
            var newX = Input.mousePosition.x < Screen.width -width? Input.mousePosition.x : Input.mousePosition.x - width;
            rootRect.position = new Vector2(newX, newY);
        }

        /// <summary>
        /// 配置菜单数据
        /// </summary>
        /// <param name="contextItems">情景数据</param>
        private void ConfigMenu(ContextItemConfigration.ContextItemDataList contextItems)
        {
            for (int i = 0; i < contextItems.Length; i++)
            {
                ContextMenuItem item = Allocate(); //分配数据，并在上下文菜单中插好
                if (null != item)
                {
                    ContextItemConfigration.ContextItemData data = contextItems[i];
                    item.Config(data.icon, data.label, data.command, (i != contextItems.Length - 1), OnItemSelected); // 装填数据
                }
            }
        }

        /// <summary>
        /// 测试当每项被点击时的事件
        /// </summary>
        /// <param name="arg"></param>
        private void OnItemClicked(string arg)
        {
            this.Hide();
        }


        /// <summary>
        /// 隐藏
        /// </summary>
        public void Hide()
        {
            if (IsShow)
            {
                canvasGroup.alpha = 0;
                canvasGroup.blocksRaycasts = false;
            }
        }

        /// <summary>
        /// 初始化组件
        /// </summary>
        public void InitComponent()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            template = transform.Find("Template/ContextMenuItem").gameObject;
            container = transform.Find("Container");
            rootRect = container as RectTransform;
            rootRect.pivot = new Vector2(0, 1);
            canvas = GetComponentInParent<Canvas>().transform;
            #region  初始化上下文菜单,实体+缓存数据
            foreach (Transform item in container)
            {
                GameObject.Destroy(item.gameObject);
            }
            inuseItems.Clear();
            recycledItems.Clear();
            this.Hide();
            #endregion

            #region 创建回收站
            GameObject go = new GameObject("[RecycleBinForContexMenu]");
            go.hideFlags = HideFlags.HideAndDontSave;
            recycleBin = go.transform;
            recycleBin.SetParent(transform, false);
            #endregion
            OnItemSelected.AddListener(OnItemClicked);
        }
        #region Poolable Behaviours
        Stack<ContextMenuItem> recycledItems = new Stack<ContextMenuItem>(10);
        List<ContextMenuItem> inuseItems = new List<ContextMenuItem>(10);
        /// <summary>
        /// 分配菜单子项
        /// </summary>
        private ContextMenuItem Allocate()
        {
            ContextMenuItem item = null;
            if (recycledItems.Count > 0) //如果回收站有，取呗
            {
                item = recycledItems.Pop(); //闲置减一
            }
            else//否则模板搞起
            {
                GameObject go = GameObject.Instantiate(template);
                item = go.GetComponent<ContextMenuItem>();
            }
            if (null != item) //如果子项存在则追加到上下文容器。
            {
                inuseItems.Add(item); //征用加一
                item.transform.SetParent(container, false);
                item.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("ContextMenuItem 为null！！");
            }
            return item;
        }

        /// <summary>
        /// 回收所有子项
        /// </summary>
        /// <param name="target">待回收</param>
        private void RecycleAll()
        {
            foreach (var item in inuseItems)
            {
                if (null != item) //非运行测试，会出现意外null
                {
                    item.Recycle(); //回收数据/注册的事件
                    item.gameObject.SetActive(false);  //不显示
                    item.transform.SetParent(recycleBin, false); //移动到回收筒
                    recycledItems.Push(item); //闲置加一
                }
            }
            inuseItems.Clear();
        }

        /// <summary>
        /// 但鼠标点击了右键菜单空白区域则关闭上下文菜单
        /// </summary>
        /// <param name="eventData"></param>
        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown!!!");
            if (IsShow)  //如果显示则隐藏
            {
                this.Hide();
            }
        }
        #endregion
    }
}
