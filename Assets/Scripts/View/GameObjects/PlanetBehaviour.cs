using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class PlanetBehaviour : MonoBehaviour
{
    bool trackCursorPosition = false;

    private void OnMouseOver()
    {
        if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize == 15)
            GameObject.Find("Camera").GetComponent<GameDirector>().DrawOnePlanetToAll(this.gameObject);
        else
        {
            trackCursorPosition = true;
            
        }

        
    }

    private void OnMouseExit()
    {
        if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize == 15)
            GameObject.Find("Camera").GetComponent<GameDirector>().DestroyAllLines(); 
        trackCursorPosition = false;
    }

    private void OnMouseDown()
    {
        if (GameObject.Find("Camera").GetComponent<Camera>().orthographicSize == 15)
        {
            GameObject.Find("pg_planet").GetComponent<PlanetPanelBehaviour>().ShowPanel(this.gameObject);
            GameObject.Find("Camera").GetComponent<GameDirector>().SetSelectedPlanet(this.gameObject);
            GameObject.Find("b_toggle_planet_view").GetComponent<TogglePlanetViewBehaviour>().TogglePlanetView();
            trackCursorPosition = true;
        }

    }

    void Update()
    {
        if (trackCursorPosition)
        {
            // 0. Get the appropriate PlanetSectors array for the size of this mesh, will be used later to pinpoint sector
            // that cursor is in once the correct coordinates for the PlanetSectors array are found
            Sector[,] PlanetSectors = GameObject.Find("Camera").GetComponent<GameDirector>().FindPlanet(this.gameObject)._PlanetSectors;
            float sphereScale = GameObject.Find("Camera").GetComponent<GameDirector>().FindPlanet(this.gameObject)._SphereSize;

            // 1. Find the North, South, East and West GameObject that were created when the planet view was toggled on
            GameObject N = GameObject.Find("North");
            GameObject S = GameObject.Find("South");
            GameObject E = GameObject.Find("East");
            GameObject W = GameObject.Find("West");

            // 2. Get the point on the mesh that wil be hit by a ray cast from the cursor
            Ray ray = GameObject.Find("Camera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            float maxLength = 100f;
            RaycastHit info = new RaycastHit();
            GameObject.Find(this.gameObject.name).GetComponent<MeshCollider>().Raycast(ray, out info, maxLength);
            Debug.Log("Hit mesh at point " + new Vector3(info.point.x - this.gameObject.transform.position.x, info.point.y - this.gameObject.transform.position.y, info.point.z - this.gameObject.transform.position.z));
            Vector3 mPoint = info.point;

            Debug.Log("Hit mesh at point absolute " + info.point);

            // 3. Find how far along the N -> S and E -> W line mPoint is and thus find its corresponding sector
            // USING FIRST METHOD FROM: https://www.youtube.com/watch?v=9wznbg_aKOo
            // 3.1. (x - x_0 / a) = (y - y_0 / b) = (z - z_0 / c), U is UP (SN), R is RIGHT (WE)
            float xDiffU = (S.transform.position.x - N.transform.position.x);
            float yDiffU = (S.transform.position.y - N.transform.position.y);
            float zDiffU = (S.transform.position.z - N.transform.position.z);

            float xDiffR = (W.transform.position.x - E.transform.position.x);
            float yDiffR = (W.transform.position.y - E.transform.position.y);
            float zDiffR = (W.transform.position.z - E.transform.position.z);

            // 3.2. Find the smallest of the numerators
            float minDiffU = FindMinFromThree(xDiffU, yDiffU, zDiffU);
            float minDiffR = FindMinFromThree(xDiffR, yDiffR, zDiffR);

            Debug.Log("xDiffU: " + xDiffU + ", yDiffU: " + yDiffU + ", zDiffU: " + zDiffU);
            Debug.Log("minDiffU: " + minDiffU);

            Debug.Log("xDiffR: " + xDiffR + ", yDiffR: " + yDiffR + ", zDiffR: " + zDiffR);
            Debug.Log("minDiffR: " + minDiffR);

            // 3.3. Divide each numerator by the smallest number from above to get the a, b and c from (2.1)
            float UA = xDiffU / minDiffU;
            float UB = yDiffU / minDiffU;
            float UC = zDiffU / minDiffU;

            float RA = xDiffR / minDiffR;
            float RB = yDiffR / minDiffR;
            float RC = zDiffR / minDiffR;

            Debug.Log("UA: " + UA + ", UB: " + UB + ", UC: " + UC);
            Debug.Log("RA: " + RA + ", RB: " + RB + ", RC: " + RC);

            // 3.4. Find d for the equation of the plane for U and R i.e. (a, b, c) . (x, y, z) = (a, b, c) . (point) = d
            float du = (UA * mPoint.x) + (UB * mPoint.y) + (UC * mPoint.z);
            float dr = (RA * mPoint.x) + (RB * mPoint.y) + (RC * mPoint.z);

            // 3.5 Substitue parametric equations into the equation of the plane
            // 3.5.1. Parametric equations for U and R are
            // tu = (x - N.transform.position.x)/UA = (y - N.transform.position.y)/UB = (z - N.transform.position.z)/UC
            // tr = (x - E.transform.position.x)/RA = (y - E.transform.position.y)/RB = (z - E.transform.position.z)/RC

            // 3.5.2. Rearrange parametric equations for x, y and z
            // (UA * t) - N.transform.position.x = x
            // (UB * t) - N.transform.position.y = y
            // (UC * t) - N.transform.position.z = z

            // (RA * t) - E.transform.position.x = x
            // (RB * t) - E.transform.position.y = y
            // (RC * t) - E.transform.position.z = z

            // 3.5.2. Calculate t coefficient and integer component
            float tcu = ((UA * UA) + (UB * UB) + (UC * UC));
            float icu = ((UA * -N.transform.position.x) + (UB * -N.transform.position.y) + (UC * -N.transform.position.z) - du);

            float tcr = ((RA * RA) + (RB * RB) + (RC * RC));
            float icr = ((RA * -E.transform.position.x) + (RB * -E.transform.position.y) + (RC * -E.transform.position.z) - dr);

            // 3.5.3 To find t divide integer component by t coefficient
            float tu = icu / tcu;
            float tr = icr / tcr;

            // 4. Finally we can find the point on the N->S and E->W lines by substituting round t-values into parametric equations
            // for the x, y and z components of the perpendicular point on the U and R lines
            Vector3 pointU = new Vector3()
            {
                x = (UA * tu) - N.transform.position.x,
                y = (UB * tu) - N.transform.position.y,
                z = (UC * tu) - N.transform.position.z
            };

            Vector3 pointR = new Vector3()
            {
                x = (RA * tr) - E.transform.position.x,
                y = (RB * tr) - E.transform.position.y,
                z = (RC * tr) - E.transform.position.z
            };

            // 4. Find the distance of the points on the U and R line from the North and East respectively
            Debug.Log("point u is " + pointU);
            Debug.Log("point r is " + pointR);

            float distFromNorth = Vector3.Distance(pointU, N.transform.position);
            float distFromEast = Vector3.Distance(pointR, E.transform.position);

            // Distance from north should indicate the "x" position of the sector that the cursor is in, and distance from
            // east should indicate the "y" position of the sector that the cursor is in, in the 2D sector's array for that shape



            // 5. Get starting vector on sphere (i.e. origin + z-level below pole), find if hit point is above/below and left/right of that position
            int maxCoord = (int)(Planet._SectorSize * sphereScale);
            float radius = sphereScale / 2;

            int hi = -1, vi = -1;

            // Use planet sectors to increment through that mesh
            for (int i = 0; i < (maxCoord); i++)
            {
                Debug.Log(this.GetComponent<MeshFilter>().mesh);
                Debug.Log(this.GetComponent<MeshFilter>().mesh.vertices);
                Debug.Log(this.GetComponent<MeshFilter>().mesh.vertices[0]);
                Debug.Log(this.GetComponent<MeshFilter>().mesh.vertices[0].z);
                Debug.Log(i);

                float z = this.GetComponent<MeshFilter>().mesh.vertices[PlanetSectors[0, i]._CoordIndex].z;
                Debug.Log(z);

                Debug.Log("radius - z: " + (radius - z) + ", distFromNorth: " + distFromNorth);

                // Find the first z-value where distFromNorth < zFromNorth 
                if (distFromNorth <= (radius - z))
                {
                    vi = (i);
                    break;
                }

            }

            for (int j = 0; j < maxCoord; j++)
            {
                float y = this.GetComponent<MeshFilter>().mesh.vertices[PlanetSectors[j, vi]._CoordIndex].y;

                // TODO: DO the parametric stuff above with X as well to find the sign and determine the exact y coordinate here, because y here could conceivable have 2 valid points
                // Find the first z-value where distFromNorth < zFromNorth 
                if (distFromEast <= (radius - y))
                {
                    hi = j;
                    break;
                }

            }

            Sector FoundSector;
            if (hi == -1 || vi == -1)
                throw new System.Exception("Valid sector was not found");
            else
                FoundSector = PlanetSectors[hi, vi];

            Debug.Log(FoundSector._Name + "index: " + FoundSector._CoordIndex);

            /*Vector3 start = PolarToVector(this.gameObject.GetComponent<SphereCollider>().radius * this.gameObject.transform.localScale.x, Inc * Mathf.Deg2Rad, 0);
            Vector3 relativePoint = new Vector3(info.point.x - this.gameObject.transform.position.x, info.point.y - this.gameObject.transform.position.y, info.point.z - this.gameObject.transform.position.z);

            

            Debug.Log("Distance from origin point " + Mathf.Sqrt(Mathf.Sqrt(Mathf.Pow(relativePoint.x, 2) + Mathf.Pow(relativePoint.y, 2)) + Mathf.Pow(relativePoint.z, 2)));
            Debug.Log("Relative point is: " + relativePoint.z);
            Debug.Log("Start z point is: " + start.z);
            Debug.Log("Info point is " + info.point);
            Debug.Log("Origin point is " + this.gameObject.transform.position);

            int zSign;
            if (start.z <= relativePoint.z)
                zSign = 1;
            else
                zSign = -1;

            int ySign;
            if (relativePoint.y < start.y)
                ySign = -1;
            else if (relativePoint.y > start.y)
                ySign = 1;
            else if (start.y == relativePoint.y && start.x < 0)
                ySign = 1; // NOTE: Doesn't really matter either way as long as it traverses the sphere in some direction to reach the other side
            else
                ySign = 0; // In other words it's at its destination

            Debug.Log("ySign: " + ySign + ", zSign: " + zSign + "start z: " + start.z);
            */


        }

    }

    // REFACTOR - DUPLICATED CODE, BELONGS IN SphereMeshGenerator.cs
    public static Vector3 PolarToVector(float radius, float IncRad, float AziRad)
    {

        float x = radius * Mathf.Sin(IncRad) * Mathf.Cos(AziRad);
        float y = radius * Mathf.Sin(IncRad) * Mathf.Sin(AziRad);
        float z = radius * Mathf.Cos(IncRad);

        return new Vector3(x, y, z);
    }

    private float FindMinFromThree(float a, float b, float c)
    {
        float min;
        if (a <= b)
        {
            if (c <= a)
                min = c;
            else
                min = a;
        }
        else if (c <= b)
            min = c;
        else
            min = b;
        return min;
    }

}
