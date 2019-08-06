using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour 
{
    [HideInInspector]
    //An array of all the map names
    public string[] maps;
    public static string scene = "Game";	

	[Header("Components")]
    //The MenuUI.cs component, located in the Menu game object
    public MenuUI ui;		
    public int whattodo;
	void Start ()
	{
        whattodo = MenuUI.getFlag();
        Debug.Log(whattodo + "debug");
        LoadMaps();
    }

	//Called when the scene starts
	void LoadMaps ()
	{
		TextAsset[] m = Resources.LoadAll<TextAsset>("Maps"); 	//Loads all the maps in as TextAssets from the resources folder
		maps = new string[m.Length];							//Sets the maps variable to be the same length as m

		for(int i = 0; i < m.Length; i++) {						//Loops through all the maps
			maps[i] = m[i].name;								//Sets the maps variable to be an array of all the map names
		}

		ui.LoadMapButtons(maps);								//Calls the LoadMapButtons function in the MenuUI.cs script, sending over the array of maps
	}

	//Called when the player clicks on a map select button in the play menu
	//The "map" value, is the name of the map, requested to be loaded
	public void PlayMap (string map)
	{
		PlayerPrefs.SetString("MapToLoad", map);	     //Sets the PlayerPrefs string: "MapToLoad" to be the map name that was sent.
        if (ui.flagMenuUI == 1) {                        //if player has clicked on multiplayer button
            SceneManager.LoadSceneAsync("Menum");        //Loads the multiplayer menu
        }
        else if (ui.flagMenuUI == 4) {
            SceneManager.LoadSceneAsync("vsPC");
        }
        else if (scene == "Space")
            SceneManager.LoadSceneAsync("Colors");
        else {
            scene = "GameArena1";
            SceneManager.LoadSceneAsync("Colors");
        }
            //SceneManager.LoadSceneAsync("GameArena1");         //Loads the game
    }

	//Called when the player wants to quit the game
	public void QuitGame ()
	{
        //Ouit the game
        Application.Quit();	
	}
}
