package dungeons;

import java.awt.Graphics;
import java.util.ArrayList;
import java.util.Random;

public class Floor {

	private final static int floorSize = 10;
	private static Random rnd;
	private Tile[][] grid;
	private ArrayList<Pair> Q;
	
	public Floor() {
		rnd = new Random();
		Q = new ArrayList<Pair>();
		grid = new Tile[floorSize][floorSize];
		start();
	}
	
	public void print() {
		for(int i=0; i<floorSize; i++) {
			for(int j=0; j<floorSize; j++) {
				Tile t = grid[i][j];
				System.out.print("["+i+"]["+j+"] ");
				if(t==null)
					System.out.print("null");
				else {
					if(t.isN()) System.out.print("N ");
					if(t.isE()) System.out.print("E ");
					if(t.isS()) System.out.print("S ");
					if(t.isW()) System.out.print("W ");
					if(t.isExit()) System.out.print("(Exit)");
					if(t.isStart()) System.out.print("(Start)");
				}
				System.out.println("");
			}
		}
	}
	
	public void paint(Graphics g) {
		for(int i=0; i<floorSize; i++) {
			for(int j=0; j<floorSize; j++) {
				paintTile(g,i,j,grid[i][j]);
			}
		}
	}
	
	private void paintTile(Graphics g, int x, int y, Tile t) {
		g.drawRect(10+(x*26), 450-(y*26), 25, 25);
		if(t!=null) {
			if(t.isN()) g.drawRect(10+(x*26)+10, 450-(y*26)-4, 5, 7);
			if(t.isE()) g.drawRect(10+(x*26)+22, 450-(y*26)+10, 7, 5);
			if(t.isS()) g.drawRect(10+(x*26)+10, 450-(y*26)+22, 5, 7);
			if(t.isW()) g.drawRect(10+(x*26)-4, 450-(y*26)+10, 7, 5);
			if(t.isExit()) g.drawString("E", 10+(x*26)+10, 450-(y*26)+17);
			if(t.isStart()) g.drawString("S", 10+(x*26)+10, 450-(y*26)+17);
		}
		else
			g.drawOval(10+(x*26), 450-(y*26), 25, 25);
	}
	
	private void start() {
		int x = (int) ((rnd.nextDouble()*(floorSize-1))+1);
		int y = (int) ((rnd.nextDouble()*(floorSize-1))+1);
		Tile t = makeTile(x,y);
		t.setStart(true);

		grid[x][y] = t;
		if(t.isN()) Q.add(new Pair(x,y+1));
		if(t.isE()) Q.add(new Pair(x+1,y));
		if(t.isS()) Q.add(new Pair(x,y-1));
		if(t.isW()) Q.add(new Pair(x-1,y));
		
		while(!Q.isEmpty())
			generate();
	}
	
	
	private Tile makeTile(int x, int y) {
		System.out.println("x:"+x+" y:"+y);
		boolean canN = true;
		boolean canE = true;
		boolean canS = true;
		boolean canW = true;
		
		boolean mustN = false;
		boolean mustE = false;
		boolean mustS = false;
		boolean mustW = false;
		
		if(x==0)
			canW = false;
		else if(grid[x-1][y]!=null)
			if(grid[x-1][y].isE()) mustW = true;
			
		if(x==(floorSize-1))
			canE = false;
		else if(grid[x+1][y]!=null)
			if(grid[x+1][y].isW()) mustE = true;
			
		if(y==0)
			canS = false;
		else if(grid[x][y-1]!=null)
			if(grid[x][y-1].isN()) mustS = true;
			
		if(y==(floorSize-1))
			canN = false;
		else if(grid[x][y+1]!=null)
			if(grid[x][y+1].isS()) mustN = true;
		
		if(Q.size()<1) //use all doors
			return new Tile(canN,canE,canS,canW);
		else if(Q.size()>1) { //can have no doors
			return new Tile(makeDoor(canN,mustN),makeDoor(canE,mustE),makeDoor(canS,mustS),makeDoor(canW,mustW));
		}
		else { //at least one door
			if(canN)
				return new Tile(true,makeDoor(canE,mustE),makeDoor(canS,mustS),makeDoor(canW,mustW));
			else if(canE)
				return new Tile(false,true,makeDoor(canS,mustS),makeDoor(canW,mustW));
			else if(canS)
				return new Tile(false,false,true,makeDoor(canW,mustW));
			else if(canW)
				return new Tile(false,false,false,true);
			else
				return new Tile(false,false,false,false);
		}
		
	}
	
	private boolean makeDoor(boolean can, boolean adj) {
		if(adj)
			return true;
		if(can)
			return rnd.nextBoolean();
		return false;
	}
	
	private void generate() {
		Pair p = Q.get(0);
		Q.remove(0);
		int x = p.getX();
		int y = p.getY();
		Tile t = makeTile(p.getX(),p.getY());
		
		if(t.isN() && grid[x][y+1]==null) Q.add(new Pair(x,y+1));
		if(t.isE() && grid[x+1][y]==null) Q.add(new Pair(x+1,y));
		if(t.isS() && grid[x][y-1]==null) Q.add(new Pair(x,y-1));
		if(t.isW() && grid[x-1][y]==null) Q.add(new Pair(x-1,y));
		
		if(Q.isEmpty()) t.setExit(true);
		grid[x][y] = t;
		}
}
