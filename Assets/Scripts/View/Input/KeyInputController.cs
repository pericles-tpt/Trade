using System.Collections;
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
        if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize != 15)
        {
            if (Input.GetKey("up"))
            {
                GameObject.Find("Camera").transform.RotateAround(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform.position, new Vector3(0, 1, 0), -1f);
            }

            if (Input.GetKey("down"))
            {
                GameObject.Find("Camera").transform.RotateAround(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform.position, new Vector3(0, 1, 0), 1f);
            }

            if (Input.GetKey("left"))
            {
                GameObject.Find("Camera").transform.RotateAround(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform.position, new Vector3(0, 0, 1), -1f);
            }

            if (Input.GetKey("right"))
            {
                GameObject.Find("Camera").transform.RotateAround(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform.position, new Vector3(0, 0, 1), 1f);

            }
        }
    }
}
