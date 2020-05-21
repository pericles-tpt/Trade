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

    int frameCount = 0;

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
            GameObject.Find("Camera").GetComponent<GameDirector>().SetPlanetPositionBeforeZoom(this.gameObject.transform.position);
            GameObject.Find("b_toggle_planet_view").GetComponent<TogglePlanetViewBehaviour>().TogglePlanetView();
            trackCursorPosition = true;

            GameObject.Find("Camera").GetComponent<GameDirector>().ToggleSectorTooltipVisible(true);
        }

    }

    void Update()
    {
        frameCount++;

        // Do stuff - Improves performance by doing this operation every 12 frames (or every 0.2s if running at 60fps)
        if (frameCount % 2 == 0)
        {
            if (trackCursorPosition)
            {
                // 0. Use the vector index data from the PlanetSectors array from this Planet, to pinpoint hovered sector
                Planet p                = GameObject.Find("Camera").GetComponent<GameDirector>().FindPlanet(this.gameObject);
                Sector[,] PlanetSectors = p._PlanetSectors;
                float radius            = p._SphereSize;

                //1. Get the point on the mesh that wil be hit by a ray cast from the cursor
                Camera cam = GameObject.Find("Camera").GetComponent<Camera>();
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit rHit;
                Physics.Raycast(ray, out rHit, Mathf.Infinity);

                // maxCoord: number of increments along long + lat, verts: list of vertices in the mesh to be indexed by PlanetSectors
                int maxCoord = (int)(Planet._SectorSize * p._SphereSize);
                Vector3[] verts = this.GetComponent<MeshFilter>().mesh.vertices;

                // 2. Find the z-value in verts that corresponds with the sector that is being hovered over
                int zi = -1;
                float z;
                for (int i = 0; i < maxCoord; i++)
                {
                    if (i < (maxCoord - 1))
                    {
                        z = (verts[PlanetSectors[0, i]._CoordIndex].z * p._SphereSize) + (this.gameObject.transform.position.z);
                        
                        if (rHit.point.z >= z)
                        {
                            zi = i;
                            break;
                        }
                    }
                    else if (i == (maxCoord - 1))
                    {
                        z = (verts[PlanetSectors[0, maxCoord - 2]._CoordIndex].z * p._SphereSize) + (this.gameObject.transform.position.z);

                        if (rHit.point.z < z)
                        {
                            zi = i;
                            break;
                        }
                    }
                }

                Debug.Log("zi, rHit is " + rHit.point);
                Debug.Log("zi is " + zi);

                if (zi == -1)
                    throw new System.Exception("z coord for Sector not found");

                // START LOOKING AT THIS FROM HERE
                int hi = -1;
                for (int i = 0; i < (maxCoord); i++)
                {
                    Debug.Log("i: " + i + ", zi: " + zi);
                    float x = (verts[PlanetSectors[i, zi]._CoordIndex].x * p._SphereSize) + (this.gameObject.transform.position.x);
                    float y = (verts[PlanetSectors[i, zi]._CoordIndex].y * p._SphereSize) + (this.gameObject.transform.position.y);
                    float diff = 0.01f * Mathf.Abs(zi) * p._SphereSize;

                    Debug.Log("Index is " + i + "level is " + zi);

                    float yp;
                    if (i < (maxCoord - 1))
                        yp = (verts[PlanetSectors[i + 1, zi]._CoordIndex].y * p._SphereSize) + (this.gameObject.transform.position.y);
                    else
                        yp = (verts[PlanetSectors[0, zi]._CoordIndex].y * p._SphereSize) + (this.gameObject.transform.position.y);

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
                    }
                    else if ((rHit.point.x > this.gameObject.transform.position.x) && (x > this.gameObject.transform.position.x))
                    {
                        // y is going down
                        if (rHit.point.y >= y && rHit.point.y < yp)
                        {
                            Debug.Log("I found u your index is " + i + "and the z coord that is < this is " + y);
                            hi = i;
                            break;

                        }
                    // Handles crossover to new quadrant, i.e. 0.95 -> 1 -> 0.95 and 0.05 -> 0 -> 0.05

                    } else if (((Mathf.Abs(rHit.point.y) <= radius) && (Mathf.Abs(rHit.point.y) >= (radius - diff))) || ((Mathf.Abs(rHit.point.y) <= diff) && (Mathf.Abs(rHit.point.y) >= 0f)))
                    {
                        if (rHit.point.y <= 0)
                        {
                            if (rHit.point.y > -radius)
                            {
                                if (rHit.point.y >= -diff)
                                {
                                    if (rHit.point.x > 0)
                                    {
                                        // Should be sector (1/4 * maxCoord)
                                        hi = (int)((1f / 4f) * maxCoord);
                                    }
                                    else
                                    {
                                        // Should be sector (3/4 * maxCoord) - 1
                                        hi = (int)((3f / 4f) * maxCoord) - 1;
                                    }
                                } else
                                {
                                    if (rHit.point.x > 0)
                                    {
                                        // Should be sector (2/4 * maxCoord) - 1
                                        hi = (int)((2f / 4f) * maxCoord) - 1;
                                    }
                                    else
                                    {
                                        // Should be sector (2/4 * maxCoord)
                                        hi = (int)((3f / 4f) * maxCoord);
                                    }
                                }
                            }
                        } else if (rHit.point.y >= 0)
                        {
                            if (rHit.point.y >= diff)
                            {
                                if (rHit.point.x > 0)
                                {
                                    // Should be sector (0)
                                    hi = 0;
                                }
                                else
                                {
                                    // Should be sector (1/4 * maxCoord)
                                    hi = (int)((1f / 4f) * maxCoord);
                                }
                            }
                            else
                            {
                                if (rHit.point.x > 0)
                                {
                                    // Should be sector (1/4 * maxCoord) - 1
                                    hi = (int)((1f / 4f) * maxCoord) - 1;
                                }
                                else
                                {
                                    // Should be sector (3/4 * maxCoord)
                                    hi = (int)((3f / 4f) * maxCoord);
                                }
                            }
                        }
                    }
                    else if (((rHit.point.x > this.gameObject.transform.position.x) && (x < this.gameObject.transform.position.x)) || ((rHit.point.x <= this.gameObject.transform.position.x) && (x <= this.gameObject.transform.position.x)))
                    {
                        Debug.Log("aha we got ya");
                    }
                    

                }

                Debug.Log("hi is: " + hi + ", zi is: " + zi);
                if (selectedSector == null)
                {
                    selectedSector = PlanetSectors[hi, zi]._CoordIndex;
                    drawNewSector = true;
                }
                else if (selectedSector != PlanetSectors[hi, zi]._CoordIndex)
                {
                    selectedSector = PlanetSectors[hi, zi]._CoordIndex;
                    drawNewSector = true;
                }
                else
                {
                    drawNewSector = false;
                }


                Debug.Log("rHit is " + rHit.point.y);

                Vector3[] meshVertices = p._GameObject.GetComponent<MeshFilter>().mesh.vertices;

                if (hi == -1)
                    throw new System.Exception("y coord for Sector not found");
                else
                {
                    if (drawNewSector)
                    {
                        p._SectorLines.DestroyAllLines();

                        float lineWidth = 0.01f;
                        if (p._SphereSize == 2)
                            lineWidth *= 2;
                        else if (p._SphereSize == 1.5f)
                            lineWidth *= 1.5f;

                        if (PlanetSectors[hi, zi]._Shape == Sector.Shape.square)
                        {
                            Debug.Log("INDEX OUT OF BOUND hi: " + hi + "zi: " + zi);
                            int nexthi = hi + 1;
                            if (hi == (maxCoord - 1))
                                nexthi = 0;
                            p.DrawQuadSectorBoundaries(verts[PlanetSectors[hi, zi]._CoordIndex], verts[PlanetSectors[nexthi, zi]._CoordIndex], verts[PlanetSectors[hi, zi - 1]._CoordIndex], verts[PlanetSectors[nexthi, zi - 1]._CoordIndex], lineWidth);

                        }
                        else if (PlanetSectors[hi, zi]._Shape == Sector.Shape.triangleUp)
                        {
                            Vector3 top = meshVertices[0];
                            Debug.Log("The top vertex is " + top);
                            int nexthi = hi + 1;
                            if (hi == (maxCoord - 1))
                                nexthi = 1;
                            p.DrawTriangleSectorBoundaries(verts[PlanetSectors[hi, zi]._CoordIndex], verts[PlanetSectors[nexthi, zi]._CoordIndex], top, lineWidth);
                        }
                        else
                        {
                            Vector3 bottom = meshVertices[meshVertices.Length - 1];
                            Debug.Log("The bottom vertex is " + bottom);
                            int nexthi = hi + 1;
                            if (hi == (maxCoord - 1))
                                nexthi = 1;
                            p.DrawTriangleSectorBoundaries(verts[PlanetSectors[hi, zi]._CoordIndex], verts[PlanetSectors[nexthi, zi]._CoordIndex], bottom, lineWidth);

                        }

                        GameObject.Find("sector_tooltip").transform.position = new Vector3(this.gameObject.transform.position.x + verts[PlanetSectors[hi, zi]._CoordIndex].x, this.gameObject.transform.position.y + verts[PlanetSectors[hi, zi]._CoordIndex].y, this.gameObject.transform.position.z + verts[PlanetSectors[hi, zi]._CoordIndex].z);

                    }
                }

            }

        } else if (frameCount == 59)
        {
            frameCount = 0;
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
