using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCreator : MonoBehaviour
{
    public GameObject planet;

    // Start is called before the first frame update
    void Start()
    {
        // Generates between 4 and 6 planets
        int planetNum = UnityEngine.Random.Range(4, 5);

        // Stores all node positions for comparing a new node to previous node
        // positions to determine if two nodes are too close together
        Vector3[] planetCoords = new Vector3[planetNum];
        var planetLines = new Dictionary<Tuple<Vector3, Vector3>, GameObject>();

        GeneratePlanets(ref planetCoords, planetNum);
        DrawTradeLines(ref planetCoords, ref planetLines);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Place planets
    void GeneratePlanets(ref Vector3[] nodeCoords, int planetNum)
    {

        // Planet are all on the same depth level
        const int planetDepth = -10;
        Vector3 v;

        int addHun = 2;
        int addFif = 2;
        float originDist = 1;

        for (int i = 0; i < (planetNum); i++)
        {
            // Instantiate and record planet coords at start of iteration
            originDist += i;

            // START SECTION: Code in here tries to create random initial placement
            // of planets' wrt distance and x,y positioning

            // Adds an extra 100px or 50px between planets to add some irregularity
            // If on 1st iteration add an extra hundred to make it a realistic distance
            // from sun.
            if (((UnityEngine.Random.Range(0, 2) == 1) || (i == 0)) && (addHun > 0))
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
            Instantiate(planet, v, Quaternion.identity);
            nodeCoords[i] = v;
            Debug.Log("Node placed at: " + "x: " + (originDist * 100 * xSign) + ", y: " + originDist * 100 * ySign);

        }

        foreach (Vector3 v3 in nodeCoords)
        {
            Debug.Log("nodeCoords list: " + v3.ToString());
        }
    }

    void DrawTradeLines(ref Vector3[] planetCoords, ref Dictionary<Tuple<Vector3, Vector3>, GameObject> planetLines)
    {
        Vector3 s;
        Color c = new Color(0, 0, 0);
        for (int i = 0; i < planetCoords.Length; i++)
        {
            s = planetCoords[i];
            Vector3 e;
            for (int j = 0; j < planetCoords.Length; j++)
            {
                e = planetCoords[j];

                if (i != j)
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
    }

    // CREDIT (paranoidray): https://answers.unity.com/questions/8338/how-to-draw-a-line-using-script.html
    // ToDo: Come up with own DrawLine function
    void DrawLine(Vector3 start, Vector3 end, Color color, ref Dictionary<Tuple<Vector3, Vector3>, GameObject> planetLines, float duration = 0.2f)
    {
        // Return if line would cross the origin (a.k.a the sun)
        float m = (end.y - start.y) / (end.x - start.x);
        float b = (start.y - (start.x * m));
        if ((b == (0)) && ((start.y < 0 && end.y > 0) || (start.y > 0 && end.y < 0)))
            return;

        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.startColor = color;
        lr.startWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        planetLines.Add(Tuple.Create(start, end), myLine);
    }
}
