using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class TerrainGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    /*public GameObject SandMiddle, SandBottomEdge, SandTopEdge, SandLeftEdge, SandRightEdge, SandBottomLeft, SandBottomRight, SandTopLeft, SandTopRight, SandOneAbove, SandOneBelow, SandOneRight, SandOneLeft;
    public GameObject sandTile;
    public GameObject waterTile;

    public GameObject shadowAbove, shadowBelow, shadowLeft, shadowRight;
    */

    public TileScriptableObject water;
    public TileScriptableObject sand;

    public GameObject tile;

    private void Awake()
    {

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

        // Was originally 256 x 144
        int sectorWidth = 96;
        int sectorHeight = 48;

        Vector3 currLand, currWater;

        Vector3[] landTiles = new Vector3[sectorWidth];
        List<Vector3> waterTiles = new List<Vector3>();

        // 4 / 128
        float jPerlinIndex, jPerlinIndexM, jPerlinIndexP, iPerlinIndex, iPerlinIndexM, iPerlinIndexP;
        float depth, depthAbove, depthBelow, depthLeft, depthRight;

        for (int i = 0; i < sectorHeight; i++)
        {
            iPerlinIndex  = (float)i / (sectorHeight / chunksPerSector);
            iPerlinIndexM = (float)(i - 1) / (sectorHeight / chunksPerSector);
            iPerlinIndexP = (float)(i + 1) / (sectorHeight / chunksPerSector);

            landTiles  = new Vector3[sectorWidth];
            waterTiles = new List<Vector3>();

            for (int j = 0; j < sectorWidth; j++)
            {
                jPerlinIndex  = (float)j / (sectorWidth / chunksPerSector);
                jPerlinIndexM = (float)(j - 1) / (sectorWidth / chunksPerSector);
                jPerlinIndexP = (float)(j + 1) / (sectorWidth / chunksPerSector);

                depth      = pn.GeneratePerlinValue(jPerlinIndex, iPerlinIndex, perlinWidthHeight, perlinWidthHeight);
                depthAbove = pn.GeneratePerlinValue(jPerlinIndex, iPerlinIndexM, perlinWidthHeight, perlinWidthHeight);
                depthBelow = pn.GeneratePerlinValue(jPerlinIndex, iPerlinIndexP, perlinWidthHeight, perlinWidthHeight);
                depthLeft  = pn.GeneratePerlinValue(jPerlinIndexM, iPerlinIndex, perlinWidthHeight, perlinWidthHeight);
                depthRight = pn.GeneratePerlinValue(jPerlinIndexP, iPerlinIndex, perlinWidthHeight, perlinWidthHeight);

                if (depth <= 0)
                {
                    //curr = Instantiate(waterTile);
                    currWater = new Vector3(((float)j * 32f / 100f), ((float)i * 32f / 100f), 0);
                    waterTiles.Add(currWater);
                    Instantiate(tile, currWater, Quaternion.identity);
                    tile.GetComponent<SpriteRenderer>().sprite = water.tiles[0];

                    //seaBed = Instantiate(sandTile);
                }
                currLand = new Vector3(((float)j * 32f / 100f), ((float)i * 32f / 100f), depth);
                landTiles[j] = currLand;
                Instantiate(tile, currLand, Quaternion.identity);
                tile.GetComponent<SpriteRenderer>().sprite = sand.tiles[DecideTileToInstantiate(depth, depthAbove, depthBelow, depthLeft, depthRight)];

                //tm.SetTile(new Vector3Int(j, i, 0), tile);

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

    private int DecideTileToInstantiate(float depth, float top, float bottom, float left, float right)
    {
        // 2. SandLeftEdge
        // 5. SandTopLeft
        // 7. SandBottomLeft
        // 9. SandOneAbove
        // 10. SandOneBelow
        // 12. SandOneRight

        // 3. SandRightEdge
        // 6. SandTopRight
        // 8. SandBottomRight
        // 11. SandOneLeft

        // 0. SandTopEdge

        // 1. SandBottomEdge

        // 4. SandMiddle
        int ret = 4;

        if (depth > left)
        {
            if (depth <= right && depth <= top && depth <= bottom)
                ret = 2;
            else if (depth <= right && depth > top && depth <= bottom)
                ret = 5;
            else if (depth <= right && depth <= top && depth > bottom)
                ret = 7;
            else if (depth > right && depth <= top && depth > bottom)
                ret = 9;
            else if (depth > right && depth > top && depth <= bottom)
                ret = 10;
            else if (depth <= right && depth > top && depth > bottom)
                ret = 12;

        } else if (depth > right)
        {
            if (depth <= left && depth <= top && depth <= bottom)
                ret = 3;
            else if (depth <= left && depth > top && depth <= bottom)
                ret = 6;
            else if (depth <= left && depth <= top && depth > bottom)
                ret = 8;
            else if (depth <= left && depth > top && depth > bottom)
                ret = 11;

        } else if (depth > top)
        {
            ret = 0;
        } else if (depth > bottom)
        {
            ret = 1;
        } else
        {
            ret = 4;
        }

        return ret;

    }

}
