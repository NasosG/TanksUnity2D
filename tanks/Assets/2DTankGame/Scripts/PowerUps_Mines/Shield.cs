using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    [Header("Stats")]
    public int tankId;                      //The tank which shot this projectile.
    public int damage;                      //How much damage this projectile will deal on impact.

    [Header("Components / Objects")]
    public GameObject hitParticleEffect;    //The particle effect prefab that will spawn when the projectile hits something.
    public Rigidbody2D rig;                 //The Rigidbody2D component of the projectile.
    public Game game;
    public UI ui;
    public Image fillPlayer1, fillPlayer2;

    void OnCollisionEnter2D(Collision2D col)
    {
        //Is the object we hit a tank and we are in singleplayer or campaign?
        if (col.gameObject.tag == "Tank" && (MenuUI.getFlag() == 2 || MenuUI.getFlag() == 4)) {              

            Tank tank = col.gameObject.GetComponent<Tank>();	//Get the tank's Tank.cs component.

            //play powerup sound effect
            FindObjectOfType<AudioManager>().Play("powerUp");

            //health increased according to the health our tank already had
            damage = 3 - tank.health;
            if (tank.health == 3)
            {
                if (tank.GetComponent<SpriteRenderer>().color == game.player1Tank.GetComponent<SpriteRenderer>().color)
                    fillPlayer1.color = Color.green;
                else
                    fillPlayer2.color = Color.green;

                //if health is full get one extra bar
                damage++;
            }

            //tank cannot take more HP
            if (tank.health > 3)
                damage = 0;

            //Call the damage function on that tank to take health
            tank.Damage(-damage);
        }
        //if we hit a tank and we are in co-op
        if (col.gameObject.tag == "Tank" && MenuUI.getFlag() != 1) {              //Is the object we hit a tank?

            Tank tank = col.gameObject.GetComponent<Tank>();	//Get the tank's Tank.cs component.

            //play powerup sound effect
            FindObjectOfType<AudioManager>().Play("powerUp");

            //health increased according to the health our tank already had
            damage = 3 - tank.health;
            if (tank.health == 3) {
                if (tank.GetComponent<SpriteRenderer>().color == OpenGame.GetColorPlayer1())
                    fillPlayer1.color = Color.green;
                else
                    fillPlayer2.color = Color.green;
                
                //if health is full get one extra bar
                damage++;
            }

            //tank cannot take more HP
            if (tank.health > 3)
                damage = 0;

            //Call the damage function on that tank to take health
            tank.Damage(-damage);
        }

        //play the hit effect every time the ball collides with objects
        GameObject hitEffect1 = Instantiate(hitParticleEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(hitEffect1, 1.0f);
        if (!(col.gameObject.tag == "Projectile" || col.gameObject.tag == "ProjectileRocket"))
            Destroy(gameObject);
    }
}
