﻿using System;
using Verse;

namespace RimWorld
{
	public class ThoughtWorker_Greedy : ThoughtWorker
	{
		public ThoughtWorker_Greedy()
		{
		}

		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!p.IsColonist)
			{
				return false;
			}
			Room ownedRoom = p.ownership.OwnedRoom;
			if (ownedRoom == null)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			int num = RoomStatDefOf.Impressiveness.GetScoreStageIndex(ownedRoom.GetStat(RoomStatDefOf.Impressiveness)) + 1;
			if (this.def.stages[num] != null)
			{
				return ThoughtState.ActiveAtStage(num);
			}
			return ThoughtState.Inactive;
		}
	}
}
