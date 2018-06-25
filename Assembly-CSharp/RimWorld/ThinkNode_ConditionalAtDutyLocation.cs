﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalAtDutyLocation : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalAtDutyLocation()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.Position == pawn.mindState.duty.focus.Cell;
		}
	}
}
