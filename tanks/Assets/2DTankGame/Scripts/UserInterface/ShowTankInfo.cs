using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowTankInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject infoText, infoText2, infoText3, infoText4, infoText5, infoText6;
    public Button redButtonP1, blueButtonP1, greenButtonP1, redButtonP2, blueButtonP2, greenButtonP2;
    public Image sp, sp2, sp3, sp4, sp5, sp6;

    // Start is called before the first frame update
    void Start()
    {
        infoText.SetActive(false);
        infoText2.SetActive(false);
        infoText3.SetActive(false);
        infoText4.SetActive(false);
        infoText5.SetActive(false);
        infoText6.SetActive(false);
        //--------------------------------------------------------------------------------//
        sp.enabled = false;
        sp2.enabled = false;
        sp3.enabled = false;
        sp4.enabled = false;
        sp5.enabled = false;
        sp6.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        OnMousee(true);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        OnMousee(false);
    }

    public void OnMousee(bool value)
    {
        // Output to console the GameObject's name and the following message
        // Debug.Log("Cursor Entering " + name + " GameObject");

        // The info text changes to whatever text the button has...
        Debug.Log(this.GetComponent<Button>().name);
        if (this.GetComponent<Button>().name == redButtonP1.name)
        {
            infoText.SetActive(value);
            sp.enabled = value;
        }
        if (this.GetComponent<Button>().name == greenButtonP1.name)
        {
            infoText2.SetActive(value);
            sp2.enabled = value;
        }
        if (this.GetComponent<Button>().name == blueButtonP1.name)
        {
            infoText3.SetActive(value);
            sp3.enabled = value;
        }
        if (this.GetComponent<Button>().name == redButtonP2.name)
        {
            infoText4.SetActive(value);
            sp4.enabled = value;
        }
        if (this.GetComponent<Button>().name == greenButtonP2.name)
        {
            infoText5.SetActive(value);
            sp5.enabled = value;
        }
        if (this.GetComponent<Button>().name == blueButtonP2.name)
        {
            infoText6.SetActive(value);
            sp6.enabled = value;
        }
    }
    
}
