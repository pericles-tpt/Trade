using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
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
    public float                                 _SphereSize           { get; private set; }

    public Vector3                               _PlanetOrigin         { get; private set; }
    public Vector3                               _PlanetTop            { get; private set; }
    public Vector3                               _PlanetBottom         { get; private set; }

    public Vector3[]                             _OtherPlanetPositions { get; set; }
    
    
    public Sector[,]                             _PlanetSectors;
    public const int                             _SectorSize = 16;
    public float                                   _ThisSectorSize      { get; private set; }
    public LineManager _SectorLines;


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
        _SectorLines = new LineManager();

        _PlanetType = pt;
        _GravFactor = gf;

        _ThisSectorSize = (float)_SectorSize * _SphereSize;

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
        Mesh m = new Mesh();

        // Initialise _PlanetSectors 2D array for this planet
        _PlanetSectors = new Sector[maxCoord, maxCoord];

        // Create Vector3[] for holding coordinates of mesh
        Vector3[] Vertices = new Vector3[maxCoord * maxCoord];
        Vertices = AssignSphereCoordinatesToVertices(size, maxCoord);
        m.vertices = Vertices;
        
        // Create int[] for holding triangles - NOTE: Return here the triangle triplets aren't being ordered appropriately to make a sphere
        // Counts structed as timesPointUsed * noOfPoints *  noOfRows = Count
        int maxMinPointsCount          = (maxCoord * 1 * 2);
        int ringToMaxMinCount          = (2 * maxCoord * 2);
        int ringToBelowRingCount       = (3 * maxCoord * 2);
        int ringToAboveBelowRingsCount = (6 * maxCoord * (maxCoord - 3));

        int ta_size = (maxMinPointsCount + ringToMaxMinCount + ringToBelowRingCount + ringToAboveBelowRingsCount);
        Debug.Log("Triangle array size is: " + ta_size + ", for planet of size " + size);
        int[] ta = new int[ta_size];
        ta = AssignSphereTrianglesToTArray(Vertices, maxCoord, ta_size);
        m.triangles = ta;

        // Create Vector3[] for holding normals of mesh... thank god there's a function for that
        m.RecalculateNormals();

        // Assign and calculate uv's for the mesh
        Vector2[] uv = new Vector2[Vertices.Length];
        uv = CalculateUVs(Vertices, Vertices.Length);
        m.uv = uv;

        // Finally assign the new mesh to the gameobject
        m.name = "Test";
        _GameObject.GetComponent<MeshFilter>().mesh = m;

        // FIX: This doesn't belong here but this is how i'll get it to work for now... cbf to refactor this file atm
        _GameObject.GetComponent<SphereCollider>().radius = _GameObject.GetComponent<SphereCollider>().radius * size;

    }

    private string NameSector (int x, int y)
    {
        // Naming scheme is 3 first letters to planet, number of planet if valid, x coord in integers, y coord in integers.
        // e.g. HOD3-4-4
        int maxSectorSize = 2 * _SectorSize;
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
            return (_Name.Substring(0, 3).ToUpper() + _NameNo + "X" + x + "Y" + y);
        else
            return (_Name.Substring(0, 3).ToUpper() + _NameNo + "X" + numberPrefixes[index] + x + "Y" + y);

    }

    private Vector3[] AssignSphereCoordinatesToVertices (float size, float maxCoord)
    {
        Vector3[] ret = new Vector3[(int)(((maxCoord - 1) * maxCoord) + 2)];
        int i = 0;

        // Gets radius of planet in Unity units
        float r = _GameObject.GetComponent<SphereCollider>().radius * size;

        // Assign the top of the sphere to the array first
        ret[i] = new Vector3(0, 0, 0 + r);
        Debug.Log("Top " + i + ": " + ret[i].ToString());
        i++;

        Vector3 Origin = new Vector3(0, 0, 0);

        // Angles for incrementing circle along "longitude" and "latitude"
        float longAngInc = 360 / maxCoord;
        float latAngInc  = 90 / (maxCoord / 2);

        // Go from bottom to top of sphere recording blCoords creating a new sector 
        // at each point like this:

        // A ... Z
        //  1 ... n

        // Up to and including (maxCoord + 1) because including both the top and bottom point on sphere
        for (int y = 0; y < (maxCoord - 1); y++)
        {
            Vector3 Offset    = PolarToVector(r, (latAngInc * (y + 1)) * Mathf.Deg2Rad, 0);
            Vector3 lastPoint = new Vector3(Origin.x + Offset.x, Origin.y + Offset.y, Origin.z + Offset.z);

            ret[i] = lastPoint;

            // Create a new sector for each vertex on the sphere including the sector's index (for the vertices array),
            // whether the sector is an up-triangle, square or down-triangle and give the sector a name using planet
            // and position
            if (y == 0)
                _PlanetSectors[0, y] = new Sector(NameSector(0, y), Sector.Shape.triangleUp, i);
            else if (y == (maxCoord - 2))
            {
                _PlanetSectors[0, y] = new Sector(NameSector(0, y), Sector.Shape.square, i);
                _PlanetSectors[0, y] = new Sector(NameSector(0, y), Sector.Shape.triangleDown, i);

            }
            else
                _PlanetSectors[0, y] = new Sector(NameSector(0, y), Sector.Shape.square, i);

            Debug.Log("Sector Name: " + NameSector(0, y) + ", Index: " + i + ": " + ret[i].ToString());
            i++;

            for (int x = 1; x < maxCoord; x++)
            {
                Offset    = PolarToVector(r, (latAngInc * (y + 1)) * Mathf.Deg2Rad, (longAngInc * x) * Mathf.Deg2Rad);
                lastPoint = new Vector3(Origin.x + Offset.x, Origin.y + Offset.y, Origin.z + Offset.z);

                ret[i] = lastPoint;

                // Create a new sector for each vertex on the sphere including the sector's index (for the vertices array),
                // whether the sector is an up-triangle, square or down-triangle and give the sector a name using planet
                // and position
                if (y == 0)
                    _PlanetSectors[x, y] = new Sector(NameSector(x, y), Sector.Shape.triangleUp, i);
                else if (y == (maxCoord - 2))
                {
                    _PlanetSectors[x, y] = new Sector(NameSector(x, y), Sector.Shape.square, i);
                    _PlanetSectors[x, y] = new Sector(NameSector(x, y), Sector.Shape.triangleDown, i);

                }
                else
                    _PlanetSectors[x, y] = new Sector(NameSector(x, y), Sector.Shape.square, i);

                Debug.Log("Sector Name: " + NameSector(x, y) + ", Index: " + i + ": " + ret[i].ToString());
                i++;

            }

        }

        // Assign the bottom of the sphere to the array last
        ret[i] = new Vector3(0, 0, 0 - r);
        Debug.Log("Bottom " + i + ": " + ret[i].ToString());
        i++;

        return ret;

    }

    private int[] AssignSphereTrianglesToTArray(Vector3[] Vertices, int maxCoord, int size)
    {
        int[] ret = new int[size];
        int topIndex = 0;
        int botIndex = Vertices.Length - 1;
        int l = 0;

        // Do the top point to row below it
        for (int i = 1; i <= (maxCoord); i++)
        {
            ret[l] = i;
            l++;
            ret[l] = topIndex;
            l++;

            int p3t;
            if (i == maxCoord)
            {
                p3t = 1;
                ret[l] = p3t;
                l++;
            }
            else
            {
                p3t = i + 1;
                ret[l] = p3t;
                l++;
            }
            Debug.Log("triangle top: " + "(" + i + ", " + topIndex + ", " + p3t + ")");


        }

        int indexTop = l;
        Debug.Log(l + " values assigned top");

        // Do all the points in between the top and bottom
        int bottomRowIndex = ((maxCoord * (maxCoord - 2)) + 1);
        for (int i = 1; i < bottomRowIndex; i++)
        {
            int bl, br, tl, tr;
            // For triangle 1: tl, bl, tr
            Debug.Log("l is: " + l);
            tl = ret[l] = i;
            l++;
            bl = ret[l] = i + maxCoord;
            l++;
            tr = ret[l] = i + 1;
            l++;

            Debug.Log("triangle middle 1: " + "(" + tl + ", " + bl + ", " + tr + ")");

            // For triangle 2: bl, tr, br
            ret[l] = bl;
            l++;
            ret[l] = tr;
            l++;
            br = ret[l] = bl + 1;
            l++;

            Debug.Log("triangle middle 2: " + "(" + bl + ", " + tr + ", " + br + ")");

        }

        Debug.Log("values assigned middle " + (l - indexTop));

        // Do the bottom point to row above it
        int c = 0;
        Debug.Log("bottom row index " + bottomRowIndex);
        for (int i = bottomRowIndex; i < botIndex; i++)
        {
            ret[l] = i;
            l++;
            c++;
            ret[l] = botIndex;
            l++;
            c++;
            int p3b;
            if (i == (botIndex - 1))
            {
                p3b = bottomRowIndex;
                ret[l] = p3b;
                l++;
            }
            else
            {
                p3b = i + 1;
                ret[l] = p3b;
                l++;
            }
            Debug.Log("triangle bottom: " + "(" + i + ", " + botIndex + ", " + p3b + ")");

        }

        Debug.Log(c + " values assigned bottom");

        for (int i = 0; i < ret.Length; i++) {
            if ((ret[i] > ((maxCoord * (maxCoord - 1)) + 2)) || (ret[i] < 0))
                Debug.Log("Hey! " + ret[i]);
        }

        return ret;
    }

    public void DrawSectorLines(int maxCoord)
    {
        _SectorLines = new LineManager();

        Debug.Log("This planet is size: " + (maxCoord / _SectorSize));

        Vector3[] meshVertices = this._GameObject.GetComponent<MeshFilter>().mesh.vertices;

        Vector3 top    = MakeRelativeToGO(meshVertices[0]);
        Vector3 bottom = MakeRelativeToGO(meshVertices[meshVertices.Length - 1]);
        for (int y = 0; y < (maxCoord - 1) ; y++)
        {
            for (int x = 0; x < maxCoord; x++)
            {
                Vector3 curr, above, neighbour;
                curr = MakeRelativeToGO(meshVertices[_PlanetSectors[x, y]._CoordIndex]);
                Debug.Log("Drawing from current point: " + curr.ToString());
                if (y == 0)
                {
                    _SectorLines.DrawLine(top, curr);

                } else if (y == (maxCoord - 2))
                {
                    above = MakeRelativeToGO(meshVertices[_PlanetSectors[x, y - 1]._CoordIndex]);
                    _SectorLines.DrawLine(curr, above);
                    _SectorLines.DrawLine(bottom, curr);

                } else
                {
                    above = MakeRelativeToGO(meshVertices[_PlanetSectors[x, y - 1]._CoordIndex]);
                    _SectorLines.DrawLine(curr, above);

                }

                int neighbourIndex;
                if (x == (maxCoord - 1))
                    neighbourIndex = 0;
                else
                    neighbourIndex = x + 1;

                neighbour = MakeRelativeToGO(meshVertices[_PlanetSectors[neighbourIndex, y]._CoordIndex]);

                _SectorLines.DrawLine(curr, neighbour);
            }
        }
    }


    private Vector3 MakeRelativeToGO(Vector3 relativeOffset)
    {
        Vector3 goPos = this._GameObject.transform.position;
        return new Vector3(
            goPos.x + (relativeOffset.x * _SphereSize),
            goPos.y + (relativeOffset.y * _SphereSize),
            goPos.z + (relativeOffset.z * _SphereSize)
        );
    }

    private Vector2[] CalculateUVs(Vector3[] vertices, int size)
    {
        Vector2[] ret = new Vector2[size];
        for (int i = 0; i < size; i++)
        {
            float u = 0.5f + (Mathf.Atan2(vertices[i].x, vertices[i].y) / (2 * Mathf.PI));
            float v = 0.5f - (Mathf.Asin(vertices[i].z) / Mathf.PI);
            ret[i] = new Vector2(u, v);

        }
        return ret;

    }

    private Vector3 PolarToVector (float radius, float IncRad, float AziRad)
    {

        float x = radius * Mathf.Sin(IncRad) * Mathf.Cos(AziRad);
        float y = radius * Mathf.Sin(IncRad) * Mathf.Sin(AziRad);
        float z = radius * Mathf.Cos(IncRad);

        return new Vector3 (x, y, z);
    }

}