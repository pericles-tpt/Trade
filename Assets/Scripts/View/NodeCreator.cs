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

                // ToDo: This statement doesn't do what it's supposed to (i.e. separate nodes too diagonally close to one another)
                if ((Mathf.Abs(v3.x - (xSign * 100 * originDist)) <= 100f) && (Mathf.Abs(v3.y - (ySign * 100 * originDist)) <= 100f))
                    xSign *= -1;
                // ToDo: This statement won't solve issue if flipping the XSign will violate one of these other conditions, for a prior node
                else if ((v3.x == (xSign * originDist)) && (v3.y == (ySign * originDist)))
                    xSign *= -1;
                else if ((v3.x == xSign) && (Mathf.Abs(v3.y - originDist * ySign) <= 100f))
                    xSign = sign;
                else if (v3.x == ySign && (Mathf.Abs(v3.y - originDist * ySign) <= 100f))
                    ySign = sign;

            }

            v = new Vector3((originDist * 100 * xSign) / 125, (originDist * 100 * ySign) / 125, nodeDepth);
            Debug.Log("Node placed at: " + "x: " + (originDist * 100 * xSign) + ", y: " + originDist * 100 * ySign);

        }
        Debug.Log(addHun + " " + addFif);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
