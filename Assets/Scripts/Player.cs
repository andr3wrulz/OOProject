using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    /* Room settings for player */
	IntVector2 currentRoom;
	public Camera mainCamera;
    public Camera miniMapCamera;

	bool isMoving = false;
	Vector3 movementDestination;

    /* On event settings */
    Animator animator;

    /* UI */
    public Text floorNumber;
	public Text health;
	public Text experienceToNextLevel;
    public Text eventText;

    // Use this for initialization
    void Start ()
    {
		// Put player in correct start room
		currentRoom = GameControl.control.startRoom;

		// Set position to one square below center in the start room
		this.transform.SetPositionAndRotation (new Vector3 (
			currentRoom.x * GameConfig.tilesPerRoom + GameConfig.tilesPerRoom/2,// Xpos
			currentRoom.y * GameConfig.tilesPerRoom + GameConfig.tilesPerRoom/2 - 1,// Ypos
			1),// ZPos
			Quaternion.identity);// Rotation

		// Add player to turn queue
		GameControl.control.addToTurnQueue(this.name);
		GameControl.control.player = this.gameObject;

        // Player attack animation 
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		// Lock cameras to current position
		mainCamera.transform.SetPositionAndRotation (new Vector3 (this.transform.position.x,
			this.transform.position.y, mainCamera.transform.position.z), Quaternion.identity);
        miniMapCamera.transform.SetPositionAndRotation(new Vector3(this.transform.position.x,
            this.transform.position.y, miniMapCamera.transform.position.z), Quaternion.identity);

        // Update ui info
        health.text = "Health: " + GameControl.control.playerData.health;
		floorNumber.text = "Floor: " + GameControl.control.playerData.floor;
		experienceToNextLevel.text = "Exp to next level: " + GameControl.control.playerData.getExperienceToNextLevel();

        // Keep moving if the player is not at destination yet
        if (isMoving) {
			// If we made it to our desination
			if (Vector3.Distance (this.transform.position, movementDestination) < .01) {
				// Stop moving
				isMoving = false;
			} else {
				// Move along our path
				this.transform.position = Vector3.MoveTowards (this.transform.position, movementDestination, GameConfig.playerMovementSpeed * Time.deltaTime);
			}
		}

		// If it is the player's turn
		if (GameControl.control.isTurn (this.name)) {
            // Is the player trying to move
            eventText.text = "";
            StartMove();
        }
    }

	void StartMove() {
		/* Player moving code */
		int x, y;

		x = (int)Input.GetAxisRaw("Horizontal");
		y = (int)Input.GetAxisRaw("Vertical");

		// If there was some input
		if (x != 0 || y != 0) {
			// This prevents diagonal move. 
			if (x != 0) {
				y = 0;
			}

			// Set our new desination
			movementDestination = this.transform.position + new Vector3(x, y, 0);

			// See if our desination is valid
			RaycastHit2D hit = Physics2D.Raycast(this.transform.position, new Vector2(x, y));

			// If our rayCast hit something
			if (hit.collider != null) {
				// Useful debug left in for future debugging help
				//Debug.Log("Hit " + hit.transform.name + " at distance " + Vector2.Distance (this.transform.position, hit.transform.position));

				// Check how far our ray went before hitting anything
				if (Vector2.Distance (this.transform.position, hit.transform.position) < 1.51) {// Distance from center of player to collider of object + some error
					// We hit something that we can't move through
					if (hit.transform.tag.Equals ("Shrine")) {
						// Handle shrine interaction
						return;// Return early to prevent moving into shrine
					} else if (hit.transform.tag.Equals ("Stairs")) {
						// Handle stair interaction
						return;// Return early to prevent moving into stairs
					} else if (hit.transform.tag.Equals ("Enemy")) {
						// Do damage to enemy
						bool killedEnemy = Attack(hit.transform);
                        if(killedEnemy)
                        {
                            // default gold and exp settings
                            int gold = 1;   
                            int exp = 1;
                            GameControl.control.playerData.addGold(gold);
                            GameControl.control.playerData.addExperience(exp);
                            eventText.text = "Collected " + gold + " coin";
                            // Debug.Log("coin collected");
                        }
                        // Attacking takes up our turn
                        GameControl.control.takeTurn ();
						return;// Return to prevent player from moving into enemy's space
					} else {
						// Wall or other non-passable object
						return;// Return so player doesnt move
					}
				}
			}

			// If we didn't return early, start moving towards destination
			isMoving = true;

			// Mark our turn as over
			GameControl.control.takeTurn ();
		}
	}

	bool Attack(Transform enemy)
    {
        /* Player attack settings */
		animator.SetTrigger("PlayerAttack");
		int damage = GameControl.control.playerData.inventory.getWeapon ().getAttackDamage ();
		bool killedEnemy = enemy.GetComponent<Enemy> ().GetHit (damage);
        return killedEnemy;
		//Debug.Log ("Hit enemy for " + damage + " damage!");
    }

	public void GetHit(int damage)
    {
        animator.SetTrigger("PlayerHit");
    }
}
