using UnityEngine;

public class GalaxyFactory
{
    public Galaxy CreateGalaxy()
    {
        int planetNum = UnityEngine.Random.Range(4, 6);
        Galaxy g = new Galaxy(planetNum);
        return g;

    }

}
