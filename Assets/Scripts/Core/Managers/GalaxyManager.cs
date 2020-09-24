using UnityEngine;

public class GalaxyManager : MonoBehaviour
{
    Galaxy current;
    LineManager lm;
    GameObject PlanetSelected;

    public GameObject smallStar;
    public GameObject mediumStar;

    public int NumberOfStars;
    public int SmallToBigStarsRatio;

    private bool TradeLinesOn = true;

    int frameCount;

    // Use this for initialization
    void Start()
    {
        frameCount = 0;
        lm = new LineManager();

        int noPlanets = 4;
        current = new Galaxy(noPlanets);

        lm.CreateGOBetweenPlanets(noPlanets);

        // Testing perlin noise function
        int seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);

        GenerateStars();

    }

    private void GenerateStars()
    {
        float xMin = -26.5f;
        float xMax = 26.5f;
        float yMin = -15f;
        float yMax = 15f;

        int i = 0;
        Vector3 sPos;
        GameObject star;

        while (i < NumberOfStars)
        {
            sPos = new Vector3(Random.Range(xMin, xMax), Random.Range(yMin, yMax), -9);
            if (Random.Range(1, SmallToBigStarsRatio) == 1)
            {
                star = Instantiate(mediumStar, sPos, Quaternion.identity);
            } else
            {
                star = Instantiate(smallStar, sPos, Quaternion.identity);
            }
            star.GetComponent<SpriteRenderer>().sortingOrder = -1;
            i++;
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
            lm.ReDrawLinesFromAllPlanets(positions);
        }
    }

    public Planet GetPlanetByGameObject(GameObject go)
    {
        for (int i = 0; i < current._Planets.Length; i++)
        {
            if (current._Planets[i]._GameObject == go)
                return current._Planets[i];
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
        if (frameCount % 5 == 0)
        {
            current.IncrementOrbits();

            lm.DeactivateAllLines();
            DrawAllPlanetsToAll();

            frameCount = 0;
        }
    }
}
