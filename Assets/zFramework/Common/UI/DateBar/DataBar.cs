using System;
using UnityEngine;
using UnityEngine.UI;
public class DataBar : MonoBehaviour
{
    public Text Date;
    public Text Week;
    public Text Time;
    void Update()
    {
        SetDateTime();
    }
    private void SetDateTime()
    {
        string week = string.Empty;
        switch ((int)DateTime.Now.DayOfWeek)
        {
            case 0:
                week = "星期日";
                break;
            case 1:
                week = "星期一";
                break;
            case 2:
                week = "星期二";
                break;
            case 3:
                week = "星期三";
                break;
            case 4:
                week = "星期四";
                break;
            case 5:
                week = "星期五";
                break;
            case 6:
                week = "星期六";
                break;
        }
        Week.text = week;
        Date.text = DateTime.Now.ToString("yyyy / MM / dd");
        Time.text = DateTime.Now.ToString("HH : mm : ss");
    }
}
