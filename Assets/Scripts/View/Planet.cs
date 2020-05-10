using UnityEngine;
public class Planet
{
    // Enums
    public enum PlanetType { arid, gas, molten, oceanic, ice, water, temperate }

    // Unity Properties
    public GameObject                            _GameObject      { get; private set; }
    public int                                   _Index           { get; private set; }
    public Sprite                                _Icon            { get; private set; }
    public string                                _Name            { get; private set; }
    public int                                   _NameNo          { get; private set; }
    public float                                 _SphereSize      { get; private set; }

    public Vector3                               _PlanetOrigin    { get; private set; }
    public Vector3                               _PlanetTop       { get; private set; }
    public Vector3                               _PlanetBottom    { get; private set; }

    public Vector3[]                             _OtherPlanetPositions { get; set; }
    
    
    public Sector[,]                             _PlanetSectors;
    private const int                            _SectorSize = 8;


    // Game Properties
    private PlanetType _PlanetType;
    private float      _GravFactor;
    private Country[]  _Countries;

    public Planet(string name, int index, Sprite spr, PlanetType pt, GameObject go, float ssize, int nn, float gf = 1.0f)
    {

        _GameObject = go;
        _Index = index;
        _Icon = spr;
        _Name = name;
        _SphereSize = ssize;
        _NameNo = nn;

        _PlanetType = pt;
        _GravFactor = gf;

        // Initialise planet sectors
        CreateSectors(ssize);

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
                //Debug.Log("Radius is: " + radius);

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

    private void CreateSectors(float size)
    {
        // NOTE: Need to store vector for bl corner of sector as RELATIVE since planet coords change in orbit of sun
        int maxCoord = (int)(size * _SectorSize);

        // Initialise _PlanetSectors 2D array for this planet
        _PlanetSectors = new Sector[maxCoord, maxCoord];

        // Gets radius of planet in Unity units
        float r   = _GameObject.GetComponent<SphereCollider>().radius;
        float d   = 2 * r;

        _PlanetOrigin = _GameObject.transform.position;
        float max_z = _PlanetOrigin.z + r;
        float min_z = _PlanetOrigin.z - r;

        Debug.Log("HEY ORIGIN: " + _PlanetOrigin);

        // Get the angle to increment sectors by dividing 360 (degrees) by no of increments
        float sectorAngle = 360 / maxCoord;

        // Temporary variables for storing x, y and z values
        float z;

        // Go from bottom to top of sphere recording blCoords creating a new sector 
        // at each point like this:

        // A ... Z
        //  1 ... n

        // Up to and including (maxCoord + 1) because including both the top and bottom point on sphere
        for (int i = 0; i <= (maxCoord); i++)
        {
            if (i == 0)
            {
                _PlanetTop = new Vector3(0, 0, max_z);
                Debug.Log("HEY TOP: " + _PlanetTop);

            }
            else if (i == (maxCoord))
            {
                _PlanetBottom = new Vector3(0, 0, min_z);
                Debug.Log("HEY BOTTOM: " + _PlanetBottom);

            }
            else
            {
                // z-value is the only component that stays the same for all points on the circumference of the level
                z = max_z - (((float)i / (float)maxCoord) * d);

                // Calculate y-value using pythagorus' thm
                float y = Mathf.Sqrt((Mathf.Pow(r, 2) - Mathf.Pow((z - _PlanetOrigin.z), 2)));
                Debug.Log(y);
                Debug.Log("z " + (Mathf.Pow(r,2)));
                Debug.Log("diff " + (z - _PlanetOrigin.z));
                Vector3 lastPoint = new Vector3(_PlanetOrigin.x, _PlanetOrigin.y + y, z);

                _PlanetSectors[0, i - 1] = new Sector(NameSector(0, i - 1), new Vector3(0, lastPoint.y - _PlanetOrigin.y, lastPoint.z - _PlanetOrigin.z));
                Debug.Log(lastPoint.ToString());

                if (Vector3.Distance(_PlanetOrigin, lastPoint) == r)
                    Debug.Log("Hoorah");
                else
                    Debug.Log("Nah");

                for (int j = 1; j < maxCoord; j++)
                {
                    float sectorAngleRadians = sectorAngle * Mathf.Deg2Rad;
                    float nextX, nextY;

                    nextX = (lastPoint.x * Mathf.Cos(-sectorAngleRadians)) + (lastPoint.y * Mathf.Sin(-sectorAngleRadians));
                    nextY = (-lastPoint.x * Mathf.Sin(-sectorAngleRadians)) + (lastPoint.y * Mathf.Cos(-sectorAngleRadians));

                    lastPoint = new Vector3(nextX, nextY, z);
                    _PlanetSectors[j, i - 1] = new Sector(NameSector(j, i - 1), new Vector3(lastPoint.x - _PlanetOrigin.x, lastPoint.y - _PlanetOrigin.y, lastPoint.z - _PlanetOrigin.z));
                    Debug.Log(lastPoint.ToString());

                    /*if (Vector3.Distance(lastPoint, _PlanetOrigin) == r)
                        Debug.Log("Hoorah");
                    else
                        Debug.Log("Nah");
                    */

                }


            }

        }

    }

    private string NameSector (int x, int y)
    {
        // Naming scheme is 3 first letters to planet, number of planet if valid, x coord in integers, y coord in ASCII upper.
        // e.g. HOD3-A4
        char yCoord = (char)('A' + y);

        return (_Name.Substring(0, 3).ToUpper() + _NameNo + "-" + x + yCoord);


    }

}
