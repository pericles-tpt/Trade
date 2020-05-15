using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
public class SphereMeshGenerator
{

    // mesh = CreateSectors(scale, scale * sectorsize, ref Sector[,] Sectors = null

    public Mesh GenerateMesh(float scale, int divisions, ref Sector[,] storedSectors)    {
        // NOTE: This is separate, used for storing vertices of mesh in Planet class
        storedSectors = new Sector[divisions, divisions];

        // Mesh:
        Mesh m = new Mesh();

        // Vertices:
        Vector3[] Vertices = CalculateVertices(scale, divisions, ref storedSectors);

        // Triangles:
        // Counts = timesPointUsed * noOfPoints *  noOfRows
        int maxMinPointsCount = (divisions * 1 * 2);
        int ringToMaxMinCount = (2 * divisions * 2);
        int ringToBelowRingCount = (3 * divisions * 2);
        int ringToAboveBelowRingsCount = (6 * divisions * (divisions - 3));

        int ta_size = (maxMinPointsCount + ringToMaxMinCount + ringToBelowRingCount + ringToAboveBelowRingsCount);
        Debug.Log("Triangle array size is: " + ta_size + ", for planet of size " + scale);
        int[] TriangleArray = DefineTriangles(Vertices, divisions, ta_size);

        // UV's: Assign and calculate uv's for the mesh
        Vector2[] UV = CalculateUVs(Vertices, Vertices.Length);

        // Normals:
        Vector3[] Normals = CalculateNormals(Vertices);//CalculateNormals(Vertices);


        // Finally assign the new mesh to the gameobject
        m.name = "Test";
        m.vertices = Vertices;
        m.normals = Normals;
        m.uv = UV;
        m.triangles = TriangleArray;

        return m;

    }

    private Vector3[] CalculateVertices(float scale, float divisions, ref Sector[,] storedSectors)
    {
        Vector3[] ret = new Vector3[(int)(((divisions - 1) * divisions) + 2)];
        int i = 0;

        // Gets radius of planet in Unity units
        float d = scale;
        float r = d / 2;

        // Assign the top of the sphere to the array first
        ret[i] = new Vector3(0, 0, 0 + r);
        Debug.Log("Top " + i + ": " + ret[i].ToString());
        i++;

        Vector3 Origin = new Vector3(0, 0, 0);

        // Angles for incrementing circle along "longitude" and "latitude"
        float longAngInc = 360 / divisions;
        float latAngInc  = 90 / (divisions / 2);

        // Go from bottom to top of sphere recording blCoords creating a new sector 
        // at each point like this:

        // A ... Z
        //  1 ... n

        // Up to and including (maxCoord + 1) because including both the top and bottom point on sphere
        for (int y = 0; y < (divisions - 1); y++)
        {
            Vector3 Offset = PolarToVector(r, (latAngInc * (y + 1)) * Mathf.Deg2Rad, 0);
            Vector3 lastPoint = new Vector3(Origin.x + Offset.x, Origin.y + Offset.y, Origin.z + Offset.z);

            ret[i] = lastPoint;

            // Create a new sector for each vertex on the sphere including the sector's index (for the vertices array),
            // whether the sector is an up-triangle, square or down-triangle and give the sector a name using planet
            // and position
            if (y == 0)
                storedSectors[0, y] = new Sector(Sector.NameSector(0, y), Sector.Shape.triangleUp, i);
            else if (y == (divisions - 2))
            {
                storedSectors[0, y] = new Sector(Sector.NameSector(0, y), Sector.Shape.square, i);
                storedSectors[0, y] = new Sector(Sector.NameSector(0, y), Sector.Shape.triangleDown, i);

            }
            else
                storedSectors[0, y] = new Sector(Sector.NameSector(0, y), Sector.Shape.square, i);

            Debug.Log("Sector Name: " + Sector.NameSector(0, y) + ", Index: " + i + ": " + ret[i].ToString());
            i++;

            for (int x = 1; x < divisions; x++)
            {
                Offset = PolarToVector(r, (latAngInc * (y + 1)) * Mathf.Deg2Rad, (longAngInc * x) * Mathf.Deg2Rad);
                lastPoint = new Vector3(Origin.x + Offset.x, Origin.y + Offset.y, Origin.z + Offset.z);

                ret[i] = lastPoint;

                // Create a new sector for each vertex on the sphere including the sector's index (for the vertices array),
                // whether the sector is an up-triangle, square or down-triangle and give the sector a name using planet
                // and position
                if (y == 0)
                    storedSectors[x, y] = new Sector(Sector.NameSector(x, y), Sector.Shape.triangleUp, i);
                else if (y == (divisions - 2))
                {
                    storedSectors[x, y] = new Sector(Sector.NameSector(x, y), Sector.Shape.square, i);
                    storedSectors[x, y] = new Sector(Sector.NameSector(x, y), Sector.Shape.triangleDown, i);

                }
                else
                    storedSectors[x, y] = new Sector(Sector.NameSector(x, y), Sector.Shape.square, i);

                Debug.Log("Sector Name: " + Sector.NameSector(x, y) + ", Index: " + i + ": " + ret[i].ToString());
                i++;

            }

        }

        // Assign the bottom of the sphere to the array last
        ret[i] = new Vector3(0, 0, 0 - r);
        Debug.Log("Bottom " + i + ": " + ret[i].ToString());
        i++;

        return ret;

    }

    private int[] DefineTriangles(Vector3[] Vertices, int maxCoord, int size)
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
            bl = ret[l] = i + maxCoord;
            l++;
            tl = ret[l] = i;
            l++;
            tr = ret[l] = i + 1;
            l++;

            // For triangle 2: bl, tr, br
            ret[l] = bl;
            l++;
            ret[l] = tr;
            l++;
            br = ret[l] = bl + 1;
            l++;

            Debug.Log("triangle 1: " + "(" + bl + ", " + tl + ", " + tr + "), triangle 2: (" + bl + ", " + tr + ", " + br + ")");

        }

        Debug.Log("values assigned middle " + (l - indexTop));

        // Do the bottom point to row above it
        int c = 0;
        Debug.Log("bottom row index " + bottomRowIndex);
        for (int i = bottomRowIndex; i < botIndex; i++)
        {
            ret[l] = botIndex;
            l++;
            c++;
            ret[l] = i;
            l++;
            c++;
            int p3b;
            if (i == (botIndex - 1))
            {
                p3b = bottomRowIndex;
                ret[l] = p3b;
                Debug.Log("botIndex: " + botIndex + ", i: " + i + ", bottomRowIndex: " + bottomRowIndex);
                l++;
            }
            else
            {
                p3b = i + 1;
                ret[l] = p3b;
                l++;
            }
            //Debug.Log("triangle bottom: " + "(" + i + ", " + botIndex + ", " + p3b + ")");

        }

        Debug.Log(c + " values assigned bottom");

        for (int i = 0; i < ret.Length; i++)
        {
            if ((ret[i] > ((maxCoord * (maxCoord - 1)) + 2)) || (ret[i] < 0))
                Debug.Log("Hey! " + ret[i]);
        }

        return ret;
    }

    private Vector2[] CalculateUVs(Vector3[] vertices, int size)
    {
        Vector2[] ret = new Vector2[size];
        for (int i = 0; i < size; i++)
        {
            //Vector3 pts = new Vector3(-vertices[i].x, -vertices[i].y, -vertices[i].z);
            //float mag = Mathf.Sqrt(Mathf.Pow(pts.x, 2f) + Mathf.Pow(pts.y, 2f) + Mathf.Pow(pts.z, 2f));
            //Vector3 puv = new Vector3(pts.x / mag, pts.y / mag, pts.z / mag);

            Vector3 c = vertices[i];

            float u = 0.5f + (Mathf.Atan2(c.x, c.z) / (-2 * Mathf.PI));

            if (u < 0f)
                u += 1f;

            float v = /*0.5f - (Mathf.Asin(c.y) / Mathf.PI)*/ Mathf.Asin(c.y) / Mathf.PI + 0.5f;
            ret[i] = new Vector2(u, v);

        }
        return ret;

    }

    private Vector3[] CalculateNormals(Vector3[] vertices)
    {
        Vector3[] ret = new Vector3[vertices.Length];
        int i = 0;
        foreach (Vector3 v in vertices)
        {
            //float mag = Mathf.Sqrt(Mathf.Pow((2 * v.x), 2) + Mathf.Pow((2 * v.y), 2) + Mathf.Pow((2 * v.z), 2));
            //Vector3 nv = new Vector3((2*v.x) / mag, (2*v.y) / mag, (2*v.z) / mag);
            ret[i] = v.normalized;
            i++;
        }
        return ret;
    }

    public static Vector3 PolarToVector(float radius, float IncRad, float AziRad)
    {

        float x = radius * Mathf.Sin(IncRad) * Mathf.Cos(AziRad);
        float y = radius * Mathf.Sin(IncRad) * Mathf.Sin(AziRad);
        float z = radius * Mathf.Cos(IncRad);

        return new Vector3(x, y, z);
    }

    public static Dictionary<string, float> VectorToPolar(Vector3 pointRelativeToOrigin)
    {
        float r = Mathf.Sqrt(Mathf.Pow(pointRelativeToOrigin.x, 2) + Mathf.Pow(pointRelativeToOrigin.y, 2) + Mathf.Pow(pointRelativeToOrigin.z, 2));
        float Azi = Mathf.Atan(pointRelativeToOrigin.y / pointRelativeToOrigin.x);
        float Inc = pointRelativeToOrigin.z / r;

        return new Dictionary<string, float>() {
            { "r", r },
            { "Azi", Azi },
            { "Inc", Inc}
        };
    }

}