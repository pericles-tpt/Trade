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

            // Destroy North, South, East and West gameObjects on planet
            Destroy(GameObject.Find("North"));
            Destroy(GameObject.Find("South"));
            Destroy(GameObject.Find("East"));
            Destroy(GameObject.Find("West"));

            // Also when zoomed into planet disable tradelines showing up
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleSectorLines(false, GameObject.Find("Camera").GetComponent<GameDirector>().FindPlanet(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet()));
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleTradeLines(true);
            planetView = false;
        } else
        {
            // 1. Create N, S, E and W gameobjects and set their positions to the top, left, right and bottom positions on sphere, should allow me to get my bearing on the sphere of where north and east is not matter how the camera is rotated
            GameObject North = new GameObject();
            GameObject South = new GameObject();
            GameObject East = new GameObject();
            GameObject West = new GameObject();

            North.name = "North";
            South.name = "South";
            East.name = "East";
            West.name = "West";

            GameObject selectedPlanet = GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet();
            float radius = GameObject.Find("Camera").GetComponent<GameDirector>().FindPlanet(selectedPlanet)._SphereSize / 2;
            North.transform.position = new Vector3(selectedPlanet.transform.position.x, selectedPlanet.transform.position.y, selectedPlanet.transform.position.z + radius);
            South.transform.position = new Vector3(selectedPlanet.transform.position.x, selectedPlanet.transform.position.y, selectedPlanet.transform.position.z - radius);
            East.transform.position = new Vector3(selectedPlanet.transform.position.x, selectedPlanet.transform.position.y + radius, selectedPlanet.transform.position.z);
            West.transform.position = new Vector3(selectedPlanet.transform.position.x, selectedPlanet.transform.position.y - radius, selectedPlanet.transform.position.z);
            
            // Zoom and rotate the camera into the planet and hide all other planets in the scene so that they don't block the view of the planet - THIS COULD CAUSE PROBLEM LATER IF SELECTING POINT ON PLANET COVERING THIS ONE
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleHidePlanetsNotSelected(false);
            Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
            galaxyCameraRotation = cam.transform.rotation;
            cam.transform.LookAt(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform, new Vector3(0, 0, -1));
            cam.orthographicSize = 0.55f * Mathf.Pow(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform.localScale.x, 2);

            // Enable sector lines on planet showing up and disable trade lines showing up
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleSectorLines(true, GameObject.Find("Camera").GetComponent<GameDirector>().FindPlanet(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet()));
            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleTradeLines(false);
            planetView = true;
        }
    }
}
