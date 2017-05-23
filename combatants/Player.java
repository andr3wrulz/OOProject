package combatants;

import java.util.ArrayList;

import equipment.Armor;
import equipment.Item;
import equipment.Weapon;

public class Player extends Combatant {
	
	private Weapon WeaponSlot;
	private Armor ArmorSlot;
	private ArrayList<Item> inventory;
	
	public Player(String name) {
		this.setName(name);
		inventory = new ArrayList<Item>();
	}
	
	public void Equip(int inventorySlot) {
		Item temp = this.inventory.remove(inventorySlot);
		if(temp.getType()==1) {
			this.inventory.add(this.WeaponSlot);
			this.WeaponSlot = (Weapon) temp;
		}
		else { //Type==2
			this.inventory.add(this.ArmorSlot);
			this.ArmorSlot = (Armor) temp;
		}
	}
	
	public int healthMax() {
		return this.ArmorSlot.getHealthBonus()+100;
	}
	
	public int getDamage() {
		return this.WeaponSlot.getDamage()+1;
	}
	
	public Item getInvenotrySlot(int invSlot) {
		return this.inventory.get(invSlot);
	}
	
	public int addItemToInventory(Item m) {
		this.inventory.add(m);
		return this.inventory.size();
	}
}
