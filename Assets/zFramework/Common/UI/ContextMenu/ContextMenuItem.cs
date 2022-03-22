using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using zFrame.Events;
using UnityEngine.Events;
using DG.Tweening;
namespace zFrame.UI
{
    public class ContextMenuItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler,IPointerDownHandler
    {
        [HideInInspector] public GameObject target; //转移参数 
        [HideInInspector] public string command; //存储的指令
        private Button button;
        private Text label;
        private Image icon;
        private GameObject separateBar;

        void Awake()
        {
            separateBar = transform.Find("SeparateBar").gameObject; 
            icon = transform.Find("Icon").GetComponent<Image>();
            label = transform.Find("Label").GetComponent<Text>();
            button = GetComponent<Button>();
        }
        /// <summary>
        /// 分发点击事件
        /// </summary>
        public void OnItmeClicked() //要求分发哪一个配置的指令
        {
            EventManager.Allocate<ContextMenuEventArgs>()
                .Config(ContextMenuEvent.Click, gameObject, target, command)
                .Invoke();
        }

        private void OnDestroy()
        {
            target = null;
        }

        void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
        {
            label.transform.DOScale(1.2f, 0.5f);
            EventManager.Allocate<ContextMenuEventArgs>()
                 .Config(ContextMenuEvent.PointEnter, gameObject, target, command)
                 .Invoke();
        }

        void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
        {
            label.transform.DOScale(1f, 0.5f);
            EventManager.Allocate<ContextMenuEventArgs>()
                 .Config(ContextMenuEvent.PointExit, gameObject, target, command)
                 .Invoke();
        }

        /// <summary>
        /// 对象池回收前的准备
        /// </summary>
        internal void Recycle()
        {
            icon.sprite = null;
            command = label.text = string.Empty;
            button.onClick.RemoveAllListeners();
        }

        /// <summary>
        /// 配置右键菜单项
        /// </summary>
        /// <param name="icon">图标</param>
        /// <param name="label">标题</param>
        /// <param name="command">传递的指令</param>
        /// <param name="unityEvent">被执行的事件</param>
        internal void Config(Sprite icon, string label, string command,bool showBar, ContextMenu.ContextMenuEvents unityEvent)
        {
            this.icon.sprite = icon;
            this.command = command;
            this.label.text = label;
            button.onClick.AddListener(() => unityEvent.Invoke(command)); //
            button.onClick.AddListener(OnItmeClicked);
            separateBar.SetActive(showBar);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            label.transform.DOScale(1f, 0.5f);
        }
    }


}
