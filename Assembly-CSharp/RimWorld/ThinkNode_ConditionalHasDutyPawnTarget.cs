﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalHasDutyPawnTarget : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalHasDutyPawnTarget()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.Thing is Pawn;
		}
	}
}
