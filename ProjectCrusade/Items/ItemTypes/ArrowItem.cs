﻿using System;

namespace ProjectCrusade
{
	public abstract class ArrowItem : WeaponItem
	{
		public ArrowItem ()
		{
			//Can stack arrows
			Stackable = true;
		}

		//All arrows have same behavior
		public override void PrimaryUse (Player player)
		{
			throw new NotImplementedException ();
		}
		public override void SecondaryUse (Player player)
		{
			throw new NotImplementedException ();
		}
	}
}

