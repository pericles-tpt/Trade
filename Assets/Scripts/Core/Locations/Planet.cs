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
            Vector2 vt = new Vector3(_GameObject.transform.position.x - 6, _GameObject.transform.position.y, _GameObject.transform.position.z);
            Vector2 v = _GameObject.transform.position;

            float radius;
            float orbitInc;
            float orbitCirc;

            if (vt.x == 0)
            {
                radius = vt.y;

            }

            else if (vt.y == 0)
            {
                radius = vt.x;

            }

            else
            {
                radius = Mathf.Sqrt(Mathf.Pow(vt.x, 2) + Mathf.Pow(vt.y, 2));
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


    private Vector3 MakeRelativeToGO(Vector3 relativeOffset)
    {
        Vector3 goPos = this._GameObject.transform.position;
        return new Vector3(
            goPos.x + (relativeOffset.x * _SphereSize),
            goPos.y + (relativeOffset.y * _SphereSize),
            goPos.z + (relativeOffset.z * _SphereSize)
        );
    }

    private Vector3 MakeRelativeToGONotScaled(Vector3 relativeOffset)
    {
        Vector3 goPos = this._GameObject.transform.position;
        return new Vector3(
            goPos.x + (relativeOffset.x),
            goPos.y + (relativeOffset.y),
            goPos.z + (relativeOffset.z)
        );
    }

}