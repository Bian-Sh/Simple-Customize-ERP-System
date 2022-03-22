using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace BenYuan.UI.Tips
{
    //在构建这个tips的方方面面的时候，就把这个预制体与脚本关联，以后调用就不用到处找预制体了
    [Path(prefab = @"Assets/RuntimePlugins/UI/StickedTips/Resources/StickedTips/NoneContentStickedTips.prefab")]
    public class NoneContentStickedTips : BaseTips
    {
        [SerializeField] private Image statusIcon;
        [SerializeField] private Data dataForTest;
        [HideInInspector]public string flag;

        public override void Start()
        {
            base.Start();
        }

        public override void UpdateContent(object data)
        {
            Data dat = data as Data;
            if (null != dat)
            {
                UpdateInfo(dat);
            }
        }

        [EditorButton]
        private void Tips赋值测试()
        {
            UpdateInfo(dataForTest);
        }


        private void UpdateInfo(Data data)
        {
            this.m_Title.text = data.title;
            if (data.titleIcon)
            {
                this.m_Icon.sprite = data.titleIcon;
            }
            this.m_Icon.SetNativeSize();
            if (null != data.statusIcon)
            {
                this.statusIcon.gameObject.SetActive(true);
                this.statusIcon.sprite = data.statusIcon;
                this.statusIcon.SetNativeSize();
            }
            else
            {
                this.statusIcon.gameObject.SetActive(false);
            }
            foreach (var item in GetComponentsInChildren<ContentSizeFitter>())
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)item.transform);
            }
        }

        [System.Serializable]
        public class Data
        {
            public string title;
            public Sprite titleIcon;
            public Sprite statusIcon;
        }

    }

}
