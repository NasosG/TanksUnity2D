using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MultiplayerTanks;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class UI : MonoBehaviour 
{
	//UI
	[Header("Health Bars")]
	public Slider p1HealthBar;		        //The health bar that is above player 1
	public Slider p2HealthBar;		        //The health bar that is above player 2
    public Slider pMultiplayerHealthBar;    //The health bar that is above player 2
    public Slider castle_up_left_HP = null;
    public Slider castle_up_right_HP = null;
    public Slider castle_down_right_HP = null;
    public Slider bigCastle_HP = null;

    [Header("Win Screen")]
	public GameObject winScreen;            //The screen that pops up once a player has won the game
    public Text winText;                    //The text on the win screen that says which player has won

    [Header("Kill Screen")]
    public GameObject killScreen = null;            //The screen that pops up once a player has died
    public Text killText = null;                    //The text on the kill screen

    [Header("Win Screen")]
    public GameObject campaignWinScreen = null;            //The screen that pops up once a player has died
    public Text campaignWinText = null;                    //The text on the win screen that says which player has won

    [Header("Lose Screen")]
    public GameObject LoseScreen = null;            //The screen that pops up once a player has died
    public Text LoseText = null;			        //The text on the win screen that says which player has won

    [Header("Other")]
	public Text scoreText;			        //The text at the top of the screen which displays the score
    public Text rocketsPL1;                 //The text at the left of the screen which displays the score
    public Text rocketsPL2;			        //The text at the right of the screen which displays the score
    public Text livesText;

    [Header("Components")]
	public Game game;                       //Current game
    public GameManager gm;                  //Multiplayer game manager
    public Text playerNameText;             //player's name text that is located above each player's tank

    public Castle castle_up_left = null;     // position of the castle in the left up corner
    public Castle castle_up_right = null;    // position of the castle in the right up corner
    public Castle castle_down_right = null;  // position of the castle in the right down corner
    public Castle bigCastle = null;          // position of the big castle in the centre

    public bool smallCastlesAreDestroyed = false;
    /*
     * void Start()
    {
        playerNameText = PhotonNetwork.Instantiate("playerNameText", gm.player.transform.position, gm.player.transform.rotation, 0).GetComponent<Text>();
    }
    */

    //Called by the Game.cs script. This sets the values of the health bars to be the same as the tank's health
    public void SetupHealthBars()
    {
        if (MenuUI.getFlag() == 2) {                                    //are we in multiplayer?
            castle_up_left_HP.maxValue = castle_up_right_HP.maxValue = castle_down_right_HP.maxValue = 10;          //set player 1 healthbar max value 
            bigCastle_HP.maxValue = 20;
        }
        if (MenuUI.getFlag() != 1) {                                    //are we in multiplayer?
            p1HealthBar.maxValue = game.player1Tank.maxHealth;          //set player 1 healthbar max value
            p2HealthBar.maxValue = game.player2Tank.maxHealth;          //set player 2 healthbar max value
            if(pMultiplayerHealthBar != null)
            pMultiplayerHealthBar.gameObject.SetActive(false);          //deactivate player's in multiplayer health bar when we don't play in multiplayer
        }
        else { 
            pMultiplayerHealthBar.maxValue = 3 /*game.playerMultiplayer.maxHealth*/;  //set player multiplayer healthbar max value

            //playerNameText.gameObject.SetActive(true);

            //deactivate players' healthbars in multiplayer and let only the one of our player which we use only on multiplayer
            p1HealthBar.gameObject.SetActive(false);                    
            p2HealthBar.gameObject.SetActive(false);
        }
    }


	void Update ()
	{
        if (MenuUI.getFlag() == 2) { //on campaign
            //healthbars position
            castle_up_left_HP.transform.position = castle_up_left.transform.position + new Vector3(0, 2, 0);
            castle_up_right_HP.transform.position = castle_up_right.transform.position + new Vector3(0, 2, 0);
            castle_down_right_HP.transform.position = castle_down_right.transform.position + new Vector3(0, 2, 0);
            bigCastle_HP.transform.position = bigCastle.transform.position + new Vector3(0, 2, 0);
            
            //healthbars remaining health
            castle_up_left_HP.value = castle_up_left.health;
            castle_up_right_HP.value = castle_up_right.health;
            castle_down_right_HP.value = castle_down_right.health;
            bigCastle_HP.value = bigCastle.health;

            if (castle_up_left.health <= 0 && castle_up_right.health <= 0 && castle_down_right.health <= 0)
                smallCastlesAreDestroyed = true;
            
            //if a castles falls (health <= 0), deactivate its healthbar 
            if (castle_up_left.health <= 0)
                castle_up_left_HP.gameObject.SetActive(false);
            if (castle_up_right.health <= 0)
                castle_up_right_HP.gameObject.SetActive(false);
            if (castle_down_right.health <= 0)
                castle_down_right_HP.gameObject.SetActive(false);
            if (bigCastle.health <= 0)
                bigCastle_HP.gameObject.SetActive(false);

            //display number of player lives
            livesText.text = "lives<b><color=" + ToHex(game.player1Color) + ">\n" + game.lives + "</color></b>";
        }

        //--------------------------------------------------------------------------------------------------------//

        // display number of rockets for the two players
        rocketsPL1.text = "rockets<b><color=" + ToHex(game.player1Color) + ">\n" + game.player1Tank.numOfRockets + "</color></b>";
        //rockets for player2 in any mode except campaign
        if (MenuUI.getFlag() != 2) {
            rocketsPL2.text = "rockets<b><color=" + ToHex(game.player2Color) + ">\n" + game.player2Tank.numOfRockets + "</color></b>";
            //set the score text to display the scores of the tanks, with their corresponding colors
            scoreText.text = "<b>SCORE</b>\n<b><color=" + ToHex(game.player1Color) + ">" + game.player1Score + "</color></b> - <b><color=" + ToHex(game.player2Color) + ">" + game.player2Score + "</color></b>";
        }
            //--------------------------------------------------------------------------------------------------------//

            if (MenuUI.getFlag() != 1) {                                                                        //exclude multiplayer
            if (game.player1Tank != null) {                                                                     //If player 1's tank exists
                p1HealthBar.transform.position = game.player1Tank.transform.position + new Vector3(0, 2, 0);    //Sets the health bar to be just above player 1's tank
                p1HealthBar.value = game.player1Tank.health;                                                    //Sets the value of the health bar to be the same as the tank's
            }
            if (game.player2Tank != null) {                                                                     //If player 2's tank exists
                p2HealthBar.transform.position = game.player2Tank.transform.position + new Vector3(0, 2, 0);    //Sets the health bar to be just above player 2's tank
                p2HealthBar.value = game.player2Tank.health;                                                    //Sets the value of the health bar to be the same as the tank's
            }
        }

        /*
         * for future multiplayer update
         *
         else {  //if (MenuUI.getFlag() == 1)
            if (gm.player!=null) {                                                                              //If player Multiplayer tank exists
                pMultiplayerHealthBar.transform.position = gm.player.transform.position + new Vector3(0, 2, 0); //Sets the health bar to be just above player 1's tank
                pMultiplayerHealthBar.value = 3;                                                                //Sets the value of the health bar to be the same as the tank's
                //Debug.Log(pMultiplayerHealthBar.value);
                //playerNameText.transform.position = gm.player.transform.position + new Vector3(0, 2.5f, 0);     //Sets the player's name to be just above player's health
                //playerNameText.text = NetworkConnectionManager.playeroname;
                //Debug.Log(playerNameText.text);
            }
        }*/

    }

    public void SetWinScreen (int winner)
	{
		winScreen.SetActive(true);

        //Player 1 wins
        if (winner == 0) {      
			winText.text = "<b><color=" + ToHex(game.player1Color) + ">PLAYER 1</color></b>\n<color=white>Wins The Game</color>";
		}
        //Player 2 wins
        else {                  
			winText.text = "<b><color=" + ToHex(game.player2Color) + ">PLAYER 2</color></b>\n<color=white>Wins The Game</color>";
		}
    }

    //Activates the screen that pops up once a player has died and pauses the game
    public void SetCampaignWinScreen()
    {
        campaignWinScreen.SetActive(true);        //activate the screen that pops up once a player has died
        Time.timeScale = 0f;                      //stop time
    }

    //Activates the screen that pops up once a player has died and pauses the game
    public void SetKillScreen()
    {
        killScreen.SetActive(true);     //activate the screen that pops up once a player has died
        Time.timeScale = 0f;            //stop time
    }

    public void SetLoseScreen()
    {
        LoseScreen.SetActive(true);     //activate the screen that pops up once a player has died
        Time.timeScale = 0f;            //stop time
    }

    //Converts an RGB color to a HEX value, and returns it as a string
    string ToHex (Color color)
	{
		return string.Format("#{0:X2}{1:X2}{2:X2}", ToByte(color.r), ToByte(color.g), ToByte(color.b));
	}


	//Converts a float to a byte. Used by the ToHex() function
	byte ToByte (float num)
	{
        // The number is clamped to between zero and one by Clamp01()
        // (value never gets less than 0 or more than 1)
        num = Mathf.Clamp01(num);
        //return the byte, which is 8 bits and represents 256 values (0 through 255)
        return (byte)(num * 255);
	}


    //Deactivates respawn screen and unpauses the game
    public void RespawnAtCheckpoint()
    {
        Time.timeScale = 1f;            //start time
        //Loads the menu level
        killScreen.SetActive(false);
    }


    //Just a wait timer
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(5);
    }


    //Called when a player wins and the home button is pressed
    public void GoToMenu ()
	{
        //Loads the menu level
        SceneManager.LoadSceneAsync("Menu");
    }


    //Called when a player wins and the restart button is pressed
    public void RestartGame()
    {
        //Loads the game scene
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
    }


    //Called when a player wins and the exit button is pressed
    public void QuitGame()
    {
        //Quit game if user presses exit button
        Application.Quit();
    }
}
