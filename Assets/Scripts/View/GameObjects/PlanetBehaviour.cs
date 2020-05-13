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

        // Draw sector lines and zoom into planet
        // TODO: Make it smooth
        GameObject.Find("Camera").GetComponent<GameDirector>().ToggleSectorLines(true, GameObject.Find("Camera").GetComponent<GameDirector>().FindPlanet(this.gameObject));
        GameObject.Find("Camera").GetComponent<GameDirector>().ToggleTradeLines(false);


        Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
        cam.transform.LookAt(this.gameObject.transform);
        cam.orthographicSize = 0.55f * Mathf.Pow(GameObject.Find("Camera").GetComponent<GameDirector>().GetSelectedPlanet().transform.localScale.x, 2);
        //cam.transform.Rotate(new Vector3(-90, 0, 0));
    }

}
