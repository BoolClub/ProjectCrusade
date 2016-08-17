using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardCreator : MonoBehaviour
{
	#region References For Simplicity

		/// <summary>
		/// The game manager.
		/// </summary>
		GameManagerScript GameManager;

		/// <summary>
		/// The player.
		/// </summary>
		PlayerControls Player;

		/// <summary>
		/// The enemy holder.
		/// </summary>
		GameObject EnemyHolder;

		/// <summary>
		/// The enemies
		/// </summary>
		public GameObject[] Enemies;


	#endregion


	// The type of tile that will be laid in a specific position.
	public enum TileType
	{
		Wall, Floor,
	}


	public int columns = 100;                                 // The number of columns on the board (how wide it will be).
	public int rows = 100;                                    // The number of rows on the board (how tall it will be).
	public IntRange numRooms = new IntRange(15, 20);         // The range of the number of rooms there can be.
	public IntRange roomWidth = new IntRange(3, 10);         // The range of widths rooms can have.
	public IntRange roomHeight = new IntRange(3, 10);        // The range of heights rooms can have.
	public IntRange corridorLength = new IntRange(6, 10);    // The range of lengths corridors between rooms can have.
	public GameObject[] floorTiles;                           // An array of floor tile prefabs.
	public GameObject[] wallTiles;                            // An array of wall tile prefabs.
	public GameObject[] outerWallTiles;                       // An array of outer wall tile prefabs.

	public GameObject ladder;                                 //This is the ladder that will take the player to the next floor.

	private TileType[][] tiles;                               // A jagged array of tile types representing the board, like a grid.
	private Room[] rooms;                                     // All the rooms that are created for this board.
	private Corridor[] corridors;                             // All the corridors that connect the rooms.
	private GameObject boardHolder;                           // GameObject that acts as a container for all other tiles.

	[HideInInspector]
	public List<GameObject> Walls;                            //All of the wall game objects
	[HideInInspector]
	public List<GameObject> AllTiles;                         //All of the tiles

	private void Start()
	{
		GameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
		Player = GameObject.FindWithTag("Player").GetComponent<PlayerControls>();
		EnemyHolder = GameObject.Find("EnemyHolder");

		// Create the board holder.
		boardHolder = new GameObject("BoardHolder");
		Walls = new List<GameObject>();
		AllTiles = new List<GameObject>();

		SetupTilesArray();

		CreateRoomsAndCorridors();

		SetTilesValuesForRooms();
		SetTilesValuesForCorridors();

		InstantiateTiles();
		InstantiateOuterWalls();

		SpawnEnemies();

		//Don't spawn a ladder on the green level, that will take the player to somewhere else.
		if (SceneManager.GetActiveScene().buildIndex != 11)
		{
			ladder = Resources.Load("Ladder") as GameObject;
			Room roomToPlaceLadderIn = rooms[numRooms.Random - 1];
			Vector3 ladderPosition = new Vector3(roomToPlaceLadderIn.xPos, roomToPlaceLadderIn.yPos, -1);
			ladder.transform.position = ladderPosition;
			Instantiate(ladder, ladderPosition, Quaternion.identity);
		}

		//Set the player's position.
		Vector3 playerPos = new Vector3(rooms[0].xPos, rooms[0].yPos, -1);
		Player.StartPosition = playerPos;
	}

	void SetupTilesArray()
	{
		// Set the tiles jagged array to the correct width.
		tiles = new TileType[columns][];

		// Go through all the tile arrays...
		for (int i = 0; i < tiles.Length; i++)
		{
			// ... and set each tile array is the correct height.
			tiles[i] = new TileType[rows];
		}
	}


	void CreateRoomsAndCorridors()
	{
		// Create the rooms array with a random size.
		rooms = new Room[numRooms.Random];

		// There should be one less corridor than there is rooms.
		corridors = new Corridor[rooms.Length - 1];

		// Create the first room and corridor.
		rooms[0] = new Room();
		corridors[0] = new Corridor();

		// Setup the first room, there is no previous corridor so we do not use one.
		rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

		// Setup the first corridor using the first room.
		corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

		for (int i = 1; i < rooms.Length; i++)
		{
			// Create a room.
			rooms[i] = new Room();

			// Setup the room based on the previous corridor.
			rooms[i].SetupRoom(roomWidth, roomHeight, columns, rows, corridors[i - 1]);

			// If we haven't reached the end of the corridors array...
			if (i < corridors.Length)
			{
				// ... create a corridor.
				corridors[i] = new Corridor();

				// Setup the corridor based on the room that was just created.
				corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
			}
		}

	}


	void SetTilesValuesForRooms()
	{
		// Go through all the rooms...
		for (int i = 0; i < rooms.Length; i++)
		{
			Room currentRoom = rooms[i];

			// ... and for each room go through it's width.
			for (int j = 0; j < currentRoom.roomWidth; j++)
			{
				int xCoord = currentRoom.xPos + j;

				// For each horizontal tile, go up vertically through the room's height.
				for (int k = 0; k < currentRoom.roomHeight; k++)
				{
					int yCoord = currentRoom.yPos + k;

					// The coordinates in the jagged array are based on the room's position and it's width and height.
					tiles[xCoord][yCoord] = TileType.Floor;
				}
			}
		}
	}


	void SetTilesValuesForCorridors()
	{
		// Go through every corridor...
		for (int i = 0; i < corridors.Length; i++)
		{
			Corridor currentCorridor = corridors[i];

			// and go through it's length.
			for (int j = 0; j < currentCorridor.corridorLength; j++)
			{
				// Start the coordinates at the start of the corridor.
				int xCoord = currentCorridor.startXPos;
				int yCoord = currentCorridor.startYPos;

				// Depending on the direction, add or subtract from the appropriate
				// coordinate based on how far through the length the loop is.
				switch (currentCorridor.direction)
				{
					case Direction.North:
						yCoord += j;
						break;
					case Direction.East:
						xCoord += j;
						break;
					case Direction.South:
						yCoord -= j;
						break;
					case Direction.West:
						xCoord -= j;
						break;
				}

				// Set the tile at these coordinates to Floor.
				tiles[xCoord][yCoord] = TileType.Floor;
			}
		}
	}


	void InstantiateTiles()
	{
		// Go through all the tiles in the jagged array...
		for (int i = 0; i < tiles.Length; i++)
		{
			for (int j = 0; j < tiles[i].Length; j++)
			{
				if(tiles[i][j] == TileType.Floor) 
				{
					// ... and instantiate a floor tile for it.
					InstantiateFromArray(floorTiles, i, j);

				
				}// If the tile type is Wall...
				else if (tiles[i][j] == TileType.Wall)
				{
					// ... instantiate a wall over the top.
					//InstantiateFromArray(wallTiles, i, j);
					InstantiateWalls(wallTiles, i, j, DetermineSpriteIndex(i,j));
				}
			}
		}
	}


	void InstantiateOuterWalls()
	{
		// The outer walls are one unit left, right, up and down from the board.
		float leftEdgeX = -1f;
		float rightEdgeX = columns + 0f;
		float bottomEdgeY = -1f;
		float topEdgeY = rows + 0f;

		// Instantiate both vertical walls (one on each side).
		InstantiateVerticalOuterWall(leftEdgeX, bottomEdgeY, topEdgeY);
		InstantiateVerticalOuterWall(rightEdgeX, bottomEdgeY, topEdgeY);

		// Instantiate both horizontal walls, these are one in left and right from the outer walls.
		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, bottomEdgeY);
		InstantiateHorizontalOuterWall(leftEdgeX + 1f, rightEdgeX - 1f, topEdgeY);
	}


	void InstantiateVerticalOuterWall(float xCoord, float startingY, float endingY)
	{
		// Start the loop at the starting value for Y.
		float currentY = startingY;

		// While the value for Y is less than the end value...
		while (currentY <= endingY)
		{
			// ... instantiate an outer wall tile at the x coordinate and the current y coordinate.
			InstantiateFromArray(outerWallTiles, xCoord, currentY);

			currentY++;
		}
	}


	void InstantiateHorizontalOuterWall(float startingX, float endingX, float yCoord)
	{
		// Start the loop at the starting value for X.
		float currentX = startingX;

		// While the value for X is less than the end value...
		while (currentX <= endingX)
		{
			// ... instantiate an outer wall tile at the y coordinate and the current x coordinate.
			InstantiateFromArray(outerWallTiles, currentX, yCoord);

			currentX++;
		}
	}


	void InstantiateFromArray(GameObject[] prefabs, float xCoord, float yCoord)
	{
		// Create a random index for the array.
		int randomIndex = UnityEngine.Random.Range(0, prefabs.Length);

		// The position to be instantiated at is based on the coordinates.
		Vector3 position = new Vector3(xCoord, yCoord, 0f);

		// Create an instance of the prefab from the random index of the array.
		GameObject tileInstance = null;


		if (prefabs == outerWallTiles)
		{
			tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;
			tileInstance.AddComponent<BoxCollider2D>();

			// Set the tile's parent to the board holder.
			tileInstance.transform.parent = boardHolder.transform;

			tileInstance.tag = "Wall";
			tileInstance.layer = 9;

			//Add to the list of walls
			Walls.Add(tileInstance);
		}
		else if(prefabs == floorTiles)
		{
			tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

			tileInstance.tag = "Floor";

			// Set the tile's parent to the board holder.
			tileInstance.transform.parent = boardHolder.transform;
		}

		AllTiles.Add(tileInstance);
	}


	void InstantiateWalls(GameObject[] prefabs, float xCoord, int yCoord, int index)
	{
		Vector3 position = new Vector3(xCoord, yCoord, 0f);

		GameObject tileInstance = null;

		if (prefabs == wallTiles)
		{
			tileInstance = Instantiate(prefabs[index], position, Quaternion.identity) as GameObject;

			tileInstance.AddComponent<BoxCollider2D>();

			// Set the tile's parent to the board holder.
			tileInstance.transform.parent = boardHolder.transform;

			tileInstance.tag = "Wall";
			tileInstance.layer = 9;

			//Add to the list of walls.
			Walls.Add(tileInstance);
		}

		AllTiles.Add(tileInstance);
	}


	void SpawnEnemies()
	{
		for (int i = 0; i < corridors.Length; i++)
		{
			GameObject enemy = GameManager.Enemies[UnityEngine.Random.Range(0,GameManager.Enemies.Length - 1)] as GameObject;

			Instantiate(enemy, new Vector3(corridors[i].startXPos, corridors[i].startYPos, -1), Quaternion.identity);

			//Spawn an extra one
			if(i % 2 != 0)
				Instantiate(enemy, new Vector3(corridors[i].EndPositionX, corridors[i].EndPositionY, -1), Quaternion.identity);
		}

		Enemies = GameObject.FindGameObjectsWithTag("Enemy");

		foreach (GameObject go in Enemies)
		{
			go.transform.SetParent(EnemyHolder.transform);
			go.layer = 10;
		}
	}
		

	int DetermineSpriteIndex(int currentX, int currentY)
	{
		int index = 0;

		//Outer wall
		if (currentX == 0 || currentY == 0 || currentX == columns - 1 || currentY == rows - 1)
		{
			index = 4;
		}

		if (currentX > 0 && currentY > 0 && currentX < columns - 1 && currentY < rows - 1)
		{
			//Next to floors
			if (tiles[currentX + 1][currentY] == TileType.Floor)
				index = 0;
			
			if (tiles[currentX - 1][currentY] == TileType.Floor)
				index = 1;
			
			if (tiles[currentX][currentY + 1] == TileType.Floor)
				index = 2;
			
			if (tiles[currentX][currentY - 1] == TileType.Floor)
				index = 3;

			//All around walls
			if (tiles[currentX + 1][currentY] == TileType.Wall && tiles[currentX - 1][currentY] == TileType.Wall
			   && tiles[currentX][currentY + 1] == TileType.Wall && tiles[currentX][currentY - 1] == TileType.Wall)
				index = 4;

			//Corner tiles
			if (tiles[currentX + 1][currentY] == TileType.Wall && tiles[currentX][currentY - 1] == TileType.Wall
			    && tiles[currentX + 1][currentY - 1] == TileType.Floor)
				index = 5;

			if (tiles[currentX - 1][currentY] == TileType.Wall && tiles[currentX][currentY - 1] == TileType.Wall
				&& tiles[currentX - 1][currentY - 1] == TileType.Floor)
				index = 6;

			if (tiles[currentX + 1][currentY] == TileType.Wall && tiles[currentX][currentY + 1] == TileType.Wall
				&& tiles[currentX + 1][currentY + 1] == TileType.Floor)
				index = 7;

			if (tiles[currentX - 1][currentY] == TileType.Wall && tiles[currentX][currentY + 1] == TileType.Wall
				&& tiles[currentX - 1][currentY + 1] == TileType.Floor)
				index = 8;


			//Other corners
			if (tiles[currentX - 1][currentY] == TileType.Floor && tiles[currentX][currentY + 1] == TileType.Floor
			    && tiles[currentX + 1][currentY] == TileType.Wall && tiles[currentX][currentY - 1] == TileType.Wall)
				index = 9;

			if (tiles[currentX + 1][currentY] == TileType.Floor && tiles[currentX][currentY + 1] == TileType.Floor
				&& tiles[currentX - 1][currentY] == TileType.Wall && tiles[currentX][currentY - 1] == TileType.Wall)
				index = 10;

			if (tiles[currentX + 1][currentY] == TileType.Floor && tiles[currentX][currentY - 1] == TileType.Floor
				&& tiles[currentX - 1][currentY] == TileType.Wall && tiles[currentX][currentY + 1] == TileType.Wall)
				index = 12;

			if (tiles[currentX - 1][currentY] == TileType.Floor && tiles[currentX][currentY - 1] == TileType.Floor
				&& tiles[currentX + 1][currentY] == TileType.Wall && tiles[currentX][currentY + 1] == TileType.Wall)
				index = 11;
		}

		return index;
	}
}