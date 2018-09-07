﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;

namespace RimWorld
{
	public class JoyGiver_TakeDrug : JoyGiver_Ingest
	{
		private static List<ThingDef> takeableDrugs = new List<ThingDef>();

		public JoyGiver_TakeDrug()
		{
		}

		protected override Thing BestIngestItem(Pawn pawn, Predicate<Thing> extraValidator)
		{
			if (pawn.drugs == null)
			{
				return null;
			}
			Predicate<Thing> predicate = (Thing t) => this.CanIngestForJoy(pawn, t) && (extraValidator == null || extraValidator(t));
			ThingOwner<Thing> innerContainer = pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (predicate(innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			JoyGiver_TakeDrug.takeableDrugs.Clear();
			DrugPolicy currentPolicy = pawn.drugs.CurrentPolicy;
			for (int j = 0; j < currentPolicy.Count; j++)
			{
				if (currentPolicy[j].allowedForJoy)
				{
					JoyGiver_TakeDrug.takeableDrugs.Add(currentPolicy[j].drug);
				}
			}
			JoyGiver_TakeDrug.takeableDrugs.Shuffle<ThingDef>();
			for (int k = 0; k < JoyGiver_TakeDrug.takeableDrugs.Count; k++)
			{
				List<Thing> list = pawn.Map.listerThings.ThingsOfDef(JoyGiver_TakeDrug.takeableDrugs[k]);
				if (list.Count > 0)
				{
					IntVec3 position = pawn.Position;
					Map map = pawn.Map;
					List<Thing> searchSet = list;
					PathEndMode peMode = PathEndMode.OnCell;
					TraverseParms traverseParams = TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false);
					Predicate<Thing> validator = predicate;
					Thing thing = GenClosest.ClosestThing_Global_Reachable(position, map, searchSet, peMode, traverseParams, 9999f, validator, null);
					if (thing != null)
					{
						return thing;
					}
				}
			}
			return null;
		}

		public override float GetChance(Pawn pawn)
		{
			int num = 0;
			if (pawn.story != null)
			{
				num = pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			}
			if (num < 0)
			{
				return 0f;
			}
			float num2 = this.def.baseChance;
			if (num == 1)
			{
				num2 *= 2f;
			}
			if (num == 2)
			{
				num2 *= 5f;
			}
			return num2;
		}

		protected override Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			return DrugAIUtility.IngestAndTakeToInventoryJob(ingestible, pawn, 9999);
		}

		// Note: this type is marked as 'beforefieldinit'.
		static JoyGiver_TakeDrug()
		{
		}

		[CompilerGenerated]
		private sealed class <BestIngestItem>c__AnonStorey0
		{
			internal Pawn pawn;

			internal Predicate<Thing> extraValidator;

			internal JoyGiver_TakeDrug $this;

			public <BestIngestItem>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Thing t)
			{
				return this.$this.CanIngestForJoy(this.pawn, t) && (this.extraValidator == null || this.extraValidator(t));
			}
		}
	}
}
