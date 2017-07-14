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
        animator = GetComponent<Animator>();
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
        animator.SetTrigger("BlueSlimeAttack");
        animator.SetTrigger("GreenSlimeAttack");
        animator.SetTrigger("RedSlimeAttack");
        //GameControl.player.GetComponent<Player> ().GetHit ();
        GameControl.control.playerData.resetHealth(attackPower);
    }

	public bool GetHit(int damage) {
		health -= damage;

        // Enemy died, return status of enemy for other triggers
        // Attack when being attacked
        Attack();
		if(health <= 0) {
			// Remove enemy from turn queue
			GameControl.control.removeFromTurnQueue(this.name);

            // Remove enemy from game
            animator.SetTrigger("SlimeDeath");
            GameObject.Destroy(this.gameObject,1);
            return true;
		}
        return false;
	}
}
