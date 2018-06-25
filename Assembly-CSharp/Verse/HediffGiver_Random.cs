﻿using System;

namespace Verse
{
	public class HediffGiver_Random : HediffGiver
	{
		public float mtbDays = 0f;

		public HediffGiver_Random()
		{
		}

		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (Rand.MTBEventOccurs(this.mtbDays, 60000f, 60f))
			{
				if (base.TryApply(pawn, null))
				{
					base.SendLetter(pawn, cause);
				}
			}
		}
	}
}
