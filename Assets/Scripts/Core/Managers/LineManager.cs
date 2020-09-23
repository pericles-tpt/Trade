using UnityEngine;
using System;
using System.Collections.Generic;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

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

        // I KNOW .ToList<Vector3>() exists in LINQ but just testing taking outu any LINQ functions to improve performance
        List<Vector3> nodesReduce = new List<Vector3>();
        for (int i = 0; i < nodes.Length; i++)
        {
            nodesReduce.Add(nodes[i]);
        }

        for (int i = 0; i < nodes.Length; i++)
        {
            Vector3 s = nodes[i];
            Vector3 e;
            for (int j = 0; j < nodesReduce.Count; j++)
            {
                e = nodesReduce[j];

                if (nodes[i] != nodesReduce[j])
                {
                    if (!LineHitsSunCollider(s, e, i))
                    {
                        ActivateLine(s, e, lCount);
                        lCount++;
                    }

                }



            }

            nodesReduce.RemoveAt(0);

        }

        Debug.Log("Created " + lCount + " lines between planets");

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

}
