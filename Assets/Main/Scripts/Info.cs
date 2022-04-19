using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct DateInfo {
    public DateTime Date;
    public Button Button;
    public Text Text;
    public bool IsHoliday;

    public DateInfo(DateTime date, Button button, Text day) {
        Date = date;
        Button = button;
        Text = day;
        Text.text = date.Day.ToString();
        IsHoliday = !(date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Saturday);
    }
}
[System.Serializable]
public struct KeepInfo {
    public GameObject KeepBase;
    public DateTime DateTime;
    public int Type;
    public string Content;
    public string Money;
    public Button Edit;
    public bool IsEdit;

    public KeepInfo(GameObject keepBase, DateTime datetime, int type, string content, string money, Button edit, bool isEdit) {
        KeepBase = keepBase;
        DateTime = datetime;
        Type = type;
        Content = content;
        Money = money;
        Edit = edit;
        IsEdit = isEdit;
    }
}
