using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class TogglePlanetViewBehaviour : MonoBehaviour
{

    public void TogglePlanetView()
    {
        bool planetView;
        if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize == 15)
            planetView = false;
        else
            planetView = true;

        Quaternion galaxyCameraRotation = new Quaternion();

        if (planetView)
        {
            //Debug.Log(planetView);
            // Zoom into planet
            // TODO: Make it smooth
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleHidePlanetsNotSelected(true);

            Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
            cam.transform.LookAt(new Vector3(0,0,0));
            cam.transform.rotation = galaxyCameraRotation;
            cam.transform.position = new Vector3 (0, 0, -20);
            cam.orthographicSize = 15;

            // Also when zoomed into planet disable tradelines showing up
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleSectorLines(false, GameObject.Find("Camera").GetComponent<GameDirector>().FindPlanet(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet()));
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleTradeLines(true);
            planetView = false;
        } else
        {   
            // Zoom and rotate the camera into the planet and hide all other planets in the scene so that they don't block the view of the planet - THIS COULD CAUSE PROBLEM LATER IF SELECTING POINT ON PLANET COVERING THIS ONE
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleHidePlanetsNotSelected(false);
            Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
            galaxyCameraRotation = cam.transform.rotation;
            cam.transform.LookAt(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform, new Vector3(0, 0, -1));
            cam.orthographicSize = 0.55f * Mathf.Pow(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform.localScale.x, 2);

            // Enable sector lines on planet showing up and disable trade lines showing up
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleSectorLines(false, GameObject.Find("Camera").GetComponent<GameDirector>().FindPlanet(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet()));
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleTradeLines(false);
            planetView = true;
        }
    }
}
