using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship {

    public InvGrid    _inventory     { get; private set; }
    public int        _fuelCurrent   { get; private set; }
    public int        _fuelCapacity  { get; private set; }
    public int        _weight        { get; private set; }
    public int        _currentPlanet { get; private set; }
    public FuelType   _fuelType      { get; set; }
    public GameObject _gObject       { get; private set; }
    public GameObject ship;
    // For now a new instance of a ship can always start off with a full fuel tank
    public Ship( int fuelCapacity, int igx, int igy, GameObject ship, int currentPlanet, FuelType ft = FuelType.ok) {
        _fuelCapacity = fuelCapacity;
        _fuelCurrent = _fuelCapacity;
        _fuelType = ft;
        _inventory = new InvGrid(igx, igy);
        _currentPlanet = currentPlanet;
        _gObject = ship;

    }

    public void UpdateShipPosition (Vector3 newPosition)
    {
        _gObject.transform.position = newPosition;
    }

    public enum FuelType { awful, bad, ok, good, great }

}
