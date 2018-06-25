﻿using System;
using Verse;

namespace RimWorld
{
	public class JobGiver_AIDefendSelf : JobGiver_AIDefendPawn
	{
		public JobGiver_AIDefendSelf()
		{
		}

		protected override Pawn GetDefendee(Pawn pawn)
		{
			return pawn;
		}

		protected override float GetFlagRadius(Pawn pawn)
		{
			return pawn.mindState.duty.radius;
		}
	}
}
