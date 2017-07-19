using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using System.Collections.Generic;
using System;

public class RoomCreationTest {

	[Test]
	public void testRoomDoors (){

		// create a new room without doors. 
		Room room = new Room (false, false, false, false, false, false); 

		// get the 4 doors. 
		bool isThereNorthDoor = room.getDoor (Room.Direction.North); 
		bool isThereEastDoor = room.getDoor (Room.Direction.East); 
		bool isThereSouthDoor = room.getDoor (Room.Direction.South); 
		bool isThereWestDoor = room.getDoor (Room.Direction.West); 

		// Compare. All should be false. 
		Assert.AreEqual (false, isThereNorthDoor); 
		Assert.AreEqual (false, isThereEastDoor); 
		Assert.AreEqual (false, isThereSouthDoor); 
		Assert.AreEqual (false, isThereWestDoor); 

	}

	[Test]
	public void testStartEndRoom (){

		// create a new room which is start room. 
		Room room = new Room (true, false, true, false, true, false); 

		// get the 4 doors. 
		bool isStartRoom = room.isStart(); 
		bool isEndRoom = room.isEnd (); 


		// Should be true. 
		Assert.AreEqual (true, isStartRoom); 

		// Should be false
		Assert.AreEqual (false, isEndRoom); 
	}

	[Test]
	public void testOpenDoor (){

		// create a new room without any open door.
		Room room = new Room (false, false, false, false, true, false); 

		// get the 4 doors. 
		bool isThereNorthDoor = room.getDoor (Room.Direction.North); 
		bool isThereEastDoor = room.getDoor (Room.Direction.East); 
		bool isThereSouthDoor = room.getDoor (Room.Direction.South); 
		bool isThereWestDoor = room.getDoor (Room.Direction.West); 

		// Compare. All should be false. 
		Assert.AreEqual (false, isThereNorthDoor); 
		Assert.AreEqual (false, isThereEastDoor); 
		Assert.AreEqual (false, isThereSouthDoor); 
		Assert.AreEqual (false, isThereWestDoor); 

		// Now open east door. 
		room.setDoor (Room.Direction.East, true); 

		// get doors again. 
		isThereNorthDoor = room.getDoor (Room.Direction.North); 
		isThereEastDoor = room.getDoor (Room.Direction.East); 
		isThereSouthDoor = room.getDoor (Room.Direction.South); 
		isThereWestDoor = room.getDoor (Room.Direction.West); 

		// Compare. Now east door should be open. 
		Assert.AreEqual (false, isThereNorthDoor); 
		Assert.AreEqual (true, isThereEastDoor); 
		Assert.AreEqual (false, isThereSouthDoor); 
		Assert.AreEqual (false, isThereWestDoor); 

		// Now open all doors. 
		room.setDoor (Room.Direction.North, true);
		room.setDoor (Room.Direction.South, true);
		room.setDoor (Room.Direction.West, true);

		// get doors again. 
		isThereNorthDoor = room.getDoor (Room.Direction.North); 
		isThereEastDoor = room.getDoor (Room.Direction.East); 
		isThereSouthDoor = room.getDoor (Room.Direction.South); 
		isThereWestDoor = room.getDoor (Room.Direction.West); 

		// Compare. Now all doors should be open. 
		Assert.AreEqual (true, isThereNorthDoor); 
		Assert.AreEqual (true, isThereEastDoor); 
		Assert.AreEqual (true, isThereSouthDoor); 
		Assert.AreEqual (true, isThereWestDoor); 
	}

	[Test]
	public void testCloseDoor (){

		// create a new room with all doors open
		Room room = new Room (true, true, true, true, true, false); 

		// get the 4 doors. 
		bool isThereNorthDoor = room.getDoor (Room.Direction.North); 
		bool isThereEastDoor = room.getDoor (Room.Direction.East); 
		bool isThereSouthDoor = room.getDoor (Room.Direction.South); 
		bool isThereWestDoor = room.getDoor (Room.Direction.West); 

		// Compare. All should be true. 
		Assert.AreEqual (true, isThereNorthDoor); 
		Assert.AreEqual (true, isThereEastDoor); 
		Assert.AreEqual (true, isThereSouthDoor); 
		Assert.AreEqual (true, isThereWestDoor); 

		// Now close east door. 
		room.setDoor (Room.Direction.East, false); 

		// get doors again. 
		isThereNorthDoor = room.getDoor (Room.Direction.North); 
		isThereEastDoor = room.getDoor (Room.Direction.East); 
		isThereSouthDoor = room.getDoor (Room.Direction.South); 
		isThereWestDoor = room.getDoor (Room.Direction.West); 

		// Compare. Now east door should be closed. 
		Assert.AreEqual (true, isThereNorthDoor); 
		Assert.AreEqual (false, isThereEastDoor); 
		Assert.AreEqual (true, isThereSouthDoor); 
		Assert.AreEqual (true, isThereWestDoor); 

		// Now close all doors. 
		room.setDoor (Room.Direction.North, false);
		room.setDoor (Room.Direction.South, false);
		room.setDoor (Room.Direction.West, false);

		// get doors again. 
		isThereNorthDoor = room.getDoor (Room.Direction.North); 
		isThereEastDoor = room.getDoor (Room.Direction.East); 
		isThereSouthDoor = room.getDoor (Room.Direction.South); 
		isThereWestDoor = room.getDoor (Room.Direction.West); 

		// Compare. Now all doors should be closed. 
		Assert.AreEqual (false, isThereNorthDoor); 
		Assert.AreEqual (false, isThereEastDoor); 
		Assert.AreEqual (false, isThereSouthDoor); 
		Assert.AreEqual (false, isThereWestDoor); 
	}

}
