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
        Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
        GalaxyManager GD = GameObject.Find("Camera").GetComponent<GalaxyManager>();
        GameObject planetGO = GameObject.Find("Camera").GetComponent<GalaxyManager>().GetSelectedPlanet();
        Planet planet = GameObject.Find("Camera").GetComponent<GalaxyManager>().FindPlanet(GameObject.Find("Camera").GetComponent<GalaxyManager>().GetSelectedPlanet());

        bool planetView;
        if (cam.orthographicSize == 15)
            planetView = false;
        else
            planetView = true;

        Quaternion galaxyCameraRotation = new Quaternion();

        if (planetView)
        {
            //Debug.Log(planetView);
            // Zoom into planet
            // TODO: Make it smooth
            GD.ToggleMovePlanetsNotSelected(false);

            planetGO.transform.position = GD.GetPlanetPositionBeforeZoom();

            cam.transform.LookAt(new Vector3(0,0,0));
            cam.transform.rotation = galaxyCameraRotation;
            cam.transform.position = new Vector3 (0, 0, -20);
            cam.orthographicSize = 15;

            GD.ToggleSectorTooltipVisible(false);

            // Also when zoomed into planet disable tradelines showing up
            GD.ToggleSectorLines(false, planet);
            GD.ToggleTradeLines(true);

        } else
        {   
            // Zoom and rotate the camera into the planet and hide all other planets in the scene so that they don't block the view of the planet - THIS COULD CAUSE PROBLEM LATER IF SELECTING POINT ON PLANET COVERING THIS ONE
            GD.ToggleMovePlanetsNotSelected(true);
            galaxyCameraRotation = cam.transform.rotation;

            planetGO.transform.position = new Vector3(-4, 0, -10);

            GD.ToggleSectorTooltipVisible(true);

            cam.transform.LookAt(planetGO.transform.position, new Vector3(planetGO.transform.position.x,planetGO.transform.position.y,planetGO.transform.position.z + 1));
            cam.orthographicSize = 0.55f * Mathf.Pow(planet._SphereSize, 2);
            cam.transform.RotateAround(planetGO.transform.position, new Vector3(0, -1, 0), 68f);

            // Enable sector lines on planet showing up and disable trade lines showing up
            GD.ToggleSectorLines(false, planet);
            GD.ToggleTradeLines(false);

        }
    }
}
