using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public bool inGate = false;
    Tank tank = null;
    public Transform[] Destinations;
    public Transform Destination;
    public int id;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Tank") {                                     //Is the object passing the black hole a tank?

            int randomPoint = Random.Range(0, Destinations.Length);             //Choose random point among the ones we have set
            //Debug.Log("lengthh" + Destinations.Length + "point" + randomPoint);
            
            //Update col position and rotation
            col.transform.position = Destinations[randomPoint].transform.position;
            col.transform.rotation = Destinations[randomPoint].transform.rotation;
        }
        //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
    }

}
