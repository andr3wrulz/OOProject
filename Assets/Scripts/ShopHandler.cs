using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHandler : MonoBehaviour {

	public GameObject itemPanel;
	public Text goldAmount;
	public Text inventorySlotsText;

	public Sprite bagIcon;
	public Sprite swordIcon;
	public Sprite daggerIcon;
	public Sprite bowIcon;
	public Sprite wandIcon;

	GameObject [] storeSlots;

	void Start() {
		// Assign the ui elements to the storeSlots array
		findStoreSlotObjects ();

		// Make the buy buttons work
		addButtonListeners ();

		// Make sure there is some inventory
		ensureInventory();

		// Update the ui elements to match the stored inventory
		updateStoreSlots();
	}

	void findStoreSlotObjects() {
		storeSlots = new GameObject[5];

		for (int i = 0; i < 5; i++) {
			storeSlots[i] = itemPanel.transform.Find ("Store Slot " + (i + 1)).gameObject;
		}
	}

	void addButtonListeners() {
		for (int i = 0; i < 5; i++) {
			int index = i + 1;
			storeSlots [i].transform.Find ("BuyButton").GetComponent<Button> ().onClick.AddListener (() => clickBuyButton (index));
		}
	}

	void ensureInventory() {
		// If the inventory was not initialized
		if (GameControl.control.playerData.shopInventory == null || GameControl.control.playerData.shopInventory.Length != 5)
			GameControl.control.rerollShopInventory ();

		// Count items in the shop inventory
		int itemCount = 0;
		for (int i = 0; i < 5; i++)
			if (GameControl.control.playerData.shopInventory [i] != null)
				itemCount++;

		// If there are no items left, generate more
		if (itemCount == 0)
			GameControl.control.rerollShopInventory ();
	}

	void updateStoreSlots() {
		goldAmount.text = "You have " + GameControl.control.playerData.gold + " gold";

		int freeSlots = GameControl.control.playerData.inventory.getFreeSlotsInBackpack ();
		if (freeSlots == 0) {
			inventorySlotsText.text = "You don't have any open inventory slots!";
		} else {
			inventorySlotsText.text = "You have " + freeSlots + " open slots in your inventory.";
		}

		for (int i = 0; i < 5; i++) {
			Item item = GameControl.control.playerData.shopInventory [i];

			if (item != null)
				Debug.Log ("Updating slot " + i + " with Item: " + item.ToString ());

			if (item != null) {
				if (item is Weapon) {
					Weapon wep = item as Weapon;
					storeSlots [i].transform.Find ("Name").GetComponent<Text> ().text = wep.getName ();
					storeSlots [i].transform.Find ("Details1").GetComponent<Text> ().text = "Damage: " + wep.getMinDamage ().ToString ("n0") + "-" + wep.getMaxDamage ().ToString ("n0");
					storeSlots [i].transform.Find ("Details2").GetComponent<Text> ().text = "Crit Multiplier: " + wep.getCritMultiplier ().ToString ("n2") + "x";
					storeSlots [i].transform.Find ("Details3").GetComponent<Text> ().text = "Range: " + wep.getRange () + " squares";
					storeSlots [i].transform.Find ("Value").GetComponent<Text> ().text = "Value: " + wep.getValue () + " gold";// Use weapon value formula
				} else {
					// Show out of stock message
					storeSlots [i].transform.Find ("Name").GetComponent<Text> ().text = "Out of Stock";
					storeSlots [i].transform.Find ("Value").GetComponent<Text> ().text = "Value: 0 gold";
					storeSlots [i].transform.Find ("Details1").GetComponent<Text> ().text = "";
					storeSlots [i].transform.Find ("Details2").GetComponent<Text> ().text = "";
					storeSlots [i].transform.Find ("Details3").GetComponent<Text> ().text = "";
					// Disable the buy button
					storeSlots [i].transform.Find ("BuyButton").gameObject.SetActive (false);
				}
			} else {
				// Show out of stock message
				storeSlots [i].transform.Find ("Name").GetComponent<Text> ().text = "Out of Stock";
				storeSlots [i].transform.Find ("Value").GetComponent<Text> ().text = "Value: 0 gold";
				storeSlots [i].transform.Find ("Details1").GetComponent<Text> ().text = "";
				storeSlots [i].transform.Find ("Details2").GetComponent<Text> ().text = "";
				storeSlots [i].transform.Find ("Details3").GetComponent<Text> ().text = "";
				// Disable the buy button
				storeSlots [i].transform.Find ("BuyButton").gameObject.SetActive (false);
			}

			storeSlots [i].transform.Find ("Icon").GetComponent<SpriteRenderer> ().sprite = getIconForItem(item);
		}
	}

	void clickBuyButton(int buttonId) {
		// If there is an item in the slot they clicked buy
		if (GameControl.control.playerData.shopInventory [buttonId - 1] != null) {
			// If they have inventory space and gold
			if (GameControl.control.playerData.inventory.getFreeSlotsInBackpack () > 0 && 
				GameControl.control.playerData.gold >= GameControl.control.playerData.shopInventory[buttonId - 1].getValue()) {
				// Buy it

				// Take the gold
				GameControl.control.playerData.gold -= GameControl.control.playerData.shopInventory[buttonId - 1].getValue();
				// Add it to their inventory
				GameControl.control.playerData.inventory.addItemToBackpack(GameControl.control.playerData.shopInventory[buttonId - 1]);
				// Remove it from the shop
				GameControl.control.playerData.shopInventory[buttonId - 1] = null;
				// Update the shop ui
				updateStoreSlots();
			}
		} else {
			Debug.Log ("[DEBUG] You can't buy an item that isn't there");
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
}
