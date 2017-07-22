using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    /* Enemy settings */
    private int health;
    private int attackPower;
    private int maxHealth;
    public int type;

    /* On event settings */
    Animator animator;
    public Image healthBar;
    public Canvas enemyCanvas;
    float range; //range only applied to long distance attacking foes
   
    // Use this for initialization
    void Start ()
    {
		GameControl.control.addToTurnQueue (this.name);
        animator = GetComponent<Animator>();
        SetStats();

        // update at least once
        UpdateHealthBar();
    }

    void SetStats()
    {
        // set health and attack power
        this.health = GameControl.control.playerData.getMaxHealth() / 2;
        this.attackPower = (GameControl.control.playerData.floor + 1) * 2;

        // update stats according to type
        switch (type)
        {
            case 1:
                // green slime
                health += (health / 2);
                attackPower += (attackPower / 2);
                break;
            case 2:
                // red slime
                health *= 2;
                attackPower *= 2;
                break;
            case 3:
                // boss slime
                health *= 3;
                attackPower *= 3;
                break;
            default:
                //blue slime, nothing changes
                break;
        }
        maxHealth = health;
    }

    // Update is called once per frame
    void Update ()
	{
        // Check if it is this object's turn
		if (GameControl.control.isTurn (this.name))
        {
			//Debug.Log (this.transform.name + " just passed its turn.");
			GameControl.control.takeTurnWithoutDelay ();
        }

        UpdateHealthBar();
    }

    void UpdateHealthBar()
    {
        // set healthbar above enemy, canvas is used for healthbar
        // healthbar will follow the player around
        float offset = 0.5f;
        enemyCanvas.transform.SetPositionAndRotation(new Vector3(this.transform.position.x, this.transform.position.y + offset,
            this.transform.position.z), Quaternion.identity);
        healthBar.transform.SetPositionAndRotation(new Vector3(this.transform.position.x, this.transform.position.y + offset,
            this.transform.position.z), Quaternion.identity);

        // Reduce health bar by damage percentage
        float fillAmount = ((float)health / (float)maxHealth);
        healthBar.fillAmount = fillAmount;
        // Debug.Log (fillAmount + " = " + health + " / " + maxHealth);
    }

    void Attack()
    {
        // Trigger respective animation
        switch (type)
        {
            case 1:
                // green slime
                animator.SetTrigger("GreenSlimeAttack");
                break;
            case 2:
                // red slime
                animator.SetTrigger("RedSlimeAttack");
                break;
            case 3:
                // boss slime
                animator.SetTrigger("RedSlimeAttack");
                break;
            default:
                //blue slime
                animator.SetTrigger("BlueSlimeAttack");
                break;
        }
        Debug.Log(attackPower);
        GameControl.control.playerData.resetHealth(attackPower);
		GameControl.control.player.GetComponent<Player> ().PlayHitAnimation ();
    }

	public int GetHit(int damage)
    {
		health -= damage;
        // Enemy died, return status of enemy for other triggers
        // Attack when its hit
        Attack();
        if (health <= 0)
        {
			// Remove enemy from turn queue
			GameControl.control.removeFromTurnQueue(this.name);

            // Remove enemy from game
            animator.SetTrigger("SlimeDeath");
            GameObject.Destroy(this.gameObject,1);
            return type;
		}
        return -1;
	}
}
