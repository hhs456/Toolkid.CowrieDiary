using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 更新紀錄: 111.04.07 改採用.json紀錄
/// </summary>
public class EntryKeeper : MonoBehaviour {
    private Calendar calendar;
    public Transform inventory;
    public GameObject keepStyle;
    public Button createButton;
    public Button deleteButton;
    public List<EntryKeep> keeps;
    private GameObject currentKeeper;
    public Text dailySumText;
    public Text monthSumText;

    public int todaySum;
    public int monthSum;

    void Awake() {
        createButton.onClick.AddListener(CreateKeep);
        deleteButton.onClick.AddListener(DeleteKeep);
        calendar = FindObjectOfType<Calendar>();
        Event.MonthChanged += OnCalendarPageUpdate;
        Event.KeepEdited += OnKeepEdit;
        Event.KeepReorder += RewriteAllKeep;
    }

    private void RewriteAllKeep(object sender, EntryKeepArgs e) {
        Transform inventory = e.KeepInfo.KeepBase.transform.parent;
        keeps.Clear();
        keeps.AddRange(inventory.GetComponentsInChildren<EntryKeep>());
        int length = keeps.Count;
        for (int i = 0; i < length; i++) {
            Save.CreateDirectory(e.KeepInfo.DateTime);
            string filePath = Save.GetKeepFilePath(e.KeepInfo.DateTime);
            string newData = JsonUtility.ToJson(keeps[i].info, true);
            File.WriteAllText(filePath + i + ".json", newData);
        }
    }

    private void OnCalendarPageUpdate(object sender, CalendarArgs e) {
        LoadKeeperFromTxt(calendar.currentDate);
    }

    public void CreateKeep() {
        EntryKeep keep = Instantiate();
        keeps.Add(keep);
    }

    private EntryKeep Instantiate() {
        GameObject keepBase = Instantiate(keepStyle, inventory);
        keepBase.transform.localPosition = Vector3.zero;
        keepBase.transform.localScale = Vector3.one;
        keepBase.SetActive(true);
        return keepBase.GetComponent<EntryKeep>();
    }     

    public void AssignKeep(KeepInfo info) {
        EntryKeep keeper = Instantiate();
        keeps.Add(keeper);
        keeper.info = info;
        keeper.info.KeepBase = keeper.gameObject;
        keeper.info.Edit = keeper.editButton;
        keeper.dropdown.value = int.Parse(info.Type.ToString());
        keeper.content.text = info.Content;
        keeper.money.text = info.Money.ToString();
    }

    public void DeleteKeep() {
        keeps.Remove(currentKeeper.GetComponent<EntryKeep>());
        string filePath = Save.GetKeepFilePath(calendar.currentDate);
        int n = currentKeeper.transform.GetSiblingIndex() - 1; // 第一個是複製用的樣本，故剃除
        File.Delete(filePath + n + ".json");    
        Destroy(currentKeeper);
        OnKeepChange(calendar.currentDate);
        ButtonStateInitial();
    }

    public void InitialKeeps() {
        int length = keeps.Count;
        for (int i = 0; i < length; i++) {            
            Destroy(keeps[i].gameObject);
        }
        keeps.Clear();
        ButtonStateInitial();
    }

    private void OnKeepEdit(object sender, EntryKeepArgs e) {
        createButton.interactable = !e.KeepInfo.IsEdit;
        deleteButton.interactable = e.KeepInfo.IsEdit;

        currentKeeper = e.KeepInfo.KeepBase;
        Save.CreateDirectory(e.KeepInfo.DateTime);
        string filePath = Save.GetKeepFilePath(e.KeepInfo.DateTime);
        int n = currentKeeper.transform.GetSiblingIndex() - 1; // 第一個是複製用的樣本，故剃除
        string newData = JsonUtility.ToJson(keeps[n].info, true);
        File.WriteAllText(filePath + n + ".json", newData);        
        OnKeepChange(e.KeepInfo.DateTime);        
    }

    private void OnKeepChange(DateTime date) {
        todaySum = Sum.GetInDay(date);
        monthSum = Sum.GetInMonth(calendar.currentDate.Year, calendar.currentDate.Month);
        dailySumText.text = todaySum.ToString();
        monthSumText.text = monthSum.ToString();
    }

    private void ButtonStateInitial() {
        int cont = keeps.Count;
        for (int i = 0; i < cont; i++) {
            keeps[i].editButton.interactable = true;
        }
        createButton.interactable = true;
        deleteButton.interactable = false;
    }

    private void LoadKeeperFromTxt(DateTime dateTime) {
        InitialKeeps();
        todaySum = 0;
        string filePath = Save.GetKeepFilePath(dateTime);
        for (int i = 0; File.Exists(filePath + i + ".json"); i++) {
            Debug.Log("LoadEvent: begin read " + filePath + i + ".json");
            string json = File.ReadAllText(filePath + i + ".json");
            KeepInfo info = JsonUtility.FromJson<KeepInfo>(json);
            if(info.Content == null)
                File.Delete(filePath + i + ".json");
            info.DateTime = dateTime;
            AssignKeep(info);
            todaySum += Sum.GetInKeep(info);
        }        
        dailySumText.text = todaySum.ToString();
        monthSum = Sum.GetInMonth(dateTime.Year, dateTime.Month);
        monthSumText.text = monthSum.ToString();
    }
}
