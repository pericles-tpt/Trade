using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TreeEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PerlinNoise
{
    Vector2[,] gradientVectorGrid;

    public float[,] Generate2dPerlin(int width, int height, bool outputToFile, int subGridSize = 32)
    {
        if ((width % subGridSize != 0) || (height % subGridSize != 0))
                throw new Exception("The provided width and height are NOT a multiple of the subgrid size");

        // 1. Generate 2D array (or grid) of gradient vectors
        gradientVectorGrid = GenerateGradientVectorGrid(width, height);

        // 2. Calculate dot product of closest 2^n (where n is dimension of
        // grid) point, i.e. for a 2D grid up, down, left and right

        // 3. Also interpolate dot products produced inside of GenerateDotProductGrid
        // function
        //float[,] dotProductGrid = GenerateDotProductGrid(gradientVectorGrid, subGridSize);
        float[,] perlinValues = new float[width, height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
                perlinValues[j, i] = GeneratePerlinValue(j, i, width, height);
        }

        return perlinValues;

    }


    public Vector2[,] GenerateGradientVectorGrid(int width, int height)
    {

        Vector2[,] ret = new Vector2[width, height];

        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                ret[j, i] = new Vector3(UnityEngine.Random.Range(-100f, 100f), UnityEngine.Random.Range(-100f, 100f)).normalized;

            }

        }
        return ret;
    }

    public float GeneratePerlinValue(float x, float y, int width, int height)
    {
        int x0 = (int)x;
        int y0 = (int)y;

        int x1;
        if (x0 + 1 < (width - 1))
            x1 = x0 + 1;
        else
            x1 = x0 - 1;

        int y1;
        if (y0 + 1 < (height - 1))
            y1 = y0 + 1;
        else
            y1 = y0 - 1;

        Vector2 tl = new Vector2(x0, y0);
        Vector2 tr = new Vector2(x1, y0);
        Vector2 bl = new Vector2(x0, y1);
        Vector2 br = new Vector2(x1, y1);

        Debug.Log("tl " + tl);

        Vector2 tlg = gradientVectorGrid[x0, y0];
        Vector2 trg = gradientVectorGrid[x1, y0];
        Vector2 blg = gradientVectorGrid[x0, y1];
        Vector2 brg = gradientVectorGrid[x1, y1];

        Debug.Log("tlg " + tlg);

        Vector2 c = new Vector2(x, y);

        Debug.Log("tl - c" + (tl - c));
        Debug.Log((tr - c));
        Debug.Log((bl - c));
        Debug.Log((br - c));

        float sx = x - (float)x0;
        float sy = y - (float)y0;

        float n0, n1, ix0, ix1, value;

        n0 = Vector2.Dot(tlg, tl - c);
        n1 = Vector2.Dot(trg, tr - c);
        ix0 = Interpolate(n0, n1, sx);

        Debug.Log("ixo: " + (tl - c));

        n0 = Vector2.Dot(blg, bl - c);
        n1 = Vector2.Dot(brg, br - c);
        ix1 = Interpolate(n0, n1,sx);

        value = Interpolate(ix0, ix1, sy);
        Debug.Log("The perlin value is " + value);
        return value;
    }

    private float dotGridGradient(int ix, int iy, float x, float y)
    {
        // Compute the distance vector
        float dx = x - (float)ix;
        float dy = y - (float)iy;

        // Compute the dot-product
        //Debug.Log("ix: " + ix + ", iy: " + iy + ", x: " + x + ", y: " + y);
        return (dx * gradientVectorGrid[ix,iy].x + dy * gradientVectorGrid[ix,iy].y);
    }

    private float[,] GenerateDotProductGrid(Vector2[,] gradientVectorGrid, int subGridSize)
    {
        int width = gradientVectorGrid.GetLength(0);
        int height = gradientVectorGrid.GetLength(1);
        float[,] ret = new float[(width) * subGridSize, (height) * subGridSize];

        int chunkNoWidth = width;
        int chunkNoHeight = height;

        for (int i = 0; i < (chunkNoHeight - 1); i++)
        {
            for (int j = 0; j < (chunkNoWidth - 1); j++)
            {
                for (int y = 0; y < subGridSize + 1; y++)
                {
                    for (int x = 0; x < subGridSize + 1; x++)
                    {
                        // Indexes for each corner of the square that the perlin point falls into
                        int tlIndexX = j;
                        int tlIndexY = i;
                        int trIndexX = j + 1;
                        int trIndexY = i;
                        int blIndexX = j;
                        int blIndexY = i + 1;
                        int brIndexX = j + 1;
                        int brIndexY = i + 1;


                        // Fetch tl, tr, bl, br gradient values values
                        Vector2 tl = gradientVectorGrid[tlIndexX, tlIndexY];
                        Vector2 tr = gradientVectorGrid[trIndexX, trIndexY];
                        Vector2 bl = gradientVectorGrid[blIndexX, blIndexY];
                        Vector2 br = gradientVectorGrid[brIndexX, brIndexY];

                        // Compute distance to candidate point of each value above
                        Vector2 candidate = new Vector2((float)j + ((float)x / (float)subGridSize), (float)i + ((float) y / (float)subGridSize));

                        // NOTE: The indexes here are multiplied by subGridSize as the corners of the square that
                        // the perlin point falls into are incremented by 1 but each increment in the outer grid
                        // it equivalent to subGridSize increments in the smaller grid
                        Vector2 tld = (candidate - new Vector2(tlIndexX, tlIndexY));
                        Vector2 trd = (candidate - new Vector2(trIndexX, trIndexY));
                        Vector2 bld = (candidate - new Vector2(blIndexX, blIndexY));
                        Vector2 brd = (candidate - new Vector2(brIndexX, brIndexY));

                        Debug.Log("candidate is " + candidate);
                        Debug.Log("tld is " + tld + "tl is " + tl);

                        float sx = candidate.x - tl.x;
                        float sy = candidate.y - tl.y;

                        if (sx > 1f || sx < 0f)
                            Debug.Log("sx Out of range bud");

                        if (sy > 1f || sy < 0f)
                            Debug.Log("sy Out of range bud");

                        // Computer dot product of distance vector and gradient vectors
                        float A = Vector2.Dot(tld, tl);
                        float B = Vector2.Dot(trd, tr);
                        float AB = Interpolate(A, B, sx);

                        Debug.Log("A: " + A + ", B: " + B);
                        Debug.Log("AB " + AB);

                        float C = Vector2.Dot(bld, bl);
                        float D = Vector2.Dot(brd, br);
                        float CD = Interpolate(C, D, sx);

                        Debug.Log("CD " + CD);

                        Debug.Log("At chunk: " + j + ", " + i + ", with x: " + x + ", y: " + y);

                        // Interpolate between dot products here
                        ret[x, y] = Interpolate(AB, CD, sy);

                    }

                }
            }

        }

        return ret;
    }

    // v: lower is flatter
    private float Interpolate(float a, float b, float w = 0.9f, float v = 5f)
    {
        // Function taken from Perlin Noise wikipedia article... may want to pick 
        // a different one later
        return v * ((1f - w) * a + (w * b));
    }

    private void WriteToFile(float[,] perlinValues)
    {
        string text = "";

        for (int i = 0; i < perlinValues.GetLength(1); i++)
        {
            for (int j = 0; j < perlinValues.GetLength(0); j++)
            {
                Debug.Log("i: " + i + ", j: " + j + "value: " + perlinValues[j, i]);
                text += FormatForWrite(perlinValues[j, i]);
            }

            text += '\n';
        }

        System.IO.File.WriteAllText(@"C:\Users\peric\Desktop\perlin2.txt", text);
    }

    private string FormatForWrite(float value)
    {
        string text;

        float rep = value / 10;

        //return text;
        string frontAppend = "";
        if (rep >= 0f)
            frontAppend = "0";
        else {
            rep *= -1;
            frontAppend = "0";
        }

        if (rep <= 1f)
            text = "W";
        else if (rep <= 2f)
            text = "G";
        else
            text = "X";

        string endAppend = "|";
        if ((frontAppend + Math.Round(rep, 2)).Length == 4)
            endAppend = "0|";
        else if ((frontAppend + Math.Round(rep, 2)).Length == 2)
            endAppend = ".00|";
        else if ((frontAppend + Math.Round(rep, 2)).Length == 1)
            endAppend = "0.00|";

        //return frontAppend + Math.Round(rep, 2) + endAppend;
        return text;

    }
}
