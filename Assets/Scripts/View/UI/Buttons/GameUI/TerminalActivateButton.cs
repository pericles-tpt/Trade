using UnityEngine;
using System.Collections;
using UnityEngine.PlayerLoop;

public class TerminalActivateButton : MonoBehaviour
{
    public void ToggleTerminal()
    {
        Transform terminalTransform = GameObject.Find("UI Canvas").transform.Find("Terminal").transform;
        Vector2 oldPos = terminalTransform.position;
        Vector2 newPos;
        if (terminalTransform.position.y < -17)
        {
            newPos = new Vector2(terminalTransform.position.x, -10f);
            terminalTransform.position = newPos;
        }
        else
        {
            newPos = new Vector2(terminalTransform.position.x, -19.4f);
            terminalTransform.position = newPos;
        }

    }

}