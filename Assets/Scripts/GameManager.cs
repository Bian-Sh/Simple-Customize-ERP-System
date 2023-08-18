using System;
using UnityEngine;
using static PInvoke;
public class GameManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void InitAppWindow()
    {
        if (Application.isEditor) return;
        var dwStyles = GetWindowLongPtr(UnityHWnd, GWL_STYLE);
        var sty = ((ulong)dwStyles);
        sty &= ~(WS_CAPTION| WS_DLGFRAME)&WS_POPUP;
        SetWindowLongPtr(UnityHWnd, GWL_STYLE, (IntPtr)sty);
    }

    RECT rect = default;
    public void OnMaxMinClicked(bool isFullScreen)
    {
        if (Application.isEditor)
        {
            Debug.LogWarning($"{nameof(OnMaxMinClicked)}: 为避免编辑器行为异常，请打包 exe 后测试！");
            return;
        }
        if (isFullScreen)
        {
            // 获取窗口位置和大小
            GetWindowRect(UnityHWnd, ref rect);
            SetFullScreen(UnityHWnd);
        }
        else
        {
            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;
            SetWindowPos(UnityHWnd, 0, rect.Left, rect.Top, width, height, SWP_SHOWWINDOW);
        }
    }

    public void Minimize() => SetMinWindows();
    public void ShutDown()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Topmost(bool isPin) => SetTopmost(isPin);

}
