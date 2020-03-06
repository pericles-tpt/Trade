using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetPanelBehaviour : MonoBehaviour
{
    public Text PlanetName;
    public Image PlanetIcon;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0;

    }

    // Update is called once per frame
    public void ActivatePanel(GameObject planet)
    {
        PopulateFields(planet);
        this.gameObject.GetComponent<CanvasGroup>().alpha = 1;

    }

    public void DeactivatePanel()
    {
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
        ClearFields();

    }

    public void ClearFields()
    {
        // Do nothing atm
    }

    public void PopulateFields(GameObject planet)
    {
        PlanetName.text = planet.GetInstanceID().ToString();
        PlanetIcon.sprite = planet.GetComponent<SpriteRenderer>().sprite;

    }

}
