using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryKeep : MonoBehaviour
{    
    private Calendar calendar;
    private EntryKeeper keeper;
    public Image dragCanvas;
    public Dropdown dropdown;    
    public InputField content;    
    public InputField money;
    public Button editButton;
    public Text editText;
    public KeepInfo info;

    void Awake() {
        calendar = FindObjectOfType<Calendar>();
        keeper = GetComponent<EntryKeeper>();
        dropdown = GetComponentInChildren<Dropdown>();
        if (money.text != "ª÷ÃB" && money.text != "")
            info = new KeepInfo(gameObject, calendar.currentDate, dropdown.value, content.text, CorrectNumber(money), editButton, false);
        else
            info = new KeepInfo(gameObject, calendar.currentDate, dropdown.value, content.text, money.text, editButton, false);
        dropdown.onValueChanged.AddListener(delegate { TypeUpdate(dropdown); });
        content.onEndEdit.AddListener(delegate { ContentUpdate(content); });
        money.onEndEdit.AddListener(delegate { CorrectNumber(money); });
        Event.KeepEdited += KeepEdited;
    }

    void OnDestroy() {
        Event.KeepEdited -= KeepEdited;
    }

    private void KeepEdited(object sender, EntryKeepArgs e) {
        if (e.KeepInfo.KeepBase != info.KeepBase) {
            info.Edit.interactable = !e.KeepInfo.IsEdit;
        }
        dragCanvas.GetComponent<Pointer>().enabled = !e.KeepInfo.IsEdit;
        dragCanvas.GetComponent<Button>().interactable = !e.KeepInfo.IsEdit;
    }
    public void BeginEdit() {
        info.IsEdit = !info.IsEdit;
        dragCanvas.raycastTarget = !info.IsEdit;
        dropdown.interactable = info.IsEdit;
        content.interactable = info.IsEdit;
        money.interactable = info.IsEdit;
        editText.text = info.IsEdit ? "½T©w" : "½s¿è";
        Event.OnKeepEdited(this, new EntryKeepArgs(info));
    }

    private string TypeUpdate(Dropdown input) {
        info.Type = input.value;        
        return info.Content;
    }

    private string ContentUpdate(InputField input) {
        info.Content = input.text;
        content.text = input.text;
        return info.Content;
    }

    private string CorrectNumber(InputField input) {
        info.Money = int.Parse(input.text).ToString();
        money.text = int.Parse(input.text).ToString();
        return info.Money;
    }
}
