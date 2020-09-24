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

    public void ReDrawLinesFromAllPlanets(Vector3[] nodes)
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
                        ActivateLine(s, e, lCount);
                        lCount++;
                        UpdateTradeRoutesUI(i, j, true);
                        Debug.Log("Node link is " + i + " " + j);
                    } else
                    {
                        UpdateTradeRoutesUI(i, j, false);
                        nlCount++;
                        Debug.Log("Node link is " + i + " " + j);
                    }
                } 
            }
        }

        Debug.Log("Created " + lCount + " lines between planets");
        Debug.Log(nlCount + " between planets info updated");

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
        RaycastHit2D[] rch = new RaycastHit2D[1000];
        
        // Get start planet by index, use its collider to cast a ray to the end point...
        // if the first position that the ray hits is on the sun's CircleCollider2D...
        // then return true to indicate that the line hits the sun's collider, defaults to false
        gm.GetPlanetByIndex(startPlanetIndex)._GameObject.GetComponent<CircleCollider2D>().Raycast(direction, rch);
        if (sun.GetComponent<CircleCollider2D>().bounds.Contains(rch[0].transform.position))
            return true; 
        return false;

    }

    public void UpdateTradeRoutesUI(int a, int b, bool isOpen)
    {
        if (GameObject.Find("Route" + a + b))
        {
            Transform UI = GameObject.Find("Route" + a + b).transform;
            Planet pa = gm.GetPlanetByIndex(a);
            Planet pb = gm.GetPlanetByIndex(b);
            Color red = new Color(197, 78, 78);
            Color green = new Color(78, 152, 86);
            UI.FindChild("PlanetAIcon").GetComponent<Image>().sprite = pa._Icon;
            UI.FindChild("PlanetBIcon").GetComponent<Image>().sprite = pb._Icon;
            UI.FindChild("PlanetAName").GetComponent<Text>().text = pa._Name;
            UI.FindChild("PlanetBName").GetComponent<Text>().text = pb._Name;

            Transform rcc = UI.FindChild("RouteChangeCountdown");
            Transform da = UI.FindChild("DoubleArrow");
            if (isOpen)
            {
                rcc.GetComponent<Text>().color = red;
                rcc.GetComponent<Text>().text = "CLOSES IN N TURNS";
                da.GetComponent<Text>().color = green;
                da.GetComponent<Text>().text = "<->";
            }
            else
            {
                rcc.GetComponent<Text>().color = green;
                rcc.GetComponent<Text>().text = "OPENS IN N TURNS";
                da.GetComponent<Text>().color = red;
                da.GetComponent<Text>().text = "<-/->";
            }
        }
    }

}
