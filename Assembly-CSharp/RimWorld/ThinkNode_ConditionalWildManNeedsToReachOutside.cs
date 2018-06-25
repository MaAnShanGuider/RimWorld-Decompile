﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalWildManNeedsToReachOutside : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalWildManNeedsToReachOutside()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsWildMan() && !pawn.mindState.wildManEverReachedOutside;
		}
	}
}
