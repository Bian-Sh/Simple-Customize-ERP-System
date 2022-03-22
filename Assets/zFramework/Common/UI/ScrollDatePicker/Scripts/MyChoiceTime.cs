using ChoiceTime;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class MyChoiceTime : MonoBehaviour
{
    public Button button;
    public Text timetext;
    public DatePickerGroup group;
    public Image arrow;
    public UIBlocker uiBlocker;
    [Space(10)]
    public ScrollDatePickerEvent OnDatetimeSelected = new ScrollDatePickerEvent();
    [Serializable] public class ScrollDatePickerEvent : UnityEvent<DateTime> { }
    public DateTime DateTime
    {
        get => group._selectDate;
        set
        {
            timetext.text = value.ToString("yyyy年MM月dd日 HH : mm : ss");
            group.UpdateDate(value);
        }
    }
    private void Awake()
    {
        uiBlocker.OnDisBlocked.AddListener(OnPickerContainerHide);
        button.onClick.AddListener(OnPickerContainerShow);
    }

    void Start()
    {
        timetext.text = group._selectDate.ToString("yyyy年MM月dd日 HH : mm : ss");        
        group.gameObject.SetActive(false);
    }

    private void OnPickerContainerShow()
    {
        arrow.transform.localEulerAngles = new Vector3(0, 0, 180);
        group.gameObject.SetActive(true);
    }

    private void OnPickerContainerHide()
    {
        timetext.text = group._selectDate.ToString("yyyy年MM月dd日 HH : mm : ss");
        OnDatetimeSelected.Invoke(group._selectDate);
        arrow.transform.localEulerAngles = Vector3.zero;
        uiBlocker.gameObject.SetActive(false);

        string str = "2021-06-21 18:00:00";
        DateTime historyTime = Convert.ToDateTime(str);
        DateTime addTime = DateTime.Now.AddMinutes(5);
        Debug.Log(addTime);
        int compNum = DateTime.Compare(group._selectDate, addTime);
        Debug.Log(compNum);
        if (compNum > 0)
        {
            Debug.Log("你输入的时间大于五分钟");
        }
        else
        {
            Debug.Log("你输入的时间小于等于五分钟");
        }
    }

}
