using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameDirector : MonoBehaviour
{
    Galaxy current;
    LineManager lm;
    GameObject PlanetSelected;
    Vector3 planetPositionBeforeZoom;
    Vector3[] oldPlanetPos;

    public GameObject tooltip;
    GameObject activeTooltip;

    private bool TradeLinesOn = true;
    private bool SectorLinesOn = false;

    // Use this for initialization
    void Start()
    {
        lm = new LineManager();
        GalaxyFactory gf = new GalaxyFactory();
        current = gf.CreateGalaxy();
        current.GeneratePlanets(current._Planets.Length);
        oldPlanetPos = new Vector3[current._Planets.Length];

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

    public void ToggleSectorLines(bool setting, Planet p)
    {
        if (setting == false)
            p._SectorLines.DestroyAllLines();
        else
        {
            p._SectorLines.DestroyAllLines();
            p.DrawSectorLines((int)p._ThisSectorSize);
        }
        SectorLinesOn = setting;
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

    public void ToggleSectorTooltipVisible(bool isVisible)
    {
        int nAlpha;
        if (isVisible)
            nAlpha = 1;
        else
            nAlpha = 0;

        GameObject.Find("sector_tooltip").GetComponent<CanvasGroup>().alpha = nAlpha;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
