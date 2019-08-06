using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitByEnter : MonoBehaviour
{
    public Game game;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {     //if user presses enter 
            SceneManager.LoadSceneAsync("Menu");    //redirect user to home screen
        }
    }
}
