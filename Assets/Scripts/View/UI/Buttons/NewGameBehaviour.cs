using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGameBehaviour : MonoBehaviour
{
    public void StartNewGame()
    {
        //Debug.Log("In new");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
