using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCreator : MonoBehaviour
{
    public GameObject planet;
    public GameObject player;
    public int fCount = 0;
    public int fsCount = 0;
    public int planetNum;
    public Ship initial;
    public Vector3[] planetCoords;
    public GameObject[] planetObjects;
    public Dictionary<Tuple<Vector3, Vector3>, GameObject> planetLines;

    // Start is called before the first frame update
    void Start()
    {
        // Generates between 4 and 6 planets
        int planetNum = UnityEngine.Random.Range(4, 6);

        // Stores all node positions for comparing a new node to previous node
        // positions to determine if two nodes are too close together
        planetCoords = new Vector3[planetNum];
        planetObjects = new GameObject[planetNum];
        planetLines = new Dictionary<Tuple<Vector3, Vector3>, GameObject>();

        GeneratePlanets(ref planetCoords, planetNum);

        Instantiate(player, new Vector3(100, 100, 100), Quaternion.identity);
        //DrawTradeLines(ref planetCoords, ref planetLines, false);
    }

    // Update is called once per frame
    void Update()
    {
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
            planetObjects[i] = go;
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
                        DrawLine(s, e, c, ref planetLines, 0);

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
        for (int i = 0; i < planetObjects.Length; i++)
            if (planetObjects[i] == findPlanet)
                selectedIndex = i;

        s = planetCoords[selectedIndex];
        Vector3 e;
        for (int k = 0; k < planetCoords.Length; k++)
        {
            e = planetCoords[k];

            if (selectedIndex != k)
            {
                if (!planetLines.ContainsKey(Tuple.Create(e, s)))
                {
                    Debug.Log("Drawn line from: " + s.ToString() + "to: " + e.ToString());
                    DrawLine(s, e, c, ref planetLines, 0);

                }
                else
                    Debug.Log("Eliminated one crossover");

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
    private void DrawLine(Vector3 start, Vector3 end, Color color, ref Dictionary<Tuple<Vector3, Vector3>, GameObject> planetLines, float duration = 0.2f)
    {
        // Return if line would cross the origin (a.k.a the sun)
        if (LineIntersectsSun(start, end))
            return;

        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
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
            planetObjects[i].transform.position = v3;

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

    private bool LineIntersectsSun(Vector3 start, Vector3 end)
    {
        float m = (end.y - start.y) / (end.x - end.y);
        bool ret = true;

        if (m > 0) {
            if ((start.y > 0) || (start.y < 0 && end.y < 0))
                ret = false;

        }

        else if (m < 0)
        {
            if (start.y < 0)
                ret = false;

        }

        else
        {
            if (((end.x > 0) && (start.x > 0)) || ((end.x < 0) && (start.x < 0)))
                ret = false;

        }

        return ret;
    }

    public Vector3 InstantiateFirstShip(int planetIndex)
    {
        return planetCoords[planetIndex];

    }
}
