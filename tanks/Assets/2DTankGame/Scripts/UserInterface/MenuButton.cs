using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    Text txt;
    Color originalColor;
    Button btn;
    bool interactableDelay;

    void Start()
    {
        txt = GetComponentInChildren<Text>();
        originalColor = txt.color;
    }

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        // The text color changes to white
        txt.color = Color.white;
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        // And the text colour changes back to the original colour
        txt.color = originalColor;
    }

}
