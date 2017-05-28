using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownStats : MonoBehaviour {

	public Text headerText;
	public Text playerInfoText;
	public Text LevelUpText;

	void Start () {
		// Add player name to header
		headerText.text = "Welcome to Town, " + GameControl.control.playerData.name + "!";

		// Add level and experience info
		playerInfoText.text = "Level: " + GameControl.control.playerData.level + "\n" +
			"Next Level In: " + GameControl.control.playerData.getExperienceToNextLevel () + "xp (" +
			GameControl.control.playerData.getExperienceForNextLevel () + "xp)\n" +
			"Total Experience: " + GameControl.control.playerData.experience + "xp";

		// If player can level up, show the notification
		if (GameControl.control.playerData.getExperienceToNextLevel () <= 0)
			LevelUpText.enabled = true;
		else
			LevelUpText.enabled = false;
	}
}
