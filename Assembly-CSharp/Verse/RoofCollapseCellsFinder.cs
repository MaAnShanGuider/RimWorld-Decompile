﻿using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000C9C RID: 3228
	public static class RoofCollapseCellsFinder
	{
		// Token: 0x06004720 RID: 18208 RVA: 0x00258778 File Offset: 0x00256B78
		public static void Notify_RoofHolderDespawned(Thing t, Map map)
		{
			if (Current.ProgramState == ProgramState.Playing)
			{
				RoofCollapseCellsFinder.ProcessRoofHolderDespawned(t.OccupiedRect(), t.Position, map, false);
			}
		}

		// Token: 0x06004721 RID: 18209 RVA: 0x002587A0 File Offset: 0x00256BA0
		public static void ProcessRoofHolderDespawned(CellRect rect, IntVec3 position, Map map, bool removalMode = false)
		{
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(rect, map);
			RoofGrid roofGrid = map.roofGrid;
			RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar.Clear();
			for (int i = 0; i < RoofCollapseUtility.RoofSupportRadialCellsCount; i++)
			{
				IntVec3 intVec = position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(map))
				{
					if (roofGrid.Roofed(intVec.x, intVec.z))
					{
						if (!map.roofCollapseBuffer.IsMarkedToCollapse(intVec))
						{
							if (!RoofCollapseUtility.WithinRangeOfRoofHolder(intVec, map, false))
							{
								if (removalMode)
								{
									map.roofGrid.SetRoof(intVec, null);
								}
								else
								{
									map.roofCollapseBuffer.MarkToCollapse(intVec);
								}
								RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar.Add(intVec);
							}
						}
					}
				}
			}
			RoofCollapseCellsFinder.CheckCollapseFlyingRoofs(RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar, map, removalMode);
			RoofCollapseCellsFinder.roofsCollapsingBecauseTooFar.Clear();
		}

		// Token: 0x06004722 RID: 18210 RVA: 0x00258894 File Offset: 0x00256C94
		public static void RemoveBulkCollapsingRoofs(List<IntVec3> nearCells, Map map)
		{
			for (int i = 0; i < nearCells.Count; i++)
			{
				RoofCollapseCellsFinder.ProcessRoofHolderDespawned(new CellRect(nearCells[i].x, nearCells[i].z, 1, 1), nearCells[i], map, true);
			}
		}

		// Token: 0x06004723 RID: 18211 RVA: 0x002588F0 File Offset: 0x00256CF0
		public static void CheckCollapseFlyingRoofs(List<IntVec3> nearCells, Map map, bool removalMode = false)
		{
			RoofCollapseCellsFinder.visitedCells.Clear();
			for (int i = 0; i < nearCells.Count; i++)
			{
				RoofCollapseCellsFinder.CheckCollapseFlyingRoofAtAndAdjInternal(nearCells[i], map, removalMode);
			}
			RoofCollapseCellsFinder.visitedCells.Clear();
		}

		// Token: 0x06004724 RID: 18212 RVA: 0x0025893C File Offset: 0x00256D3C
		public static void CheckCollapseFlyingRoofs(CellRect nearRect, Map map)
		{
			RoofCollapseCellsFinder.visitedCells.Clear();
			CellRect.CellRectIterator iterator = nearRect.GetIterator();
			while (!iterator.Done())
			{
				RoofCollapseCellsFinder.CheckCollapseFlyingRoofAtAndAdjInternal(iterator.Current, map, false);
				iterator.MoveNext();
			}
			RoofCollapseCellsFinder.visitedCells.Clear();
		}

		// Token: 0x06004725 RID: 18213 RVA: 0x00258990 File Offset: 0x00256D90
		private static bool CheckCollapseFlyingRoofAtAndAdjInternal(IntVec3 root, Map map, bool removalMode)
		{
			RoofCollapseBuffer roofCollapseBuffer = map.roofCollapseBuffer;
			if (removalMode && roofCollapseBuffer.CellsMarkedToCollapse.Count > 0)
			{
				map.roofCollapseBufferResolver.CollapseRoofsMarkedToCollapse();
			}
			for (int i = 0; i < 5; i++)
			{
				IntVec3 intVec = root + GenAdj.CardinalDirectionsAndInside[i];
				if (intVec.InBounds(map))
				{
					if (intVec.Roofed(map))
					{
						if (!RoofCollapseCellsFinder.visitedCells.Contains(intVec))
						{
							if (!roofCollapseBuffer.IsMarkedToCollapse(intVec))
							{
								if (!RoofCollapseCellsFinder.ConnectsToRoofHolder(intVec, map, RoofCollapseCellsFinder.visitedCells))
								{
									map.floodFiller.FloodFill(intVec, (IntVec3 x) => x.Roofed(map), delegate(IntVec3 x)
									{
										roofCollapseBuffer.MarkToCollapse(x);
									}, int.MaxValue, false, null);
									if (removalMode)
									{
										List<IntVec3> cellsMarkedToCollapse = roofCollapseBuffer.CellsMarkedToCollapse;
										for (int j = cellsMarkedToCollapse.Count - 1; j >= 0; j--)
										{
											RoofDef roofDef = map.roofGrid.RoofAt(cellsMarkedToCollapse[j]);
											if (roofDef != null && roofDef.VanishOnCollapse)
											{
												map.roofGrid.SetRoof(cellsMarkedToCollapse[j], null);
												cellsMarkedToCollapse.RemoveAt(j);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06004726 RID: 18214 RVA: 0x00258B44 File Offset: 0x00256F44
		public static bool ConnectsToRoofHolder(IntVec3 c, Map map, HashSet<IntVec3> visitedCells)
		{
			bool connected = false;
			map.floodFiller.FloodFill(c, (IntVec3 x) => x.Roofed(map) && !connected, delegate(IntVec3 x)
			{
				if (visitedCells.Contains(x))
				{
					connected = true;
				}
				else
				{
					visitedCells.Add(x);
					for (int i = 0; i < 5; i++)
					{
						IntVec3 c2 = x + GenAdj.CardinalDirectionsAndInside[i];
						if (c2.InBounds(map))
						{
							Building edifice = c2.GetEdifice(map);
							if (edifice != null && edifice.def.holdsRoof)
							{
								connected = true;
								break;
							}
						}
					}
				}
			}, int.MaxValue, false, null);
			return connected;
		}

		// Token: 0x0400305C RID: 12380
		private static List<IntVec3> roofsCollapsingBecauseTooFar = new List<IntVec3>();

		// Token: 0x0400305D RID: 12381
		private static HashSet<IntVec3> visitedCells = new HashSet<IntVec3>();
	}
}
