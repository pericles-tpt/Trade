using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int    _x               { get; private set; }
    public int    _y               { get; private set; }
    public Node[] _connectedNodes  { get; private set; }

    public Node(int x, int y, Node[] connectedTo)
    {
        _x = x;
        _y = y;
        _connectedNodes = connectedTo;

    }

    public float CalculateFuelUse(Ship.FuelType ft, int destinationIndex)
    {
        if (destinationIndex < _connectedNodes.Length)
        {
            Node dest = _connectedNodes[destinationIndex];
            float distance = Mathf.Sqrt(Mathf.Pow(dest._x, 2f) + Mathf.Pow(dest._y, 2f));

            // fuelEfficiency of 1.0 means that 1L of fuel will last 100km
            float fuelEfficiency;
            if (ft == Ship.FuelType.awful)
                fuelEfficiency = 0.1f;
            else
                fuelEfficiency = (int)ft * 0.25f;

            return distance / (fuelEfficiency * 100);

        }
        else
        {
            return -1;

        }
    }

}
