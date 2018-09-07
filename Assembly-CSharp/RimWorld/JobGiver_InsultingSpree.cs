﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_InsultingSpree : ThinkNode_JobGiver
	{
		public JobGiver_InsultingSpree()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_InsultingSpree mentalState_InsultingSpree = pawn.MentalState as MentalState_InsultingSpree;
			if (mentalState_InsultingSpree == null || mentalState_InsultingSpree.target == null || !pawn.CanReach(mentalState_InsultingSpree.target, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return null;
			}
			return new Job(JobDefOf.Insult, mentalState_InsultingSpree.target);
		}
	}
}
