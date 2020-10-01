using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View.UI.Buttons.GameUI
{
    public class ButtonStateChanger : MonoBehaviour
    {
        public void ToggleAllLines()
        {
            GalaxyManager gm = GameObject.Find("Camera").GetComponent<GalaxyManager>();
            gm.TradeLinesOn = !gm.TradeLinesOn;

            if (gm.TradeLinesOn == false)
                gm.LMDeactivateAllLines();
            //else
                //gm.DrawAllPlanetsToAll(gm.TradeLinesOn);  HAVE TO DECOUPLE INCREMENTING ORBITS FROM INCREMENTING TURNS

            Debug.Log("Trade lines on state is " + GameObject.Find("Camera").GetComponent<GalaxyManager>().TradeLinesOn);
        }
    }
}