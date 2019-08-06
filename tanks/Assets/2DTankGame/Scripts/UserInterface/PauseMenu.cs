using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool GameIsPaused = false;
    public Game game;
    public GameObject pauseMenuUI;
    public GameObject insidePanel;

    // Update is called once per frame
    void Update()
    {
        //Pause or resume game if user presses escape and he/she is not in the multiplayer menu
        if (Input.GetKeyDown(KeyCode.Escape) && MenuUI.getFlag() != 1) {
            if (GameIsPaused)
                Resume();       //Resume Game
            else
                Pause();        //Pause Game
        }
    }

    //Resume Game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);    //deactivate pause menu
        insidePanel.SetActive(false);    //deactivate inside panel
        Time.timeScale = 1f;             //continue time
        GameIsPaused = false;            //game is not paused anymore
    }

    //Pause Game
    void Pause()
    {
        pauseMenuUI.SetActive(true);    //make pause menu active
        insidePanel.SetActive(true);    //make inside panel visible too
        Time.timeScale = 0f;            //stop time
        GameIsPaused = true;            //make our control variable true
    }

    //This method combines the methods above into one 
    //but in the end thought that two methods make the code more readable 
    void PauseTrigger()
    {
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);    //make pause menu active
        insidePanel.SetActive(!insidePanel.activeSelf);    //make inside panel visible too
        Time.timeScale = 1 - Time.timeScale;               //stop time
        //GameIsPaused = !GameIsPaused;                     //not really needed
    }

    //Load Main Menu
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        //Redirect user to the staring menu
        game.ui.GoToMenu();
    }

    //Quit Game
    public void QuitGame() {
        //Quit game if user presses exit button
        Application.Quit();
    }
}
