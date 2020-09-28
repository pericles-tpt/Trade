using UnityEngine;
using System.Collections;

namespace Assets.Scripts.View.UI.Buttons.GameUI
{
    public class EndTurnBehaviour : MonoBehaviour
    {

        // Use this for initialization
        public void NextTurn()
        {
            GalaxyManager gm = GameObject.Find("Camera").GetComponent<GalaxyManager>();
            gm.GoToNextTurn();
        }
    }
}