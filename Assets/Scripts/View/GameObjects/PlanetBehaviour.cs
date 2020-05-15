using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class PlanetBehaviour : MonoBehaviour
{
    bool trackCursorPosition = false;

    private void OnMouseOver()
    {
        if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize == 15)
            GameObject.Find("Camera").GetComponent<GameDirector>().DrawOnePlanetToAll(this.gameObject);
        else
        {
            trackCursorPosition = true;
            
        }

        
    }

    private void OnMouseExit()
    {
        GameObject.Find("Camera").GetComponent<GameDirector>().DestroyAllLines();
        trackCursorPosition = false;
    }

    private void OnMouseDown()
    {
        if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize == 15)
        {
            GameObject.Find("pg_planet").GetComponent<PlanetPanelBehaviour>().ShowPanel(this.gameObject);
            GameObject.Find("Camera").GetComponent<GameDirector>().SetSelectedPlanet(this.gameObject);
            GameObject.Find("b_toggle_planet_view").GetComponent<TogglePlanetViewBehaviour>().TogglePlanetView();
        }

    }

    void Update()
    {
        if (trackCursorPosition)
        {
            Ray ray = GameObject.Find("Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            float maxLength = 100f;
            RaycastHit info = new RaycastHit();
            GameObject.Find(this.gameObject.name).GetComponent<MeshCollider>().Raycast(ray, out info, maxLength);
            Debug.Log("Hit mesh at point " + new Vector3(info.point.x - this.gameObject.transform.position.x, info.point.y - this.gameObject.transform.position.y, info.point.z - this.gameObject.transform.position.z));

            // 1. Get starting vector on sphere (i.e. origin + z-level below pole), find if hit point is above/below and left/right of that position
            int maxCoord = (int)(Planet._SectorSize * this.gameObject.GetComponent<SphereCollider>().radius);
            float Azi = 360 / maxCoord;
            float Inc = 90 / (maxCoord / 2);

            Vector3 start = PolarToVector(this.gameObject.GetComponent<SphereCollider>().radius * this.gameObject.transform.localScale.x, Inc * Mathf.Deg2Rad, 0);
            Vector3 relativePoint = new Vector3(info.point.x - this.gameObject.transform.position.x, info.point.y - this.gameObject.transform.position.y, info.point.z - this.gameObject.transform.position.z);

            int zSign;
            if ((start.z) <= relativePoint.z)
                zSign = 1;
                // Check this sector then go up
            else
                zSign = -1;
            // Go down

            int ySign;
            if (relativePoint.y < start.y)
                ySign = -1;
            else if (relativePoint.y > start.y)
                ySign = 1;
            else if (start.y == relativePoint.y && start.x < 0)
                ySign = 1; // NOTE: Doesn't really matter either way as long as it traverses the sphere in some direction to reach the other side
            else
                ySign = 0; // In other words it's at its destination

            Debug.Log("ySign: " + ySign + ", zSign: " + zSign + "start z: " + start.z);



        }
    }

    // REFACTOR - DUPLICATED CODE, BELONGS IN SphereMeshGenerator.cs
    public static Vector3 PolarToVector(float radius, float IncRad, float AziRad)
    {

        float x = radius * Mathf.Sin(IncRad) * Mathf.Cos(AziRad);
        float y = radius * Mathf.Sin(IncRad) * Mathf.Sin(AziRad);
        float z = radius * Mathf.Cos(IncRad);

        return new Vector3(x, y, z);
    }

}
