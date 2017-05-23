package equipment;

public class Armor extends Item{

	private int healthBonus;
	private int damageResist;
	
	public Armor(int index) {
		switch(index) {
		case 1:
			this.setName("Test_Armor_01");
			this.healthBonus = 20;
			this.damageResist = 0;
			break;
		case 2:
			this.setName("Test_Armor_02");
			this.healthBonus = 30;
			this.damageResist = 0;
			break;
		case 3:
			this.setName("Test_Armor_03");
			this.healthBonus = 40;
			this.damageResist = 1;
			break;
		case 4:
			this.setName("Test_Armor_04");
			this.healthBonus = 50;
			this.damageResist = 2;
			break;
		}
	}
	
	public int getHealthBonus() {
		return healthBonus;
	}
	
	public int getDamageResist() {
		return damageResist;
	}
	
	public int getType() {
		return 1;
	}
}
