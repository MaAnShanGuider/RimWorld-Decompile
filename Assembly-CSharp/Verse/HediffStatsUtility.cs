﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x02000B47 RID: 2887
	public static class HediffStatsUtility
	{
		// Token: 0x06003F51 RID: 16209 RVA: 0x00215AA8 File Offset: 0x00213EA8
		public static IEnumerable<StatDrawEntry> SpecialDisplayStats(HediffStage stage, Hediff instance)
		{
			if (instance != null)
			{
				if (instance.Bleeding)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "BleedingRate".Translate(), instance.BleedRate.ToStringPercent() + "/" + "LetterDay".Translate(), 0, "");
				}
			}
			float painOffsetToDisplay = 0f;
			if (instance != null)
			{
				painOffsetToDisplay = instance.PainOffset;
			}
			else if (stage != null)
			{
				painOffsetToDisplay = stage.painOffset;
			}
			if (painOffsetToDisplay != 0f)
			{
				if (painOffsetToDisplay > 0f && painOffsetToDisplay < 0.01f)
				{
					painOffsetToDisplay = 0.01f;
				}
				if (painOffsetToDisplay < 0f && painOffsetToDisplay > -0.01f)
				{
					painOffsetToDisplay = -0.01f;
				}
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Pain".Translate(), (painOffsetToDisplay * 100f).ToString("+###0;-###0") + "%", 0, "");
			}
			float painFactorToDisplay = 1f;
			if (instance != null)
			{
				painFactorToDisplay = instance.PainFactor;
			}
			else if (stage != null)
			{
				painFactorToDisplay = stage.painFactor;
			}
			if (painFactorToDisplay != 1f)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Pain".Translate(), "x" + painFactorToDisplay.ToStringPercent(), 0, "");
			}
			if (stage != null)
			{
				if (stage.partEfficiencyOffset != 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "PartEfficiency".Translate(), stage.partEfficiencyOffset.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Offset), 0, "");
				}
			}
			List<PawnCapacityModifier> capModsToDisplay = null;
			if (instance != null)
			{
				capModsToDisplay = instance.CapMods;
			}
			else if (stage != null)
			{
				capModsToDisplay = stage.capMods;
			}
			if (capModsToDisplay != null)
			{
				for (int i = 0; i < capModsToDisplay.Count; i++)
				{
					if (capModsToDisplay[i].offset != 0f)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), (capModsToDisplay[i].offset * 100f).ToString("+#;-#") + "%", 0, "");
					}
					if (capModsToDisplay[i].postFactor != 1f)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), "x" + capModsToDisplay[i].postFactor.ToStringPercent(), 0, "");
					}
					if (capModsToDisplay[i].SetMaxDefined)
					{
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, capModsToDisplay[i].capacity.GetLabelFor(true, true).CapitalizeFirst(), "max".Translate() + " " + capModsToDisplay[i].setMax.ToStringPercent(), 0, "");
					}
				}
			}
			if (stage != null)
			{
				if (stage.AffectsMemory || stage.AffectsSocialInteractions)
				{
					StringBuilder affectsSb = new StringBuilder();
					if (stage.AffectsMemory)
					{
						if (affectsSb.Length != 0)
						{
							affectsSb.Append(", ");
						}
						affectsSb.Append("MemoryLower".Translate());
					}
					if (stage.AffectsSocialInteractions)
					{
						if (affectsSb.Length != 0)
						{
							affectsSb.Append(", ");
						}
						affectsSb.Append("SocialInteractionsLower".Translate());
					}
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Affects".Translate(), affectsSb.ToString(), 0, "");
				}
				if (stage.hungerRateFactor != 1f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HungerRate".Translate(), "x" + stage.hungerRateFactor.ToStringPercent(), 0, "");
				}
				if (stage.hungerRateFactorOffset != 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "HungerRate".Translate(), stage.hungerRateFactorOffset.ToStringSign() + stage.hungerRateFactorOffset.ToStringPercent(), 0, "");
				}
				if (stage.restFallFactor != 1f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Tiredness".Translate(), "x" + stage.restFallFactor.ToStringPercent(), 0, "");
				}
				if (stage.restFallFactorOffset != 0f)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "Tiredness".Translate(), stage.restFallFactorOffset.ToStringSign() + stage.restFallFactorOffset.ToStringPercent(), 0, "");
				}
				if (stage.makeImmuneTo != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.Basics, "PreventsInfection".Translate(), (from im in stage.makeImmuneTo
					select im.label).ToCommaList(false).CapitalizeFirst(), 0, "");
				}
				if (stage.statOffsets != null)
				{
					for (int j = 0; j < stage.statOffsets.Count; j++)
					{
						StatModifier sm = stage.statOffsets[j];
						yield return new StatDrawEntry(StatCategoryDefOf.Basics, sm.stat.LabelCap, sm.ValueToStringAsOffset, 0, "");
					}
				}
			}
			yield break;
		}
	}
}
