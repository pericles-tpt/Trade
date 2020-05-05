using UnityEngine;
public class Planet
{
    // Enums
    public enum PlanetType { arid, gas, molten, oceanic, ice, water, temperate }

    // Unity Properties
    public GameObject         _GameObject      { get; private set; }
    public int                _Index           { get; private set; }
    public Sprite             _Icon            { get; private set; }
    public string             _Name            { get; private set; }
    public Vector3[]          _PlanetPositions { get; private set; }

    // Game Properties
    private PlanetType _PlanetType;
    private float      _GravFactor;
    private Country[]  _Countries;

    public Planet (string name, int index, Sprite spr, PlanetType pt, GameObject go, float gf = 1.0f)
    {

        _GameObject = go;
        _Index = index;
        _Icon = spr;
        _Name = name;

        _PlanetType = pt;
        _GravFactor = gf;

    }

    public void IncOrbit()
    {
            // Just calculate distance from origin (or radius of orbit)
            Vector3 v = _GameObject.transform.position;

            float radius;
            float orbitInc;
            float orbitCirc;

            if (v.x == 0)
            {
                radius = v.y;

            }

            else if (v.y == 0)
            {
                radius = v.x;

            }

            else
            {
                radius = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2));
                Debug.Log("Radius is: " + radius);

            }

            orbitInc = radius * 100;

            float orbitSectorAngle = (360 / orbitInc);
            float orbitSectorAngleRadians = orbitSectorAngle * Mathf.Deg2Rad;
            float nextX, nextY;

            if (_Index == 2)
            {
                nextX = (v.x * Mathf.Cos(-orbitSectorAngleRadians)) + (v.y * Mathf.Sin(-orbitSectorAngleRadians));
                nextY = (-v.x * Mathf.Sin(-orbitSectorAngleRadians)) + (v.y * Mathf.Cos(-orbitSectorAngleRadians));

            }
            else
            {
                nextX = (v.x * Mathf.Cos(orbitSectorAngleRadians)) + (v.y * Mathf.Sin(orbitSectorAngleRadians));
                nextY = (-v.x * Mathf.Sin(orbitSectorAngleRadians)) + (v.y * Mathf.Cos(orbitSectorAngleRadians));

            }

            // Create new v and assign it to the gameobject
            Vector3 v3 = new Vector3(nextX, nextY, -10);
            _GameObject.transform.position = v3;

    }

}
