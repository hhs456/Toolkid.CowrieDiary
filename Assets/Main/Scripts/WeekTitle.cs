using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeekTitle : MonoBehaviour
{
    public Image image;
    public Text weekName;

    void Awake() {
        image = GetComponent<Image>();
        weekName = GetComponentInChildren<Text>();
    }
}
