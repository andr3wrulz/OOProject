using UnityEngine;
using System.Collections;
	
	public abstract class Moving: MonoBehaviour
	{
		public LayerMask BlockingLayer;	
		public LayerMask Water;	
		private BoxCollider2D boxCollider; 		
		private Rigidbody2D rgb;
		
		//------------------------
		
		protected virtual void Awake ()
		{
			rgb = GetComponent <Rigidbody2D> ();
			boxCollider = GetComponent <BoxCollider2D> ();
		}
		
		// Check if the player is moving toward an enemy or touching it. 
		// The range is based on the weapon player is using.  
		protected bool IsObstacle (int x, int y, float range, out RaycastHit2D hitSword, out RaycastHit2D hitBow)
		{
		
			Vector2 start = rgb.position;

			// if remove start, the player will be stucked with the enemy. 

			Vector2 targetPosition = start + new Vector2 (x, y);
			
			boxCollider.enabled = false; 

			// Use linecast for sword attack. 
			//hitSword = Physics2D.Linecast (start, targetPosition, BlockingLayer);
			//This fixes the precious problem where player could not get close to walls. 
			hitSword = Physics2D.Raycast (start, new Vector2 (x, y) , 0.4f,  BlockingLayer); 
			
			// use raycast for bow attack. 
			hitBow = Physics2D.Raycast (start, new Vector2 (x, y) , range,  BlockingLayer); 

			boxCollider.enabled = true;

			if (hitBow.collider == null && hitSword.collider == null)
			{
				return false;
			}
			
			return true;
		}
		
		
		//Move the player.
		protected void ToMove (Vector2 direction)  
		{	
			// for debugging
			Debug.Log ("ToMove was called ");

			rgb.MovePosition (direction);
		}
	}
