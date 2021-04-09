using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.SceneManagement;
using MultiplayerTanks;
using UnityEngine.UI;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PunPlayerScores : MonoBehaviour
{
    public const string PlayerScoreProp = "score";
}

public class TankMultiplayer : MonoBehaviourPunCallbacks
{
    [Header("Player 1 Controls")]
    public KeyCode p1MoveForward;
    public KeyCode p1MoveBackwards;
    public KeyCode p1TurnLeft;
    public KeyCode p1TurnRight;
    public KeyCode p1Shoot;

    [Header("Stats")]
    public int id;						//The unique identifier for this player.
    public int health;						//The current health of the tank.
    public int maxHealth;					//The maximum health of this tank.
    public int damage;						//How much damage this tank can do when shooting a projectile.
    public float moveSpeed;					//How fast the tank can move.
    public float turnSpeed;					//How fast the tank can turn.
    public float projectileSpeed;                       	//How fast the tank's projectiles can move.
    public float reloadSpeed;					//How many seconds it takes to reload the tank, so that it can shoot again.
    private float reloadTimer;					//A timer counting up and resets after shooting.
    public bool collision = false;
    public PhotonView photonView;
    private Vector3 SelfPos;

    [HideInInspector]
    public Vector3 direction;					//The direction that the tank is facing. Used for movement direction.

    [Header("Bools")]
    public bool canMove;					//Can the tank move if it wants to?
    public bool canShoot;					//Can the tank shoot if it wants to?

    [Header("Components / Objects")]
    public Rigidbody2D rig;					//The tank's Rigidbody2D component. 
    public GameObject projectile;				//The projectile prefab of which the tank can shoot.
    public GameObject deathParticleEffect;			//The particle effect prefab that plays when the tank dies.
    public Transform muzzle;					//The muzzle of the tank. This is where the projectile will spawn.
    public Game game;                       			//The Game.cs script, located on the GameManager game object.
    public GameManager m_game;
    private AudioSource m_Explosion3;       			// The audio source to play when the tank explodes.
    private AudioSource PEW_SOUND;          			// The audio source to play when the tank explodes.
    public GameObject player;
    public GameObject scoreMultiplayer;			        //The text at the top of the screen which displays the score
    public int player1Score = 0;
    public int player2Score = 0;
    public int respawnDelay = 2;				//The amount of time a player will wait between dying and respawning.
    public UI ui;
    private bool gameEnd = false;

    void Start ()
    {
        //the tank is able to move and shoot in the beginning
        canMove = true;
        canShoot = true;

        //deactivate co-op objects
        GameObject scoreText = GameObject.Find("ScoreText");
        GameObject im1 = GameObject.Find("Image");
        GameObject im2 = GameObject.Find("Image (1)");
        GameObject rocketsPL1 = GameObject.Find("rocketsPL1");
        GameObject rocketsPL2 = GameObject.Find("rocketsPL2");
        //--------------------------------------------------//
        scoreText.SetActive(false);
        im1.SetActive(false);
        im2.SetActive(false);
        rocketsPL1.SetActive(false);
        rocketsPL2.SetActive(false);
        scoreMultiplayer = GameObject.Find("ScoreMultiplayer");
        scoreMultiplayer.SetActive(true);
        //--------------------------------------------------//

        //set score at 0 - 0
        scoreMultiplayer.GetComponent<Text>().text = "<b>SCORE\n<color=" + "#358BDB" + ">" + 0 + "</color> - <color=" + "#FF0000" + ">" + 0 + "</color></b>";

        //PhotonNetwork.SendRate = 50;
        //PhotonNetwork.SerializationRate = 40;

        //set second tank color to red and sync it
        if (PhotonNetwork.CountOfPlayers % 2 == 0) {
            photonView.RPC("UpdateColor", RpcTarget.AllBufferedViaServer);
        }

        //set the tank's direction up, as that is the default rotation of the sprite
        direction = Vector3.up;
        Turn(1);
        //explosion audio Source
        m_Explosion3 = GetComponent<AudioSource>();
    }

    [PunRPC]
    public void UpdateColor()
    {
        //make second tank red
        photonView.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
    }

    void Update ()
    {
        //if only one player is in game
        if (PhotonNetwork.PlayerList.Length < 2) {
            GameObject.Find("MultiplayerWin").GetComponent<Text>().text = "waiting for other players...";
            return;
        }

        scoreMultiplayer.GetComponent<Text>().text = "<b>SCORE\n<color=" + "#358BDB" + ">" + GetScore(PhotonNetwork.PlayerList[0]) + "</color> - <color=" + "#FF0000" + ">" + GetScore(PhotonNetwork.PlayerList[1]) + "</color></b>";
        //scoreMultiplayer.GetComponent<Text>().text = "<b>SCORE\n" + GetScore(PhotonNetwork.PlayerList[0]) + " - " + GetScore(PhotonNetwork.PlayerList[1]) + "</b>";

        if (GetScore(PhotonNetwork.PlayerList[0]) > 9) SetWinScreen(0); 
        if (GetScore(PhotonNetwork.PlayerList[1]) > 9) SetWinScreen(1);

        reloadTimer += Time.deltaTime;

        if (photonView.IsMine) {
            checkInput();
        }
        /*else {
            smoothNetMovement();
        }*/

    }
    
    public void SetWinScreen(int winner)
    {
        gameEnd = true;

        canMove = false;
        canShoot = false;

        GameObject winText = GameObject.Find("MultiplayerWin");
        
        //Player 1 wins
        if (winner == 0)
            winText.GetComponent<Text>().text = "<b>PLAYER 1</b>\nWins The Game. Press Esc To Exit";

        //Player 2 wins
        else
            winText.GetComponent<Text>().text = "<b>PLAYER 2</b>\nWins The Game. Press Esc To Exit";
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

 
    //the tank shoots the projectile.
    public void Shoot ()
    {
        if (reloadTimer >= reloadSpeed) {                                               //Reload time
            FindObjectOfType<AudioManager>().Play("pew");                               //Play pew sound
            GameObject proj = PhotonNetwork.Instantiate("ProjectileSimple", muzzle.transform.position, Quaternion.identity) as GameObject;	//Spawns the projectile at the muzzle
            Projectile projScript = proj.GetComponent<Projectile>();					//Gets the Projectile.cs component of the projectile object
            projScript.tankId = id;									//Sets the projectile's tankId, so that it knows which tank it was shot by
            projScript.damage = damage;									//Sets the projectile's damage
            projScript.game = game;														

	   projScript.rig.velocity = direction * projectileSpeed * Time.deltaTime;		//Makes the projectile move in the same direction that the tank is facing
	   reloadTimer = 0.0f;									//Sets the reloadTimer to 0, so that we can't shoot straight away
        }
    }

    //Called when the tank gets hit by a projectile. It sends over a "dmg" value, which is how much health the tank will lose
    public void Damage (int dmg)
    {
        if (health - dmg <= 0) {	//If the tank's health will go under 0 when it gets damaged.
            Die();			//Kill the tank since its health will be under 0.
        }
        else {					//Otherwise...
            health -= dmg;		//Subtract the dmg from the tank's health.
        }
    }

    //Called when the tank's health is or under 0.
    public void Die ()
    {
        Debug.Log("PhotonNetwork.IsMasterClient" + PhotonNetwork.IsMasterClient);
        if (PhotonNetwork.IsMasterClient && photonView.IsMine) {  //if the tank is player 1
            int current = GetScore(PhotonNetwork.PlayerList[1]) + 1;
            Hashtable score = new Hashtable();  // using PUN's implementation of Hashtable
            score[PunPlayerScores.PlayerScoreProp] = current;
            //set player 1's score with a custom property
            PhotonNetwork.PlayerList[1].SetCustomProperties(score);
        }
        else if(photonView.IsMine) {  //if the tank is player 2
            int current = GetScore(PhotonNetwork.PlayerList[0]) + 1;
            Hashtable score = new Hashtable();  // using PUN's implementation of Hashtable
            score[PunPlayerScores.PlayerScoreProp] = current;
            //set player 2's score with a custom property
            PhotonNetwork.PlayerList[0].SetCustomProperties(score); 
        }

        //Particle Effect
        GameObject deathEffect = Instantiate(deathParticleEffect, transform.position, Quaternion.identity) as GameObject;	//Spawn the death particle effect at the tank's position.
		Destroy(deathEffect, 1.5f);                     //Destroy that effect in 1.5 seconds.

        //m_Explosion3.Play();
        
        transform.position = new Vector3(0, 100, 0);	//Set the tanks position outside of the map, so that it is not visible when dead.
		StartCoroutine(RespawnTimer());					//Start the RespawnTimer coroutine.
    }

    //get player's score from player's custom properties
    public int GetScore(Player player)
    {
        if (player.CustomProperties.TryGetValue(PunPlayerScores.PlayerScoreProp, out object score))
        {
            return (int)score;
        }

        return 0;
    }

    //Called when the tank has been dead and is ready to rejoin the game.
    public void Respawn ()
    {
        health = maxHealth;
        transform.position = new Vector3(9,0,0);	//Sets the tank's position to a random spawn point.
    }

    //Called when the tank dies, and needs to wait a certain time before respawning.
    IEnumerator RespawnTimer ()
    {
        yield return new WaitForSeconds(respawnDelay);		//Waits how ever long was set
        Respawn();						//Respawns the tank.
    }
    
    private void checkInput()
    {
        player1Move();
    }

    private void smoothNetMovement()
    {
        transform.position = Vector3.Lerp(transform.position, SelfPos, Time.deltaTime * 8);
    }

    void player1Move()
    {
        //Quit game if user presses escape and redirect him to the staring menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(DisconnectToMenu());
        }

        rig.velocity = Vector2.zero;

        if (canMove)
        {
            if (Input.GetKey(p1MoveForward))
            {
                Move(1);
            }
            if (Input.GetKey(p1MoveBackwards))
            {
                Move(-1);
            }
            if (Input.GetKey(p1TurnLeft))
            {
                Turn(-1);
            }
            if (Input.GetKey(p1TurnRight))
            {
                Turn(1);
            }
        }

        if (canShoot)
        {
            if (Input.GetKeyDown(p1Shoot))
            {
                Shoot();
            }
        }
    }

    private IEnumerator DisconnectToMenu()
    {
        //leave Photon's room
        PhotonNetwork.LeaveRoom();
        //check if we are still in the room
        while (PhotonNetwork.InRoom)
            yield return null;
        // then load the menu level
        SceneManager.LoadScene("Menu");
    }

    //if a player leaves the room the other one leaves too 
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.Log(otherPlayer.NickName + " has left the game");
        /*GameObject winText = GameObject.Find("MultiplayerWin");
        if(winText.GetComponent<Text>().text.Trim() == "")
            winText.GetComponent<Text>().text = "<b>" + otherPlayer.NickName + "</b>has left the game.\nYou will be redirected to menu shortly...";
        */
        StartCoroutine(DisconnectToMenu());
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.Log(newPlayer.NickName + " has joined the game");
        GameObject.Find("MultiplayerWin").GetComponent<Text>().text = "";
    }

}
