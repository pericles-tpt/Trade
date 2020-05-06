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
        Debug.Log("Mousedown is getting triggered!");
        GameObject.Find("pg_planet").GetComponent<PlanetPanelBehaviour>().ShowPanel(this.gameObject);

    }

}
