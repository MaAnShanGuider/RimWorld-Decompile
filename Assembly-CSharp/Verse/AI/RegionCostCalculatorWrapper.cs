﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.AI
{
	public class RegionCostCalculatorWrapper
	{
		private Map map;

		private IntVec3 endCell;

		private HashSet<Region> destRegions = new HashSet<Region>();

		private int moveTicksCardinal;

		private int moveTicksDiagonal;

		private RegionCostCalculator regionCostCalculator;

		private Region cachedRegion;

		private RegionLink cachedBestLink;

		private RegionLink cachedSecondBestLink;

		private int cachedBestLinkCost;

		private int cachedSecondBestLinkCost;

		private int cachedRegionCellPathCost;

		private bool cachedRegionIsDestination;

		private Region[] regionGrid;

		public RegionCostCalculatorWrapper(Map map)
		{
			this.map = map;
			this.regionCostCalculator = new RegionCostCalculator(map);
		}

		public void Init(CellRect end, TraverseParms traverseParms, int moveTicksCardinal, int moveTicksDiagonal, ByteGrid avoidGrid, Area allowedArea, bool drafted, List<int> disallowedCorners)
		{
			this.moveTicksCardinal = moveTicksCardinal;
			this.moveTicksDiagonal = moveTicksDiagonal;
			this.endCell = end.CenterCell;
			this.cachedRegion = null;
			this.cachedBestLink = null;
			this.cachedSecondBestLink = null;
			this.cachedBestLinkCost = 0;
			this.cachedSecondBestLinkCost = 0;
			this.cachedRegionCellPathCost = 0;
			this.cachedRegionIsDestination = false;
			this.regionGrid = this.map.regionGrid.DirectGrid;
			this.destRegions.Clear();
			if (end.Width == 1 && end.Height == 1)
			{
				Region region = this.endCell.GetRegion(this.map, RegionType.Set_Passable);
				if (region != null)
				{
					this.destRegions.Add(region);
				}
			}
			else
			{
				CellRect.CellRectIterator iterator = end.GetIterator();
				while (!iterator.Done())
				{
					IntVec3 intVec = iterator.Current;
					if (intVec.InBounds(this.map) && !disallowedCorners.Contains(this.map.cellIndices.CellToIndex(intVec)))
					{
						Region region2 = intVec.GetRegion(this.map, RegionType.Set_Passable);
						if (region2 != null)
						{
							if (region2.Allows(traverseParms, true))
							{
								this.destRegions.Add(region2);
							}
						}
					}
					iterator.MoveNext();
				}
			}
			if (this.destRegions.Count == 0)
			{
				Log.Error("Couldn't find any destination regions. This shouldn't ever happen because we've checked reachability.", false);
			}
			this.regionCostCalculator.Init(end, this.destRegions, traverseParms, moveTicksCardinal, moveTicksDiagonal, avoidGrid, allowedArea, drafted);
		}

		public int GetPathCostFromDestToRegion(int cellIndex)
		{
			Region region = this.regionGrid[cellIndex];
			IntVec3 cell = this.map.cellIndices.IndexToCell(cellIndex);
			if (region != this.cachedRegion)
			{
				this.cachedRegionIsDestination = this.destRegions.Contains(region);
				if (this.cachedRegionIsDestination)
				{
					return this.OctileDistanceToEnd(cell);
				}
				this.cachedBestLinkCost = this.regionCostCalculator.GetRegionBestDistances(region, out this.cachedBestLink, out this.cachedSecondBestLink, out this.cachedSecondBestLinkCost);
				this.cachedRegionCellPathCost = this.regionCostCalculator.RegionMedianPathCost(region);
				this.cachedRegion = region;
			}
			else if (this.cachedRegionIsDestination)
			{
				return this.OctileDistanceToEnd(cell);
			}
			int result;
			if (this.cachedBestLink != null)
			{
				int num = this.regionCostCalculator.RegionLinkDistance(cell, this.cachedBestLink, this.cachedRegionCellPathCost);
				int num3;
				if (this.cachedSecondBestLink != null)
				{
					int num2 = this.regionCostCalculator.RegionLinkDistance(cell, this.cachedSecondBestLink, this.cachedRegionCellPathCost);
					num3 = Mathf.Min(this.cachedSecondBestLinkCost + num2, this.cachedBestLinkCost + num);
				}
				else
				{
					num3 = this.cachedBestLinkCost + num;
				}
				num3 += this.OctileDistanceToEndEps(cell);
				result = num3;
			}
			else
			{
				result = 10000;
			}
			return result;
		}

		private int OctileDistanceToEnd(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, this.moveTicksCardinal, this.moveTicksDiagonal);
		}

		private int OctileDistanceToEndEps(IntVec3 cell)
		{
			int dx = Mathf.Abs(cell.x - this.endCell.x);
			int dz = Mathf.Abs(cell.z - this.endCell.z);
			return GenMath.OctileDistance(dx, dz, 2, 3);
		}
	}
}
