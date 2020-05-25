using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetPanelBehaviour : MonoBehaviour
{
    public Text PlanetName;
    public Image PlanetIcon;

    void Start()
    {
        this.HidePanel();

    }


    public void ShowPanel(GameObject planet)
    {
        PopulateFields(planet);
        this.gameObject.GetComponent<CanvasGroup>().alpha = 1;

    }

    public void HidePanel()
    {
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        ClearInfo();

    }

    public void PopulateFields(GameObject planet)
    {
        Planet p = GameObject.Find("Camera").GetComponent<GalaxyManager>().FindPlanet(planet);
        if (p != null)
        {
            PlanetName.text = p._Name;
            PlanetIcon.sprite = p._Icon;

        } else
        {
            PlanetName.text = planet.GetInstanceID().ToString();

        }

    }

    public void ClearInfo()
    {
        // Do nothing atm
    }

}
