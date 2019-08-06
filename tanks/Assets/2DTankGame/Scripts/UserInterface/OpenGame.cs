using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpenGame : MonoBehaviour
{
    public Button redButtonP1, blueButtonP1, greenButtonP1, redButtonP2, blueButtonP2, greenButtonP2;
    public Button dogP1, generalP1, wsManP1, dogP2, generalP2, wsManP2;
    public Button submitButton;

    public static Color player1Color = Color.white;
    public static Color player2Color = Color.white;

    public static Sprite player1Image;
    public static Sprite player2Image;

    public static Color buttonColor;

    //color flag with getter and setter
    public static string colorChangePlayer1 { get; set; } = "red";
    public static string colorChangePlayer2 { get; set; } = "red";

    //public Color red, green, blue;
    public bool colorError = false;
    public GameObject errorText;

    //method that returns player 1 color
    public static Color GetColorPlayer1()
    {
        //Debug.Log(player1Color);
        return player1Color;
    }

    //method that returns player 2 color
    public static Color GetColorPlayer2()
    {
        //Debug.Log(player2Color);
        return player2Color;
    }

    //method that returns player 1 sprite
    public static Sprite GetPlayer1Sprite()
    {
        //Debug.Log(player1Image);
        return player1Image;
    }

    //method that returns player 2 sprite
    public static Sprite GetPlayer2Sprite()
    {
        //Debug.Log(player2Image);
        return player2Image;
    }

    //reset all at the beggining
    public void ResetAll()
    {
        //default players' color
        player1Color = Color.white;
        player2Color = Color.white;

        //default null images
        player1Image = null;
        player2Image = null;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //reset everything
        ResetAll();
        // button listeners
        // Calls the TaskOnClick/TaskWithParameters/ButtonClicked method when you click the Button
        redButtonP1.onClick.AddListener(delegate { TaskWithParameters(redButtonP1.GetComponent<Button>().colors.normalColor, 1, redButtonP1); });
        greenButtonP1.onClick.AddListener(delegate { TaskWithParameters(greenButtonP1.GetComponent<Button>().colors.normalColor, 1, greenButtonP1); });
        blueButtonP1.onClick.AddListener(delegate { TaskWithParameters(blueButtonP1.GetComponent<Button>().colors.normalColor, 1, blueButtonP1); });

        redButtonP2.onClick.AddListener(delegate { TaskWithParameters(redButtonP2.GetComponent<Button>().colors.normalColor, 2, redButtonP2); });
        greenButtonP2.onClick.AddListener(delegate { TaskWithParameters(greenButtonP2.GetComponent<Button>().colors.normalColor, 2, greenButtonP2); });
        blueButtonP2.onClick.AddListener(delegate { TaskWithParameters(blueButtonP2.GetComponent<Button>().colors.normalColor, 2, blueButtonP2); });
        
        
        //-----------------------------------------------------------------------------------------------------------------------------------------------//

        dogP1.onClick.AddListener(delegate { TaskWithParameters2(dogP1.GetComponent<Image>().sprite, 1, dogP1); });
        generalP1.onClick.AddListener(delegate { TaskWithParameters2(generalP1.GetComponent<Image>().sprite, 1, generalP1); });
        wsManP1.onClick.AddListener(delegate { TaskWithParameters2(wsManP1.GetComponent<Image>().sprite, 1, wsManP1); });

        dogP2.onClick.AddListener(delegate { TaskWithParameters2(dogP2.GetComponent<Image>().sprite, 2, dogP2); });
        generalP2.onClick.AddListener(delegate { TaskWithParameters2(generalP2.GetComponent<Image>().sprite, 2, generalP2); });
        wsManP2.onClick.AddListener(delegate { TaskWithParameters2(wsManP2.GetComponent<Image>().sprite, 2, wsManP2); });


        submitButton.onClick.AddListener(chooseScene);
        //m_YourThirdButton.onClick.AddListener(() => ButtonClicked(3));
        //m_YourThirdButton.onClick.AddListener(TaskOnClick);
    }
    void TaskOnClick()
    {
        //Output this to console when Button1 or Button3 is clicked
        Debug.Log("You have clicked the button!");
    }

    void TaskWithParameters(Color color, int playerNO, Button buttonPressed)
    {
        //Output this to console when the Button2 is clicked
        Debug.Log(color);
        if (playerNO == 1) {
            player1Color = color;
        }
        else if (playerNO == 2) {
            player2Color = color;
        }

        if (buttonPressed == redButtonP1) {
            redButtonP2.interactable = false;

            greenButtonP2.interactable = true;
            blueButtonP2.interactable = true;

            colorChangePlayer1 = "red";
        }
        else if (buttonPressed == greenButtonP1) {
            greenButtonP2.interactable = false;

            redButtonP2.interactable = true;
            blueButtonP2.interactable = true;

            colorChangePlayer1 = "green";
        }
        else if (buttonPressed == blueButtonP1) {
            blueButtonP2.interactable = false;

            greenButtonP2.interactable = true;
            redButtonP2.interactable = true;

            colorChangePlayer1 = "blue";
        }

        if (buttonPressed == redButtonP2)
        {
            redButtonP1.interactable = false;

            greenButtonP1.interactable = true;
            blueButtonP1.interactable = true;

            colorChangePlayer2 = "red";
        }
        else if (buttonPressed == greenButtonP2)
        {
            greenButtonP1.interactable = false;

            redButtonP1.interactable = true;
            blueButtonP1.interactable = true;

            colorChangePlayer2 = "green";
        }
        else if (buttonPressed == blueButtonP2)
        {
            blueButtonP1.interactable = false;

            greenButtonP1.interactable = true;
            redButtonP1.interactable = true;

            colorChangePlayer2 = "blue";
        }

    }



    void TaskWithParameters2(Sprite image, int playerNO, Button buttonPressed)
    {
        //Output this to console when the Button2 is clicked
        Debug.Log(image);
 
        if (playerNO == 1) {
            player1Image = image;
        }
        else {
            player2Image = image;
        }

    }

    void chooseScene()
    {
        // if players 1 or players 2 color is the default one or players 1 or players 2 image has not been set
        // do nothing
        if (player1Color == Color.white || player2Color == Color.white || player1Image == null || player2Image == null) {
            Debug.Log("no color or image has been chosen");
            colorError = true;
            errorText.SetActive(true);
            return;
        }

        if (Menu.scene == "GameArena1") {
            SceneManager.LoadSceneAsync("GameArena1");
        }
        else if (Menu.scene == "Space") {
            SceneManager.LoadSceneAsync("Space");
        }
        else {
            SceneManager.LoadSceneAsync("Game");
        }
    }
    void ButtonClicked(int buttonNo)
    {
        //Output this to console when the Button3 is clicked
        Debug.Log("Button color = " + buttonNo);
    }

    // Update is called once per frame
    void Update()
    {
        if (colorError)
            if (player1Color != Color.white && player2Color != Color.white && player1Image != null && player2Image != null)
            {
                colorError = !colorError;
                errorText.SetActive(false);
            }
    }

}
