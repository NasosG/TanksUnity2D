using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour 
{
	public GameObject mapButtonParent;	//The game object which the map select buttons will be a child of.

	[Header("Pages")]
	public GameObject menuPage;			
	public GameObject playPage;
    public GameObject multPage;
    public GameObject infoPage;
    public GameObject spaceCoop;

    [Header("Components / Objects")]
    public static int flag;
    public int flagMenuUI;
    public Sprite mute1;
    public Sprite unmute;
    public Button ButtonMute;
    public bool infoBool = false;
    public Button[] button;
    public GameObject mapButtonPrefab;	//The map select button prefab.
	public Menu menu;                   //The Menu.cs script, located on the Menu game object.
    //public OnMouse om;

    void Start()
    {
        //a check after menu has loaded if user had previously turned sound off
        if (AudioListener.pause)
            ButtonMute.image.overrideSprite = unmute;       //set unmute image to button's image
        else
            ButtonMute.image.overrideSprite = mute1;        //set mute image to button's image
    }

    //Called when the player presses the PLAY button or the BACK button on the menu
    //Changes the page to either the menu page, play page or any other page
    public void SetPage (string page)
	{
        if (page == "multiplayer") {                     //if player has clicked on multiplayer button
            flag = 1;
            SceneManager.LoadSceneAsync("Menum");        //Loads the multiplayer menu
            return;
        }
        if (page == "campaign")
        {
            flag = 2;
            SceneManager.LoadSceneAsync("SinglePlayer");
            return;
        }
        menuPage.SetActive(false);
        if(spaceCoop != null)
            spaceCoop.SetActive(false);
        playPage.SetActive(true);

        if (page == "menu") {
            flag = 0;
			menuPage.SetActive(true);
            if (spaceCoop != null)
                spaceCoop.SetActive(true);
            playPage.SetActive(false);
		}
        /*if (page == "multiplayer") {
            flag = 1;
        }*/
        if (page == "singleP") {
            flag = 4;
        }
        if (page == "play") {
            flag = 3;
		}

        flagMenuUI = flag;
	}

    public void LoadCoOP_Scene()
    {
        //textS = om.info.text;
        //Loads the game scene
        SceneManager.LoadSceneAsync("MenuSelection");
    }

    public void LoadGame_Scene()
    {
        flag = 3;
        Menu.scene = "Game";
        //Loads the game scene
        SceneManager.LoadSceneAsync("Colors");
    }

    public void LoadSpaceScene()
    {
        flag = 3;
        flagMenuUI = flag;
        Menu.scene = "Space";
        //Loads the game scene
        SceneManager.LoadSceneAsync("Colors");
    }

    //Load Scene function
    public void LoadScene(string a)
    {
        if (playPage.activeSelf) {
            flag = 0;
            menuPage.SetActive(true);
            playPage.SetActive(false);
        }
        else
            //Loads the scene
            SceneManager.LoadSceneAsync(a);
    }

    public void Mute()
    {
        AudioListener.pause = !AudioListener.pause;
        if (AudioListener.pause)
            ButtonMute.image.overrideSprite = unmute;
        else
            ButtonMute.image.overrideSprite = mute1;

    }

    public void iNFO() {
        infoBool = !infoBool;
        if (infoBool) {
            if (spaceCoop != null)
                spaceCoop.SetActive(false);
            menuPage.SetActive(false);
            infoPage.SetActive(true);
        }
        else {
            if (spaceCoop != null)
                spaceCoop.SetActive(true);
            menuPage.SetActive(true);
            infoPage.SetActive(false);
        }
    }

	//Spawns in the map select buttons, located in the play page.
	//The "maps" value, is an array of all the map names.
	public void LoadMapButtons (string[] maps)
	{
		for (int i = 0; i < maps.Length; i++) {												//Loops through the map names. And for each map...
			GameObject mapBut = Instantiate(mapButtonPrefab, mapButtonParent.transform.position, Quaternion.identity) as GameObject;	//Spawns the button.
			mapBut.transform.parent = mapButtonParent.transform;							//Sets the button's parent to the mapButtonParent.
			mapBut.transform.localScale = Vector3.one;										//Sets the button's scale to 1.

            //Fetch the Text and RectTransform components from the GameObject
            Text mapButtonText = mapBut.transform.Find("Text").GetComponent<Text>();        //Fetch the map's button text
            mapButtonText.text = maps[i];                                                   //Sets the button's text to display the map name.
            RectTransform mapButtonRectTransform = mapBut.GetComponent<RectTransform>();    //Fetch RectTransform components from the GameObject

            //make changes in the font size
            ChangeFontSize(mapBut.transform.Find("Text").GetComponent<Text>(), mapButtonRectTransform);
            string map = maps[i];															//Creates a temp variable which holds the map name.

			//Gets the Button component from the button game object and adds a listener to it, so that when the player clicks on it,
			//the PlayMap function gets called in the Menu.cs script, located in the Menu game object. It also sends over the map name
			//so that the function can then load up that map.
			mapBut.GetComponent<Button>().onClick.AddListener(() => {menu.PlayMap(map);});
		}
	}

    void ChangeFontSize(Text m_Text, RectTransform m_RectTransform)
    {
        //Change the Font Size
        m_Text.fontSize = 18;

        //Change the RectTransform size to allow larger fonts and sentences
        m_RectTransform.sizeDelta = new Vector2(m_Text.fontSize * 10, 100);
   
    }

    public static int getFlag() {
        return flag;
    }
}
