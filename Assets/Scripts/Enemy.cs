using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Moving
{
    public int health;
    public int type; //type of enemy, to be done in inspector
    public int attackPower;
    public int speed = 10;

    /* On event settings */
    Animator animator;

    float range = 1f; //range only applied to long distance attacking foes

    public static Enemy enemy;  

    public void setStats()
    {
        switch(type)
        {
            case 1: //slime
                health = (int)(.25 * GameControl.control.playerData.getMaxHealth());
                attackPower = (int)(.25 * GameControl.control.playerData.floor * health);
                break; 
            case 2: //skeleton
                health = (int)(.35 * GameControl.control.playerData.getMaxHealth());
                attackPower = (int)(.35 * GameControl.control.playerData.floor * health);
                break; 
            case 3: //boss
                health = (int)(.5 * GameControl.control.playerData.getMaxHealth());
                attackPower = (int)(.5 * GameControl.control.playerData.floor * health);
                break; 
            default:
                health = (int)(1 * GameControl.control.playerData.getMaxHealth());
                attackPower = (int)(1 * GameControl.control.playerData.floor * health);
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
        setStats();
        base.Awake();
    }
	
	// Update is called once per frame
	void Update ()
	{
        /* Enemy dies */
        if(this.health <= 0)
        {
            //dissapear
            base.StopAllCoroutines(); //dunno if this works
        }

        /* Attacking */
        Attack();

        /* Enemy moving */
        int x = 0, y = 0;

        Input.GetButton(name);
        //for now, the enemy will move opposite of what the player does.
        switch (name)
        {
            case ("Up"):
                y -= 1;
                break;
            case ("Down"):
                y += 1;
                break;
            case ("Right"):
                x -= 1;
                break;
            case ("Left"):
                x += 1;
                break;
            default: break;
        }

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
        bool playerPresent = false;//default for now
        if (playerPresent)
        {
            animator.SetTrigger("EnemyAttack");
            Player.player.PlayerGetHit();
            //lower player health
        }
    }
}
