using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour {
	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}

	public int monColumns = 7; // Corresponds to the number of columns monsters can spawn within, room has 2 columns more to each side
	public int monRows = 7; // Corresponds to the number of rows monsters can spawn within, room has 2 rows more to each side
	public Count numEnemies = new Count (0, 3);
	public GameObject[] exitSprites;
	public GameObject[] floorSprites;
	public GameObject[] enemySprites;
	public GameObject wallSprite;

	private Transform roomHolder;
	private List <Vector3> positions = new List<Vector3>();

	// Generates the spaces for the room
	void InitializePositions()
	{
		// Removes potential existing grid positions
		// Probably not needed for our implementation, as we will likely clear the positions for all rooms at once upon generating a new floor
		positions.Clear();

		// Creates the main area of the room, inside of the walls
		for (int x = -2; x < monColumns + 2; x++) {
			for (int y = -2; y < monRows + 2; y++) {
				positions.Add (new Vector3 (x, y, 0f));
			}
		}
	}

	// Creates wall and floor of the room
	void BoardSetup(Boolean nDoor, Boolean sDoor, Boolean eDoor, Boolean wDoor)
	{
		roomHolder = new GameObject ("Room").transform;

		// Loops through all possible positions
		for (int x = -3; x < monColumns + 3; x++) {
			for (int y = -3; y < monRows + 3; y++) {
				GameObject toInstantiate = floorSprites[Random.Range (0, floorSprites.Length)];

				// Unsure how to specify that these lead to the correct rooms, still separating for future use
				if (nDoor && (x == monColumns / 2) && y == monRows) { // North exit
					toInstantiate = exitSprites[Random.Range (0, exitSprites.Length)];
				} else if (sDoor && (x == monColumns / 2) && y == -1) { // South exit
					toInstantiate = exitSprites[Random.Range (0, exitSprites.Length)];
				} else if (wDoor && x == 0 && (y == monRows / 2)) { // West exit
					toInstantiate = exitSprites[Random.Range (0, exitSprites.Length)];
				} else if (eDoor && x == monColumns && (y == monRows / 2)) { // East exit
					toInstantiate = exitSprites[Random.Range (0, exitSprites.Length)];
				} else if(x==-1 || y==-1 || x==monColumns || y==monRows) // Outer edge but not an exit, thus a wall
					toInstantiate = wallSprite;

				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent (roomHolder);
			}
		}
	}

	Vector3 RandomPosition()
	{
		int randomIndex = Random.Range (0, positions.Count);
		// Provides a random position from within the area monsters can spawn
		Vector3 random = positions[randomIndex];
		// Removes this position so that monsters do not spawn on top of each other
		positions.RemoveAt(randomIndex);

		return random;
	}

	void spawnEnemyAtRandom(GameObject[] enemySpriteArray, int minimum, int maximum)
	{
		int numEnemies = Random.Range (minimum, maximum + 1);

		// Finds a random position and spawns an enemy there
		for (int i = 0; i < numEnemies; i++) {
			Vector3 randomPosition = RandomPosition ();
			GameObject enemyChoice = enemySpriteArray [Random.Range (0, enemySpriteArray.Length)];
			Instantiate (enemyChoice, randomPosition, Quaternion.identity);
		}
	}

	public void SetupRoom(int level, Boolean nDoor, Boolean sDoor, Boolean eDoor, Boolean wDoor)
	{
		BoardSetup(nDoor, sDoor, eDoor, wDoor);
		InitializePositions();
		spawnEnemyAtRandom (enemySprites, numEnemies.minimum, numEnemies.maximum);

	}

}
