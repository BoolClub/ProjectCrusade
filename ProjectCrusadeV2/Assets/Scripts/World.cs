using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// The class creates all of the sprites for the game environment.
/// </summary>
public class World : MonoBehaviour
{
	// Constants
	const float TILE_SCALE = 3.2f;
	const int TILE_SIZE = 32;

	/// <summary>
	/// The size of the world on the x-axis.
	/// </summary>
	public int Dimension_X;

	/// <summary>
	/// The size of the world on the y-axis;
	/// </summary>
	public int Dimension_Y;

	/// <summary>
	/// The name of the file that contains the data for the world.
	/// </summary>
	public string FileName;

	/// <summary>
	/// The game map.
	/// </summary>
	int[,] GameMap;

	/// <summary>
	/// The tiles as game objects that will appear on screen.
	/// </summary>
	[HideInInspector]
	public GameObject[,] Tiles { get; set; }

	/// <summary>
	/// The background holder. Holds the floor holder and the wall holder.
	/// 0 is the overall background holder, 1 is the floor holder, and 2 is the wall holder.
	/// </summary>
	public GameObject[] BackgroundHolder;

	/// <summary>
	/// The tile sprites.
	/// </summary>
	public Sprite[] Tile_Sprites;



	// Use this for initialization
	void Start()
	{
		BackgroundHolder[0].transform.position = new Vector3(0, 0, 0);
		Tiles = new GameObject[Dimension_X + 1, Dimension_Y + 1];

		//Load the map data from Tiled
		GameMap = LoadMapData();

		//Create the game world.
		InstantiateWorld();
	}


	/// <summary>
	/// Creates the tile gameobject.
	/// </summary>
	/// <returns>The tile.</returns>
	/// <param name="name">Name.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public void CreateTile(string name, int x, int y, int sprite_index)
	{
		GameObject obj = new GameObject(name);
		obj.transform.position = new Vector3(x + TILE_SIZE, y + TILE_SIZE, 0);
		obj.transform.localScale = new Vector3(TILE_SCALE, TILE_SCALE, 1);
		obj.isStatic = true;
		obj.AddComponent(typeof(SpriteRenderer));
		obj.GetComponent<SpriteRenderer>().sprite = Tile_Sprites[sprite_index];
		if (name.Equals("Wall"))
		{
			obj.AddComponent(typeof(BoxCollider2D));
			obj.GetComponent<BoxCollider2D>().size = new Vector2(0.46f, 0.46f);
			obj.transform.SetParent(BackgroundHolder[2].transform);
			obj.tag = "Wall";
		}
		else {
			obj.transform.SetParent(BackgroundHolder[1].transform);
		}

		//Trigger Box Collider for doors
		if (sprite_index == (int)UnitySpriteIndices.RED_DOOR_LEFT || sprite_index == (int)UnitySpriteIndices.RED_DOOR_RIGHT || sprite_index == (int)UnitySpriteIndices.BLUE_DOOR_LEFT || sprite_index == (int)UnitySpriteIndices.BLUE_DOOR_RIGHT)
		{
			obj.AddComponent(typeof(BoxCollider2D));
			obj.GetComponent<BoxCollider2D>().isTrigger = true;
			obj.AddComponent(typeof(Door));
			obj.GetComponent<Door>().Position = new Vector2(x, y);

			if (x == 15 && y == Dimension_Y - 17) obj.GetComponent<Door>().Destination = "House_1";
			if (x == 31 && y == Dimension_Y - 20) obj.GetComponent<Door>().Destination = "Church";
			if (x == 32 && y == Dimension_Y - 20) obj.GetComponent<Door>().Destination = "Church";
			if (x == 17 && y == Dimension_Y - 32) obj.GetComponent<Door>().Destination = "House_1";
			if (x == 35 && y == Dimension_Y - 33) obj.GetComponent<Door>().Destination = "House_1";
			if (x == 19 && y == Dimension_Y - 1) obj.GetComponent<Door>().Destination = "Underground_1";
			if (x == 20 && y == Dimension_Y - 1) obj.GetComponent<Door>().Destination = "Underground_1";
		}

		Tiles[x, y] = obj;
	}


	/// <summary>
	/// Loads the map data from Tiled.
	/// </summary>
	/// <returns>The map data.</returns>
	int[,] LoadMapData()
	{
		StreamReader reader = new StreamReader("/Users/adeolauthman/Documents/AdeolasCodingStuff/CSharp_Projects/ProjectCrusadeV2/Assets/GameMaps/" + FileName);
		string content = reader.ReadToEnd();
		string[] array = content.Split(',');
		int[] g = new int[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			g[i] = int.Parse(array[i]);
		}

		int[,] array2D = ArrayConversion.Make2DArray(g, Dimension_Y, Dimension_Y);

		return array2D;
	}


	/// <summary>
	/// Creates the entire game world.
	/// </summary>
	/// <returns>The world.</returns>
	void InstantiateWorld()
	{
		//Loop through and create tiles based on the numbers in the game map.
		for (int i = 0; i < Dimension_X; i++)
		{
			for (int j = 0; j < Dimension_Y; j++)
			{
				//Just create a bunch of grass tiles
				//CreateTile("Tile", i, j, (int)UnitySpriteIndices.GRASS_TILE);
				//if(equals(i,j, 2, Dimension_Y - 3)) CreateTile("Wall", i, j, (int)UnitySpriteIndices.TREE_TOP);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GRASS_TILE)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.GRASS_TILE);

				if (GameMap[i, j] == (int)TiledSpriteIndices.TREE_TOP)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.TREE_TOP);

				if (GameMap[i, j] == (int)TiledSpriteIndices.TREE_BOTTOM)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.TREE_BOTTOM);

				if (GameMap[i, j] == (int)TiledSpriteIndices.FLOWER_GRASS)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.FLOWER_GRASS);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CLOSED_TREASURE_CHEST)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CLOSED_TREASURE_CHEST);

				if (GameMap[i, j] == (int)TiledSpriteIndices.OPENED_TREASURE_CHEST)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.OPENED_TREASURE_CHEST);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ROCK)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ROCK);

				if (GameMap[i, j] == (int)TiledSpriteIndices.WALKING_AREA_TOP_LEFT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.WALKING_AREA_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.WALKING_AREA_LEFT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.WALKING_AREA_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.WALKING_AREA_TOP)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.WALKING_AREA_TOP);

				if (GameMap[i, j] == (int)TiledSpriteIndices.WALKING_AREA_TOP_RIGHT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.WALKING_AREA_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.WALKING_AREA_LEFT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.WALKING_AREA_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.WALKING_AREA_CENTER)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.WALKING_AREA_CENTER);

				if (GameMap[i, j] == (int)TiledSpriteIndices.WALKING_AREA_RIGHT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.WALKING_AREA_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.WALKING_AREA_BOTTOM_LEFT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.WALKING_AREA_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.WALKING_AREA_BOTTOM)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.WALKING_AREA_BOTTOM);

				if (GameMap[i, j] == (int)TiledSpriteIndices.WALKING_AREA_BOTTOM_RIGHT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.WALKING_AREA_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STREET_LAMP_TOP)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STREET_LAMP_TOP);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STREET_LAMP_BOTTOM)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STREET_LAMP_BOTTOM);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_ROOF1_TOP_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_ROOF1_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_ROOF1_TOP)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_ROOF1_TOP);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_ROOF1_TOP_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_ROOF1_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_ROOF1_BOTTOM_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_ROOF1_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_ROOF1_BOTTOM)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_ROOF1_BOTTOM);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_ROOF1_BOTTOM_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_ROOF1_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_ROOF2_TOP_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_ROOF2_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_ROOF2_TOP)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_ROOF2_TOP);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_ROOF2_TOP_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_ROOF2_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_ROOF2_BOTTOM_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_ROOF2_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_ROOF2_BOTTOM)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_ROOF2_BOTTOM);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_ROOF2_BOTTOM_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_ROOF2_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_WALL)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_WALL);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_LEFT1)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_LEFT1);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_CENTER1)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_CENTER1);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_RIGHT1)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_RIGHT1);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_LEFT2)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_LEFT2);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_CENTER2)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_CENTER2);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_RIGHT2)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_RIGHT2);

				if (GameMap[i, j] == (int)TiledSpriteIndices.HOUSE_WINDOW)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.HOUSE_WINDOW);

				if (GameMap[i, j] == (int)TiledSpriteIndices.RED_DOOR_LEFT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.RED_DOOR_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.RED_DOOR_RIGHT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.RED_DOOR_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.BLUE_DOOR_LEFT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.BLUE_DOOR_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.BLUE_DOOR_RIGHT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.BLUE_DOOR_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_FLOOR)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_FLOOR);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_BOTTOM_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_BOTTOM_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_TOP_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_TOP_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_WALL_TOP_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_WALL_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_WALL_TOP)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_WALL_TOP);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_WALL_TOP_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_WALL_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_WALL_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_WALL_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_WALL_CENTER)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_WALL_CENTER);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_WALL_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_WALL_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_WALL_BOTTOM_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_WALL_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_WALL_BOTTOM)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_WALL_BOTTOM);

				if (GameMap[i, j] == (int)TiledSpriteIndices.CAVE_WALL_BOTTOM_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.CAVE_WALL_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_FLOOR)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_FLOOR);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_BOTTOM_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_BOTTOM_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_TOP_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_TOP_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_WALL_TOP_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_WALL_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_WALL_TOP)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_WALL_TOP);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_WALL_TOP_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_WALL_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_WALL_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_WALL_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_WALL_CENTER)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_WALL_CENTER);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_WALL_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_WALL_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_WALL_BOTTOM_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_WALL_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_WALL_BOTTOM)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_WALL_BOTTOM);

				if (GameMap[i, j] == (int)TiledSpriteIndices.ICE_WALL_BOTTOM_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.ICE_WALL_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_FLOOR)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_FLOOR);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_BOTTOM_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_BOTTOM_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_TOP_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_TOP_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_WALL_TOP_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_WALL_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_WALL_TOP)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_WALL_TOP);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_WALL_TOP_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_WALL_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_WALL_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_WALL_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_WALL_CENTER)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_WALL_CENTER);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_WALL_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_WALL_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_WALL_BOTTOM_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_WALL_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_WALL_BOTTOM)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_WALL_BOTTOM);

				if (GameMap[i, j] == (int)TiledSpriteIndices.SAND_WALL_BOTTOM_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.SAND_WALL_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_FLOOR)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_FLOOR);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_BOTTOM_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_BOTTOM_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_TOP_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_TOP_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_WALL_TOP_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_WALL_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_WALL_TOP)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_WALL_TOP);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_WALL_TOP_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_WALL_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_WALL_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_WALL_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_WALL_CENTER)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_WALL_CENTER);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_WALL_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_WALL_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_WALL_BOTTOM_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_WALL_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_WALL_BOTTOM)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_WALL_BOTTOM);

				if (GameMap[i, j] == (int)TiledSpriteIndices.GREEN_WALL_BOTTOM_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.GREEN_WALL_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_CENTER)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_CENTER);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_CORNER_TOP_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_CORNER_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_TOP)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_TOP);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_CORNER_TOP_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_CORNER_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_CORNER_BOTTOM_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_CORNER_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_BOTTOM)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_BOTTOM);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_CORNER_BOTTOM_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_CORNER_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_TOP_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_TOP_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_BOTTOM_LEFT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_BOTTOM_RIGHT)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STONE_WALL_CENTER)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.STONE_WALL_CENTER);

				if (GameMap[i, j] == (int)TiledSpriteIndices.RED_CARPET_TOP_LEFT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.RED_CARPET_TOP_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.RED_CARPET_TOP)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.RED_CARPET_TOP);

				if (GameMap[i, j] == (int)TiledSpriteIndices.RED_CARPET_TOP_RIGHT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.RED_CARPET_TOP_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.RED_CARPET_LEFT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.RED_CARPET_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.RED_CARPET_CENTER)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.RED_CARPET_CENTER);

				if (GameMap[i, j] == (int)TiledSpriteIndices.RED_CARPET_RIGHT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.RED_CARPET_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.RED_CARPET_BOTTOM_LEFT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.RED_CARPET_BOTTOM_LEFT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.RED_CARPET_BOTTOM)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.RED_CARPET_BOTTOM);
				
				if (GameMap[i, j] == (int)TiledSpriteIndices.RED_CARPET_BOTTOM_RIGHT)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.RED_CARPET_BOTTOM_RIGHT);

				if (GameMap[i, j] == (int)TiledSpriteIndices.BOUNDARY_TILE)
					CreateTile("Wall", i, Dimension_Y - j, (int)UnitySpriteIndices.BOUNDARY_TILE);

				if (GameMap[i, j] == (int)TiledSpriteIndices.WOOD_FLOOR)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.WOOD_FLOOR);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STREET_LAMP_2_TOP)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.STREET_LAMP_2_TOP);

				if (GameMap[i, j] == (int)TiledSpriteIndices.STREET_LAMP_2_BOTTOM)
					CreateTile("Tile", i, Dimension_Y - j, (int)UnitySpriteIndices.STREET_LAMP_2_BOTTOM);
			}
		}
	}

}
