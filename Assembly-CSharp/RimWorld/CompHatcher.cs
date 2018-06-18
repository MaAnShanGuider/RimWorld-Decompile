﻿using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071A RID: 1818
	public class CompHatcher : ThingComp
	{
		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x060027E9 RID: 10217 RVA: 0x00155214 File Offset: 0x00153614
		public CompProperties_Hatcher Props
		{
			get
			{
				return (CompProperties_Hatcher)this.props;
			}
		}

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x060027EA RID: 10218 RVA: 0x00155234 File Offset: 0x00153634
		private CompTemperatureRuinable FreezerComp
		{
			get
			{
				return this.parent.GetComp<CompTemperatureRuinable>();
			}
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x060027EB RID: 10219 RVA: 0x00155254 File Offset: 0x00153654
		public bool TemperatureDamaged
		{
			get
			{
				CompTemperatureRuinable freezerComp = this.FreezerComp;
				return freezerComp != null && this.FreezerComp.Ruined;
			}
		}

		// Token: 0x060027EC RID: 10220 RVA: 0x00155284 File Offset: 0x00153684
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.gestateProgress, "gestateProgress", 0f, false);
			Scribe_References.Look<Pawn>(ref this.hatcheeParent, "hatcheeParent", false);
			Scribe_References.Look<Pawn>(ref this.otherParent, "otherParent", false);
			Scribe_References.Look<Faction>(ref this.hatcheeFaction, "hatcheeFaction", false);
		}

		// Token: 0x060027ED RID: 10221 RVA: 0x001552E4 File Offset: 0x001536E4
		public override void CompTick()
		{
			if (!this.TemperatureDamaged)
			{
				float num = 1f / (this.Props.hatcherDaystoHatch * 60000f);
				this.gestateProgress += num;
				if (this.gestateProgress > 1f)
				{
					this.Hatch();
				}
			}
		}

		// Token: 0x060027EE RID: 10222 RVA: 0x0015533C File Offset: 0x0015373C
		public void Hatch()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(this.Props.hatcherPawn, this.hatcheeFaction, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, false, false, false, false, null, null, null, null, null, null, null, null);
			for (int i = 0; i < this.parent.stackCount; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, this.parent))
				{
					if (pawn != null)
					{
						if (this.hatcheeParent != null)
						{
							if (pawn.playerSettings != null && this.hatcheeParent.playerSettings != null && this.hatcheeParent.Faction == this.hatcheeFaction)
							{
								pawn.playerSettings.AreaRestriction = this.hatcheeParent.playerSettings.AreaRestriction;
							}
							if (pawn.RaceProps.IsFlesh)
							{
								pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.hatcheeParent);
							}
						}
						if (this.otherParent != null && (this.hatcheeParent == null || this.hatcheeParent.gender != this.otherParent.gender))
						{
							if (pawn.RaceProps.IsFlesh)
							{
								pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, this.otherParent);
							}
						}
					}
					if (this.parent.Spawned)
					{
						FilthMaker.MakeFilth(this.parent.Position, this.parent.Map, ThingDefOf.Filth_AmnioticFluid, 1);
					}
				}
				else
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
			}
			this.parent.Destroy(DestroyMode.Vanish);
		}

		// Token: 0x060027EF RID: 10223 RVA: 0x00155518 File Offset: 0x00153918
		public override void PreAbsorbStack(Thing otherStack, int count)
		{
			float t = (float)count / (float)(this.parent.stackCount + count);
			CompHatcher comp = ((ThingWithComps)otherStack).GetComp<CompHatcher>();
			float b = comp.gestateProgress;
			this.gestateProgress = Mathf.Lerp(this.gestateProgress, b, t);
		}

		// Token: 0x060027F0 RID: 10224 RVA: 0x00155560 File Offset: 0x00153960
		public override void PostSplitOff(Thing piece)
		{
			CompHatcher comp = ((ThingWithComps)piece).GetComp<CompHatcher>();
			comp.gestateProgress = this.gestateProgress;
			comp.hatcheeParent = this.hatcheeParent;
			comp.otherParent = this.otherParent;
			comp.hatcheeFaction = this.hatcheeFaction;
		}

		// Token: 0x060027F1 RID: 10225 RVA: 0x001555AA File Offset: 0x001539AA
		public override void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PrePreTraded(action, playerNegotiator, trader);
			if (action == TradeAction.PlayerBuys)
			{
				this.hatcheeFaction = Faction.OfPlayer;
			}
			else if (action == TradeAction.PlayerSells)
			{
				this.hatcheeFaction = trader.Faction;
			}
		}

		// Token: 0x060027F2 RID: 10226 RVA: 0x001555E0 File Offset: 0x001539E0
		public override void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			base.PostPostGeneratedForTrader(trader, forTile, forFaction);
			this.hatcheeFaction = forFaction;
		}

		// Token: 0x060027F3 RID: 10227 RVA: 0x001555F4 File Offset: 0x001539F4
		public override string CompInspectStringExtra()
		{
			string result;
			if (!this.TemperatureDamaged)
			{
				result = "EggProgress".Translate() + ": " + this.gestateProgress.ToStringPercent();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x040015E4 RID: 5604
		private float gestateProgress = 0f;

		// Token: 0x040015E5 RID: 5605
		public Pawn hatcheeParent = null;

		// Token: 0x040015E6 RID: 5606
		public Pawn otherParent = null;

		// Token: 0x040015E7 RID: 5607
		public Faction hatcheeFaction = null;
	}
}
