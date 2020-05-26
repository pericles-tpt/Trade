using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
public class TriangleMeshGenerator
{

    // mesh = CreateSectors(scale, scale * sectorsize, ref Sector[,] Sectors = null

    public GameObject GenerateMesh(Vector3 bl, Vector3 br, Vector3 tb, bool up)
    {
        // Mesh:
        Mesh m = new Mesh();

        Vector3[] vertices = new Vector3[3]
        {
            bl,
            br,
            tb
        };
        m.vertices = vertices;

        int[] triangles;

        if (up)
        {
            triangles = new int[3]
            {
                0, 2, 1
            };
        } else
        {
            triangles = new int[3]
            {
                2, 0, 1
            };
        }
        m.triangles = triangles;

        m.RecalculateNormals();

        // Finally assign the new mesh to the gameobject
        m.name = "Triangle";

        GameObject go = new GameObject("SectorHighlight", typeof(MeshFilter), typeof(MeshRenderer));
        go.GetComponent<MeshFilter>().mesh = m;

        return go;

    }

}