using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnBehaviour : MonoBehaviour
{
    public void IncOrbit() 
    {
        GameObject.Find("Camera").GetComponent<GameDirector>().IncrementOrbits();
        GameObject.Find("Canvas").GetComponent<DayTextBehaviour>().UpdateDayText();
        //GameObject.Find("player").GetComponent<Player>().UpdateShipPositions();

    }

}
