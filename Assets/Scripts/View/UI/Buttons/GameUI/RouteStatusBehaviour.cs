using System;
using UnityEngine;

namespace Assets.Scripts.View.UI.Buttons.GameUI
{
    public class RouteStatusBehaviour : MonoBehaviour
    {

        GalaxyManager gm;
        LineManager lm;
        Vector3 A, B;
        int a, b;

        void Awake()
        {
            gm = GameObject.Find("Camera").GetComponent<GalaxyManager>();
        }

        void OnMouseEnter()
        {

            string routeName = this.GetComponentInParent<Transform>().name;

            a = (int)Char.GetNumericValue(routeName[routeName.Length - 2]);
            b = (int)Char.GetNumericValue(routeName[routeName.Length - 1]);

            Debug.Log("a is " + a + " b is " + b);

            A = gm.GetPlanetByIndex(a)._GameObject.transform.position;
            B = gm.GetPlanetByIndex(b)._GameObject.transform.position;

            Debug.Log("A is " + A + "B is " + B);

            if (gm.TradeLinesOn == false && !gm.LMLineHitsSunCollider(A, B, a))
                gm.LMActivateLine(A, B, a);
        }

        void OnMouseExit()
        {
            if (gm.TradeLinesOn == false)
                gm.LMDeactivateLineStartingAt(A);

        }

    }
}