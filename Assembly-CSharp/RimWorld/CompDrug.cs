﻿using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200070E RID: 1806
	public class CompDrug : ThingComp
	{
		// Token: 0x170005F0 RID: 1520
		// (get) Token: 0x06002787 RID: 10119 RVA: 0x00152C50 File Offset: 0x00151050
		public CompProperties_Drug Props
		{
			get
			{
				return (CompProperties_Drug)this.props;
			}
		}

		// Token: 0x06002788 RID: 10120 RVA: 0x00152C70 File Offset: 0x00151070
		public override void PostIngested(Pawn ingester)
		{
			if (this.Props.Addictive && ingester.RaceProps.IsFlesh)
			{
				HediffDef addictionHediffDef = this.Props.chemical.addictionHediff;
				Hediff_Addiction hediff_Addiction = AddictionUtility.FindAddictionHediff(ingester, this.Props.chemical);
				Hediff hediff = AddictionUtility.FindToleranceHediff(ingester, this.Props.chemical);
				float num = (hediff == null) ? 0f : hediff.Severity;
				if (hediff_Addiction != null)
				{
					hediff_Addiction.Severity += this.Props.existingAddictionSeverityOffset;
				}
				else if (Rand.Value < this.Props.addictiveness)
				{
					if (num >= this.Props.minToleranceToAddict)
					{
						ingester.health.AddHediff(addictionHediffDef, null, null, null);
						if (PawnUtility.ShouldSendNotificationAbout(ingester))
						{
							Find.LetterStack.ReceiveLetter("LetterLabelNewlyAddicted".Translate(new object[]
							{
								this.Props.chemical.label
							}).CapitalizeFirst(), "LetterNewlyAddicted".Translate(new object[]
							{
								ingester.LabelShort,
								this.Props.chemical.label
							}).AdjustedFor(ingester).CapitalizeFirst(), LetterDefOf.NegativeEvent, ingester, null, null);
						}
						AddictionUtility.CheckDrugAddictionTeachOpportunity(ingester);
					}
				}
				if (addictionHediffDef.causesNeed != null)
				{
					Need need = ingester.needs.AllNeeds.Find((Need x) => x.def == addictionHediffDef.causesNeed);
					if (need != null)
					{
						float needLevelOffset = this.Props.needLevelOffset;
						AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(ingester, this.Props.chemical, ref needLevelOffset);
						need.CurLevel += needLevelOffset;
					}
				}
				Hediff firstHediffOfDef = ingester.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.DrugOverdose, false);
				float num2 = (firstHediffOfDef == null) ? 0f : firstHediffOfDef.Severity;
				if (num2 < 0.9f && Rand.Value < this.Props.largeOverdoseChance)
				{
					float num3 = Rand.Range(0.85f, 0.99f);
					HealthUtility.AdjustSeverity(ingester, HediffDefOf.DrugOverdose, num3 - num2);
					if (ingester.Faction == Faction.OfPlayer)
					{
						Messages.Message("MessageAccidentalOverdose".Translate(new object[]
						{
							ingester.LabelIndefinite(),
							this.parent.LabelNoCount
						}).CapitalizeFirst(), ingester, MessageTypeDefOf.NegativeHealthEvent, true);
					}
				}
				else
				{
					float num4 = this.Props.overdoseSeverityOffset.RandomInRange / ingester.BodySize;
					if (num4 > 0f)
					{
						HealthUtility.AdjustSeverity(ingester, HediffDefOf.DrugOverdose, num4);
					}
				}
			}
			if (this.Props.isCombatEnhancingDrug && !ingester.Dead)
			{
				ingester.mindState.lastTakeCombatEnhancingDrugTick = Find.TickManager.TicksGame;
			}
			if (ingester.drugs != null)
			{
				ingester.drugs.Notify_DrugIngested(this.parent);
			}
		}
	}
}
