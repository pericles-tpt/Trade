using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEditor.PackageManager;
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

    public Planet(string name, int index, Sector[,] sectors, Sprite spr, PlanetType pt, GameObject go, float ssize, int nn, float gf = 1.0f)
    {

        _GameObject = go;
        _Index = index;
        _Icon = spr;
        _Name = name;
        _SphereSize = ssize;
        _PlanetSectors = sectors;
        _NameNo = nn;
        _SectorLines = new LineManager();

        _PlanetType = pt;
        _GravFactor = gf;

        _ThisSectorSize = (float)_SectorSize * _SphereSize;

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

    public void DrawQuadSectorBoundaries(Vector3 bl, Vector3 br, Vector3 tl, Vector3 tr)
    {
        bl = MakeRelativeToGO(bl);
        br = MakeRelativeToGO(br);
        tl = MakeRelativeToGO(tl);
        tr = MakeRelativeToGO(tr);

        this._SectorLines.DrawLine(bl, br);
        this._SectorLines.DrawLine(br, tr);
        this._SectorLines.DrawLine(tr, tl);
        this._SectorLines.DrawLine(tl, bl);

    }

    public void DrawTriangleSectorBoundaries(Vector3 bl, Vector3 br, Vector3 tb)
    {
        bl = MakeRelativeToGO(bl);
        br = MakeRelativeToGO(br);
        tb = MakeRelativeToGO(tb);

        this._SectorLines.DrawLine(bl, br);
        this._SectorLines.DrawLine(br, tb);
        this._SectorLines.DrawLine(tb, bl);

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

}