﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_GetJoyInPartyArea : JobGiver_GetJoy
	{
		public JobGiver_GetJoyInPartyArea()
		{
		}

		protected override Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			Job result;
			if (pawn.mindState.duty == null)
			{
				result = null;
			}
			else if (pawn.needs.joy == null)
			{
				result = null;
			}
			else
			{
				float curLevelPercentage = pawn.needs.joy.CurLevelPercentage;
				if (curLevelPercentage > 0.92f)
				{
					result = null;
				}
				else
				{
					IntVec3 cell = pawn.mindState.duty.focus.Cell;
					result = def.Worker.TryGiveJobInPartyArea(pawn, cell);
				}
			}
			return result;
		}
	}
}
