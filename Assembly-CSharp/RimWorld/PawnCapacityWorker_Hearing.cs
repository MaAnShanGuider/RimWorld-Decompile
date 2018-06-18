﻿using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000477 RID: 1143
	public class PawnCapacityWorker_Hearing : PawnCapacityWorker
	{
		// Token: 0x06001411 RID: 5137 RVA: 0x000AEAE8 File Offset: 0x000ACEE8
		public override float CalculateCapacityLevel(HediffSet diffSet, List<PawnCapacityUtility.CapacityImpactor> impactors = null)
		{
			BodyPartTagDef hearingSource = BodyPartTagDefOf.HearingSource;
			return PawnCapacityUtility.CalculateTagEfficiency(diffSet, hearingSource, float.MaxValue, default(FloatRange), impactors);
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x000AEB20 File Offset: 0x000ACF20
		public override bool CanHaveCapacity(BodyDef body)
		{
			return body.HasPartWithTag(BodyPartTagDefOf.HearingSource);
		}
	}
}
