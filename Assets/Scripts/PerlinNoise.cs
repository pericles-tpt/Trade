using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class PerlinNoise : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float[,] Generate2dPerlin(int width, int height, bool outputToFile, int subGridSize = 4)
    {
        if ((width % subGridSize != 0) || (height % subGridSize != 0))
                throw new Exception("The provided width and height are NOT a multiple of the subgrid size");

        // 1. Generate 2D array (or grid) of gradient vectors
        Vector2[,] gradientVectorGrid = GenerateGradientVectorGrid((width / subGridSize) + 1, (height/subGridSize) + 1);

        // 2. Calculate dot product of closest 2^n (where n is dimension of
        // grid) point, i.e. for a 2D grid up, down, left and right

        // 3. Also interpolate dot products produced inside of GenerateDotProductGrid
        // function
        float[,] dotProductGrid = GenerateDotProductGrid(gradientVectorGrid, subGridSize);

        if (outputToFile)
            WriteToFile(dotProductGrid);


        return dotProductGrid;

    }


    public Vector2[,] GenerateGradientVectorGrid(int width, int height)
    {

        Vector2[,] ret = new Vector2[width, height];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                ret[j, i] = new Vector2(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100)).normalized;

            }

        }
        return ret;
    }
    private float[,] GenerateDotProductGrid(Vector2[,] gradientVectorGrid, int subGridSize)
    {
        int width = gradientVectorGrid.GetLength(0);
        int height = gradientVectorGrid.GetLength(1);
        float[,] ret = new float[(width - 1) * subGridSize, (height - 1) * subGridSize];

        int chunkNoWidth = width;
        int chunkNoHeight = height;

        for (int i = 0; i < (chunkNoHeight - 1); i++)
        {
            for (int j = 0; j < (chunkNoWidth - 1); j++)
            {
                for (int y = (i * 4); y < (i * 4) + 4; y++)
                {
                    for (int x = (j * 4); x < (j * 4) + 4; x++)
                    {
                        // Fetch tl, tr, bl, br values
                        Vector2 tl = gradientVectorGrid[j, i];
                        Vector2 tr = gradientVectorGrid[j + 1, i];
                        Vector2 bl = gradientVectorGrid[j, i + 1];
                        Vector2 br = gradientVectorGrid[j + 1, i + 1];

                        // Compute distance to candidate point of each value above
                        Vector2 candidate = new Vector2(x, y);

                        Vector2 tld = (tl - candidate);
                        Vector2 trd = (tr - candidate);
                        Vector2 bld = (bl - candidate);
                        Vector2 brd = (br - candidate);

                        // Computer dot product of distance vector and gradient vectors
                        float A = Vector2.Dot(tld, tl);
                        float B = Vector2.Dot(trd, tr);
                        float AB = Interpolate(A, B);

                        float C = Vector2.Dot(bld, bl);
                        float D = Vector2.Dot(brd, br);
                        float CD = Interpolate(C, D);

                        Debug.Log("At chunk: " + j + ", " + i + ", with x: " + x + ", y: " + y);

                        // Interpolate between dot products here
                        ret[x, y] = Interpolate(AB, CD);

                    }

                }
            }
        }

        return ret;
    }

    private float Interpolate(float a, float b, float w = 0.9f)
    {
        // Function taken from Perlin Noise wikipedia article... may want to pick 
        // a different one later
        return (1f - w) * a + w * b;
    }

    private void WriteToFile(float[,] perlinValues)
    {
        string text = "";

        for (int i = 0; i < perlinValues.GetLength(1); i++)
        {
            for (int j = 0; j < perlinValues.GetLength(0); j++)
            {
                Debug.Log("i: " + i + ", j: " + j + "value: " + perlinValues[j, i]);
                if (perlinValues[j, i] <= 0f)
                {
                    text += "WW";
                } else
                {
                    if (perlinValues[j, i] < 10f)
                        text += "00";
                    else if (perlinValues[j, i] < 20f)
                        text += "01";
                    else if (perlinValues[j, i] < 30f)
                        text += "02";
                    else if (perlinValues[j, i] < 40f)
                        text += "03";
                    else if (perlinValues[j, i] < 50f)
                        text += "04";
                    else if (perlinValues[j, i] < 60f)
                        text += "05";
                    else if (perlinValues[j, i] < 70f)
                        text += "06";
                    else if (perlinValues[j, i] < 80f)
                        text += "07";
                    else if (perlinValues[j, i] < 90f)
                        text += "08";
                    else if (perlinValues[j, i] < 100f)
                        text += "09";
                    else if (perlinValues[j, i] < 110f)
                        text += "10";
                    else if (perlinValues[j, i] < 120f)
                        text += "11";
                    else if (perlinValues[j, i] < 130f)
                        text += "12";
                    else if (perlinValues[j, i] < 140f)
                        text += "13";
                    else if (perlinValues[j, i] < 150f)
                        text += "14";
                    else if (perlinValues[j, i] < 160f)
                        text += "15";
                    else if (perlinValues[j, i] < 170f)
                        text += "16";
                    else if (perlinValues[j, i] < 180f)
                        text += "17";
                    else if (perlinValues[j, i] < 190f)
                        text += "18";
                    else if (perlinValues[j, i] < 200f)
                        text += "19";
                    else if (perlinValues[j, i] < 210f)
                        text += "20";
                    else if (perlinValues[j, i] < 220f)
                        text += "21";
                    else if (perlinValues[j, i] < 230f)
                        text += "22";
                    else if (perlinValues[j, i] < 240f)
                        text += "23";
                    else if (perlinValues[j, i] < 250f)
                        text += "24";
                    else if (perlinValues[j, i] < 260f)
                        text += "25";
                    else if (perlinValues[j, i] < 270f)
                        text += "26";
                    else if (perlinValues[j, i] < 280f)
                        text += "27";
                    else if (perlinValues[j, i] < 290f)
                        text += "28";
                    else if (perlinValues[j, i] < 300f)
                        text += "29";
                    else if (perlinValues[j, i] < 310f)
                        text += "30";
                    else
                        text += "31";
                }
                //text += perlinValues[j, i] + "|";
            }

            text += '\n';
        }

        System.IO.File.WriteAllText(@"C:\Users\peric\Desktop\perlin.txt", text);
    }
}
