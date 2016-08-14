using UnityEngine;
using System.Collections;

public class ArrayConversion {

	public static int[,] Make2DArray(int[] input, int height, int width)
	{
		int[,] output = new int[height, width];
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				output[j, i] = input[i * width + j];
			}
		}
		return output;
	}

	public static void test()
	{
		int[] a = {1,2,3,4,5,6,7,8,9,10};
		int[,] b = Make2DArray(a, 2, 5);

		for (int i = 0; i < b.GetLength(0); i++)
		{
			for (int j = 0; j < b.GetLength(1); j++)
			{
				Debug.Log(b[i, j]);
			}
			Debug.Log("\n");
		}
	}

}
