using Malee.List;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using zFrame.Events;

namespace zFrame.UI
{
    [CreateAssetMenu(fileName = "ContextItemConfigration", menuName = "Configration/ContextMenuConfig")]
    public class ContextItemConfigration : ScriptableObject
    {
        [Reorderable]
        public ContextItemConfigList configList;
        [System.Serializable]
        public class ContextItemConfigList : ReorderableArray<ContextItemConfig> { }
        [System.Serializable]
        public class ContextItemConfig
        {
            public string model;
            [Reorderable]
            public ContextItemDataList items;
        }

        [System.Serializable]
        public class ContextItemDataList : ReorderableArray<ContextItemData> { }
        [System.Serializable]
        public class ContextItemData
        {
            public string label;
            public Sprite icon;
            public string command; //控制面板启动指令
        }
    }
}
