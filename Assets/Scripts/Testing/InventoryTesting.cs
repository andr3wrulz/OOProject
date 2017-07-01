using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class InventoryTesting {

	[Test]
	public void testInventoryCreatesArraysOfProperLength () {
		Inventory i = new Inventory ();

		Assert.AreEqual (GameConfig.backpackSlots, i.backpack.Length);
		Assert.AreEqual (GameConfig.storageSlots, i.storage.Length);
		// Armor should length of the enum that determines armor slots
		Assert.AreEqual (System.Enum.GetValues (typeof(Armor.ArmorType)).Length, i.armor.Length);
	}

	[Test]
	public void testBackpackAddItems () {
		Inventory i = new Inventory ();

		Item item1 = new Item (Item.ItemType.Armor, "item1", 1);
		Item item2 = new Item (Item.ItemType.Weapon, "item2", 2);

		i.addItemToBackpackAtIndex (item1, 0);
		i.addItemToBackpackAtIndex (item2, 1);

		Assert.AreEqual (item1, i.getItemFromBackpackAtIndex (0));
		Assert.AreEqual (item2, i.getItemFromBackpackAtIndex (1));
	}

	[Test]
	public void testBackpackSwapItems() {
		Inventory i = new Inventory ();

		Item item1 = new Item (Item.ItemType.Armor, "item1", 1);
		Item item2 = new Item (Item.ItemType.Weapon, "item2", 2);

		i.addItemToBackpackAtIndex (item1, 0);
		i.addItemToBackpackAtIndex (item2, 1);

		i.swapItemsInBackpackAtIndexes (0, 1);

		Assert.AreEqual (item1, i.getItemFromBackpackAtIndex (1));
		Assert.AreEqual (item2, i.getItemFromBackpackAtIndex (0));
	}

	[Test]
	public void testBackpackFreeSpace() {
		Inventory inv = new Inventory ();
		Assert.AreEqual (GameConfig.backpackSlots, inv.getFreeSlotsInBackpack ());

		for (int i = 0; i < GameConfig.backpackSlots; i++) {
			inv.addItemToBackpack (new Item (Item.ItemType.Weapon, "TestName", 1));
			Assert.AreEqual (GameConfig.backpackSlots - (i + 1), inv.getFreeSlotsInBackpack ());
		}
	}

	// The following test is commented out until I figure out how to get UnityEngine methods working with unit testing
	/*[Test]
	public void testEquipWeapon () {
		Inventory i = new Inventory ();

		Weapon wep1 = new Weapon (Weapon.WeaponType.Sword, "TestName1", 1);
		Weapon wep2 = new Weapon (Weapon.WeaponType.Dagger, "TestName2", 2);

		i.setWeapon (wep1);

		// Check wep1 was equipped
		Assert.AreEqual (wep1, i.getWeapon ());

		// When we equip wep2, we should get wep1 back
		Weapon temp = i.setWeapon(wep2);
		Assert.AreEqual (wep1, temp);

		// Make sure wep2 is equipped
		Assert.AreEqual(wep2, i.getWeapon());

		// Remove equipped weapon and make sure no weapon is equipped
		temp = i.removeWeapon();
		Assert.AreEqual (wep2, temp);
		Assert.AreEqual (null, i.getWeapon ());
	}*/
}
