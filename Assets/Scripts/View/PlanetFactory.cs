﻿using UnityEngine;
using System.Collections;

public class PlanetFactory : MonoBehaviour
{
    // Sprites needed to apply to planets
    public Sprite Temperate;
    public Sprite Molten;
    public Sprite Water;

    public GameObject Pl;

    public Planet CreateMoltenPlanet(Vector3 v, string planetName, int index)
    {
        // 1. Make GameObject at position
        GameObject go = Instantiate(Pl, v, Quaternion.identity);

        // 2. Assign sprite to GameObject
        Sprite selected = Molten;
        go.GetComponent<SpriteRenderer>().sprite = Molten;

        // 3. Choose PlanetType for Planet instance depending on distance to sun
        Planet.PlanetType pt;
        pt = Planet.PlanetType.molten;

        // 4. Create new planet instance and add it to the "planets" array        
        Planet p = new Planet(planetName, index, selected, pt, go);
        return p;

    }

    public Planet CreateTemperatePlanet(Vector3 v, string planetName, int index)
    {
        // 1. Make GameObject at position
        GameObject go = Instantiate(Pl, v, Quaternion.identity);

        // 2. Assign sprite to GameObject
        Sprite selected = Temperate;
        go.GetComponent<SpriteRenderer>().sprite = Temperate;

        // 3. Choose PlanetType for Planet instance depending on distance to sun
        Planet.PlanetType pt;
        pt = Planet.PlanetType.temperate;

        // 4. Create new planet instance and add it to the "planets" array        
        Planet p = new Planet(planetName, index, selected, pt, go);
        return p;

    }

    public Planet CreateWaterPlanet(Vector3 v, string planetName, int index)
    {
        // 1. Make GameObject at position
        GameObject go = Instantiate(Pl, v, Quaternion.identity);

        // 2. Assign sprite to GameObject
        Sprite selected = Water;
        go.GetComponent<SpriteRenderer>().sprite = Water;

        // 3. Choose PlanetType for Planet instance depending on distance to sun
        Planet.PlanetType pt;
        pt = Planet.PlanetType.water;

        // 4. Create new planet instance and add it to the "planets" array        
        Planet p = new Planet(planetName, index, selected, pt, go);
        return p;

    }
}
