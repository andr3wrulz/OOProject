using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : Moving {

	// for debugging only
	int count; 
	//-----

	int speed = 10; 

	// This is used for attacking  by bow.
	// This range can be determined by the strength of the player, etc. 
	public float range = 4f; 

	protected  override void  Start () {

		base.Start (); 
	}

	protected void FixedUpdate () {
		
		int x, y; 

		x = (int) Input.GetAxisRaw ("Horizontal"); 
		y = (int) Input.GetAxisRaw ("Vertical"); 
	
		if (x != 0 || y != 0) {


			// This prevents diagonal move. 
			if(x != 0)
			{
				y = 0;
			}
				
			Vector2 start = transform.position;  
			Vector2 end = start + new Vector2 (x, y); 
			Vector2 newPosition = Vector2.MoveTowards (start, end, speed * Time.deltaTime);

			RaycastHit2D hitSword; 
			RaycastHit2D hitBow; 

			// This line is used for testing. 
			// This line is only seen in scene window. 
			Debug.DrawLine (start, end, Color.red);

			// Here is where we implement attack. 
			// Now player just stop whenever he is moving toward an enemy within a range
			// or touching it. 
			if (IsObstacle (x, y, range, out hitSword, out hitBow)) {

				Enemy enemyByBow = hitBow.transform.GetComponent<Enemy>();
				// Can attack by bow if the player is using bow
				if (enemyByBow != null){ 
					Debug.Log ("Attack 1");
				}

				Enemy enemyBySword = hitBow.transform.GetComponent<Enemy>();
				// Attack by sword
				if (enemyBySword != null){ 
					Debug.Log ("Attack 2");
				}

				// Detect wall only by raycast and still be a distance from wall
				// so player can move until linecast indicate that player is touching wall.
				Enemy wallOrEnemy = hitBow.transform.GetComponent<Enemy>();

				if (wallOrEnemy == null && hitSword.transform == null) {

					// for debugging
					Debug.Log ("I see wall");

					// 
					ToMove (newPosition);
				}

			} 
			// if not, just move. 
			else
			{
				Debug.Log (x + "," + y);
				ToMove (newPosition);
			}
		}
	}
}
