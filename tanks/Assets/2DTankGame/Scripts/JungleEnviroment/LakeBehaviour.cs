using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LakeBehaviour : MonoBehaviour
{
    public bool inLake = false; //bool to know if the tank is in the lake
    public float startingSpeed;
    Vector2 currentPosition;
    public Transform target;
    Tank tank;

    // when the GameObjects collider arrange for this GameObject to travel to the left of the screen
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Tank") {                     //Is the object passing the lake a tank?
            tank = col.gameObject.GetComponent<Tank>();         //Get the tank's Tank.cs component.
            startingSpeed = tank.moveSpeed;
            tank.moveSpeed -= 100;
            inLake = true;
            tank.canShoot = false;
        }
        //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        // if a tank is in lake and the object leaving the trigger is a tank too
        // there are still some special cases not covered such as one out of two tanks get killed in the lake
        // but this functionality covers most cases
        if (inLake && col.gameObject.tag == "Tank") {
            tank.moveSpeed = startingSpeed;
            inLake = false;
            tank.canShoot = true;
        }
        //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
    }
}
