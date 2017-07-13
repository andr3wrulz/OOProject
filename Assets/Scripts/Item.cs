using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Item {

	public enum ItemType {Weapon = 0, Armor = 1};

	ItemType itemType;
	string name;
	int tier;

	public Item(ItemType itemType, string name, int tier) {
		this.itemType = itemType;
		this.name = name;
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

	public int getValue() {
		if (this is Weapon) {
			Weapon wep = this as Weapon;
			return wep.getValue ();
		} else {
			return 0;
		}
	}

	public override string ToString ()
	{
		if (this is Weapon) {
			Weapon wep = this as Weapon;
			return wep.ToString ();
		} else {
			return "Name: '" + name + "' Type: " + ((itemType == ItemType.Weapon) ? "Weapon" : "Armor") + " Tier: " + tier;
		}
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

	public Weapon (WeaponType weaponType, string name, int tier) : base (ItemType.Weapon, name, tier) {
		this.weaponType = weaponType;
		this.range = getRange();// Range is only based on weapon type
		itemVaration ();// Generates our min, max, and crit mult
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
            default:
                return 7 + (4 * getTier());
		}
	}

	private void itemVaration () {

		// Set minimum crit multiplier
		critMultiplier = 1;
		// Set minimum damage minimium
		minDamage = 2;
		// Set minimum damage max
		maxDamage = generateMaxDamage();

		// Add stats that affect minimum damage
		if (GameControl.control.playerData.stats.Length == 6) {// If the player has stats (ie created from main menu)
			switch (weaponType) {
				case WeaponType.Dagger:
				case WeaponType.Sword:
					minDamage += GameControl.control.playerData.stats [(int)GameControl.playerStats.Strength];
					break;
				case WeaponType.Bow:
					minDamage += GameControl.control.playerData.stats [(int)GameControl.playerStats.Dexterity];
					break;
				case WeaponType.Wand:
					minDamage += GameControl.control.playerData.stats [(int)GameControl.playerStats.Intelligence];
					break;
			}
		}

		// Generate our case (each enhancement is determined by the binary representation of random, ie each bit represents an enhancement)
		int random = UnityEngine.Random.Range (0, 8);// [0-7]

		if (random >=4) {// Enhance min damage
			minDamage *= 1 + (UnityEngine.Random.value % GameControl.control.playerData.getItemFind ());
		}

		// if our min is larger than our max, that shouldn't happen
		if (minDamage > maxDamage)
			maxDamage = minDamage;
		
		if (random == 2 || random == 3 || random == 6 || random == 7) {// Enhance max damage
			maxDamage *= 1 + (UnityEngine.Random.value % GameControl.control.playerData.getItemFind ());
		}

		if (random%2==1){// Enhance crit
			critMultiplier *= 1 + (UnityEngine.Random.value % (GameControl.control.playerData.getItemFind ()*2));
		}
	}

	public float getMinDamage() {
		return minDamage;
	}

	public float getMaxDamage() {
		return maxDamage;
	}

	public WeaponType getWeaponType() {
		return weaponType;
	}

	public float getCritMultiplier() {
		return critMultiplier;
	}

	public int getRange() {
		if (weaponType == WeaponType.Bow)
			return GameConfig.bowRange;
		if (weaponType == WeaponType.Wand)
			return GameConfig.wandRange;
		return 1;
	}
		
	public new int getValue() {
		return (int)((minDamage + maxDamage) * critMultiplier);
	}

	public new string ToString ()
	{
		return "Name: '" + getName () + "' Damage: " + minDamage + "-" + maxDamage + " Range: " + range;
	}

	public int getAttackDamage() {
		int damage = UnityEngine.Random.Range ((int) minDamage, (int) maxDamage);

		if (UnityEngine.Random.value <= GameControl.control.playerData.stats [(int)GameControl.playerStats.Luck] / 50)
			damage = (int)(critMultiplier*(float)damage);// damage *= critMult; but with casting

		return damage;
	}
}

[Serializable]
public class Armor : Item {

	public enum ArmorType {Helm = 0, Chest = 1, Legs = 2, Gloves = 3, Boots = 4};

	ArmorType armorType;
	float armorValue;// ie damage resistance
	float durability;
	float healthBonus;

	public Armor(ArmorType armorType, float armorValue, float durability, float healthBonus, string name,int value) : base (ItemType.Armor, name, value) {
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