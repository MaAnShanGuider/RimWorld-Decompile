﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class ThinkNode_ConditionalHasDutyTarget : ThinkNode_Conditional
	{
		public ThinkNode_ConditionalHasDutyTarget()
		{
		}

		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focus.IsValid;
		}
	}
}
