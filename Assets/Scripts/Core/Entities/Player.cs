using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    Item[] Assets = new Item[1000];
    Ship[] Fleet = new Ship[10];
    public GameObject ship;

    /*
    void Start()
    {
        int shipStartPlanet = UnityEngine.Random.Range(1, GameObject.Find("sun").GetComponent<GalaxyGenerator>().planets.Length);
        Vector3 firstShipPos = GameObject.Find("sun").GetComponent<GalaxyGenerator>().GetPlanetPosition(shipStartPlanet);
        float x = firstShipPos.x;
        float y = firstShipPos.y;
        float z = firstShipPos.z - 1;        
        GameObject go = Instantiate(ship, new Vector3 (x, y, z), Quaternion.identity);
        Ship f = new Ship(new Item.itemProperties(1, 1, Item.condition.good, "Kestrel", false, null), 100, 10, 10, go, shipStartPlanet, Ship.FuelType.ok);
        Assets[0] = f;
        Fleet[0] = f;
    }

    public void UpdateShipPositions()
    {
        Planet[] planetPositions = GameObject.Find("sun").GetComponent<GalaxyGenerator>().planets;
        foreach (Ship s in Fleet)
            s.UpdateShipPosition(planetPositions[s._currentPlanet]._GameObject.transform.position);

    }
    */

}
