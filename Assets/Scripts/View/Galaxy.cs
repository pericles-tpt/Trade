using UnityEngine;

public class Galaxy
{
    public int       _PlanetNum   { get; private set; }
    public Planet[]  _Planets     { get; private set; }
    public const int _PlanetDepth = -10;

    public Galaxy(int planetNum)
    {
        _Planets = new Planet[planetNum];
        _PlanetNum = planetNum;

    }

    // Place planets
    public void GeneratePlanets(int planetNum)
    {
        PlanetFactory pf = GameObject.Find("Camera").GetComponent<PlanetFactory>();

        // Planet are all on the same depth level, addHun and addFif try to 
        // vary the relative distances of planets compared to their previous planets
        Vector3 v;
        int addHun = 1;
        int addFif = 2;
        float originDist = 1;

        for (int i = 0; i < (planetNum); i++)
        {
            // Increment base distance of planet from origin (sun)
            originDist += i;

            // Decide whether to add 100 or 50 pixels onto distance from origin
            if ((UnityEngine.Random.Range(0, 2) == 1) && (addHun > 0))
            {
                addHun--;
                originDist++;
            }
            else
            {

                if (UnityEngine.Random.Range(0, 2) == 1 && addFif > 0)
                {
                    addFif--;
                    originDist += 0.5f;
                }

            }

            // Choose whether to make initial x and y coordinates +,- or 0
            int xSign, ySign = 0;

            // X is 0
            xSign = UnityEngine.Random.Range(-1, 2);
            if (xSign == 0)
                // Y must be + or -
                while (ySign == 0)
                {
                    ySign = UnityEngine.Random.Range(-1, 2);
                }
            else
                // Y can be -, + or 0
                ySign = UnityEngine.Random.Range(-1, 2);

            // Stores new planet position in v from previous section
            v = new Vector3((originDist * 100 * xSign) / 250, (originDist * 100 * ySign) / 250, _PlanetDepth);

            if (originDist <= 2)
                _Planets[i] = pf.CreateMoltenPlanet(v, "molten", i);
            else if (originDist > 5)
                _Planets[i] = pf.CreateWaterPlanet(v, "water", i);
            else
                _Planets[i] = pf.CreateTemperatePlanet(v, "temperate", i);

            Debug.Log("Planet name: " + _Planets[i]._Name + " Planet coord: " + _Planets[i]._GameObject.transform.position);

        }

        InformPlanetPositions();

    }

    public void IncrementOrbits()
    {
        foreach (Planet p in _Planets)
        {
            p.IncOrbit();

        }
    }

    private void InformPlanetPositions()
    {
        for(int i = 0; i < _PlanetNum; i++)
        {
            Planet p = _Planets[i];
            for (int j = 0; j < _PlanetNum; j++)
                p._PlanetPositions[j] = _Planets[j]._GameObject.transform.position;
        }
    }

}
