﻿using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020006B1 RID: 1713
	public class Building_Sarcophagus : Building_Grave
	{
		// Token: 0x060024BC RID: 9404 RVA: 0x0013A41E File Offset: 0x0013881E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.everNonEmpty, "everNonEmpty", false, false);
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x0013A43C File Offset: 0x0013883C
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			bool result;
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				this.thisIsFirstBodyEver = !this.everNonEmpty;
				this.everNonEmpty = true;
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x0013A480 File Offset: 0x00138880
		public override void Notify_CorpseBuried(Pawn worker)
		{
			base.Notify_CorpseBuried(worker);
			if (this.thisIsFirstBodyEver && worker.IsColonist && base.Corpse.InnerPawn.def.race.Humanlike && !base.Corpse.everBuriedInSarcophagus)
			{
				base.Corpse.everBuriedInSarcophagus = true;
				foreach (Pawn pawn in base.Map.mapPawns.FreeColonists)
				{
					if (pawn.needs.mood != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowBuriedInSarcophagus, null);
					}
				}
			}
		}

		// Token: 0x04001441 RID: 5185
		private bool everNonEmpty = false;

		// Token: 0x04001442 RID: 5186
		private bool thisIsFirstBodyEver = false;
	}
}
