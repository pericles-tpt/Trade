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
    void Start()
    {
        int shipStartPlanet = UnityEngine.Random.Range(1, GameObject.Find("sun").GetComponent<NodeCreator>().planetCoords.Length);
        Vector3 firstShipPos = GameObject.Find("sun").GetComponent<NodeCreator>().InstantiateFirstShip(shipStartPlanet);
        float x = firstShipPos.x;
        float y = firstShipPos.y;
        float z = firstShipPos.z - 1;        
        GameObject go = Instantiate(ship, new Vector3 (x, y, z), Quaternion.identity);
        Ship f = new Ship(new Item.itemProperties(1, 1, Item.condition.good, "Kestrel", false, null), 100, 10, 10, go, shipStartPlanet, Ship.FuelType.ok);
        Assets[0] = f;
        Fleet[0] = f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateShipPositions()
    {
        Vector3[] planetPositions = GameObject.Find("sun").GetComponent<NodeCreator>().planetCoords;
        foreach (Ship s in Fleet)
            s.UpdateShipPosition(planetPositions[s._currentPlanet]);

    }

    /*public Item AssetSearch()
    {
        Item foundAsset = null;
        if (Assets.Count == 1)
            foundAsset = Assets.FindIndex(;
        return foundAsset;
    }

    public void AssetInsert(Item New)
    {
        Assets[FindAssetIndex(New, 0, New.GetType().ToString(), New._Name, New._IGProductNumber)] = New;

    }

    public int FindAssetIndex(Item New, int indexStart, string searchType, string searchName, long searchPN)
    {
        int i;

        if (String.Compare(Assets[indexStart].GetType().ToString(), searchType) == 0) {

            int j = indexStart;
            while (Assets[j].GetType().ToString().Equals(searchType))
                j--;
            indexStart = (j + 1);

            j = indexStart;
            while (String.Compare(Assets[indexStart]._Name, searchName) < 0)
                j++;

            indexStart = (j - 1);

            j = indexStart;
            while (Assets[indexStart]._IGProductNumber < searchPN)
                j++;

            indexStart = (j - 1);

        } else if (String.Compare(Assets[indexStart].GetType().ToString(), searchType) > 0)

        else if (String.Compare(Assets[indexStart].GetType().ToString(), searchType) < 0)

                    return i;

    }*/
}
