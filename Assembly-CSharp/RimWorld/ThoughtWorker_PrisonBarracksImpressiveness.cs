﻿using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_PrisonBarracksImpressiveness : ThoughtWorker_RoomImpressiveness
	{
		public ThoughtWorker_PrisonBarracksImpressiveness()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsPrisoner)
			{
				return ThoughtState.Inactive;
			}
			ThoughtState result = base.CurrentStateInternal(p);
			if (result.Active && p.GetRoom(RegionType.Set_Passable).Role == RoomRoleDefOf.PrisonBarracks)
			{
				return result;
			}
			return ThoughtState.Inactive;
		}
	}
}
