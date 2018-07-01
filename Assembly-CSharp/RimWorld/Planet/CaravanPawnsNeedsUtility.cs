﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public static class CaravanPawnsNeedsUtility
	{
		private static List<Thing> tmpInvFood = new List<Thing>();

		public static void TrySatisfyPawnsNeeds(Caravan caravan)
		{
			List<Pawn> pawnsListForReading = caravan.PawnsListForReading;
			for (int i = pawnsListForReading.Count - 1; i >= 0; i--)
			{
				CaravanPawnsNeedsUtility.TrySatisfyPawnNeeds(pawnsListForReading[i], caravan);
			}
		}

		private static void TrySatisfyPawnNeeds(Pawn pawn, Caravan caravan)
		{
			if (!pawn.Dead)
			{
				List<Need> allNeeds = pawn.needs.AllNeeds;
				for (int i = 0; i < allNeeds.Count; i++)
				{
					Need need = allNeeds[i];
					Need_Rest need_Rest = need as Need_Rest;
					Need_Food need_Food = need as Need_Food;
					Need_Chemical need_Chemical = need as Need_Chemical;
					Need_Joy need_Joy = need as Need_Joy;
					if (need_Rest != null)
					{
						CaravanPawnsNeedsUtility.TrySatisfyRestNeed(pawn, need_Rest, caravan);
					}
					else if (need_Food != null)
					{
						CaravanPawnsNeedsUtility.TrySatisfyFoodNeed(pawn, need_Food, caravan);
					}
					else if (need_Chemical != null)
					{
						CaravanPawnsNeedsUtility.TrySatisfyChemicalNeed(pawn, need_Chemical, caravan);
					}
					else if (need_Joy != null)
					{
						CaravanPawnsNeedsUtility.TrySatisfyJoyNeed(pawn, need_Joy, caravan);
					}
				}
			}
		}

		private static void TrySatisfyRestNeed(Pawn pawn, Need_Rest rest, Caravan caravan)
		{
			if (caravan.Resting)
			{
				rest.TickResting(1f);
			}
		}

		private static void TrySatisfyFoodNeed(Pawn pawn, Need_Food food, Caravan caravan)
		{
			if (food.CurCategory >= HungerCategory.Hungry)
			{
				Thing thing;
				Pawn pawn2;
				if (VirtualPlantsUtility.CanEatVirtualPlantsNow(pawn))
				{
					VirtualPlantsUtility.EatVirtualPlants(pawn);
				}
				else if (CaravanInventoryUtility.TryGetBestFood(caravan, pawn, out thing, out pawn2))
				{
					food.CurLevel += thing.Ingested(pawn, food.NutritionWanted);
					if (thing.Destroyed)
					{
						if (pawn2 != null)
						{
							pawn2.inventory.innerContainer.Remove(thing);
							caravan.RecacheImmobilizedNow();
							caravan.RecacheDaysWorthOfFood();
						}
						if (!caravan.notifiedOutOfFood && !CaravanInventoryUtility.TryGetBestFood(caravan, pawn, out thing, out pawn2))
						{
							Messages.Message("MessageCaravanRanOutOfFood".Translate(new object[]
							{
								caravan.LabelCap,
								pawn.Label
							}), caravan, MessageTypeDefOf.ThreatBig, true);
							caravan.notifiedOutOfFood = true;
						}
					}
				}
			}
		}

		private static void TrySatisfyChemicalNeed(Pawn pawn, Need_Chemical chemical, Caravan caravan)
		{
			if (chemical.CurCategory < DrugDesireCategory.Satisfied)
			{
				Thing drug;
				Pawn drugOwner;
				if (CaravanInventoryUtility.TryGetDrugToSatisfyChemicalNeed(caravan, pawn, chemical, out drug, out drugOwner))
				{
					CaravanPawnsNeedsUtility.IngestDrug(pawn, drug, drugOwner, caravan);
				}
			}
		}

		public static void IngestDrug(Pawn pawn, Thing drug, Pawn drugOwner, Caravan caravan)
		{
			float num = drug.Ingested(pawn, 0f);
			Need_Food food = pawn.needs.food;
			if (food != null)
			{
				food.CurLevel += num;
			}
			if (drug.Destroyed && drugOwner != null)
			{
				drugOwner.inventory.innerContainer.Remove(drug);
				caravan.RecacheImmobilizedNow();
				caravan.RecacheDaysWorthOfFood();
			}
		}

		private static void TrySatisfyJoyNeed(Pawn pawn, Need_Joy joy, Caravan caravan)
		{
			float currentJoyGainPerTick = CaravanPawnsNeedsUtility.GetCurrentJoyGainPerTick(pawn, caravan);
			if (currentJoyGainPerTick > 0f)
			{
				joy.GainJoy(currentJoyGainPerTick, JoyKindDefOf.Social);
			}
		}

		public static float GetCurrentJoyGainPerTick(Pawn pawn, Caravan caravan)
		{
			float result;
			if (caravan.pather.MovingNow)
			{
				result = 0f;
			}
			else
			{
				Pawn pawn2 = BestCaravanPawnUtility.FindBestEntertainingPawnFor(caravan, pawn);
				if (pawn2 == null)
				{
					result = 0f;
				}
				else
				{
					float statValue = pawn2.GetStatValue(StatDefOf.SocialImpact, true);
					result = statValue * 0.035f / 2500f;
				}
			}
			return result;
		}

		public static bool CanEatForNutritionEver(ThingDef food, Pawn pawn)
		{
			return food.IsNutritionGivingIngestible && pawn.RaceProps.CanEverEat(food) && food.ingestible.preferability > FoodPreferability.NeverForNutrition && (!food.IsDrug || !pawn.IsTeetotaler());
		}

		public static bool CanEatForNutritionNow(ThingDef food, Pawn pawn)
		{
			return CaravanPawnsNeedsUtility.CanEatForNutritionEver(food, pawn) && (!pawn.RaceProps.Humanlike || pawn.needs.food.CurCategory >= HungerCategory.Starving || food.ingestible.preferability > FoodPreferability.DesperateOnlyForHumanlikes);
		}

		public static bool CanEatForNutritionNow(Thing food, Pawn pawn)
		{
			return food.IngestibleNow && CaravanPawnsNeedsUtility.CanEatForNutritionNow(food.def, pawn);
		}

		public static float GetFoodScore(Thing food, Pawn pawn)
		{
			float num = CaravanPawnsNeedsUtility.GetFoodScore(food.def, pawn, food.GetStatValue(StatDefOf.Nutrition, true));
			if (pawn.RaceProps.Humanlike)
			{
				CompRottable compRottable = food.TryGetComp<CompRottable>();
				int a = (compRottable == null) ? int.MaxValue : compRottable.TicksUntilRotAtCurrentTemp;
				float a2 = 1f - (float)Mathf.Min(a, 3600000) / 3600000f;
				num += Mathf.Min(a2, 0.999f);
			}
			return num;
		}

		public static float GetFoodScore(ThingDef food, Pawn pawn, float singleFoodNutrition)
		{
			float result;
			if (pawn.RaceProps.Humanlike)
			{
				result = (float)food.ingestible.preferability;
			}
			else
			{
				float num = 0f;
				if (food == ThingDefOf.Kibble || food == ThingDefOf.Hay)
				{
					num = 5f;
				}
				else if (food.ingestible.preferability == FoodPreferability.DesperateOnlyForHumanlikes)
				{
					num = 4f;
				}
				else if (food.ingestible.preferability == FoodPreferability.RawBad)
				{
					num = 3f;
				}
				else if (food.ingestible.preferability == FoodPreferability.RawTasty)
				{
					num = 2f;
				}
				else if (food.ingestible.preferability < FoodPreferability.MealAwful)
				{
					num = 1f;
				}
				result = num + Mathf.Min(singleFoodNutrition / 100f, 0.999f);
			}
			return result;
		}

		public static void Notify_CaravanMemberIngestedFood(Pawn p, float nutritionIngested)
		{
			if (!p.Dead && p.needs.joy != null)
			{
				if (nutritionIngested > 0f)
				{
					Pawn pawn = BestCaravanPawnUtility.FindBestEntertainingPawnFor(p.GetCaravan(), p);
					JoyKindDef joyKind = (pawn == null) ? JoyKindDefOf.Meditative : Rand.Element<JoyKindDef>(JoyKindDefOf.Meditative, JoyKindDefOf.Social);
					float amount = 0.2f * Mathf.Min(nutritionIngested / p.needs.food.MaxLevel, 1f);
					p.needs.joy.GainJoy(amount, joyKind);
				}
			}
		}

		public static bool AnyPawnOutOfFood(Caravan c, out string malnutritionHediff)
		{
			CaravanPawnsNeedsUtility.tmpInvFood.Clear();
			List<Thing> list = CaravanInventoryUtility.AllInventoryItems(c);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def.IsNutritionGivingIngestible)
				{
					CaravanPawnsNeedsUtility.tmpInvFood.Add(list[i]);
				}
			}
			List<Pawn> pawnsListForReading = c.PawnsListForReading;
			for (int j = 0; j < pawnsListForReading.Count; j++)
			{
				Pawn pawn = pawnsListForReading[j];
				if (pawn.RaceProps.EatsFood && !VirtualPlantsUtility.CanEatVirtualPlantsNow(pawn))
				{
					bool flag = false;
					for (int k = 0; k < CaravanPawnsNeedsUtility.tmpInvFood.Count; k++)
					{
						if (CaravanPawnsNeedsUtility.CanEatForNutritionEver(CaravanPawnsNeedsUtility.tmpInvFood[k].def, pawn))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						int num = -1;
						string text = null;
						for (int l = 0; l < pawnsListForReading.Count; l++)
						{
							Hediff firstHediffOfDef = pawnsListForReading[l].health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
							if (firstHediffOfDef != null && (text == null || firstHediffOfDef.CurStageIndex > num))
							{
								num = firstHediffOfDef.CurStageIndex;
								text = firstHediffOfDef.LabelCap;
							}
						}
						malnutritionHediff = text;
						CaravanPawnsNeedsUtility.tmpInvFood.Clear();
						return true;
					}
				}
			}
			malnutritionHediff = null;
			CaravanPawnsNeedsUtility.tmpInvFood.Clear();
			return false;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static CaravanPawnsNeedsUtility()
		{
		}
	}
}
