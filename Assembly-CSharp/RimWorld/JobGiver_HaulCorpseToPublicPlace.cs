﻿using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_HaulCorpseToPublicPlace : ThinkNode_JobGiver
	{
		public JobGiver_HaulCorpseToPublicPlace()
		{
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			MentalState_CorpseObsession mentalState_CorpseObsession = pawn.MentalState as MentalState_CorpseObsession;
			if (mentalState_CorpseObsession == null || mentalState_CorpseObsession.corpse == null)
			{
				return null;
			}
			Corpse corpse = mentalState_CorpseObsession.corpse;
			Building_Grave building_Grave = mentalState_CorpseObsession.corpse.ParentHolder as Building_Grave;
			if (building_Grave != null)
			{
				if (!pawn.CanReserveAndReach(building_Grave, PathEndMode.InteractionCell, Danger.Deadly, 1, -1, null, false))
				{
					return null;
				}
			}
			else if (!pawn.CanReserveAndReach(corpse, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			{
				return null;
			}
			return new Job(JobDefOf.HaulCorpseToPublicPlace, corpse, building_Grave)
			{
				count = 1
			};
		}
	}
}
