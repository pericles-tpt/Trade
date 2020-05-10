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

    public Planet CreateMoltenPlanet(Vector3 v, string planetName, int index, float ssize, int pno)
    {
        // 1. Make GameObject at position
        GameObject go = Instantiate(MoltenPlanet, v, Quaternion.identity);
        go.transform.localScale = new Vector3(ssize, ssize, ssize);

        // 2. Assign sprite to GameObject
        Sprite selected = Molten;

        // 3. Choose PlanetType for Planet instance depending on distance to sun
        Planet.PlanetType pt;
        pt = Planet.PlanetType.molten;


        // 4. Create new planet instance and add it to the "planets" array        
        Planet p = new Planet(planetName, index, selected, pt, go, ssize, pno);
        return p;

    }

    public Planet CreateTemperatePlanet(Vector3 v, string planetName, int index, float ssize, int pno)
    {
        // 1. Make GameObject at position
        GameObject go = Instantiate(TemperatePlanet, v, Quaternion.identity);
        go.transform.localScale = new Vector3(ssize, ssize, ssize);

        // 2. Assign sprite to GameObject
        Sprite selected = Temperate;

        // 3. Choose PlanetType for Planet instance depending on distance to sun
        Planet.PlanetType pt;
        pt = Planet.PlanetType.temperate;

        // 4. Create new planet instance and add it to the "planets" array        
        Planet p = new Planet(planetName, index, selected, pt, go, ssize, pno);
        return p;

    }

    public Planet CreateWaterPlanet(Vector3 v, string planetName, int index, float ssize, int pno)
    {
        // 1. Make GameObject at position
        GameObject go = Instantiate(WaterPlanet, v, Quaternion.identity);
        go.transform.localScale = new Vector3(ssize, ssize, ssize);

        // 2. Assign sprite to GameObject
        Sprite selected = Water;

        // 3. Choose PlanetType for Planet instance depending on distance to sun
        Planet.PlanetType pt;
        pt = Planet.PlanetType.water;

        // 4. Create new planet instance and add it to the "planets" array        
        Planet p = new Planet(planetName, index, selected, pt, go, ssize, pno);
        return p;

    }
}
