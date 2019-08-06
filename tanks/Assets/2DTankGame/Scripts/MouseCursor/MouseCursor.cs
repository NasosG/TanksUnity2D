using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MouseCursor : MonoBehaviour
{
    public Texture2D cursorImage;
    public bool lockCursor = true;
    public GameObject theSprite;
    public bool exc = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }
    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        exc = false;
        if (scene.name == "Menum")
        {
            Debug.Log("multiplayer menu");
            exc = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (exc) { Cursor.visible = true; }
        else { 
            Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = cursorPos;
        }
    }
}
