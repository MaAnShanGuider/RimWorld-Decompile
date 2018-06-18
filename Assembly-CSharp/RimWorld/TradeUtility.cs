﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000782 RID: 1922
	public static class TradeUtility
	{
		// Token: 0x06002A6F RID: 10863 RVA: 0x001673AC File Offset: 0x001657AC
		public static bool EverPlayerSellable(ThingDef def)
		{
			return def.tradeability.PlayerCanSell() && def.GetStatValueAbstract(StatDefOf.MarketValue, null) > 0f && (def.category == ThingCategory.Item || def.category == ThingCategory.Pawn || def.category == ThingCategory.Building) && (def.category != ThingCategory.Building || def.Minifiable);
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x00167440 File Offset: 0x00165840
		public static bool PlayerSellableNow(Thing t)
		{
			t = t.GetInnerIfMinified();
			bool result;
			if (!TradeUtility.EverPlayerSellable(t.def))
			{
				result = false;
			}
			else if (t.IsNotFresh())
			{
				result = false;
			}
			else
			{
				Apparel apparel = t as Apparel;
				result = (apparel == null || !apparel.WornByCorpse);
			}
			return result;
		}

		// Token: 0x06002A71 RID: 10865 RVA: 0x001674A8 File Offset: 0x001658A8
		public static void SpawnDropPod(IntVec3 dropSpot, Map map, Thing t)
		{
			DropPodUtility.MakeDropPodAt(dropSpot, map, new ActiveDropPodInfo
			{
				SingleContainedThing = t,
				leaveSlag = false
			}, false);
		}

		// Token: 0x06002A72 RID: 10866 RVA: 0x001674D4 File Offset: 0x001658D4
		public static IEnumerable<Thing> AllLaunchableThingsForTrade(Map map)
		{
			HashSet<Thing> yieldedThings = new HashSet<Thing>();
			foreach (Building_OrbitalTradeBeacon beacon in Building_OrbitalTradeBeacon.AllPowered(map))
			{
				foreach (IntVec3 c in beacon.TradeableCells)
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Thing t = thingList[i];
						if (t.def.category == ThingCategory.Item && TradeUtility.PlayerSellableNow(t) && !yieldedThings.Contains(t))
						{
							yieldedThings.Add(t);
							yield return t;
						}
					}
				}
			}
			yield break;
		}

		// Token: 0x06002A73 RID: 10867 RVA: 0x00167500 File Offset: 0x00165900
		public static IEnumerable<Pawn> AllSellableColonyPawns(Map map)
		{
			foreach (Pawn p in map.mapPawns.PrisonersOfColonySpawned)
			{
				if (p.guest.PrisonerIsSecure)
				{
					yield return p;
				}
			}
			foreach (Pawn p2 in map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (p2.RaceProps.Animal && p2.HostFaction == null && !p2.InMentalState && !p2.Downed && map.mapTemperature.SeasonAndOutdoorTemperatureAcceptableFor(p2.def))
				{
					yield return p2;
				}
			}
			yield break;
		}

		// Token: 0x06002A74 RID: 10868 RVA: 0x0016752C File Offset: 0x0016592C
		public static Thing ThingFromStockToMergeWith(ITrader trader, Thing thing)
		{
			Thing result;
			if (thing is Pawn)
			{
				result = null;
			}
			else
			{
				foreach (Thing thing2 in trader.Goods)
				{
					if (TransferableUtility.TransferAsOne(thing2, thing, TransferAsOneMode.Normal) && thing2.CanStackWith(thing) && thing2.def.stackLimit != 1)
					{
						return thing2;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06002A75 RID: 10869 RVA: 0x001675D0 File Offset: 0x001659D0
		public static void LaunchThingsOfType(ThingDef resDef, int debt, Map map, TradeShip trader)
		{
			while (debt > 0)
			{
				Thing thing = null;
				foreach (Building_OrbitalTradeBeacon building_OrbitalTradeBeacon in Building_OrbitalTradeBeacon.AllPowered(map))
				{
					foreach (IntVec3 c in building_OrbitalTradeBeacon.TradeableCells)
					{
						foreach (Thing thing2 in map.thingGrid.ThingsAt(c))
						{
							if (thing2.def == resDef)
							{
								thing = thing2;
								goto IL_D8;
							}
						}
					}
				}
				IL_D8:
				if (thing == null)
				{
					Log.Error("Could not find any " + resDef + " to transfer to trader.", false);
					break;
				}
				int num = Math.Min(debt, thing.stackCount);
				if (trader != null)
				{
					trader.GiveSoldThingToTrader(thing, num, TradeSession.playerNegotiator);
				}
				else
				{
					thing.SplitOff(num).Destroy(DestroyMode.Vanish);
				}
				debt -= num;
			}
		}

		// Token: 0x06002A76 RID: 10870 RVA: 0x00167744 File Offset: 0x00165B44
		public static void LaunchSilver(Map map, int fee)
		{
			TradeUtility.LaunchThingsOfType(ThingDefOf.Silver, fee, map, null);
		}

		// Token: 0x06002A77 RID: 10871 RVA: 0x00167754 File Offset: 0x00165B54
		public static Map PlayerHomeMapWithMostLaunchableSilver()
		{
			return (from x in Find.Maps
			where x.IsPlayerHome
			select x).MaxBy((Map x) => (from t in TradeUtility.AllLaunchableThingsForTrade(x)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount));
		}

		// Token: 0x06002A78 RID: 10872 RVA: 0x001677B4 File Offset: 0x00165BB4
		public static bool ColonyHasEnoughSilver(Map map, int fee)
		{
			return (from t in TradeUtility.AllLaunchableThingsForTrade(map)
			where t.def == ThingDefOf.Silver
			select t).Sum((Thing t) => t.stackCount) >= fee;
		}

		// Token: 0x06002A79 RID: 10873 RVA: 0x0016781C File Offset: 0x00165C1C
		public static void CheckInteractWithTradersTeachOpportunity(Pawn pawn)
		{
			if (!pawn.Dead)
			{
				Lord lord = pawn.GetLord();
				if (lord != null && lord.CurLordToil is LordToil_DefendTraderCaravan)
				{
					LessonAutoActivator.TeachOpportunity(ConceptDefOf.InteractingWithTraders, pawn, OpportunityType.Important);
				}
			}
		}

		// Token: 0x06002A7A RID: 10874 RVA: 0x00167868 File Offset: 0x00165C68
		public static float GetPricePlayerSell(Thing thing, float priceFactorSell_TraderPriceType, float priceGain_PlayerNegotiator, float priceGain_FactionBase)
		{
			float statValue = thing.GetStatValue(StatDefOf.SellPriceFactor, true);
			float num = thing.MarketValue * 0.5f * priceFactorSell_TraderPriceType * statValue * (1f - Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f + priceGain_PlayerNegotiator + priceGain_FactionBase;
			num = Mathf.Max(num, 0.01f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x06002A7B RID: 10875 RVA: 0x001678DC File Offset: 0x00165CDC
		public static float GetPricePlayerBuy(Thing thing, float priceFactorBuy_TraderPriceType, float priceGain_PlayerNegotiator, float priceGain_FactionBase)
		{
			float num = thing.MarketValue * 1.5f * priceFactorBuy_TraderPriceType * (1f + Find.Storyteller.difficulty.tradePriceFactorLoss);
			num *= 1f - priceGain_PlayerNegotiator - priceGain_FactionBase;
			num = Mathf.Max(num, 0.5f);
			if (num > 99.5f)
			{
				num = Mathf.Round(num);
			}
			return num;
		}

		// Token: 0x040016DD RID: 5853
		public const float MinimumBuyPrice = 0.5f;

		// Token: 0x040016DE RID: 5854
		public const float MinimumSellPrice = 0.01f;

		// Token: 0x040016DF RID: 5855
		public const float PriceFactorBuy_Global = 1.5f;

		// Token: 0x040016E0 RID: 5856
		public const float PriceFactorSell_Global = 0.5f;
	}
}
