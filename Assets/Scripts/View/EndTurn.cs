using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncOrbit() 
    {
        GameObject.Find("sun").GetComponent<NodeCreator>().IncrementOrbits();
        GameObject.Find("Canvas").GetComponent<IncDayText>().UpdateDayText();
        GameObject.Find("player").GetComponent<Player>().UpdateShipPositions();


    }
}
