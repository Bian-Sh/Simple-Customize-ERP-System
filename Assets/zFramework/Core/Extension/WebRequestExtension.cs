using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
// 代码参考了万科 UnityWebRequestConfig.cs 
//https://www.jianshu.com/p/15a537496131  Get带参数 使用 querystring 概念 URL 的最大长度是 2048 个字符
namespace zFrame.Extension
{
    public static class WebRequestExtension
    {
        public static bool IsError(this UnityWebRequest unityWebRequest)
        {
#if UNITY_2020_2_OR_NEWER
            var result = unityWebRequest.result;
            return (result == UnityWebRequest.Result.ConnectionError)
                || (result == UnityWebRequest.Result.DataProcessingError)
                || (result == UnityWebRequest.Result.ProtocolError);
#else
            return unityWebRequest.isHttpError || unityWebRequest.isNetworkError;
#endif
        }

        /// <summary>
        /// 下载 json 数据
        /// </summary>
        /// <returns></returns>
        public static async UniTask<string> DownLoadTextDataAsync(string url)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(url))
            {
                try
                {
                    await www.SendWebRequest();
                }
                catch (Exception e)
                {
                    Debug.Log($"{nameof(WebRequestExtension)}: 网络请求时发生错误，请确认 ↓ \n{e}");
                }
                return www.downloadHandler.text;
            }
        }
        public static async UniTask<T> LoadDataAsync<T>(string url)
        {
            if (string.IsNullOrEmpty(url)) return default;
            var json = await DownLoadTextDataAsync(url);
            T data = default;
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError($"{nameof(WebRequestExtension)}: 返回 json 数据为空字符串，请重试！");
            }
            else
            {
                try
                {
                    data = JsonUtility.FromJson<T>(json);
                }
                catch (Exception e)
                {
                    Debug.LogError($"{nameof(WebRequestExtension)}: json 格式与 数据模型 {typeof(T)} 不匹配！\n{e}\n{json}");
                }
            }
            return data;
        }

        /// <summary>
        /// 通过PUT方式将字节流传到服务器
        /// </summary>
        /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
        /// <param name="contentBytes">需要上传的字节流</param>
        /// <param name="timeout">设置超时</param>
        /// <param name="resultAction">设置header文件中的Content-Type属性</param>
        /// <returns>返回是否成功完成上传动作</returns>
        public static async UniTask<bool> UploadBytesAsync(string url, byte[] contentBytes, int timeout = 180, string contentType = "application/octet-stream")
        {
            using (UnityWebRequest www = new UnityWebRequest())
            {
                www.timeout = timeout;
                www.disposeUploadHandlerOnDispose = true;
                UploadHandler uploader = new UploadHandlerRaw(contentBytes);
                uploader.contentType = contentType; // Sends header: "Content-Type: custom/content-type";
                www.uploadHandler = uploader;
                try
                {
                    await www.SendWebRequest();
                    return true;
                }
                catch (Exception e)
                {
                    Debug.LogError($"{nameof(WebRequestExtension)}: {nameof(UploadBytesAsync)} {e}");
                    return false;
                }
            };
        }

        /// <summary>
        /// 带请求头 header 和 表单 WWWForm 的请求方式
        /// </summary>
        /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
        /// <param name="form">提交的表单</param>
        /// <param name="header">自定义请求头</param>
        /// <param name="timeout">设置超时</param>
        /// <returns>返回是否成功完成上传动作</returns>
        public static async UniTask<string> PostAsync(string url, WWWForm form, Dictionary<string, string> header, int timeout = 180)
        {
            using (UnityWebRequest www = UnityWebRequest.Post(url, form))
            {
                www.timeout = timeout;
                if (null != header)
                {
                    foreach (var item in header.Keys)
                    {
                        www.SetRequestHeader(item, header[item]);
                    }
                }
                try
                {
                    await www.SendWebRequest();
                    return www.downloadHandler.text;
                }
                catch (Exception e)
                {
                    Debug.LogError($"{nameof(WebRequestExtension)}: {nameof(PostAsync)} {e}");
                    return string.Empty;
                }
            };
        }
        /// <summary>
        /// 带请求头header json/object 的请求
        /// </summary>
        /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
        /// <param name="target">可序列化的实例</param>
        /// <param name="header">自定义请求头</param>
        /// <param name="timeout">设置超时</param>
        /// <returns>服务器返回数据</returns>
        public static async UniTask<string> PostAsync(string url, object target, Dictionary<string, string> header, int timeout = 180)
        {
            string jsonParam = JsonUtility.ToJson(target);
            Debug.Log($"确认提交对象 {target.GetType()} : {jsonParam}");
            byte[] body = System.Text.Encoding.UTF8.GetBytes(jsonParam);
            using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
            {
                www.disposeUploadHandlerOnDispose = true;
                www.timeout = timeout;
                www.uploadHandler = new UploadHandlerRaw(body);
                if (header != null)
                {
                    foreach (var item in header.Keys)
                    {
                        www.SetRequestHeader(item, header[item]);
                    }
                }
                www.SetRequestHeader("Content-Type", "application/json;charset=utf-8");

                try
                {
                    await www.SendWebRequest();
                    return www.downloadHandler.text;
                }
                catch (Exception e)
                {
                    Debug.LogError($"{nameof(WebRequestExtension)}: {nameof(PostAsync)} {e}");
                    return string.Empty;
                }
            };
        }
        /// <summary>
        /// 序列化 MuitlpartFormData
        /// </summary>
        /// <param name="multipartFormSections"></param>
        /// <param name="boundary"></param>
        /// <returns></returns>
        public static byte[] SerializeFormSections(List<IMultipartFormSection> multipartFormSections)
        {
            if (multipartFormSections == null || multipartFormSections.Count == 0)
            {
                return null;
            }
            string header = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n";
            byte[] crlf = Encoding.UTF8.GetBytes("\r\n");
            byte[] bound = Encoding.ASCII.GetBytes("--");
            byte[] boundary = UnityWebRequest.GenerateBoundary();
            int num = crlf.Length * 2 + bound.Length * 2;
            var reserved = crlf.Length * 3 + bound.Length * 1 + Encoding.UTF8.GetBytes(string.Format(header, "", "", "")).Length;
            foreach (IMultipartFormSection multipartFormSection in multipartFormSections)
            {
                num += reserved + multipartFormSection.sectionData.Length;
            }
            List<byte> list = new List<byte>(num);
            for (int i = 0; i < multipartFormSections.Count; i++)
            {
                var fm = multipartFormSections[i];
                if (i != 0) list.AddRange(crlf);
                list.AddRange(bound);
                list.AddRange(boundary);
                list.AddRange(crlf);
                list.AddRange(Encoding.UTF8.GetBytes(string.Format(header, fm.sectionName, fm.fileName, fm.contentType)));
                list.AddRange(crlf);
                list.AddRange(fm.sectionData);
            }
            list.AddRange(crlf);
            list.AddRange(bound);
            list.AddRange(boundary);
            list.AddRange(bound);
            list.AddRange(crlf);
            return list.ToArray();
        }
    }
}
