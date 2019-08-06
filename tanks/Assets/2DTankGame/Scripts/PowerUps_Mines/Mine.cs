using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [Header("Stats")]
    public int tankId;                      //The tank which shot this projectile.
    public int damage;                      //How much damage this projectile will deal on impact.

    [Header("Components / Objects")]
    public GameObject hitParticleEffect;    //The particle effect prefab that will spawn when the projectile hits something.
    public Rigidbody2D rig;                 //The Rigidbody2D component of the projectile.
    public Game game;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Tank" && MenuUI.getFlag() != 1) {   //Is the object we hit a tank?

            Tank tank = col.gameObject.GetComponent<Tank>();	//Get the tank's Tank.cs component.

            //play powerup sound effect
            if(tank.health > 1)
                FindObjectOfType<AudioManager>().Play("mine");
            tank.Damage(damage);
        }

        else if (col.gameObject.tag == "Tank") {  //Is the object we hit a tank in multiplayer?

            TankMultiplayer tank = col.gameObject.GetComponent<TankMultiplayer>();  //Get the tank's Tank.cs component.
            //in multiplayer tanks have 1 hp (in current version) so hitting a mine kills them
            tank.Die();
        }

        //play the hit effect every time the ball collides with objects
        GameObject hitEffect1 = Instantiate(hitParticleEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(hitEffect1, 1.0f);
        Destroy(gameObject);
    }
}
