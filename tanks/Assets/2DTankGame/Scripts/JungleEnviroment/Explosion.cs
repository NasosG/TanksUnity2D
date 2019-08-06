using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject deathParticleEffect;  //The particle effect prefab that plays when the tank dies.
    public float closeAreaEffect = 2.5f;
    public float mediumAreaEffect = 3.5f;
    public float farAreaEffect = 5.0f;

    public int closeDamage = 3;
    public int mediumDamage = 2;
    public int farDamage = 1;
    private Transform explosive;

    void Awake()
    {
        explosive = transform;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //Debug.Log(this.tag);
        if (col.gameObject.tag == "ProjectileRocket" || col.gameObject.tag == "Projectile") {
            GameObject deathEffect = Instantiate(deathParticleEffect, transform.position, Quaternion.identity) as GameObject;   //Spawn the death particle effect at the tank's position.
            Destroy(deathEffect, 1.5f);                     //Destroy that effect in 1.5 seconds.

            FindObjectOfType<AudioManager>().Play("m_Barrel");
            Destroy(gameObject, 0.2f);

            //damage players that are close to the item exploding if there are any 
            damageNearbyPlayers();
        }


        
    }
    void damageNearbyPlayers()
    {
        //get items overlapping in the collision circle
        Collider2D[] colls = Physics2D.OverlapCircleAll(explosive.position, farAreaEffect);
        
        //for each item in collision
        foreach (Collider2D col in colls) {
            //if it was a tank
            if (col.CompareTag("Tank") && MenuUI.getFlag() != 1) {
                Tank tank = col.gameObject.GetComponent<Tank>();    //Get the tank's Tank.cs component.
                float distance = Vector3.Distance(col.transform.position, explosive.position);
                //Debug.Log("tank in explosion distance:" + distance);
                int damage = farDamage;
                if (distance <= closeAreaEffect) { //if tank is in the close to the centre Area
                    damage = closeDamage;
                    //Debug.Log("close");
                }
                else if (distance <= mediumAreaEffect) { //if tank is in the middle Area
                    damage = mediumDamage;
                    //Debug.Log("medium");
                }

                //aplly damage to the tank
                tank.Damage(damage);
            }

            //if it was a tank in multiplayer mode
            else if (col.CompareTag("Tank")) {
                TankMultiplayer tank = col.gameObject.GetComponent<TankMultiplayer>();    //Get the tank's Tank.cs component.
                float distance = Vector3.Distance(col.transform.position, explosive.position);
                //Debug.Log("tank in explosion distance:" + distance);
                int damage = farDamage;
                if (distance <= closeAreaEffect) { //if tank is in the close to the centre Area
                    damage = closeDamage;
                    //Debug.Log("close");
                }
                else if (distance <= mediumAreaEffect) { //if tank is in the middle Area
                    damage = mediumDamage;
                    //Debug.Log("medium");
                }

                //aplly damage to the tank
                tank.Damage(damage);
            }

        }
    }
}

