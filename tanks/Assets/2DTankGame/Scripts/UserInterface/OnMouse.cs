using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnMouse : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Text info;
    public string m_OriginalText;

    // Start is called before the first frame update
    void Start()
    {
        m_OriginalText = info.text;
    }

    // Update is called once per frame
    void Update()
    {
        //Not used yet   
    }


    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // Output to console the GameObject's name and the following message
        // Debug.Log("Cursor Entering " + name + " GameObject");

        // The info text changes to whatever text the button has...
        info.text = pointerEventData.pointerCurrentRaycast.gameObject.GetComponentInChildren<Text>().text;
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Output the following message with the GameObject's name
        //Debug.Log("Cursor Exiting " + name + " GameObject");

        // And the info text changes back when the mouse moves away
        info.text = m_OriginalText;
    }

    /*
        //If mouse is over the button
        private bool IsMouseOverUI() {
            return EventSystem.current.IsPointerOverGameObject();
        }
    */


}
