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

    public void Minimize() => SetMinWindows();
    public void ShutDown()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Topmost(bool isUnPin) => SetTopmost(!isUnPin);

}
