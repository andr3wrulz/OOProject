using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSheetScreen : MonoBehaviour {

	enum MenuType {Character, Inventory};
	MenuType currentScreen;

	public Button characterSheetTabButton;
	public Button inventoryTabButton;

	GameObject characterSheetParent;
	GameObject inventoryParent;

	public GameObject canvas;

	public GameObject inventorySlotPrefab;
	public GameObject swordIcon;
	public GameObject daggerIcon;
	public GameObject bowIcon;
	public GameObject wandIcon;

	GameObject [] inventorySlots;

	public void Start () {
		// Create our parent objects
		characterSheetParent = new GameObject ("CharacterSheetTabParent");
		characterSheetParent.transform.SetParent (canvas.transform);
		inventoryParent = new GameObject ("InventoryTabParent");
		inventoryParent.transform.SetParent (canvas.transform);

		// Set character sheet at current tab
		currentScreen = MenuType.Character;
		inventoryParent.SetActive (false);

		// Instantiate ui elements
		inventorySlots = new GameObject[GameConfig.backpackSlots];
		createInventorySlots();
	}

	public void createInventorySlots () {
		// Find correct y offsets
		int centerHeight = Screen.height / 2;
		int[] heights = { centerHeight + 220, centerHeight + 110, centerHeight, centerHeight - 110, centerHeight - 220 };

		for (int i = 0; i < 5; i++)
			inventorySlots [i] = createInventorySlot (100, heights[i]);
		for (int i = 5; i < 10; i++)
			inventorySlots [i] = createInventorySlot (420, heights[i-5]);

	}

	void setupInventoryButtonListeners () {
		// For each button
		for (int i = 0; i < 10; i++) {
			// We can't just pass i to each function becuase c# wont store the state of i when we set the listener
			// So we need to create a new instance of each i to pass to the functions
			int numberReference = i;

			Button sellButton = inventorySlots [i].transform.Find("SellButton").GetComponent<Button>();
			sellButton.onClick.AddListener (() => sellButtonClicked (numberReference));

			Button equipButton = inventorySlots [i].transform.Find("EquipButton").GetComponent<Button>();
			equipButton.onClick.AddListener (() => equipButtonClicked (numberReference));
		}
	}

	void sellButtonClicked (int buttonId) {
		Debug.Log ("Sell button " + buttonId + " clicked!");
	}

	void equipButtonClicked (int buttonId) {
		Debug.Log ("Equip button " + buttonId + " clicked!");
	}

	GameObject createInventorySlot (float x, float y) {
		GameObject slot = Instantiate (inventorySlotPrefab);
		slot.transform.SetParent (inventoryParent.transform);
		slot.transform.SetPositionAndRotation (new Vector3 (x, y, 1), Quaternion.identity);
		return slot;
	}

	public void clickInventoryTab () {
		// Set screen variable
		currentScreen = MenuType.Inventory;

		// Swap button states
		characterSheetTabButton.interactable = true;
		inventoryTabButton.interactable = false;

		// Swap parent object states
		characterSheetParent.SetActive(false);
		inventoryParent.SetActive (true);
	}

	public void clickCharacterTab () {
		// Set screen variable
		currentScreen = MenuType.Character;

		// Swap button states
		characterSheetTabButton.interactable = false;
		inventoryTabButton.interactable = true;

		// Swap parent object states
		characterSheetParent.SetActive(true);
		inventoryParent.SetActive (false);
	}
}
