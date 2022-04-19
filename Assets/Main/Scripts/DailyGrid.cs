using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class DailyGrid : MonoBehaviour {
    private Calendar calendar;
    private Button button;
    public Image buttonColor;
    public DateInfo info;

    protected void Reset() {
        Initial();
    }

    protected void OnValidate() {
        Initial();
    }

    protected void Initial() {
        button = GetComponent<Button>();
        calendar = FindObjectOfType<Calendar>();
        info.Text = GetComponentInChildren<Text>();
        info.Button = GetComponent<Button>();
        buttonColor = GetComponent<Image>();
    }

    protected void Awake() {
        Initial();        
        button.onClick.AddListener(OnClick);
        Event.MonthChanged += OnPageUpdate;        
    }

    private void OnPageUpdate(object sender, CalendarArgs e) {
        buttonColor.color = button.colors.normalColor;
        if (HasKeep())
            buttonColor.color = button.colors.highlightedColor;
        if (e.Datetime == info.Date)
            Select();
    }

    public void OnClick() {
        calendar.SetCurrentDate(info.Date);
    }

    public void Select() {
        button.Select();
        buttonColor.color = button.colors.selectedColor;
    }

    public bool HasKeep() {
        string filePath = Save.GetKeepFilePath(info.Date);
        return File.Exists(filePath + "0.json");
    }
}