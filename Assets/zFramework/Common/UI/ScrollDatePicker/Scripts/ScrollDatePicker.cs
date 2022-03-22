using ChoiceTime;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ScrollDatePicker : MonoBehaviour
{
    public Button button;
    public Text timetext;
    public DatePickerGroup group;
    public Image arrow;
    public UIBlocker uiBlocker;
    public ScrollDatePickerEvent OnDatetimeSelected = new ScrollDatePickerEvent();
   [Serializable] public class ScrollDatePickerEvent : UnityEvent<DateTime> { }
    public DateTime DateTime
    {
        get => group._selectDate;
        set
        {
            timetext.text = value.ToString("yyyy - MM - dd");
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
        timetext.text =group._selectDate.ToString("yyyy - MM - dd");
        arrow.transform.localEulerAngles = new Vector3(0, 0, 90);
        group.gameObject.SetActive(false);
    }

    private void OnPickerContainerShow()
    {
        arrow.transform.localEulerAngles = Vector3.zero;
        group.gameObject.SetActive(true);
    }

    private void OnPickerContainerHide()
    {
        timetext.text = group._selectDate.ToString("yyyy - MM - dd");
        OnDatetimeSelected.Invoke(group._selectDate);
        arrow.transform.localEulerAngles = new Vector3(0, 0, 90);
        uiBlocker.gameObject.SetActive(false);
    }

}
