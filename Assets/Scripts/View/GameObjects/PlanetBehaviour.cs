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
        if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize == 15)
        {
            GameObject.Find("pg_planet").GetComponent<PlanetPanelBehaviour>().ShowPanel(this.gameObject);
            GameObject.Find("Camera").GetComponent<GameDirector>().SetSelectedPlanet(this.gameObject);
            GameObject.Find("b_toggle_planet_view").GetComponent<TogglePlanetViewBehaviour>().TogglePlanetView();
        }

    }

}
