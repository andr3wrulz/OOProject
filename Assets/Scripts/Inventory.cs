using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Inventory {
	Item [] backpack;
	Item [] storage;
	Armor [] armor;
	Weapon weapon;

	public Inventory () {
		backpack = new Item[GameConfig.backpackSlots];
		storage = new Item[GameConfig.storageSlots];
		armor = new Armor[System.Enum.GetValues (typeof(Armor.ArmorType)).Length];
	}

	// ------------------ Backpack Functions ------------------
	public Item getItemFromBackpackAtIndex(int index) {
		return backpack [index];
	}

	public Item removeItemFromBackpackAtIndex(int index) {
		Item removedItem = backpack [index];
		backpack [index] = null;
		return removedItem;
	}

	public void addItemToBackpackAtIndex(Item itemToAdd, int index) {
		backpack [index] = itemToAdd;
	}

	public void swapItemsInBackpackAtIndexes(int one, int two) {
		Item temp = backpack [one];
		backpack [one] = backpack [two];
		backpack[two] = temp;
	}

	// ------------------ Storage Functions ------------------
	public Item getItemFromStorageAtIndex(int index) {
		return storage [index];
	}

	public Item removeItemFromStorageAtIndex(int index) {
		Item removedItem = storage [index];
		storage [index] = null;
		return removedItem;
	}

	public void addItemToStorageAtIndex(Item itemToAdd, int index) {
		storage [index] = itemToAdd;
	}

	public void swapItemsInStorageAtIndexes(int one, int two) {
		Item temp = storage [one];
		storage [one] = storage [two];
		storage [two] = temp;
	}

	// ------------------ Armor Functions ------------------
	public Armor removeArmorOfType(Armor.ArmorType armorType) {
		return setArmorOfType (null, armorType);
	}

	public Armor setArmorOfType(Armor newArmor, Armor.ArmorType armorType) {
		Armor removedArmor = armor [(int)armorType];
		armor [(int)armorType] = newArmor;
		return removedArmor;
	}

	// ------------------ Weapon Functions ------------------
	public Weapon removeWeapon() {
		return setWeapon (null);
	}

	public Weapon setWeapon(Weapon newWeapon) {
		Weapon removedWeapon = weapon;
		weapon = newWeapon;
		return removedWeapon;
	}
}
