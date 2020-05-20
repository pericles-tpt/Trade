using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlanetBehaviour : MonoBehaviour
{
    bool trackCursorPosition = false;
    QuadMeshGenerator QMG = new QuadMeshGenerator();
    TriangleMeshGenerator TMG = new TriangleMeshGenerator();
    int selectedSector;
    bool drawNewSector = true;

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
            Planet p = GameObject.Find("Camera").GetComponent<GameDirector>().FindPlanet(this.gameObject);
            float sphereScale = GameObject.Find("Camera").GetComponent<GameDirector>().FindPlanet(this.gameObject)._SphereSize;

            Camera cam = GameObject.Find("Camera").GetComponent<Camera>();

            //2. Get the point on the mesh that wil be hit by a ray cast from the cursor
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit rHit;
            Physics.Raycast(ray, out rHit, Mathf.Infinity);

            Debug.Log("Raycast position " + rHit.point);

            int maxCoord = (int)(Planet._SectorSize * sphereScale);
            float radius = sphereScale / 2;

            Vector3[] verts = this.GetComponent<MeshFilter>().mesh.vertices;

            int zi = -1;
            for (int i = 0; i < maxCoord; i++)
            {
                Debug.Log("Index is " + i);

                float z = -1000000;
                if (i < (maxCoord - 1))
                    z = (verts[PlanetSectors[0, i]._CoordIndex].z * sphereScale) + (this.gameObject.transform.position.z);
                if (i == (maxCoord - 1))
                    z = (verts[PlanetSectors[0, maxCoord - 2]._CoordIndex].z * sphereScale) + (this.gameObject.transform.position.z);

                Debug.Log("z is " + z);
                if (i == (maxCoord - 1))
                {
                    if (rHit.point.z < z)
                    {
                        Debug.Log("I found u your index is " + i + "and the z coord that is < this is " + z);
                        zi = i;
                        break;
                    }
                } else {
                    if (rHit.point.z >= z)
                    {
                        Debug.Log("I found u your index is " + i + "and the z coord that is < this is " + z);
                        zi = i;
                        break;
                    }
                }
            }

            if (zi == -1)
                throw new System.Exception("y coord for Sector not found");

            int hi = -1;
            for (int i = 0; i < (maxCoord); i++)
            {
                Debug.Log("i: " + i + ", zi: " + zi);
                float x = (verts[PlanetSectors[i, zi]._CoordIndex].x * sphereScale) + (this.gameObject.transform.position.x);
                float y = (verts[PlanetSectors[i, zi]._CoordIndex].y * sphereScale) + (this.gameObject.transform.position.y);

                Debug.Log("Index is " + i + "level is " + zi);

                float yp;
                if (i < (maxCoord - 1))
                    yp = (verts[PlanetSectors[i + 1, zi]._CoordIndex].y * sphereScale) + (this.gameObject.transform.position.y);
                else 
                    yp = (verts[PlanetSectors[0, zi]._CoordIndex].y * sphereScale) + (this.gameObject.transform.position.y);

                Debug.Log("y is " + y + ", x is " + x);

                Debug.Log("rhit.y " + rHit.point.y + ", y: " + y + ", yp: " + yp);

                if ((rHit.point.x <= this.gameObject.transform.position.x) && (x <= this.gameObject.transform.position.x))
                {
                    if (rHit.point.y <= y && rHit.point.y > yp)
                    {
                        Debug.Log("I found u your index is " + i + "and the z coord that is < this is " + y);
                        hi = i;
                        break;

                    }
                } else if ((rHit.point.x > this.gameObject.transform.position.x) && (x > this.gameObject.transform.position.x))
                {
                    // y is going down
                    if (rHit.point.y >= y && rHit.point.y < yp)
                    {
                        Debug.Log("I found u your index is " + i + "and the z coord that is < this is " + y);
                        hi = i;
                        break;

                    }
                } else if (((rHit.point.x > this.gameObject.transform.position.x) && (x < this.gameObject.transform.position.x)) || ((rHit.point.x <= this.gameObject.transform.position.x) && (x <= this.gameObject.transform.position.x)))
                {
                    Debug.Log("aha we got ya");
                }

            }

            Debug.Log("hi is: " + hi + ", zi is: " + zi);
            if (selectedSector == null) {
                selectedSector = PlanetSectors[hi, zi]._CoordIndex;
                drawNewSector = true;
            } else if (selectedSector != PlanetSectors[hi, zi]._CoordIndex)
            {
                selectedSector = PlanetSectors[hi, zi]._CoordIndex;
                drawNewSector = true;
            } else
            {
                drawNewSector = false;
            }


            Debug.Log("rHit is " + rHit.point.y);

            GameObject highlighted = new GameObject();
            highlighted.name = "highlighted";

            if (hi == -1)
                throw new System.Exception("y coord for Sector not found");
            else {
                if (drawNewSector)
                {
                    p._SectorLines.DestroyAllLines();

                    if (PlanetSectors[hi, zi]._Shape == Sector.Shape.square)
                    {
                        p.DrawQuadSectorBoundaries(verts[PlanetSectors[hi, zi]._CoordIndex], verts[PlanetSectors[(int)(hi + sphereScale), zi]._CoordIndex], verts[PlanetSectors[hi, (int)(zi - sphereScale)]._CoordIndex], verts[PlanetSectors[(int)(hi + sphereScale), (int)(zi - sphereScale)]._CoordIndex]);

                    }
                    else if (PlanetSectors[hi, zi]._Shape == Sector.Shape.triangleDown)
                    {
                        p.DrawTriangleSectorBoundaries(verts[PlanetSectors[hi, zi]._CoordIndex], verts[PlanetSectors[(int)(hi + sphereScale), zi]._CoordIndex], new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z - radius));
                    }
                    else
                    {
                        p.DrawTriangleSectorBoundaries(verts[PlanetSectors[hi, zi]._CoordIndex], verts[PlanetSectors[(int)(hi + sphereScale), zi]._CoordIndex], new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z + radius));
                    }
                }
            }


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
