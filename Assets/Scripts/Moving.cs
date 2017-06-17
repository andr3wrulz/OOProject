using UnityEngine;
using System.Collections;
	
	public abstract class Moving: MonoBehaviour
	{
		public LayerMask BlockingLayer;	
		public LayerMask Water;	
		private BoxCollider2D boxCollider; 		
		private Rigidbody2D rgb;
		
		//------------------------
		
		protected virtual void Start ()
		{
			rgb = GetComponent <Rigidbody2D> ();
			boxCollider = GetComponent <BoxCollider2D> ();
		}
		
		protected bool IsObstacle (int x, int y, out RaycastHit2D hit)
		{
		
			Vector2 start = rgb.position;

			// if remove start, the player will be stucked with the enemy. 

			Vector2 targetPosition = start + new Vector2 (x, y);
			
			boxCollider.enabled = false; 

			hit = Physics2D.Linecast (start, targetPosition, BlockingLayer);

			boxCollider.enabled = true; 

			if (hit.collider == null)
			{
				return false;
			}
			
			return true;
		}
		

		protected void ToMove (Vector2 direction)  
		{
			Debug.Log ("ToMove was called ");
			rgb.MovePosition (direction);
		}
	}
