using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour {

	public static GameControl control;// psuedo singleton

	public int saveSlot = 4;// Initialize to debug slot (we only intend to store 3 slots)
	public PlayerData playerData;// Used to store and modifier player stats

	// Run when the level initally loads (ie. before Start())
	void Awake () {
		if (control == null) {// If we don't have a saved GameControl object, save this one
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {// If we have one, but this isn't it, destroy this one
			Destroy (gameObject);
		}
	}
		
	// Throw the serialized PlayerData class into a file based on the save slot
	public void savePlayer () {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create(Application.persistentDataPath + "/playerSave" + saveSlot + ".banana");
		bf.Serialize (file, playerData);
		file.Close ();
	}

	// Pull PlayerData from the save slot based file
	public void loadPlayer () {
		if (File.Exists(Application.persistentDataPath + "/playerSave" + saveSlot + ".dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerSave" + saveSlot + ".banana", FileMode.Open);
			playerData = (PlayerData)bf.Deserialize(file);
			file.Close();
		}
	}

}

// Player Data Container
[Serializable]
public class PlayerData {
	public string name;
	public float health;
	public float experience;
	public int level;
	public int lastMilestone;
	public int gold;
	public Inventory inventory;
}