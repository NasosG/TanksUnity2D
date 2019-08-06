using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Globalization;

public class Game : MonoBehaviour 
{
    public OpenGame openGame;
	[Header("Setup")]
	public Color player1Color = Color.blue;			    //The colour that player 1's tank will be when the game starts.
	public Color player2Color = Color.red;				//The colour that player 2's tank will be when the game starts.
    public Color playerMultiplayerColor = Color.blue;   //The colour of that player in multiplayer
    public Color playerMultiplayer2Color = Color.red;
    public bool oneHitKill = false;						//Will a projectile instantly kill its target?
	public bool canDamageOwnTank = true;				//Can a tank damage itself by shooting a projectile?
	public int respawnDelay = 1;						//The amount of time a player will wait between dying and respawning.
	public int maxScore = 10;							//The score that when a player reaches, will end the game.
	public int maxProjectileBounces = 4;				//The maximum amount of times a projectile can bounce off walls.

	[Space(10)]
	public int tankStartHealth = 3;						//The health the player tanks will get when the game starts.
	public int tankStartDamage = 1;						//The damage the player tanks will get when the game starts.
	public float tankStartMoveSpeed = 70;				//The move speed the player tanks will get when the game starts.
	public float tankStartTurnSpeed = 100;				//The turn speed the player tanks will get when the game starts.
	public float tankStartProjectileSpeed = 13;			//The projectile speed the player tanks will get when the game starts.
	public float tankStartReloadSpeed = 1;				//The reload speed the player tanks will get when the game starts.

	[Header("Tanks")]
	public Tank player1Tank;							//Player 1's tank. 
	public Tank player2Tank;							//Player 2's tank.
    public HelicopterAi helicopter;
    public Turret turret;
    public TankMultiplayer playerMultiplayer;           //tank model used for multiplayer
    public GameObject myObjectTank1;                    //game object of tank1  used to disable tank 1 in multiplayer
    public GameObject myObjectTank2;                    //game object of tank2 used to disable tank 2 in multiplayer
    public GameObject PlayerMultiplayer;

    [Header("Scores")]
	public int player1Score;							//Player 1's score.
	public int player2Score;							//Player 2's score.
    public int playerMultiplayerScore;                  //Player multiplayer's score

    [Header("Spawn Points")]
	public List<GameObject> spawnPoints = new List<GameObject>();	//A list of all the spawn points, which the players can spawn at.

	[Header("Prefabs")]
	public GameObject wallPrefab;						//The wall prefab, which will be spawned at the start of the game to make up the level.
    public GameObject wallBricksPrefab;                 //The wall Bricks prefab, which will be spawned at the start of the game to make up the level.
    public GameObject wallWhitePrefab;                  //The white wall prefab, which will be spawned at the start of the game to make up the level.
    public GameObject wallRuinPrefab;                   //The ruined wall prefab, which will be spawned at the start of the game to make up the level.
    public GameObject wallIronPrefab;                   //The ruined wall prefab, which will be spawned at the start of the game to make up the level.
    public GameObject wallSpiralPrefab;                 //The spiral wall prefab, which will be spawned at the start of the game to make up the level.

    [Header("Components")]
	public UI ui;										//The UI.cs script, located in the GameManager game object.

	private string mapToLoad;                           //The name of the map that is going to be loaded.

    public Image player1Im;
    public Image player2Im;
    public int lives = 4;

    void Start ()
	{
            // player icon only in coop
            if (MenuUI.getFlag() == 3) {
                player1Im.GetComponent<Image>().sprite = OpenGame.GetPlayer1Sprite(); //set image sprite to the sprite player 1 has chosen
                player2Im.GetComponent<Image>().sprite = OpenGame.GetPlayer2Sprite(); //set image sprite to the sprite player 2 has chosen
            }

            // Create a temporary reference to the current scene.
            Scene currentScene = SceneManager.GetActiveScene();

            // Retrieve the name of this scene.
            string sceneName = currentScene.name;
            
            if (sceneName == "GameArena1" || sceneName == "vsPC") {      //scenes with different arena maps
                //Load The Map
                mapToLoad = PlayerPrefs.GetString("MapToLoad");
                TextAsset map = Resources.Load<TextAsset>("Maps/" + mapToLoad) as TextAsset;
                //Debug.Log(mapToLoad + "\n");
                //Debug.Log("    " + map.text + "   ");
                LoadMap(map.text);
            }
            else {
                //Load The Map
                mapToLoad = "Arena";
                TextAsset map = Resources.Load<TextAsset>("Maps/" + mapToLoad) as TextAsset;
                //Debug.Log(mapToLoad + "\n");
                //Debug.Log("    " + map.text + "   ");
                LoadStandardMap(map.text);
            }

            if (MenuUI.getFlag() != 1)
            {
                //Tank Bools
                player1Tank.canMove = true;
                player1Tank.canShoot = true;

                player2Tank.canMove = true;
                player2Tank.canShoot = true;

                //Tank Start Values
                player1Tank.SetStartValues();
                player2Tank.SetStartValues();

                ui.SetupHealthBars();
                if (MenuUI.getFlag() == 3) {
                    //Tank Color
                    player1Tank.GetComponent<SpriteRenderer>().color = OpenGame.GetColorPlayer1();//player1Color;
                    player2Tank.GetComponent<SpriteRenderer>().color = OpenGame.GetColorPlayer2();//player2Color;
                    //set players colors to the colors user has chosen
                    player1Color = OpenGame.GetColorPlayer1();
                    player2Color = OpenGame.GetColorPlayer2();
                    
                    if (OpenGame.colorChangePlayer1 == "green") {       //green tank player 1
                        //Debug.Log("green");
                        player1Tank.damage++;                           //extra damage
                        player1Tank.reloadSpeed *= 2;                   //slower reload
                    }
                    else if (OpenGame.colorChangePlayer1 == "blue") {   //blue tank player 1
                        //Debug.Log("blue");
                        player1Tank.moveSpeed *= 2;                     //more move speed
                        player1Tank.turnSpeed *= 2;                     //more turn speed
                        player1Tank.reloadSpeed *= 2;                   //slower reload
                    }
                    if (OpenGame.colorChangePlayer2 == "green") {       //green tank player 2
                        //Debug.Log("green ppplayer2");
                        player2Tank.damage++;                           //extra damage
                        player2Tank.reloadSpeed *= 2;                   //more time to reload
                    }
                    else if (OpenGame.colorChangePlayer2 == "blue") {   //blue tank player 2
                        //Debug.Log("blue ppplayer2");
                        player2Tank.moveSpeed *= 2;                     //moves faster
                        player2Tank.turnSpeed *= 2;                     //more speed at turns
                        player2Tank.reloadSpeed *= 2;                   //slower reload
                    }
                }
                else {
                    //Tank Color
                    player1Tank.GetComponent<SpriteRenderer>().color = player1Color;
                    player2Tank.GetComponent<SpriteRenderer>().color = player2Color;
                }
               //spawn points campaign
                if (MenuUI.getFlag() == 2) {
                    player1Tank.transform.position = new Vector3(-48, -22, 0);
                    player2Tank.transform.position = new Vector3(-3, 21, 0);
                }
                else {
                     //set tank spawn position
                    player1Tank.transform.position = spawnPoints[0].transform.position;
                    player2Tank.transform.position = spawnPoints[1].transform.position;
                }
            }
            //else we are in multiplayer
            else {                  
                //spawnPlayer();          
                //playerMultiplayer.gameObject.SetActive(true);
                ui.SetupHealthBars();
                //deactivate the usual coop - singleplayer tanks
                myObjectTank1.SetActive(false);
                myObjectTank2.SetActive(false);
            }

    }


    void Update ()
	{
        if (MenuUI.getFlag() != 2) { //if user hasn't chosen campaign
            //Checking Scores
            if (player1Score >= maxScore) { //Does player 1 reach the score amount to win the game?
                WinGame(0);                 //Player 1 wins the game. Player1 id = 0
            }
            if (player2Score >= maxScore) { //Does player 2 reach the score amount to win the game?
                WinGame(1);                 //Player 2 wins the game. Player2 id = 1
            }
        }
        else {
            if (lives == 0) {
                ULost();
            }
        }
	}

    public double GetRandomNumber(double minimum, double maximum)
    {
        System.Random random = new System.Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }

    void ULost()
    {
        //Disable movement and shooting for the tanks.

        //For Player1
        player1Tank.canMove = false;
        player1Tank.canShoot = false;

        //For Player2
        player2Tank.canMove = false;
        player2Tank.canShoot = false;

        ui.SetLoseScreen();  //Call the SetWinScreen function in UI.cs, and send over the winning player's id.
    }

    //Called when a player's score reaches the maxScore.
    //The "playerId" value, is the id of the player that won the game.
    void WinGame (int playerId)
	{
		//Disable movement and shooting for the tanks.

        //For Player1
		player1Tank.canMove = false;
		player1Tank.canShoot = false;

        //For Player2
        player2Tank.canMove = false;
		player2Tank.canShoot = false;
        
		ui.SetWinScreen(playerId);	//Call the SetWinScreen function in UI.cs, and send over the winning player's id.
	}

	//Called when the level loads. It reads the map file and spawns in walls and spawn points.
	void LoadMap (string map)
	{
		string[] lines = map.Split("\n"[0]);    //Splits the file into seperate lines, each indicating a seperate tile.

        for (int i = 0; i < lines.Length; i++) {			//Loop through all the tiles.
			if(lines[i] != "") {							//Is the line not blank?
				string[] parts = lines[i].Split(","[0]);    //Then split that line at every comma.

                //do NOT forget culture info or pcs configurations will create problems

                if (parts[0].Equals("Wall")) {              //Is this tile a wall?
                    GameObject wall = Instantiate(wallPrefab, new Vector3(float.Parse(parts[1], new CultureInfo("en-US").NumberFormat), float.Parse(parts[2], new CultureInfo("en-US").NumberFormat), 0), Quaternion.identity) as GameObject; //Spawn in the wall game object.
                }
                if (parts[0].Equals("WallBricks")) {       //Is this tile a wall made of bricks?
                    GameObject wall = Instantiate(wallBricksPrefab, new Vector3(float.Parse(parts[1], new CultureInfo("en-US").NumberFormat), float.Parse(parts[2], new CultureInfo("en-US").NumberFormat), 0), Quaternion.identity) as GameObject; //Spawn in the wall game object.
                }
                if (parts[0].Equals("WallWhite")) {        //Is this tile a wall made of bricks?
                    GameObject wall = Instantiate(wallWhitePrefab, new Vector3(float.Parse(parts[1], new CultureInfo("en-US").NumberFormat), float.Parse(parts[2], new CultureInfo("en-US").NumberFormat), 0), Quaternion.identity) as GameObject; //Spawn in the wall game object.
                }
                if (parts[0].Equals("WallRuin")) {         //Is this tile a wall made of ruined bricks?
                    GameObject wall = Instantiate(wallRuinPrefab, new Vector3(float.Parse(parts[1], new CultureInfo("en-US").NumberFormat), float.Parse(parts[2], new CultureInfo("en-US").NumberFormat), 0), Quaternion.identity) as GameObject; //Spawn in the wall game object.
                }
                if (parts[0].Equals("Wall_Iron")) {        //Is this tile a wall made of iron bricks?
                    GameObject wall = Instantiate(wallIronPrefab, new Vector3(float.Parse(parts[1], new CultureInfo("en-US").NumberFormat), float.Parse(parts[2], new CultureInfo("en-US").NumberFormat), 0), Quaternion.identity) as GameObject; //Spawn in the wall game object.
                }
                if (parts[0].Equals("WallSpiral")) {       //Is this tile a spiral wall?
                    GameObject wall = Instantiate(wallSpiralPrefab, new Vector3(float.Parse(parts[1], new CultureInfo("en-US").NumberFormat), float.Parse(parts[2], new CultureInfo("en-US").NumberFormat), 0), Quaternion.identity) as GameObject; //Spawn in the wall game object.
                }            
                else if (parts[0].Contains("SpawnPoint")) {                 //Is this tile a spawn point?
                    GameObject spawnPoint = new GameObject("SpawnPoint");   //Spawn a blank game object which will be the spawn point.
                    spawnPoint.transform.position = new Vector3(float.Parse(parts[1], new CultureInfo("en-US").NumberFormat), float.Parse(parts[2], new CultureInfo("en-US").NumberFormat), 0);   //Set the spawn point position.
                    spawnPoints.Add(spawnPoint);            //Add the spawn point to the spawnPoints list.
                }
			}
		}
	}

    //Called when the standard "junge" level loads. It reads the map file and spawns in spawn points.
    void LoadStandardMap(string map)
    {
        string[] lines = map.Split("\n"[0]);    //Splits the file into seperate lines, each indicating a seperate tile.

        for (int i = 0; i < lines.Length; i++) {            //Loop through all the tiles.
            if (lines[i] != "") {                           //Is the line not blank?
                string[] parts = lines[i].Split(","[0]);	//Then split that line at every comma.

                if (parts[0].Contains("SpawnPoint")) {                      //Is this tile a spawn point?
                    GameObject spawnPoint = new GameObject("SpawnPoint");   //Spawn a blank game object which will be the spawn point.
                    spawnPoint.transform.position = new Vector3(float.Parse(parts[1], new CultureInfo("en-US").NumberFormat), float.Parse(parts[2], new CultureInfo("en-US").NumberFormat), 0);   //Set the spawn point position.
                    spawnPoints.Add(spawnPoint);            //Add the spawn point to the spawnPoints list.
                }
            }
        }
    }


}
