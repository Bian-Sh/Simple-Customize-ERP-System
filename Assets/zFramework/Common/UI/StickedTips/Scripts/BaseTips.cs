using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace BenYuan.UI.Tips
{
    public abstract class BaseTips : MonoBehaviour
    {
        public Image m_Icon;
        public Text m_Title;
        public Transform m_Content;
        public Transform m_Anchor;  //Tips停靠的参考锚点
        [HideInInspector] public Transform m_StickPoint;
        public string id = ""; //0x000102 这种16进制的写法,如果不需要通过网络获取数据，务必留空
        private Vector3 offsetPosition = Vector3.zero;
        public CanvasGroup canvasGroup;
        [HideInInspector] public string equipmentId = string.Empty; //用于做实时刷新的
        public bool IsShow { get { return canvasGroup.alpha != 0; } }
        /// <summary>
        /// Tips 后台数据请求码，可以不用生成到 RequestCode 配置的。
        /// </summary>
        public int ID
        {
            get
            {
                return HexStr2Hex(id);
            }
        }

        public static int HexStr2Hex(string src)
        {
            if (string.IsNullOrEmpty(src))
            {
                return -1;
            }

            if (!Regex.IsMatch(src, @"^(0[xX])?[0-9a-fA-F]{6}$"))
            {
                Debug.Log($"{src} 格式不对，必须使用 16进制的写法 ，形如 0x000001 或者不需要 0x 也行。");
                return -1;
            }
            return Convert.ToInt32(src, 16);
        }

        public virtual void Awake()
        {

            Hide();
        }

        public virtual void Start()
        {

        }

        public virtual void Show()
        {
            canvasGroup.alpha = 1;
        }

        public virtual void Hide()
        {
            canvasGroup.alpha = 0;
        }

        /// <summary>
        /// 设置Tips应该绑定到哪儿
        /// </summary>
        public void SetStickedTarget(Transform target)
        {
            m_StickPoint = target;
        }

        public void SetIcon(Sprite icon, bool nativesize = true)
        {
            m_Icon.sprite = icon;
            if (nativesize)
            {
                m_Icon.SetNativeSize();
            }
        }
        public virtual void Update() //更新Tips 位置，朝向
        {
            if (m_StickPoint && m_Anchor)
            {
                offsetPosition = transform.position - m_Anchor.position;
                transform.position = offsetPosition + m_StickPoint.position;
            }
            else
            {
                offsetPosition = transform.position - m_Anchor.position;
                transform.position = offsetPosition + Input.mousePosition;
            }
        }

        public virtual void OnResponseReceived(Response obj) { }

        public virtual void OnDestroy()
        {
        }

        //更新 数据 
        public abstract void UpdateContent(object data);
        /// <summary>
        /// 此属性用于记录tips保存的路径，使用 TipsManager 分配tips时会用到
        /// 
        /// </summary>
        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
        public class PathAttribute : Attribute
        {
            public string prefab;
        }


    }
}
