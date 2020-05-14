﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class LineManager
{

    public Dictionary<Tuple<Vector3, Vector3>, GameObject> Lines = new Dictionary<Tuple<Vector3, Vector3>, GameObject>();

    public void DrawAllLines(GameObject selectedPlanet, Vector3[] positions)
    {
        Vector3 s;
        Color c = new Color(0, 0, 0);
        int dl = 0;

        DestroyAllLines();

        int selectedIndex = 0;
        for (int i = 0; i < positions.Length; i++)
            if (positions[i] == selectedPlanet.transform.position)
                selectedIndex = i;

        s = positions[selectedIndex];
        Vector3 e;
        for (int k = 0; k < positions.Length; k++)
        {
            e = positions[k];

            if (selectedIndex != k)
            {
                //Debug.Log("Drawn line from: " + s.ToString() + "to: " + e.ToString());
                if (!LineHitsSunCollider(s, e))
                    DrawLine(s, e, ref Lines, 0);

            }
            else
            {
                //Debug.Log("Prevented node line to self");

            }

        }

        //Debug.Log("Drew " + dl + " lines");
    }

    public void DestroyAllLines()
    {
        foreach (KeyValuePair<Tuple<Vector3, Vector3>, GameObject> i in Lines)
            UnityEngine.Object.Destroy(i.Value);
        Lines.Clear();
    }

    public void ToggleHideAllLines (bool show)
    {
        foreach (KeyValuePair<Tuple<Vector3, Vector3>, GameObject> i in Lines)
            i.Value.GetComponent<LineRenderer>().forceRenderingOff = show;
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

    public void DestroyLineByGO(GameObject GO)
    {
        foreach (KeyValuePair<Tuple<Vector3, Vector3>, GameObject> i in Lines)
            if ((i.Key.Item1 == GO.transform.position) || (i.Key.Item2 == GO.transform.position))
            {
                UnityEngine.Object.Destroy(i.Value);
                Lines.Remove(i.Key);
            }
    }

    // CREDIT (paranoidray): https://answers.unity.com/questions/8338/how-to-draw-a-line-using-script.html
    // TODO: Come up with own DrawLine function
    public void DrawLine(Vector3 start, Vector3 end, ref Dictionary<Tuple<Vector3, Vector3>, GameObject> planetLines, float duration = 0.2f)
    {
        // Return if line would cross the origin (a.k.a the sun)
        //if (LineIntersectsSun(start, end))
        //    return;

        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.SetColors(Color.white, Color.white);
        lr.SetWidth(0.1f, 0.1f); // NOTE: Was 0.1f before
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        Material whiteDiffuse = new Material(Shader.Find("Unlit/Texture"));
        lr.material = whiteDiffuse;

        planetLines.Add(Tuple.Create(start, end), myLine);
    }

    public void DrawLine(Vector3 start, Vector3 end, float width = 0.01f, float duration = 0.2f)
    {
        // Return if line would cross the origin (a.k.a the sun)
        //if (LineIntersectsSun(start, end))
        //    return;

        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.SetColors(Color.white, Color.white);
        lr.SetWidth(width, width); // NOTE: Was 0.1f before
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        Material whiteDiffuse = new Material(Shader.Find("Unlit/Texture"));
        lr.material = whiteDiffuse;

        float length = Vector3.Distance(end, start);

        Debug.Log("Created line at z-position (end): " + end.z + ", of length: " + length);

        Lines.Add(Tuple.Create(start, end), myLine);
    }

    private bool LineHitsSunCollider(Vector3 start, Vector3 end)
    {
        Vector3 direction = new Vector3(end.x - start.x, end.y - start.y, end.z - start.z);
        float maxLength = Vector3.Distance(start, end);
        Ray ray = new Ray(start, direction);
        RaycastHit info = new RaycastHit();
        return(GameObject.Find("sun").GetComponent<SphereCollider>().Raycast(ray, out info, maxLength));

    }

}
