﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000101 RID: 257
	public class JoyGiver_SocialRelax : JoyGiver
	{
		// Token: 0x06000562 RID: 1378 RVA: 0x0003A81C File Offset: 0x00038C1C
		public override Job TryGiveJob(Pawn pawn)
		{
			return this.TryGiveJobInt(pawn, null);
		}

		// Token: 0x06000563 RID: 1379 RVA: 0x0003A83C File Offset: 0x00038C3C
		public override Job TryGiveJobInPartyArea(Pawn pawn, IntVec3 partySpot)
		{
			return this.TryGiveJobInt(pawn, (CompGatherSpot x) => PartyUtility.InPartyArea(x.parent.Position, partySpot, pawn.Map));
		}

		// Token: 0x06000564 RID: 1380 RVA: 0x0003A880 File Offset: 0x00038C80
		private Job TryGiveJobInt(Pawn pawn, Predicate<CompGatherSpot> gatherSpotValidator)
		{
			Job result;
			if (pawn.Map.gatherSpotLister.activeSpots.Count == 0)
			{
				result = null;
			}
			else
			{
				JoyGiver_SocialRelax.workingSpots.Clear();
				for (int i = 0; i < pawn.Map.gatherSpotLister.activeSpots.Count; i++)
				{
					JoyGiver_SocialRelax.workingSpots.Add(pawn.Map.gatherSpotLister.activeSpots[i]);
				}
				CompGatherSpot compGatherSpot;
				while (JoyGiver_SocialRelax.workingSpots.TryRandomElement(out compGatherSpot))
				{
					JoyGiver_SocialRelax.workingSpots.Remove(compGatherSpot);
					if (!compGatherSpot.parent.IsForbidden(pawn))
					{
						if (pawn.CanReach(compGatherSpot.parent, PathEndMode.Touch, Danger.None, false, TraverseMode.ByPawn))
						{
							if (compGatherSpot.parent.IsSociallyProper(pawn))
							{
								if (compGatherSpot.parent.IsPoliticallyProper(pawn))
								{
									if (gatherSpotValidator == null || gatherSpotValidator(compGatherSpot))
									{
										Job job;
										Thing t2;
										if (compGatherSpot.parent.def.surfaceType == SurfaceType.Eat)
										{
											Thing t;
											if (!JoyGiver_SocialRelax.TryFindChairBesideTable(compGatherSpot.parent, pawn, out t))
											{
												return null;
											}
											job = new Job(this.def.jobDef, compGatherSpot.parent, t);
										}
										else if (JoyGiver_SocialRelax.TryFindChairNear(compGatherSpot.parent.Position, pawn, out t2))
										{
											job = new Job(this.def.jobDef, compGatherSpot.parent, t2);
										}
										else
										{
											IntVec3 c;
											if (!JoyGiver_SocialRelax.TryFindSitSpotOnGroundNear(compGatherSpot.parent.Position, pawn, out c))
											{
												return null;
											}
											job = new Job(this.def.jobDef, compGatherSpot.parent, c);
										}
										if (pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
										{
											Thing thing;
											if (JoyGiver_SocialRelax.TryFindIngestibleToNurse(compGatherSpot.parent.Position, pawn, out thing))
											{
												job.targetC = thing;
												job.count = Mathf.Min(thing.stackCount, thing.def.ingestible.maxNumToIngestAtOnce);
											}
										}
										return job;
									}
								}
							}
						}
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06000565 RID: 1381 RVA: 0x0003AB00 File Offset: 0x00038F00
		private static bool TryFindIngestibleToNurse(IntVec3 center, Pawn ingester, out Thing ingestible)
		{
			bool result;
			if (ingester.IsTeetotaler())
			{
				ingestible = null;
				result = false;
			}
			else if (ingester.drugs == null)
			{
				ingestible = null;
				result = false;
			}
			else
			{
				JoyGiver_SocialRelax.nurseableDrugs.Clear();
				DrugPolicy currentPolicy = ingester.drugs.CurrentPolicy;
				for (int i = 0; i < currentPolicy.Count; i++)
				{
					if (currentPolicy[i].allowedForJoy && currentPolicy[i].drug.ingestible.nurseable)
					{
						JoyGiver_SocialRelax.nurseableDrugs.Add(currentPolicy[i].drug);
					}
				}
				JoyGiver_SocialRelax.nurseableDrugs.Shuffle<ThingDef>();
				for (int j = 0; j < JoyGiver_SocialRelax.nurseableDrugs.Count; j++)
				{
					List<Thing> list = ingester.Map.listerThings.ThingsOfDef(JoyGiver_SocialRelax.nurseableDrugs[j]);
					if (list.Count > 0)
					{
						Predicate<Thing> validator = (Thing t) => ingester.CanReserve(t, 1, -1, null, false) && !t.IsForbidden(ingester);
						ingestible = GenClosest.ClosestThing_Global_Reachable(center, ingester.Map, list, PathEndMode.OnCell, TraverseParms.For(ingester, Danger.Deadly, TraverseMode.ByPawn, false), 40f, validator, null);
						if (ingestible != null)
						{
							return true;
						}
					}
				}
				ingestible = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06000566 RID: 1382 RVA: 0x0003AC78 File Offset: 0x00039078
		private static bool TryFindChairBesideTable(Thing table, Pawn sitter, out Thing chair)
		{
			for (int i = 0; i < 30; i++)
			{
				IntVec3 c = table.RandomAdjacentCellCardinal();
				Building edifice = c.GetEdifice(table.Map);
				if (edifice != null && edifice.def.building.isSittable && sitter.CanReserve(edifice, 1, -1, null, false))
				{
					chair = edifice;
					return true;
				}
			}
			chair = null;
			return false;
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x0003ACF8 File Offset: 0x000390F8
		private static bool TryFindChairNear(IntVec3 center, Pawn sitter, out Thing chair)
		{
			for (int i = 0; i < JoyGiver_SocialRelax.RadialPatternMiddleOutward.Count; i++)
			{
				IntVec3 c = center + JoyGiver_SocialRelax.RadialPatternMiddleOutward[i];
				Building edifice = c.GetEdifice(sitter.Map);
				if (edifice != null && edifice.def.building.isSittable && sitter.CanReserve(edifice, 1, -1, null, false) && !edifice.IsForbidden(sitter) && GenSight.LineOfSight(center, edifice.Position, sitter.Map, true, null, 0, 0))
				{
					chair = edifice;
					return true;
				}
			}
			chair = null;
			return false;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x0003ADB0 File Offset: 0x000391B0
		private static bool TryFindSitSpotOnGroundNear(IntVec3 center, Pawn sitter, out IntVec3 result)
		{
			for (int i = 0; i < 30; i++)
			{
				IntVec3 intVec = center + GenRadial.RadialPattern[Rand.Range(1, JoyGiver_SocialRelax.NumRadiusCells)];
				if (sitter.CanReserveAndReach(intVec, PathEndMode.OnCell, Danger.None, 1, -1, null, false) && intVec.GetEdifice(sitter.Map) == null && GenSight.LineOfSight(center, intVec, sitter.Map, true, null, 0, 0))
				{
					result = intVec;
					return true;
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x040002DD RID: 733
		private static List<CompGatherSpot> workingSpots = new List<CompGatherSpot>();

		// Token: 0x040002DE RID: 734
		private const float GatherRadius = 3.9f;

		// Token: 0x040002DF RID: 735
		private static readonly int NumRadiusCells = GenRadial.NumCellsInRadius(3.9f);

		// Token: 0x040002E0 RID: 736
		private static readonly List<IntVec3> RadialPatternMiddleOutward = (from c in GenRadial.RadialPattern.Take(JoyGiver_SocialRelax.NumRadiusCells)
		orderby Mathf.Abs((c - IntVec3.Zero).LengthHorizontal - 1.95f)
		select c).ToList<IntVec3>();

		// Token: 0x040002E1 RID: 737
		private static List<ThingDef> nurseableDrugs = new List<ThingDef>();
	}
}
