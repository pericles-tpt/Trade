using UnityEngine;
using UnityEngine.UIElements;

public class GameDirector : MonoBehaviour
{
    Galaxy current;
    LineManager lm;

    // Use this for initialization
    void Start()
    {
        lm = new LineManager();
        GalaxyFactory gf = new GalaxyFactory();
        current = gf.CreateGalaxy();
        current.GeneratePlanets(current._Planets.Length);

    }

    public void IncrementOrbits()
    {
        current.IncrementOrbits();
    }

    public void DrawOnePlanetToAll(GameObject go)
    {
        Vector3[] positions = new Vector3[current._PlanetNum];
        for (int i = 0; i < current._PlanetNum; i++)
            positions[i] = current._Planets[i]._GameObject.transform.position;
        Debug.Log("POSITIONS LENGTH " + positions.Length);
        lm.DrawAllLines(go, positions);
    }

    public void DestroyAllLines()
    {
        lm.DestroyAllLines();
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

    // Update is called once per frame
    void Update()
    {

    }
}
