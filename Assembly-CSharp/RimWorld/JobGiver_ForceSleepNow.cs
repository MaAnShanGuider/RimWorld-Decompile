﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_ForceSleepNow : ThinkNode_JobGiver
	{
		public JobGiver_ForceSleepNow()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			return new Job(JobDefOf.LayDown, pawn.Position)
			{
				forceSleep = true
			};
		}
	}
}
