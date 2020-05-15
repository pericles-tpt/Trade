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

    public static string NameSector(int x, int y)
    {
        // Naming scheme is 3 first letters to planet, number of planet if valid, x coord in integers, y coord in integers.
        // e.g. HOD3-4-4
        int maxSectorSize = 2 * Planet._SectorSize;
        int numberPrefixesSize = (maxSectorSize.ToString().Length - 1);

        string[] numberPrefixes = new string[numberPrefixesSize];
        string lastPrefix = "";
        for (int i = 0; i < numberPrefixesSize; i++)
        {
            numberPrefixes[i] = lastPrefix + "0";
            lastPrefix = numberPrefixes[i];
        }

        int index = numberPrefixesSize - x.ToString().Length;

        if (index < 0)
            return ("X" + x + "Y" + y);
        else
            return ("X" + numberPrefixes[index] + x + "Y" + y);

    }

}
