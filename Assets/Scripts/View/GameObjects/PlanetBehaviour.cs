using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlanetBehaviour : MonoBehaviour
{

    int frameCount = 0;

    private void OnMouseOver()
    {
        if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize == 15)
            GameObject.Find("Camera").GetComponent<GalaxyManager>().DrawOnePlanetToAll(this.gameObject);
        
    }

    private void OnMouseExit()
    {
        if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize == 15)
            GameObject.Find("Camera").GetComponent<GalaxyManager>().DestroyAllLines(); 

    }

    private void OnMouseDown()
    {
        if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize == 15)
        {
            GameObject.Find("pg_planet").GetComponent<PlanetPanelBehaviour>().ShowPanel(this.gameObject);
            GameObject.Find("Camera").GetComponent<GalaxyManager>().SetSelectedPlanet(this.gameObject);
            GameObject.Find("Camera").GetComponent<GalaxyManager>().SetPlanetPositionBeforeZoom(this.gameObject.transform.position);
            GameObject.Find("b_toggle_planet_view").GetComponent<TogglePlanetViewBehaviour>().TogglePlanetView();
        }

    }

    void Update()
    {
        frameCount++;

        // Do stuff - Improves performance by doing this operation every 12 frames (or every 0.2s if running at 60fps)
        if (frameCount % 12 == 0)
        {
            // REMOVED TRACK CURSOR POSITION FROM HERE (mod above was 2), LAST USED IN MAY 27TH COMMIT

        } else if (frameCount == 59)
        {
            frameCount = 0;
        }

    }

}
