using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GalaxyManager : MonoBehaviour
{
    Galaxy current;
    LineManager lm;
    GameObject PlanetSelected;
    Vector3 planetPositionBeforeZoom;
    Vector3[] oldPlanetPos;

    public GameObject tooltip;
    GameObject activeTooltip;

    public GameObject smallStar;
    public GameObject mediumStar;

    private bool TradeLinesOn = true;
    private bool SectorLinesOn = false;

    int fCount;

    // Use this for initialization
    void Start()
    {
        fCount = 0;
        lm = new LineManager();
        GalaxyFactory gf = new GalaxyFactory();
        current = gf.CreateGalaxy();
        current.GeneratePlanets(current._Planets.Length);
        oldPlanetPos = new Vector3[current._Planets.Length];

        // Testing perlin noise function
        int seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);

        //PerlinNoise pn = new PerlinNoise(seed);
        //pn.Generate2dPerlin(640, 320, true, 1.25f);

        // Create companies... maybe economy director as well
        // Companies and businesses, a business will transition to a company once it reaches a certain size
        // Companies can be small (1M - 1B), medium (1B - 1T) or large (1T - 1Q)
        // Businesses usually are values at between (100K - 10M)
        // Companies have a planet and sector of origin
        // As companies get bigger they move to sectors with lower tax rates, then planets with lower tax rate, they evaluate when they move based off of the cost/benefit for that year (if the net benefit is positive they move)
        // Generate entities, decide distribution of entities across planets (depending on size of planets, say weighting is 4 : 2 : 1 (small, medium, large))
        // After distribution is decided determine an unemployment rate for each planet and then assign employed entities to companies/businesses as employees or owners (say ratio is 1000 employees : 1 owner)
        // Finally determine number of businesses, companies and unemployment rate for each sector and distribute employees accordingly across the planet

        // Maybe entities have a food/water/other meter which goes down each day, once below a certain threshold they visit a shop, entities commute from home to work to home (most time is spent at home/work, small amount of time is spent shopping)
        GenerateStars(500);


    }

    private void GenerateStars(int noStars)
    {
        float xMin = -15f;
        float xMax = 26.5f;
        float yMin = -15f;
        float yMax = 15f;

        int i = 0;
        Vector3 sPos;
        while (i < noStars)
        {
            sPos = new Vector3(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax), -9);
            if (UnityEngine.Random.Range(1, 10) == 1)
            {
                Instantiate(mediumStar, sPos, Quaternion.identity);
            } else
            {
                Instantiate(smallStar, sPos, Quaternion.identity);
            }
            i++;
        }
    }

    public void IncrementOrbits()
    {
        current.IncrementOrbits();
    }

    public void DrawOnePlanetToAll(GameObject go)
    {
        if (TradeLinesOn)
        {
            Vector3[] positions = new Vector3[current._PlanetNum];
            for (int i = 0; i < current._PlanetNum; i++)
                positions[i] = current._Planets[i]._GameObject.transform.position;
            //Debug.Log("POSITIONS LENGTH " + positions.Length);
            lm.DrawAllLines(go, positions);
        }
    }

    public void DestroyAllLines()
    {
        lm.DestroyAllLines();
    }

    public void ToggleTradeLines(bool setting)
    {
        if (setting == false)
            DestroyAllLines();
        TradeLinesOn = setting;
    }

    public void ToggleMovePlanetsNotSelected(bool moveOut)
    {
        if (moveOut)
        {
            int i = 0;
            oldPlanetPos[i] = GameObject.Find("sun").gameObject.transform.position;
            i++;
            GameObject.Find("sun").gameObject.transform.position = new Vector3(-100, -100, -100);
            foreach (Planet p in current._Planets)
            {
                if (p._GameObject != PlanetSelected)
                {
                    oldPlanetPos[i] = p._GameObject.transform.position;
                    i++;
                    p._GameObject.transform.position = new Vector3(-100, -100, -100);
                }
            }

        }
        else
        {
            int i = 0;
            GameObject.Find("sun").gameObject.transform.position = oldPlanetPos[i];
            i++;
            foreach (Planet p in current._Planets)
            {
                if (p._GameObject != PlanetSelected)
                {
                    p._GameObject.transform.position = oldPlanetPos[i];
                    i++;
                }
            }
        }
    }

    public Planet FindPlanet(GameObject go)
    {
        foreach (Planet p in current._Planets)
        {
            if (p._GameObject == go)
                return p;
        }
        return null;
    }

    public void SetSelectedPlanet(GameObject go)
    {
        PlanetSelected = go;
    }

    public GameObject GetSelectedPlanet ()
    {
        return PlanetSelected;
    }

    public void SetPlanetPositionBeforeZoom (Vector3 Pos)
    {
        planetPositionBeforeZoom= Pos;
    }

    public Vector3 GetPlanetPositionBeforeZoom()
    {
        return planetPositionBeforeZoom;
    }

    public PlanetFactory GetPlanetFactory()
    {
        return current.GetPlanetFactory();
    }

    // Update is called once per frame
    void Update()
    {
        fCount++;
        if (fCount % 12 == 0)
        {
            GameObject.Find("Camera").GetComponent<GalaxyManager>().IncrementOrbits();
            //GameObject.Find("Canvas").GetComponent<DayTextBehaviour>().UpdateDayText();
            fCount = 0;
        }
    }
}
