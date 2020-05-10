using UnityEngine;

public class Sector {

    private int      _Population {get; set;}
    private Entity[] _Members    {get;}
    public string    _Name       { get; private set; }
    public Vector3   _Coord      { get; private set; }

    public Sector(string name, Vector3 coord) {
        _Name       = name;
        _Coord      = coord;
        _Population = 0;
        _Members    = null;

    }

    public int CountPopulation(Entity[] members) {
        return members.Length;

    }

    public override string ToString()
    {
        return (_Name + ", x: " + _Coord.x + ", y: " + _Coord.y + ", z: " + _Coord.z);
    }

}
