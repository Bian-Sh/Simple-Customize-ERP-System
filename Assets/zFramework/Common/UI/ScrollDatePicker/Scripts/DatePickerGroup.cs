using System;
using System.Collections.Generic;
using UnityEngine;

namespace ChoiceTime
{
    /// <summary>
    /// 日期选择组
    /// </summary>
    public class DatePickerGroup : MonoBehaviour
    {
        /// <summary>
        /// 最小日期和最大日期
        /// </summary>
        public DateTime _minDate = DateTime.Today.AddYears(-20);
        public DateTime _maxDate = DateTime.Today.AddYears(30);
        /// <summary>
        /// 选择的日期（年月日时分秒）
        /// </summary>
        public DateTime _selectDate;
        /// <summary>
        /// 时间选择器列表
        /// </summary>
        public List<DatePickerDrop> _datePickerList;
        bool isInit = false;
        void Awake()
        {
            _selectDate = DateTime.Now;
            Init(_selectDate);
        }
        public void Init(DateTime dt)
        {
            _selectDate = dt;
            if (isInit) return;
            isInit = true;
            for (int i = 0; i < _datePickerList.Count; i++)
            {
                _datePickerList[i].myGroup = this;
                _datePickerList[i].Init();
                _datePickerList[i]._onDateUpdate = onDateUpdate;
            }
        }

        public void UpdateDate(DateTime value)
        {
            Init(value);
            onDateUpdate();
        }
        /// <summary>
        /// 当选择的日期更新
        /// </summary>
        public void onDateUpdate()
        {
            for (int i = 0; i < _datePickerList.Count; i++)
            {
                _datePickerList[i].RefreshDateList();
            }
        }
    }
}



