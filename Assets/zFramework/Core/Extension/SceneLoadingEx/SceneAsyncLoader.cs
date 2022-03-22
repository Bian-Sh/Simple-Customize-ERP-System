using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using zFrame.Time;

/// <summary>
/// 场景异步加载器
/// </summary>
public static class SceneAsyncLoader
{
    static Coroutine coroutine = null;
    static string _sceneName = string.Empty;
    static WaitUntil waitUntil = null;
    // 当系统告知场景加载完毕之时即是 CoroutineDriver 销毁之日。
    private static void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        SceneManager.activeSceneChanged -= SceneManager_activeSceneChanged;
        CoroutineDriver.Instance.StartCoroutine(DelayResetData());
    }



    /// <summary> 异步操作 </summary>
    public static AsyncOperation AsyncOperation { get; private set; } = null;

    /// <summary> 是否在加载成功后立马激活 </summary>
    public static bool AllowSceneActivation
    {
        set
        {
            if (null != AsyncOperation)
            {
                AsyncOperation.allowSceneActivation = value;
            }
        }
    }
    /// <summary>
    /// 配置中断达成条件为异步操作进度等于1
    /// </summary>
    public static CustomYieldInstruction Yield
    {
        get
        {
            return waitUntil;
        }
    }

    public static void LoadAsync(string sceneName)
    {
        if (null == coroutine)
        {
            SceneManager.activeSceneChanged += SceneManager_activeSceneChanged; ;
            _sceneName = sceneName;
            coroutine = CoroutineDriver.Instance.StartCoroutine(DelayLoadScene());
        }
    }

    static IEnumerator DelayResetData()
    {
        yield return new WaitForSeconds(1);
        coroutine = null;
        _sceneName = string.Empty;
        AsyncOperation = null;
        waitUntil = null;
        if (null != CoroutineDriver.Instance)
        {
            GameObject.Destroy(CoroutineDriver.Instance.gameObject);
        }
        Debug.Log("场景加载完毕，回收数据完成！");

    }
    static IEnumerator DelayLoadScene()
    {
        yield return new WaitForEndOfFrame(); //为了避开程序初始化高峰期，延迟到渲染帧完成后开启异步加载
        AsyncOperation = SceneManager.LoadSceneAsync(_sceneName);
        waitUntil = new WaitUntil(() => AsyncOperation.isDone);
        AsyncOperation.allowSceneActivation = false;
    }

    class CoroutineDriver : MonoSingleton<CoroutineDriver> { }
}
