using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship {

    public int      _x             { get; private set; }
    public int      _y             { get; private set; }
    public InvGrid  _inventory     { get; private set; }
    public int      _fuelCurrent   { get; private set; }
    public int      _fuelCapacity  { get; private set; }
    public int      _weight        { get; private set; }
    public FuelType _fuelType      { get; set; }
    // For now a new instance of a ship can always start off with a full fuel tank
    public Ship( int fuelCapacity, int igx, int igy, FuelType ft = FuelType.ok, int x = 0, int y = 0 ) {
        _fuelCapacity = fuelCapacity;
        _fuelCurrent = _fuelCapacity;
        _fuelType = ft;
        _x = x;
        _y = y;
        _inventory = new InvGrid(igx, igy);

    }

    public enum FuelType { awful, bad, ok, good, great }

}
