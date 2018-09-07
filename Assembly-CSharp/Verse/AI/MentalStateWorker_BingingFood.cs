﻿using System;

namespace Verse.AI
{
	public class MentalStateWorker_BingingFood : MentalStateWorker
	{
		public MentalStateWorker_BingingFood()
		{
		}

		public override bool StateCanOccur(Pawn pawn)
		{
			return base.StateCanOccur(pawn) && (!pawn.Spawned || pawn.Map.resourceCounter.TotalHumanEdibleNutrition > 10f);
		}
	}
}
