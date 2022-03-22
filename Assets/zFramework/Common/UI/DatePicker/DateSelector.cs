using SpringGUI;
using System;
using UnityEngine;
using UnityEngine.UI;

public class DateSelector : MonoBehaviour
{
    public Action<bool, DateTime> OnDateChange;
    public GameObject StartTime;
    public GameObject EndTime;
    public GameObject TimePicker;
    private Calendar calendar;
    private Button CancelButton;
    private Button ConfirmButton;
    private Text textStart;
    private Text textEnd;
  //  private bool hasSelected = false;
    // Start is called before the first frame update
    void Start()
    {
        if (textStart == null)
        {
            textStart = StartTime.transform.Find("TextStartDate").GetComponent<Text>();
            textStart.text = "";
        }
            
        if (textEnd == null)
        {
            textEnd = EndTime.transform.Find("TextEndDate").GetComponent<Text>();
            textEnd.text = "";
        }
        
        //StartTime.AddClick(() => {
        //    ShowCalender(true);
        //});

        //EndTime.AddClick(() => {
        //    ShowCalender(false);
        //});

        calendar = TimePicker.transform.Find("Calendar").GetComponent<Calendar>();
        calendar.onDayClick.AddListener((DateTime crtDate) => {
        //    hasSelected = true;
            selectedDate = crtDate;
        });

        CancelButton = TimePicker.transform.Find("CancelButton").GetComponent<Button>();
        ConfirmButton = TimePicker.transform.Find("ConfirmButton").GetComponent<Button>();

        //日历中取消确认按钮
        //CancelButton.AddClick(() => {
        //    TimePicker.SetActive(false);
        //});
        //ConfirmButton.AddClick(() => {
        //    TimePicker.SetActive(false);
        //    if (!hasSelected)
        //        return;

        //    if (isStartCalendar)
        //    {
        //        startDate = selectedDate;
        //        OnDateChange?.Invoke(true, startDate);
        //        textStart.text = startDate.ToLongDateString();
        //    }
        //    else
        //    {
        //        endDate = selectedDate;
        //        OnDateChange?.Invoke(false, endDate);
        //        textEnd.text = endDate.ToLongDateString();
        //    }
        //});
    }

    public void Reset()
    {
        if(null != textStart)
        textStart.text = "";
        if(null != textEnd)
        textEnd.text = "";
    }

    private DateTime startDate;
    private DateTime endDate;
    private DateTime selectedDate;
    private bool isStartCalendar = true;
    
    private void ShowCalender(bool isStart)
    {
        isStartCalendar = isStart;

        if (TimePicker.activeSelf)
        {
            TimePicker.SetActive(false);
        }

        Vector2 position = (TimePicker.transform as RectTransform).position;
        position.x = isStart ? StartTime.transform.position.x : EndTime.transform.position.x;
        (TimePicker.transform as RectTransform).position = position;
        TimePicker.SetActive(true);
    }

    private void SetTime()
    {
        if(textStart == null)
            textStart = StartTime.transform.Find("TextStartDate").GetComponent<Text>();
        if(textEnd == null)
            textEnd = EndTime.transform.Find("TextEndDate").GetComponent<Text>();
        textStart.text = startDate.ToLongDateString();
        textEnd.text = endDate.ToLongDateString();
    }

    /// <summary>
    /// 外部设置日期
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    public void SetDate(DateTime start, DateTime end)
    {
        startDate = start;
        endDate = end;
        SetTime();
        OnDateChange?.Invoke(true, startDate);
        OnDateChange?.Invoke(false, endDate);
    }

    private void OnDisable()
    {
        TimePicker.SetActive(false);
    }
}
