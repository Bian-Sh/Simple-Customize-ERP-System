using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace zFrame.Extension
{
    [RequireComponent(typeof(InputField))]
    public class TouchKeyBoardComponent : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        InputField field;
        [SerializeField] bool forcePopup = false;
        static Coroutine closeCoroutine = null;

        void Start()
        {
            field = GetComponent<InputField>();
        }

        static TouchKeyBoardComponent()//静态构造，无论多少实例全局只执行一次。能够避免某些时候软键盘首次无法唤起的异常
        {
            HideKeyboard();
        }

        #region Coroutine 
        IEnumerator DelayOpen()
        {
            yield return new WaitForEndOfFrame();  //延迟开启 ，避免多个 InputField 输入数据相同的问题
            string _file = "C:\\Program Files\\Common Files\\microsoft shared\\ink\\TabTip.exe";
            if (File.Exists(_file))
            {
                using (Process _process = Process.Start(_file)) { };
            }
        }
        IEnumerator DelayQuit()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            HideKeyboard();
        }
        #endregion

        static void HideKeyboard() 
        {
            try
            {
                IntPtr _touchhWnd = IntPtr.Zero;
                _touchhWnd = FindWindow("IPTip_Main_Window", null);
                if (_touchhWnd != IntPtr.Zero) PostMessage(_touchhWnd, WM_SYSCOMMAND, SC_CLOSE, 0);
            }
            catch { }
        }

        #region EventSystem Interface Implement
        void ISelectHandler.OnSelect(BaseEventData eventData)
        {
            PointerEventData data = eventData as PointerEventData;
            //如果不是鼠标左键触发的选中事件就证明是Touch触发的，那么就可以欢快的弹软键盘啦！
            if (forcePopup || (null != data && data.pointerId != -1))
            {
                if (null != closeCoroutine) //及时的阻止软键盘的关闭动作，避免了软键盘的反反复复的折叠与展开
                {
                    StopCoroutine(closeCoroutine);
                    closeCoroutine = null;
                }
                StartCoroutine("DelayOpen");
            }
        }

        void IDeselectHandler.OnDeselect(BaseEventData eventData)
        {
            //不关心哪个输入框取消了选中都延迟折叠软键盘，因为延迟，所以点了下一个输入框还能保持软键盘唤起。
            if (null == closeCoroutine)
            {
                closeCoroutine = StartCoroutine("DelayQuit");
            }
        }
        #endregion

        #region Win32API Wrapper
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "PostMessage")]
        private static extern bool PostMessage(IntPtr hWnd, int Msg, uint wParam, uint lParam);
        private const Int32 WM_SYSCOMMAND = 274;
        private const UInt32 SC_CLOSE = 61536;
        #endregion
    }
}
