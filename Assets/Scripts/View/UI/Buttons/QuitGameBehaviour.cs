using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameBehaviour : MonoBehaviour
{
    public void Quit()
    {
        //Debug.Log("In quit");
        Application.Quit();
    }
}