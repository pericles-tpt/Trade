using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class script : MonoBehaviour
{
    // Start is called before the first frame update
    List<Ship> ships = new List<Ship>();
    public GameObject ship;
    void Start()
    {
        int shipStartPlanet = UnityEngine.Random.Range(1, GameObject.Find("sun").GetComponent<NodeCreator>().planetCoords.Length);
        Vector3 firstShipPos = GameObject.Find("sun").GetComponent<NodeCreator>().InstantiateFirstShip(shipStartPlanet);
        float x = firstShipPos.x;
        float y = firstShipPos.y;
        float z = firstShipPos.z - 1;
        GameObject go = Instantiate(ship, new Vector3 (x, y, z), Quaternion.identity);
        ships.Add(new Ship(100, 10, 10, go, shipStartPlanet, Ship.FuelType.ok));

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateShipPositions()
    {
        Vector3[] planetPositions = GameObject.Find("sun").GetComponent<NodeCreator>().planetCoords;
        foreach (Ship s in ships)
            s.UpdateShipPosition(planetPositions[s._currentPlanet]);

    }
}
