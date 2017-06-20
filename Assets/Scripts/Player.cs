using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	IntVector2 currentRoom;
	public Camera mainCamera;
    public Camera miniMapCamera;
    Animator animator;

	// Use this for initialization
	void Start () {
		currentRoom = GameControl.control.startRoom;

		// Set position to one square below center in the start room
		this.transform.SetPositionAndRotation (new Vector3 (
			currentRoom.x * GameConfig.tilesPerRoom + GameConfig.tilesPerRoom/2,// Xpos
			currentRoom.y * GameConfig.tilesPerRoom + GameConfig.tilesPerRoom/2 - 1,// Ypos
			1),// ZPos
			Quaternion.identity);// Rotation

        /* Player attack animation */
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		mainCamera.transform.SetPositionAndRotation (new Vector3 (this.transform.position.x,
			this.transform.position.y, mainCamera.transform.position.z), Quaternion.identity);
        miniMapCamera.transform.SetPositionAndRotation(new Vector3(this.transform.position.x,
            this.transform.position.y, miniMapCamera.transform.position.z), Quaternion.identity);

        // Is player attacking?
        Attack();

        // Is player being attacked?
        GetHit();
    }

    void Attack()
    {
        /* Player attack settings */
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("PlayerAttack");
            /*
                Lower enemy health. 
            */
        }
    }

    void GetHit()
    {
        // must be activated on enemy attacking within distance.
        // animator.SetTrigger("PlayerHit");
    }
}
