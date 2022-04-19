using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Calendar : MonoBehaviour {
    //public Language language;
    public DateTime currentDate;
    public DailyGrid[] dailyGrids = new DailyGrid[42];
    public Text yearText;
    public Text monthText;
    public Text dayText;
    public int days;
    public DayOfWeek firstWeekOfMonth;
    public static int[] months = new int[12];    
    public Sprite keepSign;

    void Start() {
        dailyGrids = transform.Find("DailyButtons").GetComponentsInChildren<DailyGrid>();
        currentDate = DateTime.Today;
        SetDate(currentDate);        
    }

    public void GoToday() {
        currentDate = DateTime.Today;
        SetDate(currentDate);
    }

    void SetDate(DateTime dateTime) {
        days = DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
        firstWeekOfMonth = new DateTime(dateTime.Year, dateTime.Month, 1).DayOfWeek;
        SetDateText();
        OnMonthChanged(dateTime);
    }

    void SetDateText() {
        yearText.text = currentDate.Year.ToString("0000");
        monthText.text = currentDate.Month.ToString("00");
        dayText.text = currentDate.Day.ToString("00");
        int daysBrforeMonth = firstWeekOfMonth == 0 ? 7 : (int)firstWeekOfMonth;
        for (int i = 0; i < 42; i++) {
            DateTime date = currentDate.AddDays(i + 1 - daysBrforeMonth - currentDate.Day);
            dailyGrids[i].info = new DateInfo(date, dailyGrids[i].info.Button, dailyGrids[i].info.Text);
            dailyGrids[i].info.Button.interactable = date.Month == currentDate.Month;
        }
    }

    public void SetToNextMonth() {
        currentDate = new DateTime(currentDate.AddMonths(1).Year, currentDate.AddMonths(1).Month, currentDate.AddMonths(1).Day);
        SetDate(currentDate);
    }

    public void SetToLastMonth() {
        currentDate = new DateTime(currentDate.AddMonths(-1).Year, currentDate.AddMonths(-1).Month, currentDate.AddMonths(-1).Day);
        SetDate(currentDate);
    }

    public void OnMonthChanged(DateTime dateTime) {
        Event.OnMonthChanged(this, new CalendarArgs(dateTime));    
    }

    public void SetCurrentDate(DateTime dateTime) {
        currentDate = dateTime;
        SetDate(currentDate);
    }
}
