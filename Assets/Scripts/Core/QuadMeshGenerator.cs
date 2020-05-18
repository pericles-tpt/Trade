using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
public class QuadMeshGenerator
{

    // mesh = CreateSectors(scale, scale * sectorsize, ref Sector[,] Sectors = null

    public GameObject GenerateMesh(Vector3 bl, Vector3 br, Vector3 tl, Vector3 tr)
    {
        // Mesh:
        Mesh m = new Mesh();

        Vector3[] vertices = new Vector3[4]
        {
            bl,
            br,
            tl,
            tr
        };
        m.vertices = vertices;

        int[] triangles = new int[6]
        {
            0, 2, 1,
            1, 2, 3
        };
        m.triangles = triangles;

        m.RecalculateNormals();

        Vector2[] uv = new Vector2[4]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(0, 1),
            new Vector2(1, 1)
        };
        m.uv = uv;

        // Finally assign the new mesh to the gameobject
        m.name = "Quad";

        GameObject go = new GameObject("SectorHighlight", typeof(MeshFilter), typeof(MeshRenderer));
        go.GetComponent<MeshFilter>().mesh = m;

        return go;

    }

}