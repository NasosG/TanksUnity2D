using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.UI;
using Pathfinding;

public class Tank : MonoBehaviour 
{
    [Header("Stats")]
    public int id;				//The unique identifier for this player.
    public int health;				//The current health of the tank.
    public int maxHealth;			//The maximum health of this tank.
    public int damage;				//How much damage this tank can do when shooting a simple projectile.
    public int rocketDamage;			//How much damage this tank can do when shooting a rocket.
    public float moveSpeed;			//How fast the tank can move.
    public float turnSpeed;			//How fast the tank can turn.
    public float projectileSpeed;		//How fast the tank's projectiles can move.
    public float reloadSpeed;			//How many seconds it takes to reload the tank, so that it can shoot again.
    private float reloadTimer;			//A timer counting up and resets after shooting.
    public bool collision = false;          	//Boolean variable to detect collisions

    [HideInInspector]
    public Vector3 direction;			//The direction that the tank is facing. Used for movement direction.

    [Header("Bools")]
    public bool canMove;			//Can the tank move if it wants to?
    public bool canShoot;			//Can the tank shoot if it wants to?

    [Header("Components / Objects")]
    public Rigidbody2D rig;			//The tank's Rigidbody2D component. 
    public GameObject projectile;		//The projectile prefab of which the tank can shoot.
    public GameObject projectileRocket;     	//The projectile prefab of the rocket.
    public GameObject deathParticleEffect;	//The particle effect prefab that plays when the tank dies.
    public Transform muzzle;			//The muzzle of the tank. This is where the projectile will spawn.
    public Game game;                       	//The Game.cs script, located on the GameManager game object.
    private AudioSource m_Explosion3;       	// The audio source to play when the tank explodes.
    private AudioSource PEW_SOUND;          	// The audio source to play when the tank explodes.
    public Image fillPlayer1, fillPlayer2;  	// The fill image of tanks' healthbars
    public int numOfRockets = 4;            	// Number of rockets each player has

    void Start ()
    {
        direction = Vector3.up; //Sets the tank's direction up, as that is the default rotation of the sprite.
        m_Explosion3 = GetComponent<AudioSource>();
    }
    
    //Called by the Game.cs script when the game starts.
    public void SetStartValues ()
    {
	//Sets the tank's stats based on the Game.cs start values.
        health = game.tankStartHealth;
        maxHealth = game.tankStartHealth;
        damage = game.tankStartDamage;
        rocketDamage = damage + 2;
        moveSpeed = game.tankStartMoveSpeed;
        turnSpeed = game.tankStartTurnSpeed;
        projectileSpeed = game.tankStartProjectileSpeed;
        reloadSpeed = game.tankStartReloadSpeed; 
        //Debug.Log("colours" + OpenGame.GetColorPlayer1() + "   " + OpenGame.GetColorPlayer2());
        if (MenuUI.getFlag() != 3) {
            game.player1Tank.GetComponent<SpriteRenderer>().color = OpenGame.GetColorPlayer1();
            game.player2Tank.GetComponent<SpriteRenderer>().color = OpenGame.GetColorPlayer2();
        }
        else {
            game.player1Tank.GetComponent<SpriteRenderer>().color = game.player1Color;
            game.player2Tank.GetComponent<SpriteRenderer>().color = game.player2Color;
        }

        game.playerMultiplayer.GetComponent<SpriteRenderer>().color = game.playerMultiplayerColor;
    }

    void Update ()
    {
        reloadTimer += Time.deltaTime;
        if (fillPlayer1.color == Color.green && game.player1Tank.health < 4)
            fillPlayer1.color = Color.red;
        if (fillPlayer2.color == Color.green && game.player2Tank.health < 4)
            fillPlayer2.color = Color.red;
    }

   //Called by the Controls.cs script. When a player presses their movement keys, it calls this function
   //sending over a "y" value, set to either 1 or 0, depending if they are moving forward or backwards.
   public void Move (int y)
   {
       rig.velocity = direction * y * moveSpeed * Time.deltaTime;	
   }

   //Called by the Controls.cs script. When a player presses their turn keys, it calls this function
   //sending over an "x" value, set to either 1 or 0, depending if they are moving left or right.
   public void Turn (int x)
   {
       transform.Rotate(-Vector3.forward * x * turnSpeed * Time.deltaTime);
       direction = transform.rotation * Vector3.up;
   }

    //turn with a float "x" value
    public void Turn (float x)
    {
        transform.Rotate(-Vector3.forward * x * turnSpeed * Time.deltaTime);
        direction = transform.rotation * Vector3.up;
    }

    //Called by the Contols.cs script. When a player presses their shoot key, it calls this function, making the tank shoot.
    public void Shoot ()
    {
        if (reloadTimer >= reloadSpeed) {                                               //Is the reloadTimer more than or equals to the reloadSpeed? Have we waiting enough time to reload?
            FindObjectOfType<AudioManager>().Play("pew");                               //Play pew sound
            GameObject proj = Instantiate(projectile, muzzle.transform.position, Quaternion.identity) as GameObject;	//Spawns the projectile at the muzzle.
            Projectile projScript = proj.GetComponent<Projectile>();					//Gets the Projectile.cs component of the projectile object.
            projScript.tankId = id;									//Sets the projectile's tankId, so that it knows which tank it was shot by.
            projScript.damage = damage;									//Sets the projectile's damage.
            projScript.game = game;														

	    projScript.rig.velocity = direction * projectileSpeed * Time.deltaTime;		//Makes the projectile move in the same direction that the tank is facing.
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

    public void ShootLazer()
    {
        if (reloadTimer >= reloadSpeed)
        {                                                //Is the reloadTimer more than or equals to the reloadSpeed? Have we waiting enough time to reload?
            FindObjectOfType<AudioManager>().Play("plasma");                               //Play pew sound
            GameObject proj = Instantiate(projectile, muzzle.transform.position, Quaternion.identity) as GameObject;    //Spawns the projectile at the muzzle.
            Projectile projScript = proj.GetComponent<Projectile>();                    //Gets the Projectile.cs component of the projectile object.
            projScript.tankId = id;                                                     //Sets the projectile's tankId, so that it knows which tank it was shot by.
            projScript.damage = rocketDamage;                                           //Sets the projectile's damage.
            projScript.game = game;

            projScript.rig.velocity = direction * projectileSpeed * Time.deltaTime;     //Makes the projectile move in the same direction that the tank is facing.
            reloadTimer = 0.0f;                                                         //Sets the reloadTimer to 0, so that we can't shoot straight away.
            proj.transform.Rotate(0.0f, 0.0f, Mathf.Atan2(projScript.rig.velocity.y, projScript.rig.velocity.x) * Mathf.Rad2Deg);
        }
    }

    //Called when the tank gets hit by a projectile. It sends over a "dmg" value, which is how much health the tank will lose. 
    public void Damage (int dmg)
    {
        if (game.oneHitKill) {		//Is the game set to one hit kill?
            Die();			//If so instantly kill the tank.
            return;
        }

        if(health - dmg <= 0) {		//If the tank's health will go under 0 when it gets damaged.
            Die();			//Kill the tank since its health will be under 0.
        }
        else {				//Otherwise...
            health -= dmg;		//Subtract the dmg from the tank's health.
       }
    }

    //Called when the tank's health is or under 0.
    public void Die ()
    {
        if (id == 0) {               //If the tank is player 1.
            if (MenuUI.getFlag() == 2) {
                game.lives--;
            }
            else game.player2Score++;	//Add 1 to player 2's score.
        }
        if (id == 1) {				//If the tank is player 2.
            if (MenuUI.getFlag() != 2) {
                game.player1Score++;	//Add 1 to player 1's score.
            }  
        }

        canMove = false;			//The tank can now not move.
        canShoot = false;			//The tank can now not shoot.

        //Particle Effect
        GameObject deathEffect = Instantiate(deathParticleEffect, transform.position, Quaternion.identity) as GameObject;	//Spawn the death particle effect at the tank's position.
        Destroy(deathEffect, 1.5f);                     //Destroy that effect in 1.5 seconds.

        //m_Explosion3.Play();
        //teleport tank to the spawn point when it dies and clear the old path
        if (MenuUI.getFlag() == 4 && id == 1)
        {
            GetComponent<AILerp>().Teleport(game.spawnPoints[Random.Range(0, game.spawnPoints.Count)].transform.position, true);
            Respawn();
        }
        else
            transform.position = new Vector3(0, 100, 0);    //Set the tanks position outside of the map, so that it is not visible when dead.

        if (MenuUI.getFlag() != 2 || (MenuUI.getFlag() == 2 && id != 0))
            StartCoroutine(RespawnTimer());                 //Start the RespawnTimer coroutine.
        else if (game.lives == 0) {
            RespawnAtHome();
        }
        else if (MenuUI.getFlag() == 2 && id == 0) {
            game.ui.SetKillScreen();
            RespawnAtHome();                                  //just respawn
        }

    }

    //Called when the tank has been dead and is ready to rejoin the game.
    public void Respawn ()
    {
        canMove = true;
        canShoot = true;

        health = maxHealth;

        if (!(MenuUI.getFlag() == 4 && id == 1))
            transform.position = game.spawnPoints[Random.Range(0, game.spawnPoints.Count)].transform.position;  //Sets the tank's position to a random spawn point.  
    }

    //Called when the tank dies, and needs to wait a certain time before respawning.
    IEnumerator RespawnTimer ()
    {
        yield return new WaitForSeconds(game.respawnDelay);	//Waits how ever long was set in the Game.cs script.
        Respawn();						//Respawns the tank
    }

    public void RespawnAtHome()
    {
        canMove = true;
        canShoot = true;

        health = maxHealth;

        transform.position = new Vector3(-48, -22, 0);  //Sets the tank's position to a spawn point next to home
    }

}
