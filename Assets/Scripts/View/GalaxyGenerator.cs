﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyGenerator : MonoBehaviour
{
    public GameObject planet;
    public GameObject player;
    public int fCount = 0;
    public int fsCount = 0;
    public int planetNum;
    public Ship initial;
    public Vector3[] planetCoords;
    public Planet[] planets;
    public string[] planetNames;
    public Dictionary<Tuple<Vector3, Vector3>, GameObject> planetLines;

    public Sprite Earth;
    public Sprite Molten;
    public Sprite Water;

    // Start is called before the first frame update
    void Start()
    {
        // Generates between 4 and 6 planets
        int planetNum = UnityEngine.Random.Range(4, 6);

        // Stores all node positions for comparing a new node to previous node
        // positions to determine if two nodes are too close together
        planetCoords = new Vector3[planetNum];
        planetNames = Planet.GimmeSomeNames(planetNum);
        planets = new Planet[planetNum];
        planetLines = new Dictionary<Tuple<Vector3, Vector3>, GameObject>();

        GeneratePlanets(ref planetCoords, planetNum);

        Instantiate(player, new Vector3(100, 100, 100), Quaternion.identity);
        //DrawTradeLines(ref planetCoords, ref planetLines, false);
    }

    public void IncrementOrbits()
    {
        MovePlanets(ref planetCoords, planetNum);
        //DrawTradeLines(ref planetCoords, ref planetLines, true);

    }

    // Place planets
    private void GeneratePlanets(ref Vector3[] nodeCoords, int planetNum)
    {

        // Planet are all on the same depth level
        const int planetDepth = -10;
        Vector3 v;

        int addHun = 1;
        int addFif = 2;
        float originDist = 1;

        for (int i = 0; i < (planetNum); i++)
        {
            // Instantiate and record planet coords at start of iteration
            originDist += i;

            // START SECTION: Code in here tries to create random initial placement
            // of planets' wrt distance and x,y positioning

            // Adds an extra 100px or 50px between planets to add some irregularity
            // REM: If on 1st iteration add an extra hundred to make it a realistic distance
            // from sun. Taken out as planets that are generated very close to sun can be molten
            if (((UnityEngine.Random.Range(0, 2) == 1) /*|| (i == 0)*/) && (addHun > 0))
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

            // This should move nodes that are in-line on the same axis away from
            // each other if they're too close
            foreach (Vector3 v3 in nodeCoords)
            {
                int sign = 0;
                while (sign == 0)
                    sign = UnityEngine.Random.Range(-1, 1);

                // ToDo: This statement doesn't do what it's supposed to (i.e. separate nodes too diagonally close to one another)
                if ((Mathf.Abs(v3.x - (xSign * 100 * originDist)) <= 100f) && (Mathf.Abs(v3.y - (ySign * 100 * originDist)) <= 100f))
                    xSign *= -1;
                // ToDo: This statement won't solve issue if flipping the XSign will violate one of these other conditions, for a prior node
                else if ((v3.x == (xSign * originDist)) && (v3.y == (ySign * originDist)))
                    xSign *= -1;
                else if ((v3.x == xSign) && (Mathf.Abs(v3.y - originDist * ySign) <= 100f))
                    xSign = sign;
                else if (v3.x == ySign && (Mathf.Abs(v3.y - originDist * ySign) <= 100f))
                    ySign = sign;

            }

            // END SECTION

            // Stores new planet position in v from previous section

            v = new Vector3((originDist * 100 * xSign) / 250, (originDist * 100 * ySign) / 250, planetDepth);
            GameObject go = Instantiate(planet, v, Quaternion.identity);

            if (i == 1)
            {
                go.GetComponent<SpriteRenderer>().sprite = Molten;

            }
            else if (i == 2)
            {
                go.GetComponent<SpriteRenderer>().sprite = Earth;

            }
            else if (i == 3)
            {
                go.GetComponent<SpriteRenderer>().sprite = Water;

            }

            Planet.PlanetType pt;
            int ptChoice;
            if (originDist <= 1.5)
                pt = Planet.PlanetType.volcanic;
            else if (originDist <= 3) {
                ptChoice = UnityEngine.Random.Range(1, 4);
                if (ptChoice == 1)
                    pt = Planet.PlanetType.arid;
                else if (ptChoice == 2)
                    pt = Planet.PlanetType.ideal;
                else
                    pt = Planet.PlanetType.oceanic;
            } else
            {
                ptChoice = UnityEngine.Random.Range(1, 2);
                if (ptChoice == 1)
                    pt = Planet.PlanetType.ice;
                else
                {
                    pt = Planet.PlanetType.gas;
                }
            }



            Planet p = new Planet(planetNames[i], pt, go);
            nodeCoords[i] = v;
            Debug.Log("Node placed at: " + "x: " + (originDist * 100 * xSign) + ", y: " + originDist * 100 * ySign);

        }

        foreach (Vector3 v3 in nodeCoords)
        {
            Debug.Log("nodeCoords list: " + v3.ToString());
        }
    }

    private void DrawTradeLines(ref Vector3[] planetCoords, ref Dictionary<Tuple<Vector3, Vector3>, GameObject> planetLines, bool deleteOld)
    {
        Vector3 s;
        Color c = new Color(0, 0, 0);
        int dl = 0;

        if (deleteOld)
        {
            DestroyAllTradeLines();
        }

        for (int j = 0; j < planetCoords.Length; j++)
        {
            s = planetCoords[j];
            Vector3 e;
            for (int k = 0; k < planetCoords.Length; k++)
            {
                e = planetCoords[k];

                if (j != k)
                {
                    if (!planetLines.ContainsKey(Tuple.Create(e, s)))
                    {
                        Debug.Log("Drawn line from: " + s.ToString() + "to: " + e.ToString());
                        DrawLine(s, e, ref planetLines, 0);

                    }
                    else
                        Debug.Log("Eliminated one crossover");

                } else
                {
                    Debug.Log("Prevented node line to self");

                }

            }
        }

        Debug.Log("Drew " + dl + " lines");
    }

    public void DrawTradeLines(GameObject findPlanet)
    {
        Vector3 s;
        Color c = new Color(0, 0, 0);
        int dl = 0;

        DestroyAllTradeLines();

        int selectedIndex = 0;
        for (int i = 0; i < planets.Length; i++)
            if (planets[i]._GameObject == findPlanet)
                selectedIndex = i;

        s = planetCoords[selectedIndex];
        Vector3 e;
        for (int k = 0; k < planetCoords.Length; k++)
        {
            e = planetCoords[k];

            if (selectedIndex != k)
            {
                Debug.Log("Drawn line from: " + s.ToString() + "to: " + e.ToString());
                DrawLine(s, e, ref planetLines, 0);

                /* NOT RELEVANT ON A PLANET TO PLANET BASIS
                if (!planetLines.ContainsKey(Tuple.Create(e, s)))
                {
                }
                else
                    Debug.Log("Eliminated one crossover"); */

            }
            else
            {
                Debug.Log("Prevented node line to self");

            }

        }

        Debug.Log("Drew " + dl + " lines");
    }

    // CREDIT (paranoidray): https://answers.unity.com/questions/8338/how-to-draw-a-line-using-script.html
    // ToDo: Come up with own DrawLine function
    private void DrawLine(Vector3 start, Vector3 end, ref Dictionary<Tuple<Vector3, Vector3>, GameObject> planetLines, float duration = 0.2f)
    {
        // Return if line would cross the origin (a.k.a the sun)
        //if (LineIntersectsSun(start, end))
        //    return;

        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.SetColors(Color.white, Color.white);
        lr.SetWidth(0.1f, 0.1f);
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        Material whiteDiffuse = new Material(Shader.Find("Unlit/Texture"));
        lr.material = whiteDiffuse;

        planetLines.Add(Tuple.Create(start, end), myLine);
    }

    private void MovePlanets(ref Vector3[] nodeCoords, int planetNum)
    {
        // 1 unit from the sun is 100 day orbit
        int i = 0;

        foreach (Vector3 v in nodeCoords)
        {
            // Just calculate distance from origin (or radius of orbit)
            float radius;
            float orbitInc;
            float orbitCirc;
            if (v.x == 0)
            {
                radius = v.y;

            }
            else if (v.y == 0)
            {
                radius = v.x;

            }
            else
            {
                radius = Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2));
                Debug.Log("Radius is: " + radius);

            }

            orbitInc = radius * 100;
            orbitCirc = 2 * Mathf.PI * radius;

            float orbitSectorAngle = (360 / orbitInc);
            float orbitSectorAngleRadians = orbitSectorAngle * Mathf.Deg2Rad;
            float nextX, nextY;

            if (i == 2)
            {
                nextX = (v.x * Mathf.Cos(-orbitSectorAngleRadians)) + (v.y * Mathf.Sin(-orbitSectorAngleRadians));
                nextY = (-v.x * Mathf.Sin(-orbitSectorAngleRadians)) + (v.y * Mathf.Cos(-orbitSectorAngleRadians));

            } else
            {
                nextX = (v.x * Mathf.Cos(orbitSectorAngleRadians)) + (v.y * Mathf.Sin(orbitSectorAngleRadians));
                nextY = (-v.x * Mathf.Sin(orbitSectorAngleRadians)) + (v.y * Mathf.Cos(orbitSectorAngleRadians));
            }

            // Create new v
            Vector3 v3 = new Vector3(nextX, nextY, -10);

            // Destroy old planet
            planets[i]._GameObject.transform.position = v3;

            nodeCoords[i] = v3;
            i++;

        }
    }

    public void DestroyAllTradeLines()
    {
        foreach (KeyValuePair<Tuple<Vector3, Vector3>, GameObject> i in planetLines)
            Destroy(i.Value);
        planetLines.Clear();
    }

    // FIX: Just doesn't work...
    private bool LineIntersectsSun(Vector3 start, Vector3 end)
    {
        float m = (end.y - start.y) / (end.x - end.y);
        float b = end.y - (m * end.x);

        int circRadius = 40;

        float eq = ((circRadius^2) - (b * b)) / (1 + (m * m));


        if (eq >= 0 || (eq - 1) >= 0)
            return true;
        else
            return false;
    }

    public Vector3 GetPlanetPosition(int planetIndex)
    {
        return planetCoords[planetIndex];

    }
}