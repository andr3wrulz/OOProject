using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomManager : MonoBehaviour{

	public GameObject [] floors;
	public GameObject wall;
	public GameObject stairs;
	public GameObject shrine;
	//public List<Mob> mobs;// List of mobs to choose from

	Room [,] rooms;
	Queue<IntVector2> roomGenerationQueue;

	public RoomManager () {
		
	}

	public void Awake () {
		// Allocate our room array
		rooms = new Room[GameConfig.roomsPerLevel, GameConfig.roomsPerLevel];
		roomGenerationQueue = new Queue<IntVector2>();

		// Pick a random room to start at
		// Note that Random.Range returns [min, max) ie min is inclusive and max is exclusive
		IntVector2 startRoom = new IntVector2 (Random.Range (0, GameConfig.roomsPerLevel), Random.Range (0, GameConfig.roomsPerLevel));

		// Add our first room to the queue
		roomGenerationQueue.Enqueue (startRoom);

		// Generate our rooms
		generateRooms ();

		// Set our start room flag
		rooms[startRoom.x, startRoom.y].setStart(true);

		// Instantiate prefabs for each room
		instantiateRooms();

		// Print debug if we need to
		if (GameConfig.debugMode)
			printRoomDebugInfo ();
	}

	private void generateRooms() {
		// c# queues don't have an isEmpty and their count is O(1) anyways
		while (roomGenerationQueue.Count > 0) {
			// Pull coordinates out of queue
			IntVector2 currentRoom = roomGenerationQueue.Dequeue ();

			// Populate our options and requirements
			bool[] doorsRoomCanHave = findDoorsRoomCanHave (currentRoom);
			bool[] doorsRoomMustHave = findDoorsRoomMustHave (currentRoom);

			// If this is the last room in queue, use all the doors we can
			if (roomGenerationQueue.Count < 1) {
				rooms [currentRoom.x, currentRoom.y] = new Room (doorsRoomCanHave);
			} else if (roomGenerationQueue.Count > 1) {// Since we have several paths available, leave doors up to chance
				rooms [currentRoom.x, currentRoom.y] = new Room (maybeDoors(doorsRoomCanHave, doorsRoomMustHave));
			} else {// Must have at least one door
				// Determine door placement
				bool [] possibleDoors = maybeDoors(doorsRoomCanHave, doorsRoomMustHave);

				// Iterate until we get at least one door
				if (doorsRoomCanHave [(int)Room.Direction.North])
					rooms [currentRoom.x, currentRoom.y] = new Room (true, possibleDoors [1], possibleDoors [2], possibleDoors [3]);
				else if (doorsRoomCanHave [(int) Room.Direction.East])
					rooms [currentRoom.x, currentRoom.y] = new Room (false, true, possibleDoors [2], possibleDoors [3]);
				else if (doorsRoomCanHave [(int) Room.Direction.South])
					rooms [currentRoom.x, currentRoom.y] = new Room (false, false, true, possibleDoors [3]);
				else if (doorsRoomCanHave [(int) Room.Direction.West])
					rooms [currentRoom.x, currentRoom.y] = new Room (false, false, false, true);
				else // Fail?
					rooms [currentRoom.x, currentRoom.y] = new Room (false, false, false, false);
			}

			// Add any new possible paths to queue
			if (rooms[currentRoom.x, currentRoom.y].getDoor(Room.Direction.North) && rooms[currentRoom.x, currentRoom.y - 1] == null)
				roomGenerationQueue.Enqueue(new IntVector2 (currentRoom.x, currentRoom.y - 1));
			else if (rooms[currentRoom.x, currentRoom.y].getDoor(Room.Direction.East) && rooms[currentRoom.x + 1, currentRoom.y] == null)
				roomGenerationQueue.Enqueue(new IntVector2 (currentRoom.x + 1, currentRoom.y));
			else if (rooms[currentRoom.x, currentRoom.y].getDoor(Room.Direction.South) && rooms[currentRoom.x, currentRoom.y + 1] == null)
				roomGenerationQueue.Enqueue(new IntVector2 (currentRoom.x, currentRoom.y + 1));
			else if (rooms[currentRoom.x, currentRoom.y].getDoor(Room.Direction.West) && rooms[currentRoom.x - 1, currentRoom.y] == null)
				roomGenerationQueue.Enqueue(new IntVector2 (currentRoom.x - 1, currentRoom.y));

			// If we are out of rooms to generate, this last room is our end
			if (roomGenerationQueue.Count == 0)
				rooms[currentRoom.x, currentRoom.y].setEnd(true);
		}
	}

	private void printRoomDebugInfo () {
		for (int i = 0; i < GameConfig.roomsPerLevel; i++)
			for (int j = 0; j < GameConfig.roomsPerLevel; j++)
				if (rooms [i, j] == null)
					Debug.Log ("Room: " + i + ", " + j + " is null");
				else
					Debug.Log ("Room: " + i + ", " + j + " - Doors N: " + rooms [i, j].getDoor (Room.Direction.North) +
					" E: " + rooms [i, j].getDoor (Room.Direction.East) + " S: " + rooms [i, j].getDoor (Room.Direction.South) +
					" W: " + rooms [i, j].getDoor (Room.Direction.West) + " Start: " + rooms [i, j].isStart () + " End: " + rooms [i, j].isEnd ());
	}

	// Populates all possible doors in array form
	private bool [] maybeDoors(bool [] can, bool [] must) {
		bool[] doors = new bool[4];

		for (int i = 0; i < 4; i++)
			doors [i] = maybeDoor (can [i], must [i]);
		
		return doors;
	}

	// Determines whether or not to place a door
	private bool maybeDoor(bool can, bool must) {
		// Return true if we must, return true 50% if we can
		return must || (can && Random.value >= 0.5f);
	}

	private bool [] findDoorsRoomCanHave (IntVector2 coords) {
		bool[] doors = { true, true, true, true };// Assume we can have all initially

		// Check x
		if (coords.x == 0)// Left edge
			doors [(int) Room.Direction.West] = false;
		else if (coords.x == GameConfig.roomsPerLevel - 1)// Right edge
			doors [(int) Room.Direction.East] = false;

		// Check y
		if (coords.y == 0)// Top edge
			doors [(int) Room.Direction.North] = false;
		else if (coords.y == GameConfig.roomsPerLevel - 1)// Bottom edge
			doors [(int) Room.Direction.South] = false;

		return doors;
	}

	private bool [] findDoorsRoomMustHave (IntVector2 coords) {
		bool[] doors = { false, false, false, false };// Assume we don't have any requirements initially

		// Check room to the left if it has east door
		if (coords.x > 0 && rooms[coords.x - 1, coords.y] != null && rooms [coords.x - 1, coords.y].getDoor (Room.Direction.East))
			doors [(int)Room.Direction.West] = true;
		// Check room to the right if it has west door
		if (coords.x < GameConfig.roomsPerLevel - 1 && rooms[coords.x + 1, coords.y] != null && rooms [coords.x + 1, coords.y].getDoor (Room.Direction.West))
			doors [(int)Room.Direction.East] = true;
		// Check room above if it has south door
		if (coords.y > 0 && rooms[coords.x, coords.y - 1] != null && rooms [coords.x, coords.y - 1].getDoor (Room.Direction.South))
			doors [(int)Room.Direction.North] = true;
		// Check room below if it has north door
		if (coords.y < GameConfig.roomsPerLevel - 1 && rooms[coords.x, coords.y + 1] != null && rooms [coords.x, coords.y + 1].getDoor (Room.Direction.North))
			doors [(int)Room.Direction.South] = true;

		return doors;
	}

	private void instantiateRooms() {
		for (int x = 0; x < GameConfig.roomsPerLevel; x++)
			for (int y = 0; y < GameConfig.roomsPerLevel; y++)
				if (rooms[x,y] != null)// Don't try to spawn null rooms
					instantiateRoom (x, y);
	}

	private void instantiateRoom(int x, int y) {
		GameObject parent = new GameObject ("Room (" + x + "," + y + ")");

		Vector2 roomOffset = new Vector2 (x * 11, y * 11);

		for (int i = 0; i < GameConfig.tilesPerRoom; i++)
			for (int j = 0; j < GameConfig.tilesPerRoom; j++)
				Instantiate (getObjectToInstatiate(i, j, x, y), new Vector3(roomOffset.x + i, roomOffset.y + j, 0f), Quaternion.identity, parent.transform);
	}

	// Get the correct game object (wall, floor, shrine etc) 
	private GameObject getObjectToInstatiate(int offX, int offY, int x, int y) {
		// Store instead of repeated computation
		int center = GameConfig.tilesPerRoom / 2;

		// if we are looking at a doorway
		if ( (offX == 0 && offY == center && rooms[x, y].getDoor(Room.Direction.West))// West doorway
			|| (offX == GameConfig.tilesPerRoom - 1 && offY == center && rooms[x, y].getDoor(Room.Direction.East))// East doorway
			|| (offX == center && offY == 0 && rooms[x, y].getDoor(Room.Direction.North))// North doorway
			|| (offX == center && offY == GameConfig.tilesPerRoom - 1 && rooms[x, y].getDoor(Room.Direction.South)) )// South doorway
			return floors[Random.Range(0,floors.Length)];// Return random floorway

		// If we are on a wall
		if (offX == 0 || offY == 0 || offX == GameConfig.tilesPerRoom - 1 || offY == GameConfig.tilesPerRoom - 1)
			return wall;

		// If we are in the center of a room, check for start or end
		if (offX == center && offY == center) {
			if (rooms [x, y].isStart ())
				return shrine;
			if (rooms [x, y].isEnd ())
				return stairs;
		}


		// Else we're on the floor
		return floors[Random.Range(0,floors.Length)];// Return random floorway
	}

}

class Room {

	public enum Direction {North = 0, East = 1, South = 2, West = 3};

	bool [] doors;

	bool isStartRoom;
	bool isEndRoom;

	//List<Mob> mobs;// Mob List

	// Overloaded constructor, in the absence of arguments, assume no door and no flags
	public Room () : this (false, false, false, false, false, false) {}

	// Overloaded constructor, if no isStart or isEnd is passed, assume the room isn't a start or end
	public Room (bool northDoor, bool eastDoor, bool southDoor, bool westDoor)
		: this (northDoor, eastDoor, southDoor, westDoor, false, false) {}

	// Overloaded constructor, make it easier to just pass an array of booleans
	public Room (bool [] doors) : this (doors[0], doors[1], doors[2], doors[3], false, false) {}

	// Primary constructor
	public Room (bool northDoor, bool eastDoor, bool southDoor, bool westDoor, bool isStartRoom, bool isEndRoom) {
		// Allocate our door array
		doors = new bool[4];

		// Set the individual doors
		doors [(int) Room.Direction.North] = northDoor;
		doors [(int) Room.Direction.East] = eastDoor;
		doors [(int) Room.Direction.South] = southDoor;
		doors [(int) Room.Direction.West] = westDoor;

		// Set our flags
		this.isStartRoom = isStartRoom;
		this.isEndRoom = isEndRoom;
	}

	public bool getDoor (Room.Direction direction) {
		return doors [(int)direction];
	}

	public void setDoor (Room.Direction direction, bool state) {
		// Set the given door to a given state
		doors [(int) direction] = state;
	}

	public bool isStart () {
		return isStartRoom;
	}

	public void setStart (bool state) {
		isStartRoom = state;
	}

	public bool isEnd () {
		return isEndRoom;
	}

	public void setEnd (bool state) {
		isEndRoom = state;
	}
}

class IntVector2 {
	public int x, y;

	public IntVector2 (int x, int y) {
		this.x = x;
		this.y = y;
	}
}


/*
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
	public GameObject exitSprite;
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
				if (nDoor && (x == monColumns / 2) && y == (monRows+2)) { // North exit
					toInstantiate = exitSprite;
				} else if (sDoor && (x == monColumns / 2) && y == -3) { // South exit
					toInstantiate = exitSprite;
				} else if (wDoor && x == -3 && (y == monRows / 2)) { // West exit
					toInstantiate = exitSprite;
				} else if (eDoor && x == (monColumns+2) && (y == monRows / 2)) { // East exit
					toInstantiate = exitSprite;
				} else if(x==-3 || y==-3 || x==(monColumns+2) || y==(monRows+2)) // Outer edge but not an exit, thus a wall
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
		//Monster sprites not yet finished, cannot be implemented
		// spawnEnemyAtRandom (enemySprites, numEnemies.minimum, numEnemies.maximum);

	}
*/