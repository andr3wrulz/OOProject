package equipment;

public class Weapon extends Item {

	private int damage;
	private int range;
	private int weaponType;
	
	public Weapon(int index) {
		switch(index) {
		case 1:
			this.setName("Test_Melee_Weapon_01");
			this.weaponType = 1;
			this.damage = 2;
			this.range = 1;
			break;
		case 2:
			this.setName("Test_Melee_Weapon_02");
			this.weaponType = 1;
			this.damage = 3;
			this.range = 1;
			break;
		case 3:
			this.setName("Test_Melee_Weapon_03");
			this.weaponType = 1;
			this.damage = 4;
			this.range = 1;
			break;
		case 4:
			this.setName("Test_Melee_Weapon_04");
			this.weaponType = 1;
			this.damage = 5;
			this.range = 1;
			break;
		}
	}
	
	public int getDamage() {
		return damage;
	}
	
	public int getRange() {
		return range;
	}
	
	public int getType() {
		return 1;
	}
	
	public int getWeaponType() {
		return weaponType;
	}

}
