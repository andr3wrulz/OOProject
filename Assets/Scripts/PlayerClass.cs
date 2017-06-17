using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass : Moving {

	int count; 
	int speed = 10; 


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

			RaycastHit2D hit; 

			Debug.DrawLine (start, end, Color.red);

			if (IsObstacle (x, y, out hit)) {
				
				Debug.Log ("Encounter obstacle " + count++);

				Debug.Log (hit.transform); 
			
			} 
			else
			{
				ToMove (newPosition);
			}
		}
	}
}
