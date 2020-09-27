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

    public bool TradeLinesOn = false;

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

        current.IncrementOrbits();

        lm.DeactivateAllLines();
        DrawAllPlanetsToAll(TradeLinesOn);

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

    public void DrawAllPlanetsToAll(bool drawLines)
    {
        Vector3[] positions = new Vector3[current._PlanetNum];
        int i;
        for (i = 0; i < current._PlanetNum; i++)
            positions[i] = current._Planets[i]._GameObject.transform.position;
        lm.CheckLinesFromAllPlanets(positions, drawLines);
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

    public bool LMLineHitsSunCollider(Vector3 A, Vector3 B, int a)
    {
        return lm.LineHitsSunCollider(A, B, a);
    }

    public void LMDeactivateLineStartingAt(Vector3 A)
    {
        lm.DeactivateLineStartingAt(A);
    }

    public void LMActivateLine(Vector3 A, Vector3 B, int a)
    {
        lm.ActivateLine(A, B, a);
    }

    // Update is called once per frame
    void Update()
    {
        frameCount++;
        if (frameCount % 480 == 0)
        {
            current.IncrementOrbits();

            lm.DeactivateAllLines();
            DrawAllPlanetsToAll(TradeLinesOn);

            frameCount = 0;
        }
    }
}
