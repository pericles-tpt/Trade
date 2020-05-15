using UnityEngine;

public class GalaxyFactory
{
    public Galaxy CreateGalaxy()
    {
        int planetNum = 4;//UnityEngine.Random.Range(4, 5);
        Galaxy g = new Galaxy(planetNum);
        return g;

    }

}
