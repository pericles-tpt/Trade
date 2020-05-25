using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyInputController : MonoBehaviour
{
    int incline = 0;
    int frameCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
        //float scale = GameObject.Find("Camera").GetComponent<GalaxyManager>().FindPlanet(GameObject.Find("Camera").GetComponent<GalaxyManager>().GetSelectedPlanet())._SphereSize;
        if (Input.GetKey("left"))
        {
            GameObject.Find("Camera").transform.RotateAround(GameObject.Find("Camera").GetComponent<GalaxyManager>().GetSelectedPlanet().transform.position, new Vector3(0, 0, 1), -1f);
        }

        if (Input.GetKey("right"))
        {
            GameObject.Find("Camera").transform.RotateAround(GameObject.Find("Camera").GetComponent<GalaxyManager>().GetSelectedPlanet().transform.position, new Vector3(0, 0, 1), 1f);

        }

        int inclineBefore = incline;

        if (Input.GetKeyDown("up"))
        {
            if (incline < 2)
            {
                GameObject.Find("Camera").transform.RotateAround(GameObject.Find("Camera").GetComponent<GalaxyManager>().GetSelectedPlanet().transform.position, new Vector3(0, 1, 0), -20f);
                incline++;
            }

        }

        if (Input.GetKeyDown("down"))
        {
            if (incline > -2)
            {
                GameObject.Find("Camera").transform.RotateAround(GameObject.Find("Camera").GetComponent<GalaxyManager>().GetSelectedPlanet().transform.position, new Vector3(0, 1, 0), 20f);
                incline--;
            }

        }

        /*float zoomAmount = 0.10f;
        if ((incline < inclineBefore) && (incline < 0))
            cam.orthographicSize -= zoomAmount * scale;
        else if ((incline > inclineBefore) && (incline > 0))
            cam.orthographicSize -= zoomAmount * scale;
        else if ((incline < inclineBefore) && (incline >= 0))
            cam.orthographicSize += zoomAmount * scale;
        else if ((incline > inclineBefore) && (incline <= 0))
            cam.orthographicSize += zoomAmount * scale;
        */

        frameCount++;
        if (frameCount % 15 == 0)
        {
            if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize != 15)
            {


            }
        }
        else if (frameCount == 59)
        {
            frameCount = 0;
        } 
    }
}
