using System.Collections.Generic;
using UnityEngine;
using zFrame.UI;
using System.Linq;
using zFrame.UI.Components;

namespace zFrame.Example
{
    public struct PassengerInfo:ISearchable
    {
        public string Name;
        public string Gender;
        public string id;
        public string SearchContext => $"{id} {Name} {Gender}";
    }

    /// <summary>
    /// ReusableScrollViewController （RSVC）的数据载体演示脚本 
    /// 可以是你的任意控制器啥的，控制反转按需配置自己的个性化 cell Item 
    /// </summary>
    public class SearchPanelDemo : MonoBehaviour, ISearchDataSource
    {
        [SerializeField]
        private int _dataLength;
        [SerializeField]
        SearchPanel searchPanel; //在全工程只有一个SearchPanel的情况下可以使用全局变量持有该搜索面板

        private List<PassengerInfo> _contactList = new List<PassengerInfo>();

        /// <summary>
        /// 将数据进行类型转换获得支持被SearchPanel搜索的数据
        /// </summary>
        public List<ISearchable> SearchableSourceData =>_contactList.Cast<ISearchable>().ToList() ;

        private void Awake() //数据初始化务必放在 低于Awake 启动的Mono方法中
        {
            InitData();
        }
        private void Start()
        {
            searchPanel.DataSource = this; 
        }

        private void InitData()
        {
            if (_contactList != null) _contactList.Clear();

            string[] genders = { "Male", "Female" };
            for (int i = 0; i < _dataLength; i++)
            {
                PassengerInfo obj = new PassengerInfo();
                obj.Name = i + "_Name";
                obj.Gender = genders[Random.Range(0, 2)];
                obj.id = "item : " + i;
                _contactList.Add(obj);
            }
        }

        private void ModifidCell(int index, string data)
        {
          var d=  _contactList[index];
            d.Name = data;
            _contactList[index] = d;
            searchPanel.DataSource = this; 
        }

        public void UpdateCell(BaseCell cell, ISearchable data)
        {
            var item = cell as SearchabelDemoCell;
            item.ConfigureCell((PassengerInfo)data);
        }
    }
}
