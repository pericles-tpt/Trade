using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    /*public GameObject SandMiddle, SandBottomEdge, SandTopEdge, SandLeftEdge, SandRightEdge, SandBottomLeft, SandBottomRight, SandTopLeft, SandTopRight, SandOneAbove, SandOneBelow, SandOneRight, SandOneLeft;
    public GameObject sandTile;
    public GameObject waterTile;

    public GameObject shadowAbove, shadowBelow, shadowLeft, shadowRight;
    */
    GameObject DELETEME;

    private void Awake()
    {
         DELETEME = new GameObject();
    }

    void Start()
    {

        // Number of sectors = n * n
        // Chunks per sector, more chunks = more hilliness 

        // chunksPerSectorMax: the max number of chunks across every sector, can have more than n
        // in one sector, and less in another, as long as it averages out to n chunks per sector

        // chunksPerSector: the number of chunks in this generated sector

        int chunksPerSectorMax = 2; // Can do 2 -> 256
        int chunksPerSector = 2;
        int seed = UnityEngine.Random.Range(0, int.MaxValue);

        int planetSectorMax = 32;
        int chunksLeft = chunksPerSectorMax * planetSectorMax;
        int perlinWidthHeight = planetSectorMax * chunksPerSectorMax;

        PerlinNoise pn = new PerlinNoise(seed);
        pn.Generate2dPerlin(perlinWidthHeight, perlinWidthHeight, false);

        int sectorWidth = 1024;
        int sectorHeight = 32;

        Vector3 currLand, currWater;

        Vector3[] landTiles = new Vector3[sectorWidth];
        List<Vector3> waterTiles = new List<Vector3>();

        // 4 / 128
        float jPerlinIndex, iPerlinIndex;
        float depth, depthAbove, depthBelow, depthLeft, depthRight;

        for (int i = 0; i < sectorHeight; i++)
        {
            iPerlinIndex = (float)i / (sectorHeight / chunksPerSector);
            landTiles  = new Vector3[sectorWidth];
            waterTiles = new List<Vector3>();
            for (int j = 0; j < sectorWidth; j++)
            {
                jPerlinIndex = (float)j / (sectorWidth / chunksPerSector);
                depth = pn.GeneratePerlinValue(jPerlinIndex, iPerlinIndex, perlinWidthHeight, perlinWidthHeight);

                jPerlinIndex = (float)j / (sectorWidth / chunksPerSector);
                iPerlinIndex = (float)(i - 1) / (sectorHeight / chunksPerSector);
                depthAbove = pn.GeneratePerlinValue(jPerlinIndex, iPerlinIndex, perlinWidthHeight, perlinWidthHeight);

                jPerlinIndex = (float)j / (sectorWidth / chunksPerSector);
                iPerlinIndex = (float)(i + 1) / (sectorHeight / chunksPerSector);
                depthBelow = pn.GeneratePerlinValue(jPerlinIndex, iPerlinIndex, perlinWidthHeight, perlinWidthHeight);

                jPerlinIndex = (float)(j - 1) / (sectorWidth / chunksPerSector);
                iPerlinIndex = (float)i / (sectorHeight / chunksPerSector);
                depthLeft = pn.GeneratePerlinValue(jPerlinIndex, iPerlinIndex, perlinWidthHeight, perlinWidthHeight);

                jPerlinIndex = (float)(j + 1) / (sectorWidth / chunksPerSector);
                iPerlinIndex = (float)i / (sectorHeight / chunksPerSector);
                depthRight = pn.GeneratePerlinValue(jPerlinIndex, iPerlinIndex, perlinWidthHeight, perlinWidthHeight);

                if (depth <= 0)
                {
                    //curr = Instantiate(waterTile);
                    currWater = new Vector3(((float)j * 32f / 100f), ((float)i * 32f / 100f), 0);
                    waterTiles.Add(currWater);
                    //seaBed = Instantiate(sandTile);
                    currLand = new Vector3(currWater.x, currWater.y, depth);
                    landTiles[j] = currLand;
                }
                else
                {
                    //curr = Instantiate(DecideTileToInstantiate(depth, depthAbove, depthBelow, depthLeft, depthRight));
                    currLand = new Vector3(((float)j * 32f / 100f), ((float)i * 32f / 100f), depth);
                    landTiles[j] = currLand;
                }

                //Vector3 aboveCurr = new Vector3(curr.transform.position.x, curr.transform.position.y, curr.transform.position.z + 0.1f);

                // Shadow tile instantiation removed temporarily
                /*if (depth > 0.5f)
                {


                    if ((i - 1) >= 0)
                    {

                        if (depthAbove > depth)
                        {
                            shadow = Instantiate(shadowAbove);
                            shadow.transform.position = aboveCurr;
                        }
                    }
                    if ((i + 1) < tiles.GetLength(1))
                    {

                        if (depthBelow > depth)
                        {
                            shadow = Instantiate(shadowBelow);
                            shadow.transform.position = aboveCurr;
                        }
                    }
                    if ((j - 1) >= 0)
                    {

                        if (depthLeft > depth)
                        {
                            shadow = Instantiate(shadowLeft);
                            shadow.transform.position = aboveCurr;
                        }
                    }
                    if ((j + 1) < tiles.GetLength(0))
                    {

                        if (depthRight > depth)
                        {
                            shadow = Instantiate(shadowRight);
                            shadow.transform.position = aboveCurr;
                        }
                    }
                }*/



                //Debug.Log("Position of perlin point is " + currPos);
            }
            Vector3[] waterTilesArray = waterTiles.ToArray();
        }

        //TileSpawnerSand TSS = new TileSpawnerSand(SandTilePositions, sandTile);
        //TileSpawnerSand TSW = new TileSpawnerSand(WaterTilePositions, waterTile);

        //TSS.SpawnEntities();
        //TSW.SpawnEntities();

    }

    private GameObject DecideTileToInstantiate(float depth, float top, float bottom, float left, float right)
    {
        // SandMiddle      - top: equal, left: equal, right: equal, bottom: equal
        // SandBottomEdge  - top: equal, left: equal, right: equal, bottom: less
        // SandRightEdge   - top: equal, left: equal, right: less, bottom: equal
        // SandBottomRight - top: greater/equal, left: greater/equal, right: less, bottom: less
            // SandLeftEdge    - top: equal, left: less, right: equal, bottom: equal
            // SandBottomLeft  - top: greater/equal, left: less, right: greater/equal, bottom: less
                // SandOneAbove    - top: greater/equal, left: less, right: less, bottom: less
                // SandOneBelow    - top: greater/equal, left: less, right: less, bottom: greater/equal

        // SandTopRight    - top: less, left: greater/equal, right: less, bottom: greater/equal
        // SandOneLeft     - top: less, left: greater/equal, right: less, bottom: less
        // SandTopEdge     - top: less, left: equal, right: equal, bottom: equal
            // SandTopLeft     - top: less, left: less, right: greater/equal, bottom: greater/equal
                // SandOneRight    - top: less , left: less, right: greater/equal, bottom: less
        GameObject ret;

        if (top >= depth)
        {
            if (left >= depth)
            {
                if (right == depth)
                {
                    if (bottom < depth)
                    {
                        //ret = SandBottomEdge;
                    } 
                    else 
                    {
                        //ret = SandMiddle;
                    }
                } else
                {
                    if (bottom < depth)
                    {
                        //ret = SandBottomRight;
                    }
                    else
                    {
                        //ret = SandRightEdge;
                    }
                }
            } else
            {
                if (right >= depth)
                {
                    if (bottom == depth)
                    {
                        //ret = SandLeftEdge;
                    } else
                    {
                        //ret = SandBottomLeft;
                    }
                } else
                {
                    if (bottom >= depth)
                    {
                        //ret = SandOneBelow;
                    } else
                    {
                        //ret = SandOneAbove;
                    }
                }
            }
        } else
        {
            if (left >= depth)
            {
                if (right == depth)
                {
                    //ret = SandTopEdge;
                } else
                {
                    if (bottom < depth)
                    {
                        //ret = SandOneLeft;
                    } else
                    {
                        //ret = SandTopRight;
                    }
                }
            } else
            {
                if (bottom < depth)
                {
                    //ret = SandOneRight;
                } else
                {
                    //ret = SandTopLeft;
                }
            }
        }

        /*if (top == depth && left == depth && right == depth && bottom < depth)
        {
            ret = SandBottomEdge;
        } else if (top == depth && left == depth && right < depth && bottom == depth)
        {
            ret = SandRightEdge;
        }
        else if (top == depth && left == depth && right == depth && bottom == depth)
        {
            ret = SandMiddle;
        } else if ( top >= depth && left >= depth && right < depth && bottom < depth)
        {
            ret = SandBottomRight;
        } else if (top == depth && left < depth && right == depth && bottom == depth)
        {
            ret = SandLeftEdge;
        } else if (top >= depth && left < depth && right >= depth && bottom < depth)
        {
            ret = SandBottomLeft;
        } else if (top >= depth && left < depth && right < depth && bottom < depth)
        {
            ret = SandOneAbove;
        } else if (top >= depth && left < depth && right < depth && bottom >= depth)
        {
            ret = SandOneBelow;
        } else if (top < depth && left >= depth && right < depth && bottom >= depth)
        {
            ret = SandTopRight;
        } else if (top < depth && left >= depth && right < depth && bottom < depth)
        {
            ret = SandOneLeft;
        } else if (top < depth && left == depth && right == depth && bottom == depth)
        {
            ret = SandTopEdge;
        } else if (top < depth && left < depth && right >= depth && bottom >= depth)
        {
            ret = SandTopLeft;
        } else if (top < depth && left < depth && right >= depth && bottom < depth)
        {
            ret = SandOneRight;
        } else
        {
            ret = SandMiddle;
        }*/

        return DELETEME;

    }

}
