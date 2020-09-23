using UnityEngine;

public class Galaxy
{
    public int       _PlanetNum   { get; private set; }
    public Planet[]  _Planets     { get; private set; }
    public const int _PlanetDepth = -10;
    public const int _DistScale   = 75;

    private PlanetFactory pf;

    public Galaxy(int planetNum)
    {
        _Planets = new Planet[planetNum];
        _PlanetNum = planetNum;
        GeneratePlanets(planetNum);

    }

    // Place planets
    public void GeneratePlanets(int planetNum)
    {
        pf = GameObject.Find("Camera").GetComponent<PlanetFactory>();
        Vector3 sunPos = GameObject.Find("sun").transform.position;

        Vector3 v;
        float originDist = 0;
        float size;

        for (int i = 0; i < (planetNum); i++)
        {

            if (i == 0 || i == 1)
                size = 1;
            else
                size = 1.5f;

            // Choose whether to make initial x and y coordinates +,- or 0
            int xSign = 0, ySign = 0;

            // Each planet is assigned a set distance from the sun and initialy position...
            // ... maybe randomise xSign, ySign and originDist later?
            switch(i)
            {
                case (0):
                    originDist = 2f;//UnityEngine.Random.Range(0f, 0.5f);
                    xSign = 0;
                    ySign = 1;
                    break;
                case (1):
                    originDist += 1.75f;//UnityEngine.Random.Range(1f, 1.25f);
                    xSign = 1;
                    ySign = 0;
                    break;
                case (2):
                    originDist += 2.75f;//UnityEngine.Random.Range(1f, 1.25f);
                    xSign = -1;
                    ySign = 0;
                    break;
                case (3):
                    originDist += 2.75f; //UnityEngine.Random.Range(0f, 0.2f);
                    xSign = 0;
                    ySign = -1;
                    break;

            }

            Debug.Log("originDist is " + originDist);

            // Stores new planet position in v from previous section
            v = new Vector3(sunPos.x + ((originDist * 100 * xSign) / _DistScale), sunPos.y + ((originDist * 100 * ySign) / _DistScale), _PlanetDepth);
            Debug.Log(v);

            int pno = 0;
            string name = NamePlanet(ref pno);

            Debug.Log(v + " " + name + " " + i + " " + size + " " + pno);
            if (originDist <= 2)
                _Planets[i] = pf.CreatePlanet(v, Planet.PlanetType.molten, name, i, size, pno);
            else if (originDist > 5)
                _Planets[i] = pf.CreatePlanet(v, Planet.PlanetType.water, name, i, size, pno);
            else
                _Planets[i] = pf.CreatePlanet(v, Planet.PlanetType.temperate, name, i, size, pno);

        }

        PopulateOtherPlanetPositions();

    }

    public void IncrementOrbits()
    {
        for (int i = 0; i < _Planets.Length; i++)
        {
            _Planets[i].IncOrbit();

        }
    }

    private void PopulateOtherPlanetPositions()
    {
        for(int i = 0; i < _PlanetNum; i++)
        {
            Planet p = _Planets[i];
            p._OtherPlanetPositions = new Vector3[_PlanetNum];
            for (int j = 0; j < _PlanetNum; j++)
            {
                p._OtherPlanetPositions[j] = _Planets[j]._GameObject.transform.position;
            }
        }
    }

    private string NamePlanet(ref int pno)
    {
        string[] names = { "Yelmeree", "Zvaat", "Frotuus", "Kaxias", "Bireen", "Meldys", "Santoon", "Aorfun", "Codruis", "Dexlaa", "Epveen", "Gweroon", "Hexctus", "Inir", "Jkor", "Lqi", "Nexeneetf", "Onzii", "Plecsis", "Qaatni", "Riredun", "Tevelis", "Unbarekh", "Vol", "Wvalo", "Xeveles"};
        string[] numbers = { "I", "II", "III", "IV", "V", "VI", "VII", "IX", "X" };

        int chosen = Random.Range(0, names.Length);
        string ret = names[chosen];

        if (Random.Range(0, 3) == 1)
        {
            int num = Random.Range(0, numbers.Length);
            ret = ret + " " + numbers[num];
            pno = num; 

        }

        return ret;
    }

}
