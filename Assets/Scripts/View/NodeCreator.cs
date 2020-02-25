using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCreator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        const int depth = -10;
        GameObject node = GameObject.Find("node");
        Debug.Log("This should print");
        int[] res = new int[2] { 1920, 1080 };
        Vector3 v = new Vector3( 0, 0, depth );
        int planetNum = Random.Range(4, 8);
        float oldX;
        for (int i = 0; i < planetNum; i++) {
            Instantiate(node, v, Quaternion.identity);

            // Figure out the co-ordinate of the next point in the space
            float newX, newY;
            if (v.x - 100 < (-res[0]/2 + 20))
                newX = Random.Range(0, 100);
            else if (v.x + 100 > (res[0]/2 - 20))
                newX = Random.Range(0, -100);
            else
                newX = Random.Range(-100, 100);

            int signY;
            if (v.y - 100 < (-res[1] / 2 + 20))
                signY = 1;
            else if (v.y + 100 > (res[1] / 2 - 20))
                signY = -1;
            else
            {
                // TODO: There's probably a more efficient way of doing this
                signY = Random.Range(-1, 1);
                while (signY == 0)
                {
                    signY = Random.Range(-1, 1);

                }
            }

            oldX = v.x;
            Debug.Log(newX - oldX);
            newY = Mathf.Sqrt(Mathf.Pow(100, 2) - (Mathf.Sqrt(Mathf.Abs(newX - oldX))));

            v = new Vector3(newX / 30, (newY * signY) / 30, depth);
            Debug.Log("Node placed at: " + "x: " + newX + ", y: " + newY);

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
