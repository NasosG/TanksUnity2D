using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public Dialogue dialogue;
    public GameObject DialoguePanel;

    public void TriggerDialogue ()
	{
        DialoguePanel.SetActive(false);
        Time.timeScale = 1f;
        //FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

}
