using UnityEngine;

public class PlanetFactory : MonoBehaviour
{
    public Sprite Temperate;
    public Sprite Molten;
    public Sprite Water;

    public GameObject TemperatePlanet;
    public GameObject MoltenPlanet;
    public GameObject WaterPlanet;

    public Planet CreatePlanet(Vector3 v, Planet.PlanetType pt, string planetName, int index, float scale, int pno)
    {
        // 1. Instantiate GameObject and Assign its Sprite
        GameObject go = new GameObject();
        Sprite selected = Molten;

        switch (pt) {
            case Planet.PlanetType.molten:
                go = Instantiate(MoltenPlanet, v, Quaternion.identity);
                break;
            case Planet.PlanetType.temperate:
                go = Instantiate(TemperatePlanet, v, Quaternion.identity);
                selected = Temperate;
                break;
            case Planet.PlanetType.water:
                go = Instantiate(WaterPlanet, v, Quaternion.identity);
                selected = Water;
                break;
        }

        // 2. Scale the gameobject accordingly
        go.transform.localScale = new Vector2(scale, scale);

        // 3. Create new planet instance to return    
        return new Planet(planetName, index, selected, pt, go, scale, pno);

    }

}
 