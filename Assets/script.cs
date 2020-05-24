using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class script : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject sandTile;
    public GameObject waterTile;

    public GameObject shadowAbove, shadowBelow, shadowLeft, shadowRight;

    void Start()
    {

        // Number of sectors = n * n
        // Chunks per sector, more chunks = more hilliness 

        int chunksPerSector = 2;
        int seed = UnityEngine.Random.Range(0, int.MaxValue);

        int planetSectorMax = 32;
        int perlinWidthHeight = planetSectorMax * chunksPerSector; 

        PerlinNoise pn = new PerlinNoise(seed);
        float[,] tiles = pn.Generate2dPerlin(perlinWidthHeight, perlinWidthHeight, false, 1f);

        int sectorWidth = 256;
        int sectorHeight = 144;

        // 4 / 128

        for (int i = 0; i < sectorHeight; i++)
        {
            float iPerlinIndex = (float)i / (sectorHeight / chunksPerSector);
            for (int j = 0; j < sectorWidth; j++)
            {
                float jPerlinIndex = (float)j / (sectorWidth / chunksPerSector);
                float depth = Mathf.Clamp(pn.GeneratePerlinValue(jPerlinIndex, iPerlinIndex, perlinWidthHeight, perlinWidthHeight) + 0.5f, 0f, 1f);
                GameObject curr, shadow;

                if (depth <= 0.3f)
                {
                    curr = Instantiate(waterTile);
                    //curr = Instantiate(sandTile);
                }
                else
                {
                    curr = Instantiate(sandTile);
                }



                curr.transform.position = new Vector3(((float)j * 32f / 100f), ((float)i * 32f / 100f), depth);
                Vector3 aboveCurr = new Vector3(curr.transform.position.x, curr.transform.position.y, curr.transform.position.z + 0.1f);

                if (depth > 0.5f)
                {
                    float depthAbove, depthBelow, depthLeft, depthRight;

                    if ((i - 1) >= 0)
                    {
                        depthAbove = Mathf.Clamp(pn.GeneratePerlinValue(jPerlinIndex, iPerlinIndex, perlinWidthHeight, perlinWidthHeight) + 0.5f, 0f, 1f);
                        if (depthAbove > depth)
                        {
                            shadow = Instantiate(shadowAbove);
                            shadow.transform.position = aboveCurr;
                        }
                    }
                    if ((i + 1) < tiles.GetLength(1))
                    {
                        depthBelow = Mathf.Clamp(pn.GeneratePerlinValue(jPerlinIndex, iPerlinIndex, perlinWidthHeight, perlinWidthHeight) + 0.5f, 0f, 1f);
                        if (depthBelow > depth)
                        {
                            shadow = Instantiate(shadowBelow);
                            shadow.transform.position = aboveCurr;
                        }
                    }
                    if ((j - 1) >= 0)
                    {
                        depthLeft = Mathf.Clamp(pn.GeneratePerlinValue(jPerlinIndex, iPerlinIndex, perlinWidthHeight, perlinWidthHeight) + 0.5f, 0f, 1f);
                        if (depthLeft > depth)
                        {
                            shadow = Instantiate(shadowLeft);
                            shadow.transform.position = aboveCurr;
                        }
                    }
                    if ((j + 1) < tiles.GetLength(0))
                    {
                        depthRight = Mathf.Clamp(pn.GeneratePerlinValue(jPerlinIndex, iPerlinIndex, perlinWidthHeight, perlinWidthHeight) + 0.5f, 0f, 1f);
                        if (depthRight > depth)
                        {
                            shadow = Instantiate(shadowRight);
                            shadow.transform.position = aboveCurr;
                        }
                    }
                }



                Debug.Log("Position of perlin point is " + curr.transform.position);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
