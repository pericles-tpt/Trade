using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlanetViewBehaviour : MonoBehaviour
{
    bool planetView = true;
    public void TogglePlanetView()
    {
        if (planetView)
        {
            //Debug.Log(planetView);
            // Zoom into planet
            // TODO: Make it smooth
            Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
            cam.transform.LookAt(GameObject.Find("sun").transform);
            cam.orthographicSize = 15;

            // Also when zoomed into planet disable tradelines showing up
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleTradeLines(true);
            planetView = false;
        } else
        {
            // Debug.Log(planetView);
            // Zoom into planet
            // TODO: Make it smooth
            Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
            cam.transform.LookAt(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform);
            cam.orthographicSize = 0.65f;

            // Also when zoomed into planet disable tradelines showing up
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleTradeLines(false);
            planetView = true;
        }
    }
}
