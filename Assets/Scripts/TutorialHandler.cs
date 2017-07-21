using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialHandler : MonoBehaviour {

	public GameObject messageBox;
	// Store references so we don't have to find them each time
	Text titleText;
	Text line1Text;
	Text line2Text;
	Text line3Text;
	Text line4Text;
	Text line5Text;

	public int tutorialState = 0;// Keeps track of player's progress in the tutorial; see handleTutorialStatus for notes
	int stateAfterButtonClick;// tutorial state will switch to this after player clicks okay

	// Use this for initialization
	void Start () {
		findMessageBoxComponents ();
	}

	void findMessageBoxComponents() {
		titleText = messageBox.transform.Find ("TitleText").GetComponent<Text> ();
		line1Text = messageBox.transform.Find ("Line1").GetComponent<Text> ();
		line2Text = messageBox.transform.Find ("Line2").GetComponent<Text> ();
		line3Text = messageBox.transform.Find ("Line3").GetComponent<Text> ();
		line4Text = messageBox.transform.Find ("Line4").GetComponent<Text> ();
		line5Text = messageBox.transform.Find ("Line5").GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		handleTutorialStatus ();
	}

	void handleTutorialStatus () {
		// Each major stage in the tutorial should be a multiple of 100 (ie 100, 200, 300, ...)
		// Even states are used to trigger message boxes or actions and odd states are waiting for the player
		// To exit to the next stage, just set the state to 10000

		// Example flow:
		//	* 100 - Hello and welcome to Dungeon Champion
		//	* 101 - Waiting for player to click from 100
		//	* 102 - This tutorial will teach you about the game
		//	* 103 - Waiting for player to click from 102

		switch (tutorialState) {
		case 0:
			tutorialState = 101;// Wait state
			showMessageBox ("Welcome!", "Thanks for taking a chance", "to play our game, Dungeon Champion!", 102);
			break;
		case 102:
			tutorialState = 103;
			showMessageBox ("Tutorial", "This tutorial will teach you", "about how to play and hopefully", "give you some idea of what's", "going on in our game world.", 104);
			break;
		case 104:
			tutorialState = 105;
			showMessageBox ("Tutorial", "Dungeon Champion is set in", "a world of darkness, plagued by", "monsters of all kinds.", 106);
			break;

		case 10000:// Exit state
			SceneManager.LoadScene(3);// Load town menu
			break;
		default:
			// Do nothing, waiting on player
			break;
		}
	}

	public void clickOkayButton() {
		messageBox.SetActive (false);
		tutorialState = stateAfterButtonClick;
	}

	void showMessageBox (string title, string line1, string line2, string line3, string line4, string line5, int newState) {
		// Store new state
		stateAfterButtonClick = newState;

		// Update message box text
		titleText.text = title;
		line1Text.text = line1;
		line2Text.text = line2;
		line3Text.text = line3;
		line4Text.text = line4;
		line5Text.text = line5;

		// Show message box
		messageBox.SetActive(true);
	}


	// ---------- Show Message Box Overloads -----------
	void showMessageBox (string title, string line1, string line2, string line3, string line4, int newState) {
		showMessageBox (title, line1, line2, line3, line4, "", newState);
	}

	void showMessageBox (string title, string line1, string line2, string line3, int newState) {
		showMessageBox (title, line1, line2, line3, "", "", newState);
	}

	void showMessageBox (string title, string line1, string line2, int newState) {
		showMessageBox (title, line1, line2, "", "", "", newState);
	}

	void showMessageBox (string title, string line1, int newState) {
		showMessageBox (title, line1, "", "", "", "", newState);
	}
	// ---------- End Show Message Box Overloads -----------
}
