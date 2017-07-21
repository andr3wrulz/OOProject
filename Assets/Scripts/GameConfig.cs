using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig {

	public static int backpackSlots = 10;
	public static int storageSlots = 20;

	public static int maxLevel = 40;
	public static int pointsPerLevelUp = 3;
	public static int hitpointsPerConstitutionPoint = 2;

	public static float playerMovementSpeed = 1f;// Should be units/second

	public static int tilesPerRoom = 11;// Width and height of each room in tiles, odd so there is a middle
	public static int roomsPerLevel = 5;// Width and height of each level in rooms
	public static float minRoomDensityPerLevel = 0.6f;// 60% of the floor must be populated with rooms

	public static int enemyChancePerTile = 200;
    public static int lootChancePerTile = 600;

	public static int minDistanceFromPlayerForEnemyTurn = 10;

	public static bool debugMode = true;

	public static int bowRange = 4;
	public static int wandRange = 3;
}
