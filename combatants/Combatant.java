package combatants;

public class Combatant {

	private String name;
	private int health;
	//private int skillA;
	//Variables for skills/attributes in future expansion
	
	public int healthMax() {
		return 0;
	}
	
	public int getDamage() {
		return 0;
	}
	
	public void maxHeal() {
		this.health = healthMax();
	}

	public String getName() {
		return name;
	}

	public void setName(String name) {
		this.name = name;
	}

	public int dealDamage(int damage) {
		this.health -= damage;
		return health;
	}
}
