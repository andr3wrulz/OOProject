using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour {

	public bool debugLoadFirstPlayerSlot = false;// Manually set to true, this will load the first player slot to get stats and inventory for testing

	public enum playerStats {Strength = 0, Dexterity = 1, Constitution = 2, Intelligence = 3, Charisma = 4, Luck = 5};

	public static GameControl control;// psuedo singleton

	public int saveSlot = 4;// Initialize to debug slot (we only intend to store 3 slots)
	public PlayerData playerData;// Used to store and modifier player stats

	public bool inDungeon = false;// This lets us control the music that is played

	public bool stop;

	// These values are stored to enable us to dynamically create items
	public Sprite swordIcon;
	public Sprite daggerIcon;
	public Sprite bowIcon;
	public Sprite wandIcon;
	public Animation swordAnimation;
	public Animation daggerAnimation;
	public Animation bowAnimation;
	public Animation wandAnimation;

	public IntVector2 startRoom;

	// Turn handling
	ArrayList turnQueue;
	public GameObject player;
	public float timeSinceLastTurn = 0;

	// Run when the level initally loads (ie. before Start())
	void Awake () {
		if (control == null) {// If we don't have a saved GameControl object, save this one
			DontDestroyOnLoad (gameObject);
			control = this;
		} else if (control != this) {// If we have one, but this isn't it, destroy this one
			Destroy (gameObject);
		}

		// Initialize turn queue
		turnQueue = new ArrayList();

		if (debugLoadFirstPlayerSlot) {
			saveSlot = 1;
			LoadPlayer ();
		}
			
	}

	// Entities will check if it is their turn via this function
	public Boolean isTurn (String entity) {
		return turnQueue[0].Equals(entity) && timeSinceLastTurn > 1f;// It's that entity's turn and it has been long enough
	}

	// When an entity does something, this function will remove them from the front of the queue and place them at the end
	public void takeTurn () {
		if(!stop)
		{
			turnQueue.Add (turnQueue[0]);
			turnQueue.RemoveAt (0);
			timeSinceLastTurn = 0;// Reset timer
		}
	}

	public void takeTurnWithoutDelay () {
		if (!stop) 
		{
			turnQueue.Add (turnQueue [0]);
			turnQueue.RemoveAt (0);
		}
	}

	public void addToTurnQueue (String entity) {
		turnQueue.Add(entity);
	}

	void Update () {
		updateTurnQueue ();
	}

	public void removeFromTurnQueue (String name) {
		turnQueue.Remove (name);
	}

	void updateTurnQueue () {
		timeSinceLastTurn += Time.deltaTime;
		if (timeSinceLastTurn < 1f)// Wait a second between turns
			return;

		// If we have something in the queue
		if (turnQueue != null && turnQueue.Count > 0) {
			// If enemy is at front of queue
			if (!turnQueue[0].Equals("Player")) {
				// If we don't have a player
				if (player == null) {
					// We need to wait until we have a player
					Debug.Log("Waiting for player");
					return;
				}
				// Let enemy take care of its own turn
				Debug.Log("Waiting on " + turnQueue[0] + " to take its turn.");
				return;
			}
			// Else, this means player is at front of queue and should take care of its own turn
		}
	}

	// Throw the serialized PlayerData class into a file based on the save slot
	public void savePlayer () {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file;

		// Try to open file and serialize playerData
		try {
			file = File.Create(Application.persistentDataPath + "/playerSave" + saveSlot + ".banana");
			bf.Serialize (file, playerData);
			file.Close ();
		} catch (Exception e) {
			Debug.LogError ("[GameControl] Failed to save file in slot: " + saveSlot);
			Debug.LogError ("[GameControl] " + e);
		}

		Debug.Log ("[GameControl] Saved file: " + Application.persistentDataPath + "/playerSave" + saveSlot + ".banana");
	}

	// Pull PlayerData from the save slot based file
	public void LoadPlayer () {
		if (File.Exists(Application.persistentDataPath + "/playerSave" + saveSlot + ".banana")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = null;

			try {
				file = File.Open(Application.persistentDataPath + "/playerSave" + saveSlot + ".banana", FileMode.Open);
			} catch (Exception e) {
				Debug.LogError ("[GameControl] Failed to open file in slot: " + saveSlot + " while trying to load!");
				Debug.LogError ("[GameControl] " + e);
			}

			try {
				playerData = (PlayerData)bf.Deserialize(file);
			} catch (Exception e) {
				Debug.LogError ("[GameControl] Failed to load data from save file in slot: " + saveSlot);
				Debug.LogError ("[GameControl] " + e);
			}

			if (file != null)
				file.Close();

			Debug.Log ("[GameControl] Loaded file: " + Application.persistentDataPath + "/playerSave" + saveSlot + ".banana");
		}
	}

	// Pull PlayerData from the save slot based file
	public void DeletePlayer () {
		if (File.Exists(Application.persistentDataPath + "/playerSave" + saveSlot + ".banana")) {
			FileStream file = null;

			try {
				File.Delete(Application.persistentDataPath + "/playerSave" + saveSlot + ".banana");
			} catch (Exception e) {
				Debug.LogError ("[GameControl] Failed to delete file in slot: " + saveSlot + "!");
				Debug.LogError ("[GameControl] " + e);
			}

			if (file != null)
				file.Close();

			Debug.Log ("[GameControl] Deleted file: " + Application.persistentDataPath + "/playerSave" + saveSlot + ".banana");
		}
	}

	// Initialize player file
	public void createNewPlayer (string name, int [] stats) {
		playerData.name = name;
		playerData.stats = stats;
		playerData.health = playerData.getMaxHealth ();
		playerData.experience = 0;
		playerData.level = 1;
		playerData.floor = 1;
		playerData.lastMilestone = 1;
		playerData.gold = 0;
		playerData.unspentPoints = 0;
		//playerData.inventory.setWeapon(new Weapon (Weapon.WeaponType.Sword, 1, 1, 2, 1, 
		for(int i=0; i<GameConfig.backpackSlots; i++) // Start with empty backpack
			playerData.inventory.removeItemFromBackpackAtIndex(i);

		// After we populate the data, go ahead and save it
		savePlayer();
	}

	public void rerollShopInventory () {
		// Make new array to hold items
		control.playerData.shopInventory = new Item[5];

		// Generate each item
		for (int i = 0; i < 5; i++) {
			control.playerData.shopInventory [i] = new Weapon (Weapon.WeaponType.Sword, "Sword of Sir Alpha Testings", control.playerData.floor);
			Debug.Log ("Generated Weapon: " + control.playerData.shopInventory [i].ToString ());
		}
	}
}

// Player Data Container
[Serializable]
public class PlayerData {
	public string name;
	public float health;
	public int[] stats;
	public int experience;
	public int level;
	public int lastMilestone;
	public int gold;
	public int floor;
	public Inventory inventory;
	public int unspentPoints;
	public Item[] shopInventory;

	public float getItemFind () {
		return 1;
	}

    public void resetHealth (int damage)
    {
		if (this.health - damage <= 0) 
		{
			this.health = 0;
		}
		else
        	this.health -= damage;
    }

	public int getMaxHealth () {
		return 100 + stats [(int) GameControl.playerStats.Constitution] * GameConfig.hitpointsPerConstitutionPoint;
	}
		
	public void addGold (int amount) {
		gold += amount;
	}

	public int refreshLevel () {
		level = getLevelForExperience (experience);
		return level;
	}

	public void addExperience (int amount) {
		experience += amount;
	}

	public int getExperienceForNextLevel () {
		int baseXp = 50;
		int factor = 2;
		return (int) (baseXp * Mathf.Pow(level, factor));// We don't add 1 because our formula assumes 0 experience at level 1, would normally be base*(level - 1)^factor
	}

	public int getExperienceToNextLevel () {
		int exp = getExperienceForNextLevel () - experience;// Next - current
		if (exp < 0)
			exp = 0;
		return exp;
	}

	public int getLevelForExperience (int experience) {// May be off by one
		int lvl = (int) Math.Sqrt(experience / 50) + 1;

		if (lvl > GameConfig.maxLevel)
			lvl = GameConfig.maxLevel;

		return lvl;
	}

}