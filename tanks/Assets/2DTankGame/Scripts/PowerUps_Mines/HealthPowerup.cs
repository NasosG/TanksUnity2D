using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerup : MonoBehaviour
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
        //Is the object we hit a tank?
        if (col.gameObject.tag == "Tank" && MenuUI.getFlag() != 1) {
            
            Tank tank = col.gameObject.GetComponent<Tank>();	//Get the tank's Tank.cs component.

            //play powerup sound effect
            FindObjectOfType<AudioManager>().Play("powerUp");

            //health increased according to the health our tank already had
            damage = 3 - tank.health;
            //tank cannot take more HP
            if (tank.health > 3)
                damage = 0;
            //Call the damage function on that tank to damage it.
            tank.Damage(-damage);
            
        }

        //play the hit effect every time the ball collides with objects
        GameObject hitEffect1 = Instantiate(hitParticleEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(hitEffect1, 1.0f);
        if (!(col.gameObject.tag == "Projectile" || col.gameObject.tag == "ProjectileRocket"))
            Destroy(gameObject);
    }
}
