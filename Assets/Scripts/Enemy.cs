using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public int health = 1;
    public int attackPower;

    /* On event settings */
    Animator animator;

    float range; //range only applied to long distance attacking foes
   
    // Use this for initialization
    void Start ()
    {
		GameControl.control.addToTurnQueue (this.name);
		health = 100;// Placeholder so enemies don't destroy themselves every frame
    }
	
	// Update is called once per frame
	void Update ()
	{
        // Check if it is this object's turn
		if (GameControl.control.isTurn (this.name)) {
			//Debug.Log (this.transform.name + " just passed its turn.");
			GameControl.control.takeTurnWithoutDelay ();
		}
    }

    void Attack() {    
        animator.SetTrigger("enemyAttack");
		//GameControl.player.GetComponent<Player> ().GetHit ();
        GameControl.control.playerData.resetHealth(attackPower);
    }

	public bool GetHit(int damage) {
		health -= damage;

		// Enemy died, return status of enemy for other triggers
		if(health <= 0) {
			// Add experience to player
			//GameControl.control.playerData.addExperience(   );

			// Remove enemy from turn queue
			GameControl.control.removeFromTurnQueue(this.name);

			// Remove enemy from game
			GameObject.Destroy(this.gameObject);
            return true;
		}
        return false;
	}
}
