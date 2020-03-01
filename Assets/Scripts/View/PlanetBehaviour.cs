using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseOver()
    {
        GameObject.Find("sun").GetComponent<NodeCreator>().DrawTradeLines(this.gameObject);
    }

    private void OnMouseExit()
    {

        GameObject.Find("sun").GetComponent<NodeCreator>().DestroyAllTradeLines();
    }
}
