using UnityEngine;
using System.Collections;

/// <summary>
/// This class's only responsibility is to create the trees in the game world. The reason it is in a separate file
/// is because in Xamarin I cannot collapse methods so it would be too messy to have it all in the world class.
/// </summary>
public class TreeCreator {


	/// <summary>
	/// Creates the trees.
	/// </summary>
	/// <returns>The trees.</returns>
	public void CreateTrees(World world)
	{
		//Trees on the right
		for (int i = 31; i > 1; i -= 2)
		{
			world.CreateTile("Wall", 0, i, (int)UnitySpriteIndices.TREE_TOP);
			world.CreateTile("Wall", 0, i - 1, (int)UnitySpriteIndices.TREE_BOTTOM);
		}

		//Trees on the left
		for (int i = 31; i > 1; i -= 2)
		{
			world.CreateTile("Wall", 31, i, (int)UnitySpriteIndices.TREE_TOP);
			world.CreateTile("Wall", 31, i - 1, (int)UnitySpriteIndices.TREE_BOTTOM);
		}

		//Trees on the top and bottom
		for (int i = 0; i < world.Dimension_X; i++)
		{
			world.CreateTile("Wall", i, 1, (int)UnitySpriteIndices.TREE_TOP);
			world.CreateTile("Wall", i, 0, (int)UnitySpriteIndices.TREE_BOTTOM);
			world.CreateTile("Wall", i, 31, (int)UnitySpriteIndices.TREE_TOP);
			world.CreateTile("Wall", i, 30, (int)UnitySpriteIndices.TREE_BOTTOM);
		}
	}

}
