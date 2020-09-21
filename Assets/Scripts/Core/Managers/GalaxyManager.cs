﻿using UnityEngine;

public class GalaxyManager : MonoBehaviour
{
    Galaxy current;
    LineManager lm;
    GameObject PlanetSelected;

    public GameObject smallStar;
    public GameObject mediumStar;

    private bool TradeLinesOn = true;

    int frameCount;

    // Use this for initialization
    void Start()
    {
        frameCount = 0;
        lm = new LineManager();

        int noPlanets = 4;
        current = new Galaxy(noPlanets);

        // Testing perlin noise function
        int seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);

        GenerateStars(500, 10);

    }

    private void GenerateStars(int noStars, int smallToBigStars)
    {
        float xMin = -15f;
        float xMax = 15f;
        float yMin = -15f;
        float yMax = 15f;

        int i = 0;
        Vector3 sPos;
        while (i < noStars)
        {
            sPos = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), -9);
            if (Random.Range(1, smallToBigStars) == 1)
            {
                Instantiate(mediumStar, sPos, Quaternion.identity);
            } else
            {
                Instantiate(smallStar, sPos, Quaternion.identity);
            }
            i++;
        }
    }

    public void DrawOnePlanetToAll(GameObject go)
    {
        if (TradeLinesOn)
        {
            Vector3[] positions = new Vector3[current._PlanetNum];
            for (int i = 0; i < current._PlanetNum; i++)
                positions[i] = current._Planets[i]._GameObject.transform.position;
            lm.DrawLinesFromOnePlanet(go.transform.position, positions);
        }
    }

    public void DrawAllPlanetsToAll()
    {
        if (TradeLinesOn)
        {
            Vector3[] positions = new Vector3[current._PlanetNum];
            int i;
            for (i = 0; i < current._PlanetNum; i++)
                positions[i] = current._Planets[i]._GameObject.transform.position;
            lm.DrawLinesFromAllPlanets (positions);
        }
    }

    public void ToggleTradeLines(bool setting)
    {
        if (setting == false)
            lm.ToggleHideAllLines(setting);
        TradeLinesOn = setting;
    }

    public Planet GetPlanetByGameObject(GameObject go)
    {
        foreach (Planet p in current._Planets)
        {
            if (p._GameObject == go)
                return p;
        }
        return null;
    }

    public Planet GetPlanetByIndex(int i)
    {
        return current._Planets[i];
    }

    public void SetSelectedPlanet(GameObject go)
    {
        PlanetSelected = go;
    }

    public GameObject GetSelectedPlanet ()
    {
        return PlanetSelected;
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        if (frameCount % 12 == 0)
        {
            current.IncrementOrbits();

            lm.DestroyAllLines();
            DrawAllPlanetsToAll();

            frameCount = 0;
        }
    }
}
