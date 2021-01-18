using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBarsBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnMouseEnter()
    {
        this.transform.parent.FindChild("TimeLegend").GetComponent<CanvasGroup>().alpha = 1;
    }

    private void OnMouseExit()
    {
        this.transform.parent.FindChild("TimeLegend").GetComponent<CanvasGroup>().alpha = 0;
    }
}
