using UnityEngine;

public class Planet : Region {

	public enum PlanetType { arid, gas, volcanic, oceanic, ice, ideal }
    private PlanetType _PlanetType;
	private float      _GravFactor;
	private Country[]  _Countries;
	private string     _Name;
    public  GameObject _GameObject { get; private set; }

    public Planet(string name, PlanetType pt, GameObject go, float gf = 1.0f)
    {
        _Name = name;
        _PlanetType = pt;
        _GameObject = go;
        _GravFactor = gf;
    }

    public static string[] GimmeSomeNames(int numberONames)
    {
        string[] possibleNames = { "Aorfice", "Blorfus", "Dorfus", "Quorfus", "Snorfus", "Torfus", "Corfus II", "Zyorfus" };
        string[] returnedNames = new string[numberONames];

        for (int i = 0; i < numberONames; i++)
            returnedNames[i] = possibleNames[i];

        return returnedNames;

    }


}