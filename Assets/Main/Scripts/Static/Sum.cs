using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Access to get sum in different time range with date (or directly get from data)
/// </summary>
public static class Sum {
    public static int GetInKeep(KeepInfo keep) {        
        if (keep.Money != "" && keep.Money != "金額") // 未輸入前預設字串"金額"
            return int.Parse(keep.Money);
        return 0; 
    }

    public static int GetInDay(DateTime date) {
        Debug.Log("SumEvent: try to get sum in 'day' of " + date.Day);
        int sum = 0;
        string filePath = Save.GetKeepFilePath(date);
        for (int i = 0; File.Exists(filePath + i + ".json"); i++) {
            string json = File.ReadAllText(filePath + i + ".json");
            sum += GetInKeep(JsonUtility.FromJson<KeepInfo>(json));
        }
        return sum;
    }

    public static int GetInMonth(DateTime date) {        
        return GetInMonth(date.Year, date.Month);
    }

    public static int GetInMonth(int year, int month) {
        Debug.Log("SumEvent: try to get sum in 'month' of " + month);
        int i_length = DateTime.DaysInMonth(year, month) + 1;
        int sum = 0;
        for (int i = 1; i < i_length; i++) {
            DateTime callDate = new DateTime(year, month, i);
            sum += GetInDay(callDate);
        }
        return sum;
    }
    public static int GetInYear(DateTime date) {
        return GetInYear(date.Year);
    }

    public static int GetInYear(int year) {
        int sum = 0;
        for (int i = 0; i < 12; i++) {            
            sum += GetInMonth(year, i);
        }
        return sum;
    }

    public static int GetIn() {
        return 0;
    }    
}
