using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePlanetViewBehaviour : MonoBehaviour
{
    public void TogglePlanetView()
    {
        bool planetView;
        if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize == 15)
            planetView = false;
        else
            planetView = true;

        if (planetView)
        {
            //Debug.Log(planetView);
            // Zoom into planet
            // TODO: Make it smooth
            Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
            cam.transform.LookAt(GameObject.Find("sun").transform);
            cam.orthographicSize = 15;

            // Also when zoomed into planet disable tradelines showing up
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleSectorLines(false, GameObject.Find("Camera").GetComponent<GameDirector>().FindPlanet(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet()));
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleTradeLines(true);
            planetView = false;
        } else
        {
            // Debug.Log(planetView);
            // Zoom into planet
            // TODO: Make it smooth
            Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
            cam.transform.LookAt(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform);
            cam.orthographicSize = 0.55f * Mathf.Pow(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform.localScale.x, 2);

            // Also when zoomed into planet disable tradelines showing up
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleSectorLines(true, GameObject.Find("Camera").GetComponent<GameDirector>().FindPlanet(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet()));
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleTradeLines(false);
            planetView = true;
        }
    }
}
