using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;
public class SphereMeshGenerator
{

    // mesh = CreateSectors(scale, scale * sectorsize, ref Sector[,] Sectors = null

    public Mesh GenerateMesh(float scale, int divisions)    {
        // Mesh:
        Mesh m1 = new Mesh();

        // Vertices:
        Vector3[] Vertices = CalculateVertices(scale, divisions);

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
        Vector2[] UV = CalculateUVs(Vertices, Vertices.Length, divisions);

        // Normals:
        Vector3[] Normals1 = CalculateNormals(Vertices, 1);

        // Finally assign the new mesh to the gameobject
        m1.name = "Test";
        m1.vertices = Vertices;
        m1.normals = Normals1;
        m1.uv = UV;
        m1.triangles = TriangleArray;

        return m1;

    }

    // CalculateVertices function for generating new preset mesh in unity
    private Vector3[] CalculateVertices(float scale, float divisions)
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

            Debug.Log("Sector Name: " + Sector.NameSector(0, y) + ", Index: " + i + ": " + ret[i].ToString());
            i++;

            for (int x = 1; x < divisions; x++)
            {
                Offset = PolarToVector(r, (latAngInc * (y + 1)) * Mathf.Deg2Rad, (longAngInc * x) * Mathf.Deg2Rad);
                lastPoint = new Vector3(Origin.x + Offset.x, Origin.y + Offset.y, Origin.z + Offset.z);

                ret[i] = lastPoint;

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

    // Calculate vertices for storedSectors in Planet - Maybe put in Planet class?
    public void CalculateVertices(float scale, float divisions, ref Sector[,] storedSectors)
    {
        int i = 0;
        i++;

        // Gets radius of planet in Unity units
        float d = scale;
        float r = d / 2;

        Vector3 Origin = new Vector3(0, 0, 0);

        // Angles for incrementing circle along "longitude" and "latitude"
        float longAngInc = 360 / divisions;
        float latAngInc = 90 / (divisions / 2);

        // Go from bottom to top of sphere recording blCoords creating a new sector 
        // at each point like this:

        // A ... Z
        //  1 ... n

        // Up to and including (maxCoord + 1) because including both the top and bottom point on sphere
        for (int y = 0; y < (divisions - 1); y++)
        {
            Vector3 Offset = PolarToVector(r, (latAngInc * (y + 1)) * Mathf.Deg2Rad, 0);
            Vector3 lastPoint = new Vector3(Origin.x + Offset.x, Origin.y + Offset.y, Origin.z + Offset.z);

            // Create a new sector for each vertex on the sphere including the sector's index (for the vertices array),
            // whether the sector is an up-triangle, square or down-triangle and give the sector a name using planet
            // and position
            if (y == 0)
                storedSectors[0, y] = new Sector(Sector.NameSector(0, y), Sector.Shape.triangleDown, i);
            else if (y == (divisions - 2))
            {
                storedSectors[0, y] = new Sector(Sector.NameSector(0, y), Sector.Shape.square, i);
                storedSectors[0, y] = new Sector(Sector.NameSector(0, y), Sector.Shape.triangleUp, i);

            }
            else
                storedSectors[0, y] = new Sector(Sector.NameSector(0, y), Sector.Shape.square, i);

            Debug.Log("Sector Name: " + Sector.NameSector(0, y) + ", Index: " + i + ": " + +i); // i was ret[i].ToString()
            i++;

            for (int x = 1; x < divisions; x++)
            {
                Offset = PolarToVector(r, (latAngInc * (y + 1)) * Mathf.Deg2Rad, (longAngInc * x) * Mathf.Deg2Rad);
                lastPoint = new Vector3(Origin.x + Offset.x, Origin.y + Offset.y, Origin.z + Offset.z);

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

                Debug.Log("Sector Name: " + Sector.NameSector(x, y) + ", Index: " + i + ": " + i); // i was ret[i].ToString()
                i++;

            }

        }

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

            ret[l] = topIndex;
            l++;
            ret[l] = i;
            l++;

            Debug.Log("triangle top: " + "(" + i + ", " + topIndex + ", " + p3t + ")");


        }

        int indexTop = l;
        Debug.Log(l + " values assigned top");

        // Do all the points in between the top and bottom
        int bottomRowIndex = ((maxCoord * (maxCoord - 2)) + 1);
        for (int i = 1; i < bottomRowIndex; i++)
        {
            int bl, br, tl, tr;
            Debug.Log("l is: " + l);
            if (i % maxCoord == 0)
            {
                Debug.Log("Index is " + i + " at i % maxCoord == 0");
                tr = ret[l] = i - (maxCoord - 1);
                l++;
                br = ret[l] = i;
                l++;
                bl = ret[l] = i + maxCoord;
                l++;

                // For triangle 2: bl, tr, br
                tl = ret[l] = tr + maxCoord;
                l++;
                ret[l] = tr;
                l++;
                ret[l] = bl;
                l++;

            }
            else
            {
                tr = ret[l] = i + 1;
                l++;
                tl = ret[l] = i;
                l++;
                bl = ret[l] = i + maxCoord;
                l++;

                // For triangle 2: bl, tr, br
                br = ret[l] = bl + 1;
                l++;
                ret[l] = tr;
                l++;
                ret[l] = bl;
                l++;
            }


            Debug.Log("triangle 1: " + "(" + bl + ", " + tl + ", " + tr + "), triangle 2: (" + bl + ", " + tr + ", " + br + ")");

        }

        Debug.Log("values assigned middle " + (l - indexTop));

        // Do the bottom point to row above it
        int c = 0;
        Debug.Log("bottom row index " + bottomRowIndex);
        for (int i = bottomRowIndex; i < botIndex; i++)
        {
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
            c++;
            //Debug.Log("triangle bottom: " + "(" + i + ", " + botIndex + ", " + p3b + ")");
            ret[l] = i;
            l++;
            c++;
            ret[l] = botIndex;
            l++;
            c++;

        }

        Debug.Log(c + " values assigned bottom");

        for (int i = 0; i < ret.Length; i++)
        {
            if ((ret[i] > ((maxCoord * (maxCoord - 1)) + 2)) || (ret[i] < 0))
                Debug.Log("Hey! " + ret[i]);
        }

        return ret;
    }

    private Vector2[] CalculateUVs(Vector3[] vertices, int size, int divisions)
    {

        int multipleOf = -1;
        Vector2[] ret = new Vector2[size];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 c = vertices[i];

            Debug.Log("The length of this bad boy is " + vertices.Length);

            float u, v;

            u = 0.5f + ((Mathf.Atan2(c.x, c.y)) / (2 * Mathf.PI));
            v = 0.5f - ((Mathf.Asin(c.z)) / (1 * Mathf.PI));


            ret[i] = new Vector2(u, v);
        }

        return ret;

    }

    private Vector3[] CalculateNormals(Vector3[] vertices, int normalsSign)
    {
        Vector3[] ret = new Vector3[vertices.Length];
        int i = 0;
        Debug.Log("VERTICES IS " + vertices.Length + " HALF VERTICES IS " + vertices.Length / 2);
        foreach (Vector3 v in vertices)
        {
            ret[i] = new Vector3(normalsSign * v.x, normalsSign * v.y, normalsSign * v.z);
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