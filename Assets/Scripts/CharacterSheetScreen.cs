using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSheetScreen : MonoBehaviour {

	enum MenuType {Character, Inventory};
	MenuType currentScreen;

	public Button characterSheetTabButton;
	public Button inventoryTabButton;

	public GameObject characterSheetParent;
	GameObject inventoryParent;
	public GameObject canvas;

	public GameObject inventorySlotPrefab;
	public GameObject swordIcon;
	public GameObject daggerIcon;
	public GameObject bowIcon;
	public GameObject wandIcon;

	public Text unspentPointsLabel;
	public Text levelUpMessage;
	public Text strLabel;
	public Text dexLabel;
	public Text conLabel;
	public Text intLabel;
	public Text chaLabel;
	public Text lckLabel;

	int pointsRemaining;
	int strength;
	int dexterity;
	int constitution;
	int intelligence;
	int charisma;
	int luck;

	GameObject [] inventorySlots;// Holds ui objects

	public void Start () {
		// Create our parent object
		inventoryParent = new GameObject ("InventoryTabParent");
		inventoryParent.transform.SetParent (canvas.transform);

		// Set character sheet at current tab
		currentScreen = MenuType.Character;
		inventoryParent.SetActive (false);

		// Instantiate ui elements
		inventorySlots = new GameObject[GameConfig.backpackSlots];
		createInventorySlots();

		// If the player has leveled up, show the label and add the points
		if (GameControl.control.playerData.getExperienceToNextLevel () <= 0) {
			// Show label
			levelUpMessage.enabled = true;

			// Figure out what level they are (could have leveled multiple times before coming back to town)
			int newLevel = GameControl.control.playerData.getLevelForExperience (GameControl.control.playerData.experience);
			// Add points for them to spend
			GameControl.control.playerData.unspentPoints += GameConfig.pointsPerLevelUp * (newLevel - GameControl.control.playerData.level);
			// Update their stored level so they don't get anymore points
			GameControl.control.playerData.level = newLevel;
		} else {
			levelUpMessage.enabled = false;
		}

		// Pull current stats as starting point
		getInitialStats();
		updateCharacterSheetLabels ();
	}

	void getInitialStats() {
		pointsRemaining = GameControl.control.playerData.unspentPoints;
		strength = GameControl.control.playerData.stats [(int) GameControl.playerStats.Strength];
		dexterity = GameControl.control.playerData.stats [(int) GameControl.playerStats.Dexterity];
		constitution = GameControl.control.playerData.stats [(int) GameControl.playerStats.Constitution];
		intelligence = GameControl.control.playerData.stats [(int) GameControl.playerStats.Intelligence];
		charisma = GameControl.control.playerData.stats [(int) GameControl.playerStats.Charisma];
		luck = GameControl.control.playerData.stats [(int) GameControl.playerStats.Luck];
	}

	public void createInventorySlots () {
		// Find correct y offsets
		int centerHeight = Screen.height / 2;
		int[] heights = { centerHeight + 220, centerHeight + 110, centerHeight, centerHeight - 110, centerHeight - 220 };

		for (int i = 0; i < 5; i++)
			inventorySlots [i] = createInventorySlot (100, heights[i]);
		for (int i = 5; i < 10; i++)
			inventorySlots [i] = createInventorySlot (420, heights[i-5]);

	}

	void setupInventoryButtonListeners () {
		// For each button
		for (int i = 0; i < 10; i++) {
			// We can't just pass i to each function becuase c# wont store the state of i when we set the listener
			// So we need to create a new instance of each i to pass to the functions
			int numberReference = i;

			Button sellButton = inventorySlots [i].transform.Find("SellButton").GetComponent<Button>();
			sellButton.onClick.AddListener (() => sellButtonClicked (numberReference));

			Button equipButton = inventorySlots [i].transform.Find("EquipButton").GetComponent<Button>();
			equipButton.onClick.AddListener (() => equipButtonClicked (numberReference));
		}
	}

	void sellButtonClicked (int buttonId) {
		Debug.Log ("Sell button " + buttonId + " clicked!");
	}

	void equipButtonClicked (int buttonId) {
		Debug.Log ("Equip button " + buttonId + " clicked!");
	}

	GameObject createInventorySlot (float x, float y) {
		GameObject slot = Instantiate (inventorySlotPrefab);
		slot.transform.SetParent (inventoryParent.transform);
		slot.transform.SetPositionAndRotation (new Vector3 (x, y, 1), Quaternion.identity);
		return slot;
	}

	public void clickInventoryTab () {
		// Set screen variable
		currentScreen = MenuType.Inventory;

		// Swap button states
		characterSheetTabButton.interactable = true;
		inventoryTabButton.interactable = false;

		// Swap parent object states
		characterSheetParent.SetActive(false);
		inventoryParent.SetActive (true);
	}

	public void clickCharacterTab () {
		// Set screen variable
		currentScreen = MenuType.Character;

		// Swap button states
		characterSheetTabButton.interactable = false;
		inventoryTabButton.interactable = true;

		// Swap parent object states
		characterSheetParent.SetActive(true);
		inventoryParent.SetActive (false);
	}

	void updateCharacterSheetLabels() {
		unspentPointsLabel.text = "Unspent Points: " + pointsRemaining;
		strLabel.text = "Strength:\t\t\t" + strength + ((strength != GameControl.control.playerData.stats[(int)GameControl.playerStats.Strength]) ? "*" : "");
		dexLabel.text = "Dexterity:\t\t\t" + dexterity + ((dexterity != GameControl.control.playerData.stats[(int)GameControl.playerStats.Dexterity]) ? "*" : "");
		conLabel.text = "Constitution:\t" + constitution + ((constitution != GameControl.control.playerData.stats[(int)GameControl.playerStats.Constitution]) ? "*" : "");
		intLabel.text = "Intelligence:\t\t" + intelligence + ((intelligence != GameControl.control.playerData.stats[(int)GameControl.playerStats.Intelligence]) ? "*" : "");
		chaLabel.text = "Charisma:\t\t" + charisma + ((charisma != GameControl.control.playerData.stats[(int)GameControl.playerStats.Charisma]) ? "*" : "");
		lckLabel.text = "Luck:\t\t\t\t" + luck + ((luck != GameControl.control.playerData.stats[(int)GameControl.playerStats.Luck]) ? "*" : "");
	}

	// ------------------- Character sheet button handlers -------------------

	public void saveChangesButton() {
		GameControl.control.playerData.unspentPoints = pointsRemaining;
		GameControl.control.playerData.stats [(int)GameControl.playerStats.Strength] = strength;
		GameControl.control.playerData.stats [(int)GameControl.playerStats.Dexterity] = dexterity;
		GameControl.control.playerData.stats [(int)GameControl.playerStats.Constitution] = constitution;
		GameControl.control.playerData.stats [(int)GameControl.playerStats.Intelligence] = intelligence;
		GameControl.control.playerData.stats [(int)GameControl.playerStats.Charisma] = charisma;
		GameControl.control.playerData.stats [(int)GameControl.playerStats.Luck] = luck;
	}

	public void strUp() {
		// If the player has unspent points
		if (pointsRemaining > 0) {
			strength++;
			pointsRemaining--;
		}

		updateCharacterSheetLabels ();
	}

	public void strDown() {
		// If working stat is greater than saved
		if (strength > GameControl.control.playerData.stats [(int)GameControl.playerStats.Strength]) {
			strength--;
			pointsRemaining++;
		}

		updateCharacterSheetLabels ();
	}

	public void dexUp() {
		// If the player has unspent points
		if (pointsRemaining > 0) {
			dexterity += 1;
			pointsRemaining -= 1;
		}

		updateCharacterSheetLabels ();
	}

	public void dexDown() {
		// If working stat is greater than saved
		if (dexterity > GameControl.control.playerData.stats [(int)GameControl.playerStats.Dexterity]) {
			dexterity--;
			pointsRemaining++;
		}

		updateCharacterSheetLabels ();
	}

	public void conUp() {
		// If the player has unspent points
		if (pointsRemaining > 0) {
			constitution += 1;
			pointsRemaining -= 1;
		}

		updateCharacterSheetLabels ();
	}

	public void conDown() {
		// If working stat is greater than saved
		if (constitution > GameControl.control.playerData.stats [(int)GameControl.playerStats.Constitution]) {
			constitution--;
			pointsRemaining++;
		}

		updateCharacterSheetLabels ();
	}

	public void intUp() {
		// If the player has unspent points
		if (pointsRemaining > 0) {
			intelligence += 1;
			pointsRemaining -= 1;
		}

		updateCharacterSheetLabels ();
	}

	public void intDown() {
		// If working stat is greater than saved
		if (intelligence > GameControl.control.playerData.stats [(int)GameControl.playerStats.Intelligence]) {
			intelligence--;
			pointsRemaining++;
		}

		updateCharacterSheetLabels ();
	}

	public void chaUp() {
		// If the player has unspent points
		if (pointsRemaining > 0) {
			charisma += 1;
			pointsRemaining -= 1;
		}

		updateCharacterSheetLabels ();
	}

	public void chaDown() {
		// If working stat is greater than saved
		if (charisma > GameControl.control.playerData.stats [(int)GameControl.playerStats.Charisma]) {
			charisma--;
			pointsRemaining++;
		}

		updateCharacterSheetLabels ();
	}

	public void lckUp() {
		// If the player has unspent points
		if (pointsRemaining > 0) {
			luck += 1;
			pointsRemaining -= 1;
		}

		updateCharacterSheetLabels ();
	}

	public void lckDown() {
		// If working stat is greater than saved
		if (luck > GameControl.control.playerData.stats [(int)GameControl.playerStats.Luck]) {
			luck--;
			pointsRemaining++;
		}

		updateCharacterSheetLabels ();
	}
}
