using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using zFrame.Extension;
using zFrame.Time;

namespace UnityFramework.Utils
{
    public class TextureDownLoader
    {
        private Dictionary<string, Texture2D> spriteDic = new Dictionary<string, Texture2D>();
        private Dictionary<string, Action<Texture2D>> taskDic = new Dictionary<string, Action<Texture2D>>();
        private string timerkey;

        public TextureDownLoader()
        {
            timerkey = this.GetHashCode().ToString();
        }

        /// <summary>
        /// 重置下载器的定时器，避免在请求方展示时意外回收
        /// </summary>
        public void ResetTimer()
        {
            Timer.DelTimer(timerkey);
        }
        public void DownLoadImage(string url, Action<Texture2D> callBack, Action OnError = null)
        {

            var key = ParseKey(url);
            if (spriteDic.TryGetValue(key, out Texture2D texture))
            {
                if (texture)
                {
                    callBack?.Invoke(texture);
                    return;
                }
            }
            if (taskDic.TryGetValue(key, out Action<Texture2D> task))
            {
                //啥也不说，直接覆盖并等待协程完成~
                taskDic[key] = callBack;
                return;
            }
            taskDic[key] = callBack;
            CoroutineDriver.RunTask(Load(url, callBack, OnError));
        }

        string ParseKey(string url)
        {
            var index = url.LastIndexOf("/") + 1;
            return url.Substring(index, url.Length - index);
        }

        private IEnumerator Load(string url, Action<Texture2D> callBack, Action OnError)
        {
            var key = ParseKey(url);
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
            yield return www.SendWebRequest();
            if (!www.IsError())
            {
                Texture2D texture = DownloadHandlerTexture.GetContent(www);
                spriteDic[key] = texture;
                callBack?.Invoke(texture);
            }
            else
            {
                string msg = null == www ? " WWW 实例化异常！" : www.error;
                Debug.LogError(msg + " : " + url);
                OnError?.Invoke();
            }
            taskDic.Remove(key);
            www.Dispose();
        }
        /// <summary>
        /// 清除图片缓存
        /// </summary>
        /// <param name="url"></param>
        public void ClearTextureCache()
        {
            // 要求全部销毁，则做个延迟，
            Timer.DelTimer(timerkey);
            Timer.AddTimer(60, timerkey)
                .OnCompleted(() =>
                {
                    foreach (var item in spriteDic)
                    {
                        if (item.Value)
                        {
                            UnityEngine.Object.DestroyImmediate(item.Value);
                        }
                    }
                    spriteDic.Clear();
                    taskDic.Clear();
                });
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        public void UpdateCachedTexture(string current, string newname, Texture2D scr)
        {
            if (spriteDic.ContainsKey(current))
            {
                if (spriteDic.TryGetValue(current, out Texture2D texture))
                {
                    if (texture)
                    {
                        UnityEngine.Object.DestroyImmediate(texture);
                        Resources.UnloadUnusedAssets();
                        GC.Collect();
                    }
                }
                spriteDic.Remove(current);
            }
            spriteDic.Add(newname, scr);
        }
        class CoroutineDriver : MonoSingleton<CoroutineDriver>
        {
            protected override void Awake()
            {
                base.Awake();
                hideFlags = HideFlags.HideAndDontSave;
            }
            public static void RunTask(IEnumerator routine)
            {
                Instance.StartCoroutine(routine);
            }
        }


    }
}
