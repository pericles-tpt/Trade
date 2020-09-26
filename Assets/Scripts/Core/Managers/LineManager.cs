using UnityEngine;
using System;
using System.Collections.Generic;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using UnityEngine.UI;

public class LineManager
{
    private GameObject[] betweenPlanetLines;
    private GameObject sun = GameObject.Find("sun");
    private GalaxyManager gm = GameObject.Find("Camera").GetComponent<GalaxyManager>();

    private GameObject smallPlanetCollider = GameObject.Find("SmallPlanetCollider");
    private GameObject mediumPlanetCollider = GameObject.Find("MediumPlanetCollider");

    public void CreateGOBetweenPlanets (int noPlanets)
    {
        // FIGURE OUT NO LINES NEEDED TO DRAW LINES BETWEEN ALL PLANETS
        int noLines = 0;
        int i;
        for (i = noPlanets - 1; i >  0; i--)
            noLines += i;
        betweenPlanetLines = new GameObject[noLines];

        // ASSIGN EACH GO IN ARRAY AND ADD LINERENDERER COMPONENT TO THEM
        for (i = 0; i < noLines; i++)
        {
            betweenPlanetLines[i] = new GameObject();
            betweenPlanetLines[i].AddComponent<LineRenderer>();

            LineRenderer lr = betweenPlanetLines[i].GetComponent<LineRenderer>();
            lr.startColor = lr.endColor = Color.white;
            lr.startWidth = lr.endWidth = 0.1f;
            lr.material = new Material(Shader.Find("Unlit/Texture"));

            betweenPlanetLines[i].SetActive(false);
        }

    }

    public void CheckLinesFromAllPlanets(Vector3[] nodes, bool drawAllLines = false, int oneLineA = -1, int oneLineB = -1)
    {
        int lCount = 0;
        int nlCount = 0;



        // I KNOW .ToList<Vector3>() exists in LINQ but just testing taking outu any LINQ functions to improve performance
        Vector3[] nodes2 = nodes;

        for (int i = 0; i < nodes.Length; i++)
        {
            Vector3 s = nodes[i];
            Vector3 e;
            for (int j = 0; j < nodes2.Length; j++)
            {
                e = nodes2[j];
                if (j > i)
                {
                    if (!LineHitsSunCollider(s, e, i))
                    {
                        if ((drawAllLines == true) || (i == oneLineA && j == oneLineB))
                        {
                            ActivateLine(s, e, lCount);
                        }
                        lCount++;
                        int changeStateTurns = CalculateRouteChangeStateTurns(i, j, true);
                        UpdateTradeRoutesUI(i, j, true, changeStateTurns);
                    } else
                    {
                        nlCount++;
                        int changeStateTurns = CalculateRouteChangeStateTurns(i, j, false);
                        UpdateTradeRoutesUI(i, j, false, changeStateTurns);
                    }
                    Debug.Log("Node link is " + i + " " + j);
                } 
            }
        }

        Debug.Log("Created " + lCount + " lines between planets");
        Debug.Log(nlCount + " between planets info updated");

    }

    private int CalculateRouteChangeStateTurns(int a, int b, bool isOpen)
    {
        int changeStateTurns = 0;
        // GET CURRENT POSITIONS OF A AND B
        Vector3 A = gm.GetPlanetByIndex(a)._GameObject.transform.position;
        Vector3 B = gm.GetPlanetByIndex(b)._GameObject.transform.position;

        // WHILE A -> B IS IN THE SAME STATE, INCREMENT A COUNTER
        while (LineHitsSunCollider(A, B, a) == !isOpen)
        {
            A = IncOrbitTest(A, a);
            B = IncOrbitTest(B, b);
            changeStateTurns++;
        }

        return changeStateTurns;
    }

    public void ActivateLine(Vector3 start, Vector3 end, int index, float duration = 0.2f)
    {
        betweenPlanetLines[index].transform.position = start;

        LineRenderer lr = betweenPlanetLines[index].GetComponent<LineRenderer>();
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        betweenPlanetLines[index].SetActive(true);
    }

    // LINE DESTRUCTION/HIDING FUNCTIONS
    public void DeactivateAllLines()
    {
        for (int i = 0; i < betweenPlanetLines.Length; i++)
            betweenPlanetLines[i].SetActive(false);
    }

    // CHECKING IF LINES HIT OBJECTS
    private bool LineHitsSunCollider(Vector3 start, Vector3 end, int startPlanetIndex)
    {
        Vector2 direction = new Vector2(end.x - start.x, end.y - start.y);
        RaycastHit2D[] rch = new RaycastHit2D[3];
        GameObject chosen;

        // Get start planet by index, use its collider to cast a ray to the end point...
        // if the first position that the ray hits is on the sun's CircleCollider2D...
        // then return true to indicate that the line hits the sun's collider, defaults to false
        if (gm.GetPlanetByIndex(startPlanetIndex)._GameObject.transform.localScale.x == 1f)
        {
            chosen = smallPlanetCollider;
        } else
        {
            chosen = mediumPlanetCollider;
        }

        chosen.transform.position = start;
        int hits = chosen.GetComponent<CircleCollider2D>().Raycast(direction, rch);

        Debug.Log("There have been " + hits + " starting at index " + startPlanetIndex);

        for (int i = 0; i < hits; i++)
        {
            if (rch[i].transform.gameObject == sun)
                return true;
        }

        return false;

    }

    public void UpdateTradeRoutesUI(int a, int b, bool isOpen, int turnsUntilChangeState)
    {
        // TODO: COLOR ASSIGNMENT TO TEXT COMPONENTS NOT WORKING ATM... FIX
        if (GameObject.Find("Route" + a + b))
        {
            Transform UI = GameObject.Find("Route" + a + b).transform;
            Planet pa = gm.GetPlanetByIndex(a);
            Planet pb = gm.GetPlanetByIndex(b);
            Color red = new Color(197, 78, 78);
            Color green = new Color(78, 152, 86);
            UI.FindChild("PlanetAIcon").GetComponent<Image>().sprite = pa._Icon;
            UI.FindChild("PlanetBIcon").GetComponent<Image>().sprite = pb._Icon;
            UI.FindChild("PlanetAName").GetComponent<Text>().text = pa._Name + " " + pa._NameNo;
            UI.FindChild("PlanetBName").GetComponent<Text>().text = pb._Name + " " + pb._NameNo;

            Transform rcc = UI.FindChild("RouteChangeCountdown");
            Transform da = UI.FindChild("DoubleArrow");
            if (isOpen)
            {
                rcc.GetComponent<Text>().color = red;
                rcc.GetComponent<Text>().text = "CLOSES IN " + turnsUntilChangeState + " TURNS";
                da.GetComponent<Text>().color = green;
                da.GetComponent<Text>().text = "<--->";
            }
            else
            {
                rcc.GetComponent<Text>().color = green;
                rcc.GetComponent<Text>().text = "OPENS IN " + turnsUntilChangeState + " TURNS";
                da.GetComponent<Text>().color = red;
                da.GetComponent<Text>().text = "<-/->";
            }
        }
    }

    private Vector3 IncOrbitTest(Vector3 planetPos, int planetIndex)
    {
        // Just calculate distance from origin (or radius of orbit)
        Vector3 sunPos = sun.transform.position;
        Vector2 vt = new Vector3(planetPos.x - sunPos.x, planetPos.y, planetPos.z);

        float radius;
        float orbitInc;

        radius = Mathf.Sqrt(Mathf.Pow(vt.x, 2) + Mathf.Pow(vt.y, 2));
        orbitInc = radius * 100;

        float orbitSectorAngle = (360 / orbitInc);
        float orbitSectorAngleRadians = orbitSectorAngle * Mathf.Deg2Rad;
        float nextX, nextY;

        if (planetIndex == 2)
        {
            nextX = sunPos.x + (vt.x * Mathf.Cos(-orbitSectorAngleRadians)) + (vt.y * Mathf.Sin(-orbitSectorAngleRadians));
            nextY = (-vt.x * Mathf.Sin(-orbitSectorAngleRadians)) + (vt.y * Mathf.Cos(-orbitSectorAngleRadians));

        }
        else
        {
            nextX = sunPos.x + (vt.x * Mathf.Cos(orbitSectorAngleRadians)) + (vt.y * Mathf.Sin(orbitSectorAngleRadians));
            nextY = (-vt.x * Mathf.Sin(orbitSectorAngleRadians)) + (vt.y * Mathf.Cos(orbitSectorAngleRadians));

        }

        // Create new v and assign it to the gameobject
        return new Vector3(nextX, nextY, -10);

    }

}
