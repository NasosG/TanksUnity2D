using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public bool inGate = false;
    Tank tank = null;


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Tank") {                     //Is the object passing the lake a tank?
            tank = col.gameObject.GetComponent<Tank>();         //Get the tank's Tank.cs component.
            col.gameObject.GetComponent<Renderer>().enabled = !col.gameObject.GetComponent<Renderer>().enabled;
            inGate = true;
        }
        //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (inGate)
        {
            col.gameObject.GetComponent<Renderer>().enabled = !col.gameObject.GetComponent<Renderer>().enabled;
        }
    }
}
