using System;
using UnityEngine;
using static PInvoke;
public class GameManager : MonoBehaviour
{
    public static int height = 1080;
    public static int width = 1920;
    public static string h_key = "erp_client_height";
    public static string w_key = "erp_client_width";

    [IngameDebugConsole.ConsoleMethod("DeleteKey","清除软件分辨率信息")]
    private static void ClearPrefs()
    {
        if (PlayerPrefs.HasKey(h_key))
        {
            PlayerPrefs.DeleteKey(h_key);
        }
        if (PlayerPrefs.HasKey(w_key))
        {
            PlayerPrefs.DeleteKey(w_key);
        }
    }
 
    public void OnResoulutionChanged(int h, int w, bool fs)
    {
        PlayerPrefs.SetInt(h_key, height = h);
        PlayerPrefs.SetInt(w_key, width = w);
    }
    private static void SetResolution()
    {
        height = PlayerPrefs.GetInt(h_key, height);
        width = PlayerPrefs.GetInt(w_key, width);
        Screen.SetResolution(width, height, false);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void InitAppWindow()
    {
        if (Application.isEditor) return;
        var dwStyles = GetWindowLongPtr(UnityHWnd, GWL_STYLE);
        var sty = ((ulong)dwStyles);
        sty &= ~(WS_CAPTION| WS_DLGFRAME)&WS_POPUP;
        SetWindowLongPtr(UnityHWnd, GWL_STYLE, (IntPtr)sty);
        SetResolution();
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
}
