using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class script : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject sandTile;
    public GameObject waterTile;
    void Start()
    {

        PerlinNoise pn = new PerlinNoise();
        float[,] tiles = pn.Generate2dPerlin(128, 64, false, 8);

        for (int i = 0; i < tiles.GetLength(1); i++)
        {
            for (int j = 0; j < tiles.GetLength(0); j++)
            {
                float depth = pn.GeneratePerlinValue(((float)j * 32f / 100f), ((float)i * 32f / 100f), 128, 64);
                GameObject curr;
                if (depth <= -0.1f)
                    curr = Instantiate(waterTile);
                else
                    curr = Instantiate(sandTile);

                curr.transform.position = new Vector3(((float)j * 32f/100f), ((float)i * 32f/100f), depth);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
