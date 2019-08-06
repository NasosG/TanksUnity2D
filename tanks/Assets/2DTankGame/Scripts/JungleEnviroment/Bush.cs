using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    Animator animator;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Tank") {                     //Is the object passing the lake a tank?
            //Tank tank = col.gameObject.GetComponent<Tank>();    //Get the tank's Tank.cs component.
            //animator.Play("treeAnimation");
            this.GetComponent<Animator>().Play("treeAnimation");
        }
        //Debug.Log(col.gameObject.name + " : " + gameObject.name + " : " + Time.time);
    }
}
