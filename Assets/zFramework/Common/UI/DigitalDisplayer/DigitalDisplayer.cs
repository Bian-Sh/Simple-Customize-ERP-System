using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace zFrame.UI.Components
{
    // 因为涉及到了游戏对象的销毁，只能运行时测试
    [ExecuteInEditMode]
    public class DigitalDisplayer : MonoBehaviour
    {
        [Header("0-9 数值的精灵")]
        public Sprite[] icons;
        #region Private Fields
        private GameObject template; //模板
        private Text label;
        private Text unit;
        private string current; //缓存的数值
        #endregion
        #region Public Property
        /// <summary>
        /// 标签
        /// </summary>
        public string Label
        {
            set
            {
                if (label.text != value)
                {
                    label.text = value;
                }
            }
        }
        /// <summary>
        ///单位
        /// </summary>
        public string Unit
        {
            set
            {
                if (unit.text != value)
                {
                    unit.text = value;
                }
            }
        }
        /// <summary>
        /// 数值
        /// </summary>
        public string Value
        {
            set
            {
                if (current != value)
                {
                    current = value;
                    AssignComponentDate(value);
                }
            }
        }
        #endregion
        private void Awake()
        {
            #region InitComponent
            template = transform.Find("Label/Template").gameObject;
            label = transform.Find("Label").GetComponent<Text>();
            unit = transform.Find("Label/Count/Unit").GetComponent<Text>();
            #endregion
        }

        [EditorButton]
        private void Test(string label = "Test .. ",string data = "55254",string unit = "km/h")
        {
            this.Label = label;
            this.Unit = unit;
            this.Value = data;
        }

        [EditorButton]
        private void Test(string data = "55254")
        {
            this.Value = data;
        }

        private void AssignComponentDate(string data)
        {
            Transform root = transform.Find("Label/Count");
            List<Transform> childs = new List<Transform>();

            foreach (Transform item in root)
            {
                childs.Add(item);
            }
            foreach (Transform item in childs)
            {
                if (item.name != "Unit")
                {
                    if (Application.isPlaying)
                    {
                        GameObject.Destroy(item.gameObject);
                    }
                    else
                    {
                        GameObject.DestroyImmediate(item.gameObject);
                    }
                }
            }

            var datastr = data.ToString();
            for (int i = 0; i < datastr.Length; i++)
            {
                int pos = 0;
                //Debug.Log(datastr[i]);
                if(datastr[i].Equals('.'))
                {
                    pos = icons.Length - 1;
                }
                else
                {
                    pos = int.Parse(datastr[i].ToString());
                }
                SetNumber(pos, root);
            }
        }

        private void SetNumber(int pos, Transform root)
        {
            GameObject go = Instantiate(template);
            go.SetActive(true);
            go.transform.GetChild(0).GetComponent<Image>().sprite = icons[pos];
            go.transform.SetParent(root, false);
        }
    }
}
