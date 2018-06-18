﻿using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000A6D RID: 2669
	public class MentalStateWorker_Jailbreaker : MentalStateWorker
	{
		// Token: 0x06003B4A RID: 15178 RVA: 0x001F68F0 File Offset: 0x001F4CF0
		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && JailbreakerMentalStateUtility.FindPrisoner(pawn) != null;
		}
	}
}
