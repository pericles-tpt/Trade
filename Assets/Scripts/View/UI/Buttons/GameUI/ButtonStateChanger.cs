using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View.UI.Buttons.GameUI
{
    public class ButtonStateChanger : MonoBehaviour
    {
        public void ToggleAllLines()
        {
            GameObject.Find("Camera").GetComponent<GalaxyManager>().TradeLinesOn = !GameObject.Find("Camera").GetComponent<GalaxyManager>().TradeLinesOn;
            Debug.Log("Trade lines on state is " + GameObject.Find("Camera").GetComponent<GalaxyManager>().TradeLinesOn);
        }
    }
}