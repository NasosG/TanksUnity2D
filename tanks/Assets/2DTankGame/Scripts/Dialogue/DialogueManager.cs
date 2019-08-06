using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

	public Text nameText;
	public Text dialogueText;
    public GameObject DialoguePanel;
    public Dialogue startingDialogue;
    //public Animator animator;

    private Queue<string> sentences;

	// Use this for initialization
	void Start ()
    {
        if (MenuUI.getFlag() != 1) {
            Time.timeScale = 0f;
            DialoguePanel.SetActive(true);
            sentences = new Queue<string>();
            FindObjectOfType<DialogueManager>().StartDialogue(startingDialogue);
        }
    }

	public void StartDialogue (Dialogue dialogue)
	{

		//animator.SetBool("IsOpen", true);

		nameText.text = dialogue.name;

        //clear sentences from the previous conversation
		sentences.Clear();

		foreach (string sentence in dialogue.sentences) {
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
        Debug.Log(sentences.Count);
        if (sentences.Count == 0)
		{
            //Debug.Log("im heree");
            //EndDialogue();
            DialoguePanel.SetActive(false);
            return;
		}

		string sentence = sentences.Dequeue();
		StopAllCoroutines();
		StartCoroutine(TypeSentence(sentence));
        Debug.Log(sentences.Count);
    }

	IEnumerator TypeSentence (string sentence)
	{
		dialogueText.text = "";
		foreach (char letter in sentence.ToCharArray())
		{
			dialogueText.text += letter;
			yield return null;
		}
	}

	/*void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
	}*/

}
