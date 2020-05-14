﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up"))
        {
            if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize != 15)
                GameObject.Find("Camera").transform.RotateAround(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform.position, new Vector3(0, 1, 0), -2f);
        }

        if (Input.GetKey("down"))
        {
            if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize != 15)
                GameObject.Find("Camera").transform.RotateAround(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform.position, new Vector3(0, 1, 0), 2f);
        }

        if (Input.GetKey("left"))
        {
            if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize != 15)
                GameObject.Find("Camera").transform.RotateAround(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform.position, new Vector3(0, 0, 1), -2f);
        }

        if (Input.GetKey("right"))
        {
            if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize != 15)
                GameObject.Find("Camera").transform.RotateAround(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform.position, new Vector3(0, 0, 1), 2f);
            
        }
    }
}
