using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Moving
{
    /* Room settings for player */
	IntVector2 currentRoom;
	public Camera mainCamera;
    public Camera miniMapCamera;

    /* On event settings */
    Animator animator;
    int maxHP; //to be displayed so can't change
    int currentHealth;
    int tempHealth;
    int attackPower;


    // for testing
    int count;
    //-----

    int speed = 10;

    // This is used for attacking  by bow. 
    float range = 4f;

    // Use this for initialization
    void Start ()
    {
		currentRoom = GameControl.control.startRoom;

		// Set position to one square below center in the start room
		this.transform.SetPositionAndRotation (new Vector3 (
			currentRoom.x * GameConfig.tilesPerRoom + GameConfig.tilesPerRoom/2,// Xpos
			currentRoom.y * GameConfig.tilesPerRoom + GameConfig.tilesPerRoom/2 - 1,// Ypos
			1),// ZPos
			Quaternion.identity);// Rotation

        /* Player Event settings */
        // maxHp = 50 + DR(10*level, CON, 20)
        // currentHealth = maxHP;
        // tempHealth = maxHp;
        // attackPower = ???;

        // Player attack animation 
        base.Start();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		mainCamera.transform.SetPositionAndRotation (new Vector3 (this.transform.position.x,
			this.transform.position.y, mainCamera.transform.position.z), Quaternion.identity);
        miniMapCamera.transform.SetPositionAndRotation(new Vector3(this.transform.position.x,
            this.transform.position.y, miniMapCamera.transform.position.z), Quaternion.identity);

        // Is player attacking?
        Attack();

        // Is player being attacked?
        GetHit();

        /* Player moving code */
        int x, y;

        x = (int)Input.GetAxisRaw("Horizontal");
        y = (int)Input.GetAxisRaw("Vertical");

        if (x != 0 || y != 0)
        {
            // This prevents diagonal move. 
            if (x != 0)
            {
                y = 0;
            }

            Vector2 start = transform.position;
            Vector2 end = start + new Vector2(x, y);
            Vector2 newPosition = Vector2.MoveTowards(start, end, speed * Time.deltaTime);

            RaycastHit2D hitSword;
            RaycastHit2D hitBow;

            Debug.DrawLine(start, end, Color.red);

            if (IsObstacle(x, y, range, out hitSword, out hitBow))
            {
                Debug.Log("Encounter obstacle " + count++);

                Debug.Log(hitSword.transform);
                Debug.Log(hitBow.transform);
            }else
            {
                ToMove(newPosition);
            }
        }
    }

    void Attack()
    {
        /* Player attack settings */
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("PlayerAttack");
            /*
                Lower enemy health.
                Enemy.health -= attackPower; 
            */
        }
    }

    void GetHit()
    {
    /*  To be uncommented once enemy's are implemented.
        if(currentHealth < tempHealth)
        {
            tempHealth = currentHealth;
            animator.SetTrigger("PlayerHit");
        }
    */
    }
}
