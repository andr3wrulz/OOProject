package equipment;

public class Lootable {
	
	private int dropIndex;
	private int dropChance;
	private int itemIndex;
	private boolean unique;
	
	public Lootable(int index) {
		
	}
	
	public void setLootable(int d_index, int i_index, int chance, boolean unique) {
		this.dropChance = chance;
		this.dropIndex = d_index;
		this.itemIndex = i_index;
		this.unique = unique;
	}

	public int getDropIndex() {
		return dropIndex;
	}

	public int getDropChance() {
		return dropChance;
	}
	
	public int getItemIndex() {
		return itemIndex;
	}
	
	public boolean isUnique() {
		return unique;
	}
}
