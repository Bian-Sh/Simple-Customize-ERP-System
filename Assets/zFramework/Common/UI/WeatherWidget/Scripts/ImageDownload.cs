using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityFramework.Utils;

public class ImageDownload : MonoBehaviour
{


    void Start()
    {
        for (int i = 0; i < 34; i++)
        {
            int index = i;
            string pathFormat = $"zFrame/Common/UI/WeatherWidget/Resource/Test/{index}.png";
            string path = Path.Combine(Application.dataPath, pathFormat);
            string url = $"http://image.nmc.cn/static2/site/nmc/themes/basic/weather/white/day/{index}.png";
            StartCoroutine(Load(url, v =>
             {
                 Debug.Log($"下载 {index}.png");
                 SaveNativeFile(v,path);
                 if (index ==33)
                 {
                     Debug.Log("图片下载完成！");
#if UNITY_EDITOR
                     UnityEditor.AssetDatabase.Refresh();
#endif
                 }
             }));
        }
        
    }
    private IEnumerator Load(string url, Action<byte[]> action)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();
            if (www != null && string.IsNullOrEmpty(www.error))
            {
                byte[] data = www.downloadHandler.data;
                action.Invoke(data);
            }
            else
            {
                string msg = null == www ? " WWW 实例化异常！" : www.error;
                Debug.LogError(msg + " : " + url);
            }
        }
    }
    /// <summary>
    /// 在本地保存文件
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="path"></param>
    public void SaveNativeFile(byte[] bytes, string path)
    {
        FileStream fs = new FileStream(path, FileMode.Create);
        fs.Write(bytes, 0, bytes.Length);
        fs.Flush();
        fs.Close();
    }
}
