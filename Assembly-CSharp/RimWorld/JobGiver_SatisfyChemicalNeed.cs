﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JobGiver_SatisfyChemicalNeed : ThinkNode_JobGiver
	{
		private static List<Need_Chemical> tmpChemicalNeeds = new List<Need_Chemical>();

		[CompilerGenerated]
		private static Func<Need_Chemical, float> <>f__am$cache0;

		public JobGiver_SatisfyChemicalNeed()
		{
		}

		public override float GetPriority(Pawn pawn)
		{
			if (pawn.needs.AllNeeds.Any((Need x) => this.ShouldSatisfy(x)))
			{
				return 9.25f;
			}
			return 0f;
		}

		protected override Job TryGiveJob(Pawn pawn)
		{
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
			List<Need> allNeeds = pawn.needs.AllNeeds;
			for (int i = 0; i < allNeeds.Count; i++)
			{
				if (this.ShouldSatisfy(allNeeds[i]))
				{
					JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Add((Need_Chemical)allNeeds[i]);
				}
			}
			if (!JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Any<Need_Chemical>())
			{
				return null;
			}
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.SortBy((Need_Chemical x) => x.CurLevel);
			for (int j = 0; j < JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Count; j++)
			{
				Thing thing = this.FindDrugFor(pawn, JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds[j]);
				if (thing != null)
				{
					JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
					return DrugAIUtility.IngestAndTakeToInventoryJob(thing, pawn, 1);
				}
			}
			JobGiver_SatisfyChemicalNeed.tmpChemicalNeeds.Clear();
			return null;
		}

		private bool ShouldSatisfy(Need need)
		{
			Need_Chemical need_Chemical = need as Need_Chemical;
			return need_Chemical != null && need_Chemical.CurCategory <= DrugDesireCategory.Desire;
		}

		private Thing FindDrugFor(Pawn pawn, Need_Chemical need)
		{
			Hediff_Addiction addictionHediff = need.AddictionHediff;
			if (addictionHediff == null)
			{
				return null;
			}
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (this.DrugValidator(pawn, addictionHediff, innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, ThingRequest.ForGroup(ThingRequestGroup.Drug), PathEndMode.ClosestTouch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false), 9999f, (Thing x) => this.DrugValidator(pawn, addictionHediff, x), null, 0, -1, false, RegionType.Set_Passable, false);
		}

		private bool DrugValidator(Pawn pawn, Hediff_Addiction addiction, Thing drug)
		{
			if (!drug.def.IsDrug)
			{
				return false;
			}
			if (drug.Spawned)
			{
				if (drug.IsForbidden(pawn))
				{
					return false;
				}
				if (!pawn.CanReserve(drug, 1, -1, null, false))
				{
					return false;
				}
				if (!drug.IsSociallyProper(pawn))
				{
					return false;
				}
			}
			CompDrug compDrug = drug.TryGetComp<CompDrug>();
			if (compDrug == null || compDrug.Props.chemical == null)
			{
				return false;
			}
			if (compDrug.Props.chemical.addictionHediff != addiction.def)
			{
				return false;
			}
			if (pawn.drugs != null && !pawn.drugs.CurrentPolicy[drug.def].allowedForAddiction && pawn.story != null)
			{
				int num = pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
				if (num <= 0 && (!pawn.InMentalState || !pawn.MentalStateDef.ignoreDrugPolicy))
				{
					return false;
				}
			}
			return true;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static JobGiver_SatisfyChemicalNeed()
		{
		}

		[CompilerGenerated]
		private bool <GetPriority>m__0(Need x)
		{
			return this.ShouldSatisfy(x);
		}

		[CompilerGenerated]
		private static float <TryGiveJob>m__1(Need_Chemical x)
		{
			return x.CurLevel;
		}

		[CompilerGenerated]
		private sealed class <FindDrugFor>c__AnonStorey0
		{
			internal Pawn pawn;

			internal Hediff_Addiction addictionHediff;

			internal JobGiver_SatisfyChemicalNeed $this;

			public <FindDrugFor>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing x)
			{
				return this.$this.DrugValidator(this.pawn, this.addictionHediff, x);
			}
		}
	}
}
