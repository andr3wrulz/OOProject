using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	IntVector2 currentRoom;
	public Camera mainCamera;

	// Use this for initialization
	void Start () {
		currentRoom = GameControl.control.startRoom;

		// Set position to one square below center in the start room
		this.transform.SetPositionAndRotation (new Vector3 (
			currentRoom.x * GameConfig.tilesPerRoom + GameConfig.tilesPerRoom/2,// Xpos
			currentRoom.y * GameConfig.tilesPerRoom + GameConfig.tilesPerRoom/2 - 1,// Ypos
			1),// ZPos
			Quaternion.identity);// Rotation
	}
	
	// Update is called once per frame
	void Update () {
		mainCamera.transform.SetPositionAndRotation (new Vector3 (this.transform.position.x,
			this.transform.position.y, mainCamera.transform.position.z), Quaternion.identity);

	}
}
