package equipment;

import java.util.ArrayList;
import java.util.Random;

public class DropTable {

	private ArrayList<Lootable> dropTable;
	private Random rnd;
	
	public DropTable(int[] list) {
		this.dropTable = new ArrayList<Lootable>();
		rnd = new Random();
		populate(list);
	}
	
	private void populate(int[] list) {
		for(int i=0; i<list.length; i++) {
			dropTable.add(new Lootable(list[i]));
		}
		sort();
	}
	
	private void sort() {
		//sort by dropIndex
	}
	
	public Lootable pull(int index, int pullMod) {
		for(Lootable loot : dropTable) {
			if(index >= loot.getDropIndex()) {
				int roll = (int) ((rnd.nextDouble()*99)+1);
				if(roll <= loot.getDropChance()) {
					if(loot.isUnique()) dropTable.remove(loot);
					return loot;
				}
			}
		}
		return null;
	}
}
