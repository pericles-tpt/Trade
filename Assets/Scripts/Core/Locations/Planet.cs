using UnityEngine;
using Vector2 = UnityEngine.Vector2;
public class Planet
{
    // Enums
    public enum PlanetType { arid, gas, molten, oceanic, ice, water, temperate }

    // Unity Properties
    public GameObject                            _GameObject           { get; private set; }
    public int                                   _Index                { get; private set; }
    public Sprite                                _Icon                 { get; private set; }
    public string                                _Name                 { get; private set; }
    public int                                   _NameNo               { get; private set; }
    public float                                 _Scale                { get; private set; }
    public Vector3[]                             _OtherPlanetPositions { get; set; }


    // Game Properties
    private PlanetType _PlanetType;
    private float      _GravFactor;

    public Planet(string name, int index, Sprite spr, PlanetType pt, GameObject go, float scale, int nn, float gf = 1.0f)
    {

        _GameObject = go;
        _Index = index;
        _Icon = spr;
        _Name = name;
        _NameNo = nn;
        _Scale = scale;

        _PlanetType = pt;
        _GravFactor = gf;

    }

    public void IncOrbit()
    {
            // Just calculate distance from origin (or radius of orbit)
            Vector3 sunPos = GameObject.Find("sun").transform.position;
            Vector2 vt = new Vector3(_GameObject.transform.position.x - sunPos.x, _GameObject.transform.position.y, _GameObject.transform.position.z);

            float radius;
            float orbitInc;

            radius = Mathf.Sqrt(Mathf.Pow(vt.x, 2) + Mathf.Pow(vt.y, 2));
            orbitInc = radius * 100;

            float orbitSectorAngle = (360 / orbitInc);
            float orbitSectorAngleRadians = orbitSectorAngle * Mathf.Deg2Rad;
            float nextX, nextY;

            if (_Index == 2)
            {
                nextX = sunPos.x + (vt.x * Mathf.Cos(-orbitSectorAngleRadians)) + (vt.y * Mathf.Sin(-orbitSectorAngleRadians));
                nextY = (-vt.x * Mathf.Sin(-orbitSectorAngleRadians)) + (vt.y * Mathf.Cos(-orbitSectorAngleRadians));

            }
            else
            {
                nextX = sunPos.x + (vt.x * Mathf.Cos(orbitSectorAngleRadians)) + (vt.y * Mathf.Sin(orbitSectorAngleRadians));
                nextY = (-vt.x * Mathf.Sin(orbitSectorAngleRadians)) + (vt.y * Mathf.Cos(orbitSectorAngleRadians));

            }

            // Create new v and assign it to the gameobject
            Vector3 v3 = new Vector3(nextX, nextY, -10);
            _GameObject.transform.position = v3;

    }

}