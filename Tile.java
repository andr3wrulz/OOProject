package dungeons;

import java.util.ArrayList;

import combatants.Mob;

public class Tile {

	private boolean N;
	private boolean E;
	private boolean S;
	private boolean W;
	private ArrayList<Mob> mobs;
	private boolean exit;
	private boolean start;
	
	public Tile(boolean n, boolean e, boolean s, boolean w) {
		this.N = n;
		this.E = e;
		this.S = s;
		this.W = w;
		mobs = new ArrayList<Mob>();
	}

	public boolean isN() {
		return N;
	}

	public void setN(boolean n) {
		N = n;
	}

	public boolean isE() {
		return E;
	}

	public void setE(boolean e) {
		E = e;
	}

	public boolean isS() {
		return S;
	}

	public void setS(boolean s) {
		S = s;
	}

	public boolean isW() {
		return W;
	}

	public void setW(boolean w) {
		W = w;
	}

	public ArrayList<Mob> getMobs() {
		return mobs;
	}

	public void setMobs(ArrayList<Mob> mobs) {
		this.mobs = mobs;
	}

	public boolean isExit() {
		return exit;
	}

	public void setExit(boolean exit) {
		this.exit = exit;
	}

	public boolean isStart() {
		return start;
	}

	public void setStart(boolean start) {
		this.start = start;
	}
}
