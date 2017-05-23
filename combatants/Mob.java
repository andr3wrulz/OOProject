package combatants;

import java.util.Random;

public class Mob extends Combatant{

	private int damage;
	private int healthMax;
	
	private static Random rnd = new Random();
	
	public Mob() {
		int index = (int) ((rnd.nextDouble())*7)+1;
		switch(index) {
		case 1:
			this.setName("");
			this.damage = 0;
			this.healthMax = 0;
			break;
		case 2:
			this.setName("");
			this.damage = 0;
			this.healthMax = 0;
			break;
		case 3:
			this.setName("");
			this.damage = 0;
			this.healthMax = 0;
			break;
		case 4:
			this.setName("");
			this.damage = 0;
			this.healthMax = 0;
			break;
		case 5:
			this.setName("");
			this.damage = 0;
			this.healthMax = 0;
			break;
		case 6:
			this.setName("");
			this.damage = 0;
			this.healthMax = 0;
			break;
		case 7:
			this.setName("");
			this.damage = 0;
			this.healthMax = 0;
			break;
		case 8:
			this.setName("");
			this.damage = 0;
			this.healthMax = 0;
			break;
		}
		this.maxHeal();
	}
	
	public int getDamage() {
		return damage;
	}
	
	public int MaxHealth() {
		return healthMax;
	}
}
