using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class LoadingManager : MonoBehaviour {

	public Text currentSlotText;
	public Text slot1Text;
	public Text slot2Text;
	public Text slot3Text;
	bool slot1Valid = false;
	bool slot2Valid = false;
	bool slot3Valid = false;

	int currentSelection = 1;

	void Start () {
		// Initialize our slot info and see if we found valid saves
		slot1Valid = setSlotText (1, slot1Text);
		slot2Valid = setSlotText (2, slot2Text);
		slot3Valid = setSlotText (3, slot3Text);
	}

	bool setSlotText(int slot, Text label) {
		if (File.Exists (Application.persistentDataPath + "/playerSave" + slot + ".banana")) {// If we find a save
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = null;
			// Try to open file
			try {
				file = File.Open (Application.persistentDataPath + "/playerSave" + slot + ".banana", FileMode.Open);
			} catch (Exception e) {
				Debug.LogError ("[LoadingManager] Failed to open save file in slot: " + slot);
				Debug.LogError ("[LoadingManager] " + e.ToString());
				label.text = "Couldn't open save file!";
				return false;
			}

			// Try and parse file
			try {
				PlayerData playerData = (PlayerData)bf.Deserialize (file);
				label.text = "Name:\t\t" + playerData.name + "\nLevel:\t\t" + playerData.level + "\nDungeon Progress:\t" + playerData.lastMilestone;
			} catch (Exception e) {
				Debug.LogError ("[LoadingManager] Failed to parse save file in slot: " + slot);
				Debug.LogError ("[LoadingManager] " + e.ToString());
				label.text = "Couldn't open save file!";
				return false;
			}

			if (file != null)
				file.Close ();

			return true;
		} else {// if we don't
			label.text = "Save file not found!\nSelect this slot to start a new file.";
			Debug.Log ("[LoadingManager] Couldn't find: " + Application.persistentDataPath + "/playerSave" + slot + ".banana");
		}
		return false;
	}

	public void slot1Pressed () {
		currentSelection = 1;
		updateCurrentSlotText ();
	}

	public void slot2Pressed () {
		currentSelection = 2;
		updateCurrentSlotText ();
	}

	public void slot3Pressed () {
		currentSelection = 3;
		updateCurrentSlotText ();
	}

	void updateCurrentSlotText () {
		currentSlotText.text = "Current Selection: Slot " + currentSelection;
	}

	public void ContinuePressed() {
		// No matter what, set current selection in GameControl
		GameControl.control.saveSlot = currentSelection;

		// No we see if we can load the selected slot
		if ((currentSelection == 1 && slot1Valid) ||
			(currentSelection == 2 && slot2Valid) ||
			(currentSelection == 3 && slot3Valid)) {
			GameControl.control.LoadPlayer ();
			SceneManager.LoadScene ("TownMenu");
		} else {// No save file found in slot, start new game
			SceneManager.LoadScene ("NewGame");
		}
	}

	public void DeletePressed() {
		// No matter what, set current selection in GameControl
		GameControl.control.saveSlot = currentSelection;

		// No we see if we can load the selected slot
		if ((currentSelection == 1 && slot1Valid) ||
		    (currentSelection == 2 && slot2Valid) ||
		    (currentSelection == 3 && slot3Valid)) {
			GameControl.control.DeletePlayer ();
		}
		slot1Valid = setSlotText (1, slot1Text);
		slot2Valid = setSlotText (2, slot2Text);
		slot3Valid = setSlotText (3, slot3Text);
	}
}
