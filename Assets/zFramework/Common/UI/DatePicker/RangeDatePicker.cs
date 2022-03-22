using Malee.List;
using SpringGUI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RangeDatePicker : MonoBehaviour
{
    #region   Components
    [SerializeField] Toggle startTimeButton; //选择开始时间的按钮
    [SerializeField] Toggle endTimeButton; //选择结束时间的按钮
    [SerializeField] GameObject dataPicker; //时间选择器
    [SerializeField] Transform toggleContainer; //用于自定义时间选区的toggle
    [SerializeField, Reorderable, Header("配置预设时间选区")] PresetArray presets = new PresetArray(); //用于配置预设时间选区
    private Toggle activedToggle;//当前激活的时间展示窗toggle
    private Text starttimeText;
    private Text endtimeText;
    private Calendar calendar;
    #endregion

    #region Datas
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    private DateTime cachedStart;
    private DateTime cachedEnd;
    private bool isDirty = false;
    /// <summary>
    /// 前面的参数是开始时间，后面的是结束时间
    /// </summary>
    [SerializeField] public RangeDatePickerEvent OnDateTimeChanged = new RangeDatePickerEvent();
    #endregion

    void Awake()
    {
        // 初始化时间
        EndTime = StartTime=DateTime.Today;
        foreach (var item in presets)
        {
            if (item.toggle.isOn)
            {
                StartTime=StartTime.AddDays(item.duration * -1);
                break;
            }
        }
        ComponentInitAndEventRegister();
    }


    #region 以下代码方便测试本组件，测试完毕请注释掉
    //private void Start()
    //{
    //    OnDateTimeChanged.AddListener(TestThisEvent);
    //}
    //private void TestThisEvent(DateTime arg0, DateTime arg1)
    //{
    //    Debug.Log("用户选择开始时间为："+ arg0.ToShortDateString());
    //    Debug.Log("用户选择结束时间为："+ arg1.ToShortDateString());
    //}

    //private void OnGUI()
    //{
    //    if (GUILayout.Button("主动获取时间"))
    //    {
    //        Debug.Log("当前开始时间为：" + StartTime.ToShortDateString());
    //        Debug.Log("当前结束时间为：" + EndTime.ToShortDateString());
    //    }
    //}
    #endregion



    /// <summary>
    /// 初始化组件及其组件内事件
    /// </summary>
    private void ComponentInitAndEventRegister()
    {
        starttimeText = startTimeButton.GetComponentInChildren<Text>();
        endtimeText = endTimeButton.GetComponentInChildren<Text>();
        starttimeText.text = StartTime.ToString("yyyy年MM月dd日");
        endtimeText.text = EndTime.ToString("yyyy年MM月dd日");

        startTimeButton.onValueChanged.AddListener(v => OnDataMonitorTriggered(startTimeButton));
        endTimeButton.onValueChanged.AddListener(v => OnDataMonitorTriggered(endTimeButton));

        calendar = dataPicker.GetComponentInChildren<Calendar>();
        calendar.onDayClick.AddListener(OnCalendarTimeSelected);
        dataPicker.transform.Find("CancelButton").GetComponent<Button>().onClick.AddListener(OnDatePickerCanceled);
        dataPicker.transform.Find("ConfirmButton").GetComponent<Button>().onClick.AddListener(OnDatePickerConfirmed);
        dataPicker.SetActive(false);

        foreach (Toggle item in toggleContainer.GetComponentsInChildren<Toggle>())
        {
            item.onValueChanged.AddListener(v =>
            {
                if (v) OnDataPresetToggleTriggered(item);
            });
        }
    }


    /// <summary>
    /// 当日历选择了时间时
    /// </summary>
    /// <param name="arg0">选择的时间</param>
    private void OnCalendarTimeSelected(DateTime arg0)
    {
        if (activedToggle == startTimeButton && StartTime != arg0)
        {
            cachedStart = arg0;
            isDirty = true;
        }
        else if (activedToggle == endTimeButton && EndTime != arg0)
        {
            cachedEnd = arg0;
            isDirty = true;
        }
    }

    /// <summary>
    /// 当日期展示窗被点击时
    /// </summary>
    /// <param name="toggle"></param>
    private void OnDataMonitorTriggered(Toggle toggle)
    {
        dataPicker.SetActive(toggle.isOn);
        if (toggle.isOn)
        {
            activedToggle = toggle;
            cachedStart = StartTime;
            cachedEnd = EndTime;
            Vector3 pos = dataPicker.transform.position;
            pos.x = activedToggle.transform.position.x;
            dataPicker.transform.position = pos;
        }
    }

    /// <summary>
    /// 当时间选择器点击了确认按钮
    /// </summary>
    private void OnDatePickerConfirmed()
    {
        activedToggle.isOn = false;
        activedToggle = null;
        if (isDirty)
        {
            isDirty = false;
            StartTime = cachedStart;
            EndTime = cachedEnd;
            ComponentPostUpdate();
        }
    }

    /// <summary>
    /// 当时间选择器点击了取消按钮
    /// </summary>
    private void OnDatePickerCanceled()
    {
        activedToggle.isOn = false;
        activedToggle = null;
        isDirty = false;
    }

    /// <summary>
    /// 当预设时间的toggle 被点击时
    /// </summary>
    /// <param name="item"></param>
    private void OnDataPresetToggleTriggered(Toggle item)
    {
        startTimeButton.interactable = (item.name == "Custom") ? true : false;
        endTimeButton.interactable = (item.name == "Custom") ? true : false;
        if (item.name != "Custom")
        {
            if(null!=activedToggle)activedToggle.isOn = false;
            PresetInfo preset = presets.Find(v => v.toggle == item);
            StartTime = DateTime.Today.AddDays(preset.duration * -1);
            EndTime = DateTime.Today;
            ComponentPostUpdate();
        }
    }

    /// <summary>
    /// RangeDatePicker 后期处理
    /// </summary>
    private void ComponentPostUpdate()
    {
        if (StartTime>DateTime.Today )
        {
            StartTime = DateTime.Today;
        }
        if (EndTime>DateTime.Today)
        {
            EndTime = DateTime.Today;
        }
        if (StartTime>EndTime)
        {
            var temp = StartTime;
            StartTime = EndTime;
            EndTime = temp;
        }
        starttimeText.text = StartTime.ToString("yyyy年MM月dd日");
        endtimeText.text = EndTime.ToString("yyyy年MM月dd日");
        OnDateTimeChanged?.Invoke(StartTime, EndTime);
    }

#if UNITY_EDITOR
    /// <summary>
    /// 根据用户配置自动创建UI界面
    /// </summary>
    [EditorButton]
    private void UpdatePresets()
    {
        Transform template = toggleContainer.Find("Template"); //获取模板

        List<Transform> childs = new List<Transform>();
        foreach (Transform item in toggleContainer) //清空除了自定义和模板以外的所有多余子节点
        {
            if (item.name != "Custom" && item.name != "Template")
            {
                childs.Add(item);
            }
        }
        foreach (var item in childs)
        {
            GameObject.DestroyImmediate(item.gameObject);
        }
        childs.Clear();
        foreach (var item in presets) //根据配置创建预设UI
        {
            GameObject go = GameObject.Instantiate(template.gameObject);
            Toggle toggle = go.GetComponent<Toggle>();
            ToggleGroup toggleGroup = toggleContainer.GetComponent<ToggleGroup>();
            toggle.group = toggleGroup;
            toggleGroup.RegisterToggle(toggle);
            //多个预设信息都勾选了 isDefault 那么只有最后一个生效
            toggle.isOn = item.isDefault;
            toggle.group.NotifyToggleOn(toggle);
            item.toggle = toggle;  //存应用便于查询
            go.GetComponentInChildren<Text>().text = item.label;
            go.name = "Preset";
            go.transform.SetParent(toggleContainer, false);
            go.SetActive(true);
        }
        ContentSizeFitter sizeFitter = toggleContainer.gameObject.AddComponent<ContentSizeFitter>(); //借助SizeFitter自动更新容器尺寸
        sizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
        Canvas.ForceUpdateCanvases();
        GameObject.DestroyImmediate(sizeFitter); //销毁辅助组件
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif


    #region Structs
    [Serializable] public class RangeDatePickerEvent : UnityEvent<DateTime, DateTime> { };
    [Serializable]
    internal class PresetArray : ReorderableArray<PresetInfo>
    {
    }

    /// <summary>
    /// 预设时间段
    /// </summary>
    [Serializable]
    internal class PresetInfo
    {
        /// <summary>
        /// 预设名称，显示UI上
        /// </summary>
        public string label;
        /// <summary>
        /// 时间跨度
        /// </summary>
        public int duration;
        /// <summary>
        /// 初始化时应该是被选中的
        /// </summary>
        public bool isDefault;

        /// <summary>
        /// 预设对应的触发组件
        /// </summary>
        [Tooltip("请点击下方按钮更新")]
        public Toggle toggle;
    }
    #endregion
}
