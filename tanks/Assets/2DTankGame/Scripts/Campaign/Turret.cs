using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public int damage;
    public bool inRange = false;

    //a reference to our target (players1 tank)
    public Transform target;

    [Header("Components / Objects")] 
    public GameObject projectile;			//The projectile prefab of which the tank can shoot.
    public GameObject projectileRocket;     //The projectile prefab of the rocket.
    public GameObject deathParticleEffect;  //The particle effect prefab that plays when the tank dies.
    public Transform muzzle;                //The muzzle of the tank. This is where the projectile will spawn.
    public Game game;                       //The Game.cs script, located on the GameManager game object.
    private AudioSource m_Explosion3;       // The audio source to play when the tank explodes.
    private AudioSource PEW_SOUND;          // The audio source to play when the tank explodes.
    public int numOfRockets = 4;            // Number of rockets each player has
    private float waitTime;
    public float startWaitTime;
    
    //projectile
    public float projectileSpeed;           //How fast the tank's projectiles can move.
    public float reloadSpeed;               //How many seconds it takes to reload the tank, so that it can shoot again.
    private float reloadTimer;              //A timer counting up and resets after shooting.

    public int id;                          //The unique identifier for this player.
    int delay;


    // Update is called once per frame
    void Update()
    {
        reloadTimer += Time.deltaTime;
        delay++;
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 270;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (inRange)
            Shoot();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tank" && collision.gameObject.GetComponent<Tank>().id == 0) {   //Is the object passing by our tank?
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tank" && collision.gameObject.GetComponent<Tank>().id == 0) {   //Is the object passing by our tank?
            inRange = false;
        }
    }

    //Called by the Contols.cs script. When a player presses their shoot key, it calls this function, making the tank shoot.
    public void Shoot()
    {
        if (reloadTimer >= reloadSpeed) {                                               //Is the reloadTimer more than or equals to the reloadSpeed? Have we waiting enough time to reload?
            //Debug.Log("turret shooooot");
            FindObjectOfType<AudioManager>().Play("pew");                               //Play pew sound
            GameObject proj = Instantiate(projectile, this.transform.position, Quaternion.identity) as GameObject;    //Spawns the projectile at the muzzle.
            Projectile projScript = proj.GetComponent<Projectile>();                    //Gets the Projectile.cs component of the projectile object.
            projScript.tankId = id;                                                     //Sets the projectile's tankId, so that it knows which tank it was shot by.
            projScript.damage = damage;                                                 //Sets the projectile's damage.
            projScript.game = game;

            projScript.rig.velocity = (target.position - transform.position) * projectileSpeed * Time.deltaTime;     //Makes the projectile move in the same direction that the tank is facing.
            reloadTimer = 0.0f;                                                         //Sets the reloadTimer to 0, so that we can't shoot straight away.
        }
    }
}
