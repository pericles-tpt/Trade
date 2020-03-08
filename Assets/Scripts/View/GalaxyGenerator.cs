using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyGenerator : MonoBehaviour
{
    public GameObject planet;
    public GameObject player;

    public Sprite Earth;
    public Sprite Molten;
    public Sprite Water;

    public const int planetDepth = -10;
    public int planetNum;

    public Planet[] planets;
    public string[] planetNames;
    public Dictionary<Tuple<Vector3, Vector3>, GameObject> planetLines;


    // Start is called before the first frame update
    void Start()
    {
        // Chooses between 4 and 5 (inclusive) planets to generate
        int planetNum = UnityEngine.Random.Range(4, 6);

        // Stores all node positions for comparing a new node to previous node
        // positions to determine if two nodes are too close together
        planetLines = new Dictionary<Tuple<Vector3, Vector3>, GameObject>();
        planetNames = Planet.GimmeSomeNames(planetNum);
        planets = new Planet[planetNum];

        GeneratePlanets(planetNum);

        // Create the player instance
        Instantiate(player, new Vector3(100, 100, 100), Quaternion.identity);
    }

    public void IncrementOrbits()
    {
        MovePlanets(planetNum);
        //DrawTradeLines(ref planetCoords, ref planetLines, true);

    }

    // Place planets
    private void GeneratePlanets(int planetNum)
    {

        // Planet are all on the same depth level
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

            // FIX: This should move nodes that are in-line on the same axis
            // away from each other if they're too close
            foreach (Planet p in planets)
            {
                Vector3 v3 = p._GameObject.transform.position;
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
            InstantiatePlanet(v, i, originDist);

        }

    }

    private void DrawTradeLines(ref Dictionary<Tuple<Vector3, Vector3>, GameObject> planetLines, bool deleteOld)
    {
        Vector3 s;
        Color c = new Color(0, 0, 0);
        int dl = 0;

        if (deleteOld)
        {
            planetLines.Clear();
        }

        for (int j = 0; j < planets.Length; j++)
        {
            s = planets[j]._GameObject.transform.position;
            Vector3 e;
            for (int k = 0; k < planets.Length; k++)
            {
                e = planets[k]._GameObject.transform.position;

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

        planetLines.Clear();

        int selectedIndex = 0;
        for (int i = 0; i < planets.Length; i++)
            if (planets[i]._GameObject == findPlanet)
                selectedIndex = i;

        s = planets[selectedIndex]._GameObject.transform.position;
        Vector3 e;
        for (int k = 0; k < planets.Length; k++)
        {
            e = planets[k]._GameObject.transform.position;

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

    private void MovePlanets(int planetNum)
    {
        // 1 unit from the sun is 100 day orbit
        int i = 0;

        foreach (Planet p in planets)
        {
            // Just calculate distance from origin (or radius of orbit)
            Vector3 v = p._GameObject.transform.position;
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
            i++;

        }
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
        return planets[planetIndex]._GameObject.transform.position;

    }

    public void InstantiatePlanet(Vector3 v, int index, float originDist)
    {
        // 1. Make GameObject at position
        GameObject go = Instantiate(planet, v, Quaternion.identity);

        // 2. Assign sprite to GameObject
        if (index == 1)
        {
            go.GetComponent<SpriteRenderer>().sprite = Molten;

        }
        else if (index == 2)
        {
            go.GetComponent<SpriteRenderer>().sprite = Earth;

        }
        else if (index == 3)
        {
            go.GetComponent<SpriteRenderer>().sprite = Water;

        }

        // 3. Choose PlanetType for Planet instance depending on distance to sun
        Planet.PlanetType pt;
        int ptChoice;
        if (originDist <= 1.5)
            pt = Planet.PlanetType.volcanic;
        else if (originDist <= 3)
        {
            ptChoice = UnityEngine.Random.Range(1, 4);
            if (ptChoice == 1)
                pt = Planet.PlanetType.arid;
            else if (ptChoice == 2)
                pt = Planet.PlanetType.ideal;
            else
                pt = Planet.PlanetType.oceanic;
        }
        else
        {
            ptChoice = UnityEngine.Random.Range(1, 2);
            if (ptChoice == 1)
                pt = Planet.PlanetType.ice;
            else
            {
                pt = Planet.PlanetType.gas;
            }
        }

        // 4. Create new planet instance and add it to the "planets" array        
        Planet p = new Planet(planetNames[index], pt, go);
        planets[index] = p;

    } 
}
