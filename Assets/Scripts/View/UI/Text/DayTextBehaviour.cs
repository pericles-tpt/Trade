using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayTextBehaviour : MonoBehaviour
{
    public Text DayText;
    public void UpdateDayText(int incAmount = 1)
    {
        int oldDay = Int32.Parse((DayText.text).Substring(4));
        DayText.text = "Day " + (oldDay + incAmount).ToString();

    }
}
