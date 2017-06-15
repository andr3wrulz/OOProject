using UnityEngine;
using System.Collections;

	// Player and enemies will inherit this class. 
	
	public abstract class Moving: MonoBehaviour
	{
		
		// How fast objects will move. 
		public int speed = 10; 

		public LayerMask blockingLayer;			
		private BoxCollider2D boxCollider; 		
		private Rigidbody2D rigidbd;				
		
		protected virtual void Start ()
		{
			// Get the rigid object and the box colider object. 
			rigidbd = GetComponent <Rigidbody2D> ();
			boxCollider = GetComponent <BoxCollider2D> ();
		}
		
		// Check if something was hit.
		// If true, the object that was hit is stored in RaycastHit2D hit.
		// Otherwise, hit stores NULL. 

		protected bool CheckObstacle (int x, int y, out RaycastHit2D hit)
		{
		
			Vector2 start = transform.position;

			Vector2 targetPosition = start + new Vector2 (x, y);

			boxCollider.enabled = false;

			hit = Physics2D.Linecast (start, targetPosition, blockingLayer);

			boxCollider.enabled = true;
			
			//After casting, check if anything was hit
			if(hit.transform == null)
			{
				return false;
			}
			
			return true;
		}
		
		// Will be called by TryMoving method. 
		// Received the directions and move the object. 
		// Return true if nothing was hit, and the object can move. 
		// Otherwise, false. 
		protected bool CanMove (int xDir, int yDir, out RaycastHit2D hit)
		{
			Vector2 start = transform.position;

			Vector2 targetPosition = start + new Vector2 (xDir, yDir);
			
			boxCollider.enabled = false;
			
			hit = Physics2D.Linecast (start, targetPosition, blockingLayer);
			
			boxCollider.enabled = true;
			
			if(hit.transform == null)
			{
				// Only alive enemies can move.  
				if (gameObject.activeSelf)
				{
					Vector2 newPosition = Vector2.MoveTowards (start, targetPosition, speed * Time.deltaTime);
					Move (newPosition);
				}
				return true;
				
			}
			
			return false;
		}
		
		
		//Calculate the distance from the object to the target position. 
		//Square this distance. 
		//While this distance is greater than Epsilon (the smallest value a float can be except 0)
		//Move the object closer to the target position by the inverse time. 
		// Repeat the process. 

		protected void Move (Vector2 direction)
		{
			
			rigidbd.MovePosition (direction);
			
		}

		
		// Move the object by calling CanMove. 
		// Also, the CanMove tells if it was able to move the objects. 
		// If yes, return. 
		// If not, something was hit. Call corresponding OnCantMove function in the child class
		// to deal with this. 

		protected virtual void TryMoving <T> (int horizontal, int vertical)
			where T : Component
		{
		
			RaycastHit2D hit;

			//bool canMove = Move (xDir, yDir, out hit);
			bool canMove = CanMove (horizontal, vertical, out hit);
			
			if(hit.transform == null)
				return;

			// Hit something. 
		
			T hitComponent = hit.transform.GetComponent <T> ();
			
			if (!canMove && hitComponent != null) 
			{
				CantMove (hitComponent);
			}
		}
		
		protected abstract void CantMove <T> (T component)
			where T : Component;
	}
