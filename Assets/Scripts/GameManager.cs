using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Temporary game initiation for testing
public class GameManager : MonoBehaviour {

	public RoomManager roomScript;

	private int level = 1;

	// Use this for initialization
	void Awake () {
		roomScript = GetComponent<RoomManager> ();
		InitGame();
	}

	void InitGame()
	{
		roomScript.SetupRoom (level, true, true, true, true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
