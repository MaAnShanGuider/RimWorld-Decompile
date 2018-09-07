﻿using System;
using Verse;

namespace RimWorld
{
	public class PlaceWorker_NotUnderRoof : PlaceWorker
	{
		public PlaceWorker_NotUnderRoof()
		{
		}

		public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			if (map.roofGrid.Roofed(loc))
			{
				return new AcceptanceReport("MustPlaceUnroofed".Translate());
			}
			return true;
		}
	}
}
