using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Save {
    public static string GetKeepFilePath(DateTime dateTime) {
        string basePath = Application.persistentDataPath + "/Keeps/";
        string timePath = basePath + dateTime.Year + "/" + dateTime.Month;
        string filePath = timePath + "/" + dateTime.Day + "_";
        return filePath;
    }
    public static void CreateDirectory(DateTime dateTime) {
        string basePath = Application.persistentDataPath + "/Keeps/";
        string timePath = basePath + dateTime.Year + "/" + dateTime.Month;
        Directory.CreateDirectory(timePath);
    }
}
