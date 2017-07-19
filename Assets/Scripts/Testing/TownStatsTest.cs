using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TownStatsTest{

	// store player name and player's experience, level. 
	public string headerText; 
	public string playerInfoText; 

	[Test]
	public void testPlayerDataRendering () {

		PlayerData p = new PlayerData();

		// set the variable
		p.name = "Khoi"; 
		p.experience = 100; 
		p.level = 10; 

		// set headerText and playerInfoText. 
		headerText = "Welcome to Town, Khoi!";
		playerInfoText = "Experience : " + p.experience + " Level : " + p.level; 

		// see if the playerData's info stored correctly. 
		// this info will be passed to the headerText and playerInfoTest. 

		Assert.AreEqual ("Welcome to Town, Khoi!", headerText);
		Assert.AreEqual ("Experience : 100 Level : 10", playerInfoText);

		// Test the UI for displaying the player info. 
		// Use string instead. They can be replaced by UI.Text.
		// This should be done by creating new UI, but using string is sufficient. 

	}
}