using UnityEngine;

public class Galaxy
{
    public int       _PlanetNum   { get; private set; }
    public Planet[]  _Planets     { get; private set; }
    public const int _PlanetDepth = -10;
    public const int _DistScale   = 75;

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
        float size;

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
            v = new Vector3((originDist * 100 * xSign) / _DistScale, (originDist * 100 * ySign) / _DistScale, _PlanetDepth);

            if (i == 0 || i == 1)
                size = 1;
            else if (i == 3)
                size = 1.5f;
            else
                size = 2;

            int pno = 0;
            string name = GimmeAName(ref pno);

            if (originDist <= 2)
                _Planets[i] = pf.CreateMoltenPlanet(v, name, i, size, pno);
            else if (originDist > 5)
                _Planets[i] = pf.CreateWaterPlanet(v, name, i, size, pno);
            else
                _Planets[i] = pf.CreateTemperatePlanet(v, name, i, size, pno);

            //Debug.Log("Planet name: " + _Planets[i]._Name + " Planet coord: " + _Planets[i]._GameObject.transform.position);

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
            p._OtherPlanetPositions = new Vector3[_PlanetNum];
            for (int j = 0; j < _PlanetNum; j++)
            {
                //Debug.Log("tis " + _Planets[j]._GameObject.transform.position); ", x: "
                p._OtherPlanetPositions[j] = _Planets[j]._GameObject.transform.position;
            }
        }
    }

    // DELETEME
    private string GimmeAName(ref int pno)
    {
        string[] names = { "Yelmeree", "Zvaat", "Frotuus", "Kaxias", "Bireen", "Meldys", "Santoon", "Aorfun", "Codruis", "Dexlaa", "Epveen", "Gweroon", "Hexctus", "Inir", "Jkor", "Lqi", "Nexeneetf", "Onzii", "Plecsire", "Qaatni", "Riredun", "Tevelis", "Unbarekh", "Vol", "Wvalo", "Xeveles"};
        string[] numbers = { "I", "II", "III", "IV", "V", "VI", "VII", "IX", "X" };

        int chosen = UnityEngine.Random.Range(0, names.Length);
        string ret = names[chosen];

        if (UnityEngine.Random.Range(0, 3) == 1)
        {
            int num = UnityEngine.Random.Range(0, numbers.Length);
            ret = ret + " " + numbers[num];
            pno = num; 

        }

        return ret;
    }

}
