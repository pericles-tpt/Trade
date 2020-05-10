using UnityEngine;

public class PlanetBehaviour : MonoBehaviour
{

    private void OnMouseOver()
    {
        GameObject.Find("Camera").GetComponent<GameDirector>().DrawOnePlanetToAll(this.gameObject);
        
    }

    private void OnMouseExit()
    {
        GameObject.Find("Camera").GetComponent<GameDirector>().DestroyAllLines();

    }

    private void OnMouseDown()
    {
        int zoom = 20;
        int speed = 50;

        GameObject.Find("pg_planet").GetComponent<PlanetPanelBehaviour>().ShowPanel(this.gameObject);
        GameObject.Find("Camera").GetComponent<GameDirector>().SetSelectedPlanet(this.gameObject);
        
        // Zoom into planet
        // TODO: Make it smooth
        Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
        cam.transform.LookAt(this.gameObject.transform);
        cam.orthographicSize = 0.65f;

        // Also when zoomed into planet disable tradelines showing up
        GameObject.Find("Camera").GetComponent<GameDirector>().ToggleTradeLines(false);
    }

}
