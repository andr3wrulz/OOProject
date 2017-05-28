using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NewGameButtonManager : MonoBehaviour {

	/*
	 * 		This script handles clicking the adjustment buttons and updating labels relevant to point spending on the NewGame screen.
	 * 		The general process is that either clickUpButton or clickDownButton is called when one of the arrows is pressed and the
	 * 		string that is passed in is converted to its value in the playerStats enum. The functions then pass that value and the
	 * 		change value to the adjustStat function. The adjustStat changes the relevant stat variable and refreshes the labels.
	 * 
	 * 
	 * 		This is not the optimal solution as far as efficiency (reading a string > converting to enum > switching base on the enum),
	 * 		I did it this way because it is extensible for later and this is not a performance critical system.
	 */

	public InputField nameField;

	public Text pointsLeftLabel;
	public Text strengthLabel;
	public Text dexterityLabel;
	public Text constitutionLabel;
	public Text intelligenceLabel;
	public Text charismaLabel;
	public Text luckLabel;

	public Text errorLabel;

	int pointsLeft = 10;
	int strength = 5;
	int dexterity = 5;
	int constitution = 5;
	int intelligence = 5;
	int charisma = 5;
	int luck = 5;

	public void clickUpButton (string stat) {
		adjustStat(getStatFromString(stat), 1);
	}

	public void clickDownButton (string stat) {
		adjustStat(getStatFromString(stat), -1);
	}

	GameControl.playerStats getStatFromString(string stat) {
		// Returns the right enum value based on the input string
		if (stat == "strength")
			return GameControl.playerStats.Strength;
		if (stat == "dexterity")
			return GameControl.playerStats.Dexterity;
		if (stat == "constitution")
			return GameControl.playerStats.Constitution;
		if (stat == "intelligence")
			return GameControl.playerStats.Intelligence;
		if (stat == "charisma")
			return GameControl.playerStats.Charisma;
		return GameControl.playerStats.Luck;
	}

	public void adjustStat (GameControl.playerStats stat, int change) {
		// Don't let the player add points if they have none
		if (pointsLeft - change < 0)
			return;

		// Adjust relevant variable but make sure it is valid
		switch (stat) {
		case GameControl.playerStats.Strength:
			if (strength + change <= 0)
				return;
			strength += change;
			break;
		case GameControl.playerStats.Dexterity:
			if (dexterity + change <= 0)
				return;
			dexterity += change;
			break;
		case GameControl.playerStats.Constitution:
			if (constitution + change <= 0)
				return;
			constitution += change;
			break;
		case GameControl.playerStats.Intelligence:
			if (intelligence + change <= 0)
				return;
			intelligence += change;
			break;
		case GameControl.playerStats.Charisma:
			if (charisma + change <= 0)
				return;
			charisma += change;
			break;
		case GameControl.playerStats.Luck:
			if (luck + change <= 0)
				return;
			luck += change;
			break;
		}
		// Subtract (or add) points from total
		pointsLeft -= change;

		// Refresh the text labels
		updateLabels ();
	}

	void updateLabels () {
		pointsLeftLabel.text = "Points Left to Distribute: " + pointsLeft;
		strengthLabel.text = "Strength:\t\t\t\t" + strength;
		dexterityLabel.text = "Dexterity:\t\t\t\t" + dexterity;
		constitutionLabel.text = "Constitution:\t\t" + constitution;
		intelligenceLabel.text = "Intelligence:\t\t\t" + intelligence;
		charismaLabel.text = "Charisma:\t\t\t" + charisma;
		luckLabel.text = "Luck:\t\t\t\t\t" + luck;
	}

	// Called when player presses the continue button
	public void continueClick() {
		if (nameField.text != "") {// Player entered a name
			if (pointsLeft != 0) {// They didn't use all their points
				// Show an error and don't do anything
				errorLabel.text = "Distribute your points before continuing!";
				return;
			}

			// Entered a name and used all their points - good to go
			// Create their character
			GameControl.control.createNewPlayer(nameField.text, new int[] {strength, dexterity, constitution, intelligence, charisma, luck});
			// Throw them into town, maybe later we can replace this with a tutorial
			SceneManager.LoadScene("TownMenu");
			return;

		} else {// Player didn't enter a name
			// Show an error and don't do anything
			errorLabel.text = "Please enter a name before continuing!";
			return;
		}
	}
}
