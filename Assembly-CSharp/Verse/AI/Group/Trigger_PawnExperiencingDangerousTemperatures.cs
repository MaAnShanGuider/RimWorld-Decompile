﻿using System;
using RimWorld;

namespace Verse.AI.Group
{
	public class Trigger_PawnExperiencingDangerousTemperatures : Trigger
	{
		private float temperatureHediffThreshold = 0.15f;

		public Trigger_PawnExperiencingDangerousTemperatures()
		{
		}

		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick && Find.TickManager.TicksGame % 197 == 0)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Pawn pawn = lord.ownedPawns[i];
					if (pawn.Spawned && !pawn.Dead && !pawn.Downed)
					{
						Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Heatstroke, false);
						if (firstHediffOfDef != null && firstHediffOfDef.Severity > this.temperatureHediffThreshold)
						{
							return true;
						}
						Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Hypothermia, false);
						if (firstHediffOfDef2 != null && firstHediffOfDef2.Severity > this.temperatureHediffThreshold)
						{
							return true;
						}
					}
				}
			}
			return false;
		}
	}
}
