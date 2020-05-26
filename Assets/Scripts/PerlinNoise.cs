using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PerlinNoise
{
    Vector2[,] gradientVectorGrid;
    int seed;
    // v: lower is flatter, higher is hillier
    float v;
    float w;

    public PerlinNoise(int seed, float v = 1f, float w = 0.9f)
    {
        UnityEngine.Random.InitState(seed);
        this.v = v;
        this.w = w;
    }

    /*public float[,] Generate2dPerlin(int width, int height, bool outputToFile, float v = 1f, float w = 0.9f)
    {
        // 1. Generate 2D array (or grid) of gradient vectors
        gradientVectorGrid = GenerateGradientVectorGrid(width, height);

        this.v = v;
        this.w = w;
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

    }*/

    public void Generate2dPerlin(int width, int height, bool outputToFile)
    {
        // 1. Generate 2D array (or grid) of gradient vectors
        gradientVectorGrid = GenerateGradientVectorGrid(width, height);

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

        Debug.Log("x0 is " + x0);
        Debug.Log("x is " + x);

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
        this.w = sx;

        n0 = Vector2.Dot(tlg, tl - c);
        n1 = Vector2.Dot(trg, tr - c);
        ix0 = Interpolate(n0, n1);

        Debug.Log("ixo: " + (tl - c));

        n0 = Vector2.Dot(blg, bl - c);
        n1 = Vector2.Dot(brg, br - c);
        ix1 = Interpolate(n0, n1);

        this.w = sy;
        value = Interpolate(ix0, ix1);
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

    private float Interpolate(float a, float b)
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

        if (rep <= 0.5f)
            text = "W";
        else if (rep <= 1f)
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
