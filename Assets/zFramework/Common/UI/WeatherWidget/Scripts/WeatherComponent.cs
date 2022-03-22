using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace zFrame.UI.Components
{
    /// <summary>
    /// 天气组件
    /// </summary>
    public class WeatherComponent : MonoBehaviour
    {
        /* -----------怎么拿到 City Code----------
         * 只有拿到城市code 才能读取天气，怎么取这个城市 code？
         * Step1. 打开 http://www.nmc.cn/f/rest/province  获得省份直辖市 code ，形如（AXG） 这是香港的
         * Step2. 拼接拿到的 省份直辖市 code 并访问 ：http://www.nmc.cn/f/rest/province/AXG
         * Step3. 在获得的数据中查找你想要的城市名称和对应的code。
         */

        [SerializeField] string cityCode = "59493"; //城市代码 59493 - 深圳
        [SerializeField] Sprite[] sprites;
        Text temp, info;
        Image icon;
        Coroutine cor;
        ContentSizeFitter sizeFitter;
        private void Awake() //初始化组件
        {
            temp = transform.Find("Temp").GetComponent<Text>();
            info = transform.Find("Content").GetComponent<Text>();
            icon = transform.Find("Icon").GetComponent<Image>();
            sizeFitter = temp.GetComponent<ContentSizeFitter>();
            sizeFitter.enabled = false;
        }

        private void Start()
        {
            UpdateWeather();
        }

        public void UpdateWeather()
        {
            if (null != cor) StopCoroutine(cor);
            cor = StartCoroutine(ConnectWebside());
        }

        /// <summary>
        /// 下载 json 数据
        /// </summary>
        /// <returns></returns>
        IEnumerator ConnectWebside()
        {
            using (UnityWebRequest www = UnityWebRequest.Get($"http://www.nmc.cn/f/rest/real/{cityCode}"))
            {
                yield return www.SendWebRequest();
                if (www.error != null)
                {
                    Debug.Log(www.error);
                    yield break;
                }
                string json = www.downloadHandler.text;
                if (!string.IsNullOrEmpty(json))
                {
                    UpdateUIInterface(json);
                }
                else
                {
                    Debug.LogWarning("WeatherComponent ：json download failed !");
                }
            }
        }
        /// <summary>
        /// 刷新 UI 展示
        /// </summary>
        /// <param name="data"></param>
        private void UpdateUIInterface(string json)
        {

            Data data = JsonUtility.FromJson<Data>(json);
            if (null != data)
            {
                temp.text = data.weather.temperature.ToString("F1") + "℃";
                info.text = data.weather.info;
                int index = 0;
                try
                {
                    index = Convert.ToInt32(data.weather.img);
                    index = Mathf.Clamp(index, 0, 33);
                }
                catch (Exception)
                {
                    Debug.LogError("Icon 下标转换失败！" + data.weather.img);
                }
                icon.sprite = sprites[index];
                StartCoroutine("RefreshTextSize");
            }
            else
            {
                Debug.LogWarning("WeatherComponent ：json convert to object failed !");
            }
        }
        /// <summary>
        /// 刷新一下 Text 的 size
        /// </summary>
        /// <returns></returns>
        private IEnumerator RefreshTextSize()
        {
            sizeFitter.enabled = true;
            yield return new WaitForEndOfFrame();
            sizeFitter.enabled = false;
        }

        [System.Serializable]
        public class Data
        {
            public Station station;
            public string week;
            public string moon;
            public string jie_qi;
            public string publish_time;
            public Weather weather;
            public Wind wind;
            public Warn warn;
        }

        [System.Serializable]
        public class Station
        {
            public string id;
            public string url;
            public string code; //城市代码，深圳：59493
            public string city; //城市
            public string province; //省份
        }

        [System.Serializable]
        public class Weather //天气
        {
            public float temperature; //温度
            public float temperatureDiff; //温差
            public float airpressure; //大气压
            public float humidity; //湿度
            public float rain; //降雨量
            public int rcomfort; //舒适度
            public int icomfort;
            public string info; //天气 - 晴
            public string img; // Icon 下标
            public float feelst; //体感温度
        }

        [System.Serializable]
        public class Wind //风
        {
            public string direct; //风向
            public string power;//风力等级
            public string speed; //风速
        }

        [System.Serializable]
        public class Warn //预警
        {
            public string alert;
            public string pic;
            public string province;
            public string city;
            public string url;
            public string issuecontent;
            public string fmeans;
        }
    }
}
