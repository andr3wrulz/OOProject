using System;
using NUnit.Framework;
using UnityEngine;

public class ItemTesting {
	
	[Test]
	public void testCreateAndModifyItem () {
		Item i = new Item (Item.ItemType.Weapon, "TestName", 1);

		// Verify the item was created as specified
		Assert.AreEqual (Item.ItemType.Weapon, i.getItemType());
		Assert.AreEqual ("TestName", i.getName ());
		Assert.AreEqual (1, i.getTier ());

		// Now lets change the tier, the rest of the properties are read only
		i.setTier(2);
		Assert.AreEqual (2, i.getTier ());
	}

	// The following tests are commented out until I figure out how to include UnityEngine functions in tests
	/*[Test]
	public void testCreateAndModifyWeapon () {
		Weapon w = new Weapon (Weapon.WeaponType.Sword, "TestName", 1);

		// Verify the weapon was created as specified
		Assert.AreEqual (Weapon.WeaponType.Sword, w.getWeaponType());
		Assert.AreEqual ("TestName", w.getName ());
		Assert.AreEqual (1, w.getTier ());
		// Range of a sword should be 1
		Assert.AreEqual (1, w.getRange());
		// We can't test min or max damage as well as crit multiplier becuase those are random
	}

	[Test]
	public void testWeaponRange () {
		Weapon sword = new Weapon (Weapon.WeaponType.Sword, "TestName", 1);
		Assert.AreEqual (1, sword.getRange ());

		Weapon dagger = new Weapon (Weapon.WeaponType.Dagger, "TestName", 1);
		Assert.AreEqual (1, dagger.getRange ());

		Weapon bow = new Weapon (Weapon.WeaponType.Bow, "TestName", 1);
		Assert.AreEqual (GameConfig.bowRange, bow.getRange ());

		Weapon wand = new Weapon (Weapon.WeaponType.Wand, "TestName", 1);
		Assert.AreEqual (GameConfig.wandRange, wand.getRange ());
	}*/
}

