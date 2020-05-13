using UnityEngine;

public class Sector {

    public enum Shape { square, triangleUp, triangleDown };

    private int      _Population {get; set;}
    private Entity[] _Members    {get;}
    public string    _Name       { get; private set; }
    public int      _CoordIndex  { get; private set; }
    public Shape     _Shape      { get; private set; }       

    public Sector(string name, Shape shape, int coordIndex) {
        _Name       = name;
        _CoordIndex = coordIndex;
        _Population = 0;
        _Members    = null;
        _Shape      = shape;

    }

    public int CountPopulation(Entity[] members) {
        return members.Length;

    }

}
