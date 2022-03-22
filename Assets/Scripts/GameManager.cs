using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using static PInvoke;
public class GameManager : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
    static void DisableMaximum()
    {
        Debug.LogError($"{nameof(GameManager)}: 2555");
#if !UNITY_EDITOR
        // 禁用最大化窗口
        // Find window handle of main Unity window.
        var dwStyles = GetWindowLongPtr(UnityHWnd, GWL_STYLE);
        var sty = ((ulong)dwStyles);
        sty &= ~WS_MAXIMIZEBOX;
        SetWindowLongPtr(UnityHWnd, GWL_STYLE, (IntPtr)sty);
#endif
    }
    public void ShutDown()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
