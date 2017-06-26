using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Moving
{
    public int health;    //type of enemy, 1 slime, 2 skeleton, 3 boss
    public int attackPower;
    public int speed = 10;

    /* On event settings */
    Animator animator;

    float range; //range only applied to long distance attacking foes

    public static Enemy enemy;  

    public Enemy(int type)
    {
        switch(type)
        {
            case 1: //slime
                health = (int)(.25 * GameControl.control.playerData.getMaxHealth());
                attackPower = (int)(.25 * GameControl.control.playerData.floor * health);
                range = 1;
                break; 
            case 2: //skeleton
                health = (int)(.35 * GameControl.control.playerData.getMaxHealth());
                attackPower = (int)(.35 * GameControl.control.playerData.floor * health);
                range = 0;
                break; 
            case 3: //boss
                health = (int)(.5 * GameControl.control.playerData.getMaxHealth());
                attackPower = (int)(.5 * GameControl.control.playerData.floor * health);
                range = 0;
                break; 
            default:
                health = (int)(1 * GameControl.control.playerData.getMaxHealth());
                attackPower = (int)(1 * GameControl.control.playerData.floor * health);
                range = 0;
                break;
        }
    }

    public void resetHealth(int damage)
    {
        this.health -= damage;
    }
   
    // Use this for initialization
    void Start ()
    {
        base.Awake();
    }
	
	// Update is called once per frame
	void Update ()
	{
        /* Enemy dies */
        if(this.health <= 0)
        {
           Destroy(this);
        }

        /* Enemy moving */
        int x = 0, y = 0;


        // need to set x and y

        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(x, y);
        Vector2 newPosition = Vector2.MoveTowards(start, end, speed * Time.deltaTime);

        RaycastHit2D hit;
        RaycastHit2D longHit;

        // This line is used for testing. 
        // This line is only seen in scene window.

        Debug.DrawLine(start, end, Color.red);

        if (IsObstacle(x, y, 0, out hit, out longHit))
        {
            Debug.Log("Hit Something");
            // Detect wall only by raycast and still be a distance from wall
            // so player can move until linecast indicate that player is touching wall.
            Enemy wallOrEnemy = longHit.transform.GetComponent<Enemy>(); //enemies shouldn't go through each other

            Player temp = hit.transform.GetComponent<Player>();

            if (temp != null)  //attack when you see a player
            {
                Attack();
            }

            if (wallOrEnemy == null && hit.transform == null)
            {
                // for debugging
                Debug.Log("I see wall");
                ToMove(newPosition);
            }

        }else
        {
            Debug.Log(x + "," + y);
            ToMove(newPosition);
        }
    }
    void Attack()
    {    
        animator.SetTrigger("enemyAttack");
        Player.player.PlayerGetHit();
        GameControl.control.playerData.resetHealth(attackPower); 
    }
}
