using UnityEngine;
using System.Collections;
using System;

public class Projectile : MonoBehaviour 
{
	[Header("Stats")]
	public int tankId;						//The tank which shot this projectile.
	public int damage;						//How much damage this projectile will deal on impact.

	[Header("Components / Objects")]
	public GameObject hitParticleEffect;	//The particle effect prefab that will spawn when the projectile hits something.
	public Rigidbody2D rig;					//The Rigidbody2D component of the projectile.
	public Game game;

	private int bounces;					//The amount of times the projectile has bounced off a wall.


	//Called when the projectile's CircleCollider2D component enters a another collider or trigger. 
	//The "col" parameter is the collider that it enters.
	void OnCollisionEnter2D (Collision2D col)
	{
        //Debug.Log(this.tag);
        //if it is a rocket we have bounce
        if (this.tag == "ProjectileRocket")
            ProjectileRocket(col);
        //else this is a simple projectile so we have no bounce
        else 
            ProjectileSimple(col);
    }



    void ProjectileRocket(Collision2D col)
    {
        float angle = Mathf.Atan2(rig.velocity.y, rig.velocity.x) * Mathf.Rad2Deg;
        // bullet bounces off the surface
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        bounces++;

        if (col.gameObject.tag == "Tank") {                     //Is the object we hit a tank?
            Tank tank = col.gameObject.GetComponent<Tank>();    //Get the tank's Tank.cs component.

            if (!game.canDamageOwnTank) {                       //Can we not damage our own tank?
                if (tank.id != tankId) {                        //Is the tank we hit not the one that shot this projectile?
                    tank.Damage(damage);                        //Call the damage function on that tank to damage it.
                }
            }
            else {
                tank.Damage(damage);
            }
        }

        if (bounces >= game.maxProjectileBounces || col.gameObject.tag == "Tank") {
            //Particle Effect
            GameObject hitEffect = Instantiate(hitParticleEffect, transform.position, Quaternion.identity) as GameObject;   //Spawn the hit particle effect at the position of impact.
            Destroy(hitEffect, 1.0f);   //Destroy that effect after 1 second.
            Destroy(gameObject);        //Destroy the projectile.
        }
    }



    void ProjectileSimple(Collision2D col)
    {
        if (col.gameObject.tag == "Tank" && MenuUI.getFlag() != 1)
        {                       //Is the object we hit a tank?

            Tank tank = col.gameObject.GetComponent<Tank>();	//Get the tank's Tank.cs component.

            if (!game.canDamageOwnTank)
            {                       //Can we not damage our own tank?
                if (tank.id != tankId) {                            //Is the tank we hit not the one that shot this projectile?
                    tank.Damage(damage);                            //Call the damage function on that tank to damage it.
                }
            }
            else
            {
                tank.Damage(damage);
            }
        }
        else if (col.gameObject.tag == "Tank")
        {//Is the object we hit a tank?
            Debug.Log("tankMultiplayer");
            TankMultiplayer tank = col.gameObject.GetComponent<TankMultiplayer>();

            /*if (!game.canDamageOwnTank)
            {                       //Can we not damage our own tank?
                if (tank.id != tankId)
                {                           //Is the tank we hit not the one that shot this projectile?
                    tank.Damage(damage);                        //Call the damage function on that tank to damage it.
                }
            }*/
            //else
            //{
                tank.Damage(damage);
            //}
        }

        GameObject hitEffect1 = Instantiate(hitParticleEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(hitEffect1, 1.0f);
        Destroy(gameObject);
    }
}
