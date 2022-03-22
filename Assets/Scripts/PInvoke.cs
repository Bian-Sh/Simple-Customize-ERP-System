using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

public static class PInvoke
{
    static IntPtr ptr;
    public static IntPtr UnityHWnd
    {
        get
        {
            if (ptr == null || ptr == IntPtr.Zero)
            {
                ptr = GetUnityWindow();
            }
            return ptr;
        }
    }

    #region 常量
    //https://docs.microsoft.com/zh-cn/windows/win32/winmsg/window-styles
    public const ulong WS_MAXIMIZEBOX = 0x00010000L; //最大化的按钮禁用
    public const ulong WS_DLGFRAME = 0x00400000L; //不现实边框
    public const int WS_POPUP = 0x800000;
    public const int GWL_STYLE = -16;
    //边框参数
    public const uint SWP_SHOWWINDOW = 0x0040;
    public const int WS_BORDER = 1;
    public const int SW_SHOWMINIMIZED = 2;//(最小化窗口)
    // Name of the Unity window class used to find the window handle.
    public const string UNITY_WND_CLASSNAME = "UnityWndClass";
    #endregion

    #region Win32 API

    //获得窗口样式
    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    public static extern IntPtr GetWindowLongPtr(IntPtr hwnd, int nIndex);
    // 改变指定窗口的属性 ，该函数还在额外窗口内存中的指定偏移处设置一个值。
    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Auto)]
    public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
   
    // 通过将每个窗口的句柄依次传递给应用程序定义的回调函数，枚举与线程关联的所有非子窗口。
    [DllImport("user32.dll")]
    private static extern bool EnumThreadWindows(uint dwThreadId, EnumWindowsProc lpEnumFunc, IntPtr lParam);
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
    //检索调用线程的线程标识符。
    [DllImport("kernel32.dll")]
    private static extern uint GetCurrentThreadId();
    // 检索指定窗口所属的类的名称。
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetClassName(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    //设置当前窗口的显示状态
    [DllImport("user32.dll")]
    public static extern bool ShowWindow(System.IntPtr hwnd, int nCmdShow);

    //设置窗口边框
    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowLong(IntPtr hwnd, int _nIndex, int dwNewLong);

    //设置窗口位置，大小
    [DllImport("user32.dll")]
    public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    //窗口拖动
    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();
    [DllImport("user32.dll")]
    public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);


    #endregion
    #region Static Function
    //最小化窗口
    //具体窗口参数看这     https://msdn.microsoft.com/en-us/library/windows/desktop/ms633548(v=vs.85).aspx
    public static void SetMinWindows() => ShowWindow(UnityHWnd, SW_SHOWMINIMIZED);

    //设置无边框，并设置框体大小，位置
    public static void SetNoFrameWindow(Rect rect)
    {
        SetWindowLong(UnityHWnd, GWL_STYLE, WS_POPUP);
        bool result = SetWindowPos(UnityHWnd, 0, (int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height, SWP_SHOWWINDOW);
    }

    //拖动窗口
    public static void DragWindow(IntPtr window)
    {
        ReleaseCapture();
        SendMessage(window, 0xA1, 0x02, 0);
        SendMessage(window, 0x0202, 0, 0);
    }

    public static IntPtr GetUnityWindow()
    {
        var unityHWnd = IntPtr.Zero;
        EnumThreadWindows(GetCurrentThreadId(), (hWnd, lParam) =>
        {
            var classText = new StringBuilder(UNITY_WND_CLASSNAME.Length + 1);
            GetClassName(hWnd, classText, classText.Capacity);

            if (classText.ToString() == UNITY_WND_CLASSNAME)
            {
                unityHWnd = hWnd;
                return false;
            }
            return true;
        }, IntPtr.Zero);
        return unityHWnd;
    }
    #endregion
}

