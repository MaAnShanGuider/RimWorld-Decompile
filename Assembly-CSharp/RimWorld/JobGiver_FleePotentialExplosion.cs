﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_FleePotentialExplosion : ThinkNode_JobGiver
	{
		public const float FleeDist = 9f;

		public JobGiver_FleePotentialExplosion()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			if (pawn.RaceProps.intelligence < Intelligence.Humanlike)
			{
				return null;
			}
			if (pawn.mindState.knownExploder == null)
			{
				return null;
			}
			if (!pawn.mindState.knownExploder.Spawned)
			{
				pawn.mindState.knownExploder = null;
				return null;
			}
			if (PawnUtility.PlayerForcedJobNowOrSoon(pawn))
			{
				return null;
			}
			Thing knownExploder = pawn.mindState.knownExploder;
			if ((float)(pawn.Position - knownExploder.Position).LengthHorizontalSquared > 81f)
			{
				return null;
			}
			IntVec3 c;
			if (!RCellFinder.TryFindDirectFleeDestination(knownExploder.Position, 9f, pawn, out c))
			{
				return null;
			}
			return new Job(JobDefOf.Goto, c)
			{
				locomotionUrgency = LocomotionUrgency.Sprint
			};
		}
	}
}
