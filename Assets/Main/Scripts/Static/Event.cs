using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Event {

    public static event EventHandler<EntryKeepArgs> KeepEdited;
    public static event EventHandler<EntryKeepArgs> KeepReorder;
    public static event EventHandler<CalendarArgs> MonthChanged;

    public static void OnKeepEdited(object sender, EntryKeepArgs e) {
        KeepEdited?.Invoke(sender, e);
    }
    public static void OnKeepReorder(object sender, EntryKeepArgs e) {
        KeepReorder?.Invoke(sender, e);
    }

    public static void OnMonthChanged(object sender, CalendarArgs e) {
        MonthChanged?.Invoke(sender, e);
    }    
}

public class CalendarArgs : EventArgs {
    public DateTime Datetime;

    public CalendarArgs(DateTime datetime) {
        Datetime = datetime;
    }
}

public class EntryKeepArgs : EventArgs {
    public KeepInfo KeepInfo;

    public EntryKeepArgs(KeepInfo keepInfo) {
        KeepInfo = keepInfo;
    }
}
