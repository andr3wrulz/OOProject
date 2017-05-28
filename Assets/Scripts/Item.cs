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

	public Item(ItemType itemType, string name, Sprite icon, int value) {
		this.itemType = itemType;
		this.name = name;
		this.icon = icon;
		this.value = value;
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
	float baseDamage;
	float critChance;
	float critMultiplier;
	int range;// Number of squares
	Animation attackAnimation;

	public Weapon (WeaponType weaponType, float baseDamage, float critChance, float critMultiplier, int range, Animation attackAnimation, string name, Sprite icon, int value) : base (ItemType.Weapon, name, icon, value) {
		this.weaponType = weaponType;
		this.baseDamage = baseDamage;
		this.critChance = critChance;
		this.critMultiplier = critMultiplier;
		this.range = range;
		this.attackAnimation = attackAnimation;
	}

	public WeaponType getWeaponType() {
		return weaponType;
	}

	public float getBaseDamage() {
		return baseDamage;
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