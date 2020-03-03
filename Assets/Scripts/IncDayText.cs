using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncDayText : MonoBehaviour
{

    public Text DayText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDayText()
    {
        int oldDay = Int32.Parse(DayText.text);
        DayText.text = (oldDay + 1).ToString();

    }
}
