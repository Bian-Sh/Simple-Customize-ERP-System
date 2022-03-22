using UnityEngine;
using UnityEngine.UI;
using zFrame.UI;
namespace zFrame.Example
{
    public class SearchabelDemoCell : BaseCell
    {
        //UI
        public Text nameLabel;
        public Text genderLabel;
        public Text idLabel;
        public Button ply;

        //Model
        private PassengerInfo _contactInfo;


        protected override void Start()
        {
            base.Start();
            //演示对内的事件注册, 
            //对外事件的话，有2种途径：事件总线 、Inspector面板挂数据
            // Inspector 面板挂数据的话，ReusableScrollViewController.cellTemplate 就要放预先在场景中
            GetComponent<Button>().onClick.AddListener(ButtonListener);
        }

        //必须有这个方法或者类似于这样的方法，由继承自IReusableScrollViewController 的脚本驱动以刷新cell
        public void ConfigureCell(PassengerInfo contactInfo)
        {
            _contactInfo = contactInfo;
            nameLabel.text = contactInfo.Name;
            genderLabel.text = contactInfo.Gender;
            idLabel.text = contactInfo.id;
        }


        private void ButtonListener()
        {
            Debug.Log("Index : " + dataIndex + ", Name : " + _contactInfo.Name + ", Gender : " + _contactInfo.Gender);
        }

        public enum MonitorSearchCellEvent
        {
            Play,
            Pined,
        }
    }
}
