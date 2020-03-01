using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        GameObject.Find("sun").GetComponent<NodeCreator>().IncrementOrbits();
        GameObject.Find("player").GetComponent<script>().UpdateShipPositions();
    }
}
