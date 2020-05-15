using UnityEngine;
using System.Collections;

public class OLDCODE
{

    private void SeparatePlanets(Planet[] planets)
    {
        Vector3 vOld = new Vector3(0, 0, 0);

        foreach (Planet p in planets)
        {
            Vector3 vCurr = p._GameObject.transform.position;
            int sign = 0;
            while (sign == 0)
                sign = UnityEngine.Random.Range(-1, 1);

            if (vOld != null)
            {
                // ToDo: This statement doesn't do what it's supposed to (i.e. separate nodes too diagonally close to one another)
                if ((Mathf.Abs(vCurr.x - vOld.x) <= 100f) && (Mathf.Abs(vCurr.y - vOld.y) <= 100f))
                    vCurr.x *= -1;
                /* ToDo: This statement won't solve issue if flipping the XSign will violate one of these other conditions, for a prior node
                else if ((v3.x == (xSign * originDist)) && (v3.y == (ySign * originDist)))
                    xSign *= -1;
                else if ((v3.x == xSign) && (Mathf.Abs(v3.y - originDist * ySign) <= 100f))
                    xSign = sign;
                else if (v3.x == ySign && (Mathf.Abs(v3.y - originDist * ySign) <= 100f))
                    ySign = sign;
                */
            }

            vOld = vCurr;

        }
    }
}
