using UnityEngine;
using System.Collections;

public class PlanetFactory : MonoBehaviour
{
    // Sprites needed to apply to planets - REMOVED
    public Sprite Temperate;
    public Sprite Molten;
    public Sprite Water;

    public GameObject TemperatePlanet;
    public GameObject MoltenPlanet;
    public GameObject WaterPlanet;

    private SphereMeshGenerator SMG = new SphereMeshGenerator();
    public Sector[,] storedSectorsSmall;    
    public Sector[,] storedSectorsMedium;    
    public Sector[,] storedSectorsLarge;
    private Mesh SmallMesh;
    private Mesh MediumMesh;
    private Mesh LargeMesh;

    void Awake()
    {
         SmallMesh = SMG.GenerateMesh(1, Planet._SectorSize * 1, ref storedSectorsSmall);
         MediumMesh = SMG.GenerateMesh(1.5f, (int)(Planet._SectorSize * 1.5f), ref storedSectorsMedium);
         LargeMesh = SMG.GenerateMesh(2, Planet._SectorSize * 2, ref storedSectorsLarge);
    }

    public Planet CreateMoltenPlanet(Vector3 v, string planetName, int index, float ssize, int pno)
    {
        // 1. Make GameObject at position
        GameObject go = Instantiate(MoltenPlanet, v, Quaternion.identity);
        go.transform.localScale = new Vector3(ssize, ssize, ssize);
        go.GetComponent<SphereCollider>().radius = go.GetComponent<SphereCollider>().radius * ssize;

        // 2. Assign sprite to GameObject
        Sprite selected = Molten;

        // 3. Choose PlanetType for Planet instance depending on distance to sun
        Planet.PlanetType pt;
        pt = Planet.PlanetType.molten;

        // 4. Create new planet instance and add it to the "planets" array        
        Planet p = new Planet(planetName, index, GetSectorsForSize(ssize), selected, pt, go, ssize, pno);

        // 5. Assign mesh to the planet's gameobject
        Mesh m = GetMeshForSize(ssize);

        Debug.Log("MESH HAS " + m.vertices + " VERTICES WHICH SHOULD BE > 0");

        go.GetComponent<MeshFilter>().mesh = m;
        go.GetComponent<MeshCollider>().sharedMesh = m;

        return p;

    }

    public Planet CreateTemperatePlanet(Vector3 v, string planetName, int index, float ssize, int pno)
    {
        // 1. Make GameObject at position
        GameObject go = Instantiate(TemperatePlanet, v, Quaternion.identity);
        go.transform.localScale = new Vector3(ssize, ssize, ssize);
        go.GetComponent<SphereCollider>().radius = go.GetComponent<SphereCollider>().radius * ssize;

        // 2. Assign sprite to GameObject
        Sprite selected = Temperate;

        // 3. Choose PlanetType for Planet instance depending on distance to sun
        Planet.PlanetType pt;
        pt = Planet.PlanetType.temperate;

        // 4. Create new planet instance and add it to the "planets" array        
        Planet p = new Planet(planetName, index, GetSectorsForSize(ssize), selected, pt, go, ssize, pno);

        // 5. Assign mesh to the planet's gameobject
        Mesh m = GetMeshForSize(ssize);

        go.GetComponent<MeshFilter>().mesh = m;
        go.GetComponent<MeshCollider>().sharedMesh = m;

        return p;

    }

    public Planet CreateWaterPlanet(Vector3 v, string planetName, int index, float ssize, int pno)
    {
        // 1. Make GameObject at position
        GameObject go = Instantiate(WaterPlanet, v, Quaternion.identity);
        go.transform.localScale = new Vector3(ssize, ssize, ssize);
        go.GetComponent<SphereCollider>().radius = go.GetComponent<SphereCollider>().radius * ssize;

        // 2. Assign sprite to GameObject
        Sprite selected = Water;

        // 3. Choose PlanetType for Planet instance depending on distance to sun
        Planet.PlanetType pt;
        pt = Planet.PlanetType.water;

        // 4. Create new planet instance and add it to the "planets" array        
        Planet p = new Planet(planetName, index, GetSectorsForSize(ssize), selected, pt, go, ssize, pno);

        // 5. Assign mesh to the planet's gameobject
        Mesh m = GetMeshForSize(ssize);

        go.GetComponent<MeshFilter>().mesh = m;
        go.GetComponent<MeshCollider>().sharedMesh = m;

        return p;

    }

    public Sector[,] GetSectorsForSize(float size)
    {
        if (size == 1)
            return storedSectorsSmall;
        else if (size == 1.5)
            return storedSectorsMedium;
        else
            return storedSectorsLarge;
    }

    public Mesh GetMeshForSize(float size)
    {
        if (size == 1)
            return SmallMesh;
        else if (size == 1.5)
            return MediumMesh;
        else
            return LargeMesh;
    }
}
 