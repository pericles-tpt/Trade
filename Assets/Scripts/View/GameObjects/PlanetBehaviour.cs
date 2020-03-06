using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBehaviour : MonoBehaviour
{
    private void OnMouseOver()
    {
        GameObject.Find("sun").GetComponent<GalaxyGenerator>().DrawTradeLines(this.gameObject);

    }

    private void OnMouseExit()
    {
        GameObject.Find("sun").GetComponent<GalaxyGenerator>().DestroyAllTradeLines();

    }

    private void OnMouseDown()
    {
        GameObject.Find("PlanetPanelGroup").GetComponent<PlanetPanelBehaviour>().ActivatePanel(this.gameObject);

    }

}
