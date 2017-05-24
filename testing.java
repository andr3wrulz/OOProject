package dungeons;

import java.applet.Applet;
import java.awt.Graphics;

public class testing extends Applet {

	Floor f;
	
	public void init() {
		f = new Floor();
		f.print();
		this.setSize(500,500);
	}
	
	public void paint(Graphics g) {
		f.paint(g);
	}

}
