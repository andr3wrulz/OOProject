using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item {

	public enum ItemType {Weapon = 0, Armor = 1};

	ItemType itemType;
	string name;
	Sprite icon;
	int value;
	int tier;

	public Item(ItemType itemType, string name, Sprite icon, int value, int tier) {
		this.itemType = itemType;
		this.name = name;
		this.icon = icon;
		this.value = value;
		this.tier = tier;
	}

	public int getTier() {
		return tier;
	}

	public void setTier (int newTier) {
		tier = newTier;
	}

	public ItemType getItemType() {
		return itemType;
	}

	public string getName() {
		return name;
	}

	public Sprite getIcon() {
		return icon;
	}

	public int getValue() {
		return value;
	}
}

[Serializable]
public class Weapon : Item {

	public enum WeaponType {Sword = 0, Dagger = 1, Bow = 2, Wand = 3};

	WeaponType weaponType;
	float minDamage;
	float maxDamage;
	float critMultiplier;
	int range;// Number of squares
	Animation attackAnimation;

	public Weapon (WeaponType weaponType, int range, Animation attackAnimation, string name, Sprite icon, int tier) : base (ItemType.Weapon, name, icon, tier) {
		this.weaponType = weaponType;
		//this.range = range;// Range is only based on weapon type
		this.attackAnimation = attackAnimation;
		this.maxDamage = generateMaxDamage ();
	}

	private int getRange() {
		return 1;
	}

	private float generateMaxDamage() {
		switch(weaponType) {
			case WeaponType.Sword:
			return 7 + (4 * getTier());
			case WeaponType.Dagger:
			return 6 + (2 * getTier());
			case WeaponType.Bow:
			return 7 + (3 * getTier());
			case WeaponType.Wand:
			return 8 + (4 * getTier());
		}
	}

	private void itemVaration () {

		// Set minimum crit chance
		critMultiplier = 1;

		// Set minimum minimium
		minDamage = 2;
		// Add stats
		switch (weaponType) {
			case WeaponType.Dagger:
			case WeaponType.Sword:
				minDamage += GameControl.control.playerData.stats [GameControl.playerStats.Strength];
				break;
			case WeaponType.Bow:
				minDamage += GameControl.control.playerData.stats [GameControl.playerStats.Dexterity];
				break;
			case WeaponType.Wand:
				minDamage += GameControl.control.playerData.stats [GameControl.playerStats.Intelligence];
				break;
		}

		int random = UnityEngine.Random.Range (0, 8);// [0-7]
		if (random >=4) {
			minDamage *= 1 + (UnityEngine.Random.value % GameControl.control.playerData.getItemFind ());
		}

		// Minimum max damage
		maxDamage = minDamage;
		if (random == 2 || random == 3 || random == 6 || random == 7) {
			// Max change
		}
		if (random%2==1){
			// Crit change
		}

	

		// crit mult
	}

	public WeaponType getWeaponType() {
		//return weaponType;
	}

	public float getCritChance() {
		return critChance;
	}

	public float getCritMultiplier() {
		return critMultiplier;
	}

	public int getRange() {
		return range;
	}
    
	public Animation getAttackAnimation() {
		return attackAnimation;
	}
}

[Serializable]
public class Armor : Item {

	public enum ArmorType {Helm = 0, Chest = 1, Legs = 2, Gloves = 3, Boots = 4};

	ArmorType armorType;
	float armorValue;// ie damage resistance
	float durability;
	float healthBonus;

	public Armor(ArmorType armorType, float armorValue, float durability, float healthBonus, string name, Sprite icon, int value) : base (ItemType.Armor, name, icon, value) {
		this.armorType = armorType;
		this.armorValue = armorValue;
		this.durability = durability;
		this.healthBonus = healthBonus;
	}

	public ArmorType getArmorType() {
		return armorType;
	}

	public float getArmorValue() {
		return armorValue;
	}

	public float getDurability() {
		return durability;
	}

	public float getHealthBonus() {
		return healthBonus;
	}
}