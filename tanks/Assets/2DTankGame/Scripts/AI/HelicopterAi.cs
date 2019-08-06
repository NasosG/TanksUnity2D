using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelicopterAi : MonoBehaviour
{
    [Header("Stats")]
    public int id;                          //The unique identifier for this player.
    public int maxHealth;                   //The maximum health of this tank.
    public int damage;						//How much damage this tank can do when shooting a simple projectile.
    public int rocketDamage;                //How much damage this tank can do when shooting a rocket.
    public float moveSpeed;                 //How fast the tank can move.
    public float turnSpeed;                 //How fast the tank can turn.
    public float projectileSpeed;           //How fast the tank's projectiles can move.
    public float reloadSpeed;               //How many seconds it takes to reload the tank, so that it can shoot again.
    private float reloadTimer;				//A timer counting up and resets after shooting.

    [HideInInspector]
    public Vector3 direction;               //The direction that the tank is facing. Used for movement direction.

    [Header("Bools")]
    public bool canMove;                    //Can the tank move if it wants to?
    public bool canShoot;                   //Can the tank shoot if it wants to?

    [Header("Components / Objects")]
    public Rigidbody2D rig;                 //The tank's Rigidbody2D component. 
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
    public AudioSource birdSound;

    public int cnt = 0;                     //after how many waupoints we want our helicopter to leave the stage
    public bool helicopterLeft = false;     //boolean that shows if helicopter has left

    public Transform[] moveSpots;
    public Transform[] escapeSpots;

    private int randomSpot;
    private int randomEscapeSpot;

    public Text helicopterText;

    public Transform spot;

    public float timeLeft = 40.0f;

    void Start()
    {
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);
        randomEscapeSpot = Random.Range(0, escapeSpots.Length);
        direction = Vector3.up; //Sets the tank's direction up, as that is the default rotation of the sprite.
        birdSound = GetComponent<AudioSource>();
        //startTransform = GameObject.FindGameObjectWithTag("helicopter").transform.position;
        //Debug.Log(startTransform);
    }

    //Called by the Game.cs script when the game starts.
    public void SetStartValues()
    {
        //Sets the tank's stats based on the Game.cs start values.
        rocketDamage = damage + 2;
    }

    void Update()
    {
        if (MenuUI.getFlag() != 1 && Time.timeScale != 0f)
        {
            //Debug.Log(this.transform.position);
            helicopterText.transform.position = transform.position + new Vector3(0, 2.5f, 0);
            reloadTimer += Time.deltaTime;

            if (helicopterLeft)
            {
                timeLeft -= Time.deltaTime;
            }
            if (timeLeft <= 0)
            {
                helicopterLeft = false;
            }
            //Called when the heli leaves
            if (cnt > 1)
            {
                spot = escapeSpots[randomEscapeSpot];
                helicopterLeft = true;
                cnt = 0;
                timeLeft = 40f;
            }
            else if (!helicopterLeft)
            {
                birdSound.Play();
                spot = moveSpots[randomSpot];
                if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
                {
                    if (waitTime <= 0)
                    {
                        cnt++;
                        randomSpot = Random.Range(0, moveSpots.Length);
                        waitTime = startWaitTime;
                    }
                    else
                    {
                        waitTime -= Time.deltaTime;
                    }
                }

                // shoot
                game.helicopter.Shoot();
            }
            transform.position = Vector2.MoveTowards(transform.position, spot.position, moveSpeed * Time.deltaTime);

            Vector3 dir = spot.position - transform.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 270;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        //disable helicopter text in multiplayer
        else helicopterText.enabled = false;
    }


    //Called by the Contols.cs script. When a player presses their shoot key, it calls this function, making the tank shoot.
    public void Shoot()
    {
        if (reloadTimer >= reloadSpeed)
        {                                                //Is the reloadTimer more than or equals to the reloadSpeed? Have we waiting enough time to reload?
            FindObjectOfType<AudioManager>().Play("pew");                               //Play pew sound
            GameObject proj = Instantiate(projectile, transform.position /*+ new Vector3(2f, 0, 0)*/, Quaternion.identity) as GameObject;    //Spawns the projectile at the muzzle.
            Projectile projScript = proj.GetComponent<Projectile>();                    //Gets the Projectile.cs component of the projectile object.
            projScript.tankId = id;                                                     //Sets the projectile's tankId, so that it knows which tank it was shot by.
            projScript.damage = damage;                                                 //Sets the projectile's damage.
            projScript.game = game;

            projScript.rig.velocity = (moveSpots[randomSpot].position - transform.position) * projectileSpeed * Time.deltaTime;     //Makes the projectile move in the same direction that the helicopter is facing(towards the right waypoint).
            reloadTimer = 0.0f;                                                         //Sets the reloadTimer to 0, so that we can't shoot straight away.
        }
    }

    public void ShootRocket()
    {
        if (reloadTimer >= reloadSpeed && numOfRockets > 0)
        {                                                //Is the reloadTimer more than or equals to the reloadSpeed? Have we waiting enough time to reload?
            FindObjectOfType<AudioManager>().Play("pew");                               //Play pew sound
            GameObject proj = Instantiate(projectileRocket, muzzle.transform.position, Quaternion.identity) as GameObject;    //Spawns the projectile at the muzzle.
            Projectile projScript = proj.GetComponent<Projectile>();                    //Gets the Projectile.cs component of the projectile object.
            projScript.tankId = id;                                                     //Sets the projectile's tankId, so that it knows which tank it was shot by.
            projScript.damage = rocketDamage;                                           //Sets the projectile's damage.
            projScript.game = game;

            projScript.rig.velocity = direction * projectileSpeed * Time.deltaTime;     //Makes the projectile move in the same direction that the tank is facing.
            reloadTimer = 0.0f;                                                         //Sets the reloadTimer to 0, so that we can't shoot straight away.
            //projScript.rig.transform.Rotate(direction);
            proj.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(projScript.rig.velocity.y, projScript.rig.velocity.x) * Mathf.Rad2Deg);
            //shot 1 out of 4 rockets;
            numOfRockets--;
        }
    }


}
