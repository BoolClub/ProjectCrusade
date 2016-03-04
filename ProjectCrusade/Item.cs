using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ProjectCrusade
{
	public class Item
	{
		public string Name;
		public string Identifier;
		public string Tooltip;
		public int Count = 1;
		public int Max;
		public string Type; 
		public List<Tuple<string,float,int,int,int>> Behavior;
		public Rectangle TextureResource;

		public T ShallowCopy<T>() where T : Item
		{
			return (T)(MemberwiseClone());
		}

		public int Add(int x){
			int remain = Count + x - Max;
			Count = remain < 0 ? Count + x : Max;
			return remain > 0 ? remain : 0;
		}

		public bool Remove(int x){
			Count=Count-x<0?0:Count-x;
			return !(Count - x < 0);
		}
	}
}

