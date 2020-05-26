using UnityEngine;
using System.Collections;
using UnityEditor;

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

    public Mesh SmallMesh;
    public Mesh MediumMesh;
    public Mesh LargeMesh;

    void Awake()
    {

        // Make small, medium and large planets
        float scale = 1;
        int divisions = (int)(Planet._SectorSize * scale);


        // Below will generate new meshes with n divisions, small = 1n, medium = 1.5n, large = 2n
        //SaveNewPlanetMesh(n);

        storedSectorsSmall = new Sector[divisions, divisions];
        SMG.CalculateVertices(scale, divisions, ref storedSectorsSmall);

        scale = 1.5f;
        divisions = (int)(Planet._SectorSize * scale);

        storedSectorsMedium = new Sector[divisions, divisions];
        SMG.CalculateVertices(scale, divisions, ref storedSectorsMedium);

        scale = 2f;
        divisions = (int)(Planet._SectorSize * scale);

        storedSectorsLarge = new Sector[divisions, divisions];
        SMG.CalculateVertices(scale, divisions, ref storedSectorsLarge);
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

    private void SaveNewPlanetMesh(int divisions)
    {
        if (!((1.5f * (float)divisions) % 1 == 0))
            throw new System.Exception("Provided number of 'division' in the sphere mesh must be an integer when multiplied by 1.5");

        float smallScale = 1f;
        float mediumScale = 1.5f;
        float largeScale = 2f;

        Mesh small  = SMG.GenerateMesh(smallScale, divisions);
        Mesh medium = SMG.GenerateMesh(mediumScale, divisions);
        Mesh large  = SMG.GenerateMesh(largeScale, divisions);

        // TODO: Come back and get uncommented section below to work
        //MeshSaverEditor.SaveMesh(small, "SmallPlanet" + (int)((float)divisions * smallScale), false, false);
        //MeshSaverEditor.SaveMesh(medium, "MediumPlanet" + (int)((float)divisions * mediumScale), false, false);
        //MeshSaverEditor.SaveMesh(large, "LargePlanet" + (int)((float)divisions * largeScale), false, false);

    }

}
 