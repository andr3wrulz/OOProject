using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSheetScreen : MonoBehaviour {

	public Button characterSheetTabButton;
	public Button inventoryTabButton;

	public GameObject characterSheetParent;
	public GameObject inventoryParent;

	public Sprite swordIcon;
	public Sprite daggerIcon;
	public Sprite bowIcon;
	public Sprite wandIcon;
	public Sprite bagIcon;

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
	GameObject equippedWeaponSlot;

	public void Start () {
		// Set character sheet at current tab
		inventoryParent.SetActive (false);

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

		// Find inventory slot objects
		findInventorySlots();
		// Update inventory slots to match inventory
		updateInventorySlots();
	}

	void findInventorySlots() {
		inventorySlots = new GameObject[10];

		for (int i = 0; i < GameConfig.backpackSlots; i++) {
			inventorySlots [i] = inventoryParent.transform.Find ("Slot " + (i+1)).gameObject;

			// Setup listeners
			int index = i + 1;// We must create a new reference to pass to the listener
			inventorySlots[i].transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(() => sellButtonClicked (index));
			inventorySlots [i].transform.Find ("EquipButton").GetComponent<Button> ().onClick.AddListener (() => equipButtonClicked (index));
		}

		equippedWeaponSlot = inventoryParent.transform.Find ("Current Weapon").gameObject;
	}

	void updateInventorySlots() {
		// Update equipped weapon
		Weapon equipped = GameControl.control.playerData.inventory.getWeapon();
		if (equipped != null) {
			equippedWeaponSlot.transform.Find ("Name").GetComponent<Text> ().text = equipped.getName ();
			equippedWeaponSlot.transform.Find ("Value").GetComponent<Text> ().text = "Value: " + equipped.getValue () + " gold";
			equippedWeaponSlot.transform.Find ("Details1").GetComponent<Text> ().text = "Damage: " + equipped.getMinDamage ().ToString("n0") + "-" + equipped.getMaxDamage ().ToString("n0");
			equippedWeaponSlot.transform.Find ("Details2").GetComponent<Text> ().text = "Crit Multiplier: " + equipped.getCritMultiplier ().ToString("n2") + "x";
			equippedWeaponSlot.transform.Find ("Details3").GetComponent<Text> ().text = "Range: " + equipped.getRange () + " squares";
		} else {
			equippedWeaponSlot.transform.Find ("Name").GetComponent<Text> ().text = "Empty";
			equippedWeaponSlot.transform.Find ("Value").GetComponent<Text> ().text = "Value: 0 gold";
			equippedWeaponSlot.transform.Find ("Details1").GetComponent<Text> ().text = "";
			equippedWeaponSlot.transform.Find ("Details2").GetComponent<Text> ().text = "";
			equippedWeaponSlot.transform.Find ("Details3").GetComponent<Text> ().text = "";
		}
		equippedWeaponSlot.transform.Find ("Icon").GetComponent<SpriteRenderer> ().sprite = getIconForItem(equipped);

		// Update inventory
		for (int i = 0; i < GameConfig.backpackSlots; i++) {
			Item item = GameControl.control.playerData.inventory.getItemFromBackpackAtIndex (i);
			// If there is an item in that slot
			if (item != null) {
				inventorySlots [i].transform.Find ("Name").GetComponent<Text> ().text = item.getName ();
				//Debug.LogError ("Item updated: " + inventorySlots [i].ToString());

				// Determine type to fill out details and icon
				if (item is Weapon) {
					// Cast it
					Weapon wep = item as Weapon;
					inventorySlots [i].transform.Find ("Details1").GetComponent<Text> ().text = "Damage: " + wep.getMinDamage ().ToString("n0") + "-" + wep.getMaxDamage ().ToString("n0");
					inventorySlots [i].transform.Find ("Details2").GetComponent<Text> ().text = "Crit Multiplier: " + wep.getCritMultiplier ().ToString("n2") + "x";
					inventorySlots [i].transform.Find ("Details3").GetComponent<Text> ().text = "Range: " + wep.getRange () + " squares";
					inventorySlots [i].transform.Find ("Value").GetComponent<Text> ().text = "Value: " + wep.getValue () + " gold";// Use weapon value formula
				} else {// Is not a weapon
					inventorySlots [i].transform.Find ("Details1").GetComponent<Text> ().text = "";
					inventorySlots [i].transform.Find ("Details2").GetComponent<Text> ().text = "";
					inventorySlots [i].transform.Find ("Details3").GetComponent<Text> ().text = "";
					inventorySlots [i].transform.Find ("Value").GetComponent<Text> ().text = "Value: " + item.getValue () + " gold";// Use default value formula
				}
			} else {// if that slot is empty
				inventorySlots [i].transform.Find ("Name").GetComponent<Text> ().text = "Empty";
				inventorySlots [i].transform.Find ("Value").GetComponent<Text> ().text = "Value: 0 gold";
				inventorySlots [i].transform.Find ("Details1").GetComponent<Text> ().text = "";
				inventorySlots [i].transform.Find ("Details2").GetComponent<Text> ().text = "";
				inventorySlots [i].transform.Find ("Details3").GetComponent<Text> ().text = "";
			}
			inventorySlots [i].transform.Find ("Icon").GetComponent<SpriteRenderer> ().sprite = getIconForItem(item);
		}
	}

	Sprite getIconForItem (Item i) {
		// If the slot is empty
		if (i == null)
			return bagIcon;

		// If the item is a weapon
		if (i is Weapon) {
			switch ((i as Weapon).getWeaponType ()) {
				case Weapon.WeaponType.Sword:
					return swordIcon;
				case Weapon.WeaponType.Dagger:
					return daggerIcon;
				case Weapon.WeaponType.Bow:
					return bowIcon;
				case Weapon.WeaponType.Wand:
					return wandIcon;
			}
		}

		return bagIcon;
	}

	void getInitialStats() {
		// Only if player stats are initialized
		if (GameControl.control.playerData.stats.Length != 0) {
			pointsRemaining = GameControl.control.playerData.unspentPoints;
			strength = GameControl.control.playerData.stats [(int)GameControl.playerStats.Strength];
			dexterity = GameControl.control.playerData.stats [(int)GameControl.playerStats.Dexterity];
			constitution = GameControl.control.playerData.stats [(int)GameControl.playerStats.Constitution];
			intelligence = GameControl.control.playerData.stats [(int)GameControl.playerStats.Intelligence];
			charisma = GameControl.control.playerData.stats [(int)GameControl.playerStats.Charisma];
			luck = GameControl.control.playerData.stats [(int)GameControl.playerStats.Luck];
		}
	}

	public void sellButtonClicked (int buttonId) {
		Debug.Log ("Sell button " + buttonId + " clicked!");

		Item item = GameControl.control.playerData.inventory.removeItemFromBackpackAtIndex (buttonId - 1);

		// Check if slot wasn't empty
		if (item == null) {
			Debug.Log ("[DEBUG] Can't sell an item from an empty slot!");
			return;
		}

		// Add the gold to the players inventory
		GameControl.control.playerData.addGold(item.getValue());

		// We already removed the item from the inventory so now we just need to update the labels
		updateInventorySlots();
	}

	public void equipButtonClicked (int buttonId) {
		Debug.Log ("Equip button " + buttonId + " clicked!");

		// Take item from inventory
		Weapon temp = GameControl.control.playerData.inventory.removeItemFromBackpackAtIndex (buttonId - 1) as Weapon;

		// Make sure that slot wasn't empty
		if (temp == null) {
			Debug.Log ("[DEBUG] Can't equip an item from an empty slot!");
			return;
		}

		// Unequip current weapon and put it in the inventory
		GameControl.control.playerData.inventory.addItemToBackpackAtIndex(GameControl.control.playerData.inventory.removeWeapon (), buttonId - 1);
		// Equip new weapon
		GameControl.control.playerData.inventory.setWeapon(temp);

		// Update inventory slot labels
		updateInventorySlots();
	}

	public void clickInventoryTab () {
		// Swap button states
		characterSheetTabButton.interactable = true;
		inventoryTabButton.interactable = false;

		// Swap parent object states
		characterSheetParent.SetActive(false);
		inventoryParent.SetActive (true);
	}

	public void clickCharacterTab () {
		// Swap button states
		characterSheetTabButton.interactable = false;
		inventoryTabButton.interactable = true;

		// Swap parent object states
		characterSheetParent.SetActive(true);
		inventoryParent.SetActive (false);
	}

	void updateCharacterSheetLabels() {
		// Only if player stats are initialized
		if (GameControl.control.playerData.stats.Length != 0) {
			unspentPointsLabel.text = "Unspent Points: " + pointsRemaining;
			strLabel.text = "Strength:\t\t\t" + strength + ((strength != GameControl.control.playerData.stats [(int)GameControl.playerStats.Strength]) ? "*" : "");
			dexLabel.text = "Dexterity:\t\t\t" + dexterity + ((dexterity != GameControl.control.playerData.stats [(int)GameControl.playerStats.Dexterity]) ? "*" : "");
			conLabel.text = "Constitution:\t" + constitution + ((constitution != GameControl.control.playerData.stats [(int)GameControl.playerStats.Constitution]) ? "*" : "");
			intLabel.text = "Intelligence:\t\t" + intelligence + ((intelligence != GameControl.control.playerData.stats [(int)GameControl.playerStats.Intelligence]) ? "*" : "");
			chaLabel.text = "Charisma:\t\t" + charisma + ((charisma != GameControl.control.playerData.stats [(int)GameControl.playerStats.Charisma]) ? "*" : "");
			lckLabel.text = "Luck:\t\t\t\t" + luck + ((luck != GameControl.control.playerData.stats [(int)GameControl.playerStats.Luck]) ? "*" : "");
		}
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
