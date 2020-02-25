using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeCreator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        const int nodeDepth = -10;
        int planetNum = Random.Range(4, 6);
        int[] res = new int[2] { 1920, 1080 };
        Vector3[] nodeCoords = new Vector3[planetNum];

        GameObject node = GameObject.Find("node");
        Vector3 v = new Vector3( 100, 0, nodeDepth );

        float oldX, oldY;
        int addHun = 2;
        int addFif = 2;
        float originDist;

        for (int i = 0; i < planetNum; i++) {
            Instantiate(node, v, Quaternion.identity);
            nodeCoords[i] = v;

            oldX = v.x;
            oldY = v.y;
            originDist = i + 2;

            // Adds an extra 100px or 50px between planets to add some irregularity
            if (Random.Range(0, 2) == 1 && addHun > 0)
            {
                addHun--;
                originDist++;
            }

            if (Random.Range(0, 2) == 1 && addFif > 0)
            {
                addFif--;
                originDist += 0.5f;
            }

            // Choose whether to make initial x and y coordinates +,- or 0
            int xSign, ySign = 0;

            // X is 0
            xSign = Random.Range(-1, 2);
            if (xSign == 0)
                // Y must be + or -
                while (ySign == 0)
                {
                    ySign = Random.Range(-1, 2);
                }
            else
                // Y can be -, + or 0
                ySign = Random.Range(-1, 2);

            // This should move nodes that are in-line on the same axis away from
            // each other if they're too close
            foreach (Vector3 v3 in nodeCoords) {
                int sign = 0;
                while (sign == 0)
                    sign = Random.Range(-1, 1);

                if ((v3.x == xSign) && (Mathf.Abs(v3.y - originDist * ySign) <= 100f))
                    xSign = sign;
                else if (v3.x == ySign && (Mathf.Abs(v3.y - originDist * ySign) <= 100f))
                    ySign = sign;

            }

            v = new Vector3((originDist * 100 * xSign) / 125, (originDist * 100 * ySign) / 125, nodeDepth);
            Debug.Log("Node placed at: " + "x: " + (originDist * 100 * xSign) + ", y: " + originDist * 100 * ySign);


            /*nodeCoords[i] = v;

            // Find allowable "signs" for the relative x and y positions that will not go
            // out of bounds of the game space
            float newX, newY;
            int[] signX, signY;
            if (((v.x - 100) < (-res[0] / 2 + 20)) || ((v.x + 100) > (res[0] / 2 - 20)))
            {
                if ((v.x - 100) < (-res[0] / 2 + 20))
                    signX = new int[2] { 0, 1 };
                else
                    signX = new int[2] { -1, 0 };
            }
            else
                signX = new int[2] { -1, 1 };

            if (((v.y - 100) < (-res[1] / 2 + 20)) || ((v.y + 100) > (res[1] / 2 - 20)))
            {
                if ((v.y - 100) < (-res[1] / 2 + 20))
                    signY = new int[2] { 0, 1 };
                else
                    signY = new int[2] { -1, 0 };
            }
            else
                signY = new int[2] { -1, 1 };

            // I have n coordinates x, y and I want to find a position on the game space where a
            // new point will be at least 100px away from all other points
            for(int j = 0; j < nodeCoords.Length; j++)
            {

            }


            oldX = v.x;
            newY = Mathf.Sqrt(Mathf.Pow(100, 2) - (Mathf.Sqrt(Mathf.Abs(newX - oldX))));
            */



        }
        Debug.Log(addHun + " " + addFif);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
