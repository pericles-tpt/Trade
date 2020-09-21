﻿using UnityEngine;
using System;
using System.Collections.Generic;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using System.Linq;

public class LineManager
{
    public Dictionary<Tuple<Vector3, Vector3>, GameObject> Lines = new Dictionary<Tuple<Vector3, Vector3>, GameObject>();

    // LINE DRAWING FUNCTIONS
    public void DrawLinesFromOnePlanet(Vector3 selectedPlanetPosition, Vector3[] positions)
    {
        Vector3 s;

        int selectedIndex = 0;
        for (int i = 0; i < positions.Length; i++)
            if (positions[i] == selectedPlanetPosition)
                selectedIndex = i;

        s = positions[selectedIndex];
        Vector3 e;
        for (int k = 0; k < positions.Length; k++)
        {
            e = positions[k];

            if (selectedIndex != k)
            {
                if (!LineHitsSunCollider(s, e, selectedIndex))
                    DrawLine(s, e, ref Lines, 0);

            }

        }

    }

    public void DrawLinesFromAllPlanets(Vector3[] nodes)
    {
        int dCount = 0;

        List<Vector3> nodesReduce = nodes.ToList<Vector3>();
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
                        DrawLine(s, e, ref Lines, 0);

                }

                dCount++;

            }

            nodesReduce.RemoveAt(0);

        }

        Debug.Log("Created " + dCount + " lines between planets");

    }

    public void DrawLine(Vector3 start, Vector3 end, ref Dictionary<Tuple<Vector3, Vector3>, GameObject> planetLines, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;

        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.startColor = lr.endColor = Color.white;
        lr.startWidth = lr.endWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);

        Material whiteDiffuse = new Material(Shader.Find("Unlit/Texture"));
        lr.material = whiteDiffuse;

        planetLines.Add(Tuple.Create(start, end), myLine);
    }

    // LINE DESTRUCTION/HIDING FUNCTIONS
    public void DestroyAllLines()
    {
        foreach (KeyValuePair<Tuple<Vector3, Vector3>, GameObject> i in Lines)
            UnityEngine.Object.Destroy(i.Value);
        Lines.Clear();
    }

    public void DestroyLineByCoord(Tuple<Vector3, Vector3> Coord)
    {
        foreach (KeyValuePair<Tuple<Vector3, Vector3>, GameObject> i in Lines)
            if (i.Key == Coord)
            {
                UnityEngine.Object.Destroy(i.Value);
                Lines.Remove(i.Key);
            }
    }

    public void ToggleHideAllLines (bool show)
    {
        foreach (KeyValuePair<Tuple<Vector3, Vector3>, GameObject> i in Lines)
            i.Value.GetComponent<LineRenderer>().forceRenderingOff = show;
    }

    public void ToggleHideLinesByCoord(Vector3 coord, bool show)
    {
        foreach (KeyValuePair<Tuple<Vector3, Vector3>, GameObject> i in Lines)
            if (i.Key.Item1 == coord || i.Key.Item2 == coord)
                i.Value.GetComponent<LineRenderer>().forceRenderingOff = show;
    }

    // CHECKING IF LINES HIT OBJECTS
    private bool LineHitsSunCollider(Vector3 start, Vector3 end, int startPlanetIndex)
    {
        Vector2 direction = new Vector2(end.x - start.x, end.y - start.y);
        GameObject sun = GameObject.Find("sun");
        GalaxyManager gm = GameObject.Find("Camera").GetComponent<GalaxyManager>();
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
