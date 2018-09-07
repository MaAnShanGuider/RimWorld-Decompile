﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	public class FeatureWorker_OuterOcean : FeatureWorker
	{
		private List<int> group = new List<int>();

		private List<int> edgeTiles = new List<int>();

		public FeatureWorker_OuterOcean()
		{
		}

		public override void GenerateWhereAppropriate()
		{
			WorldGrid worldGrid = Find.WorldGrid;
			int tilesCount = worldGrid.TilesCount;
			this.edgeTiles.Clear();
			for (int i = 0; i < tilesCount; i++)
			{
				if (this.IsRoot(i))
				{
					this.edgeTiles.Add(i);
				}
			}
			if (!this.edgeTiles.Any<int>())
			{
				return;
			}
			this.group.Clear();
			WorldFloodFiller worldFloodFiller = Find.WorldFloodFiller;
			int rootTile = -1;
			Predicate<int> passCheck = (int x) => this.CanTraverse(x);
			Func<int, int, bool> processor = delegate(int tile, int traversalDist)
			{
				this.group.Add(tile);
				return false;
			};
			List<int> extraRootTiles = this.edgeTiles;
			worldFloodFiller.FloodFill(rootTile, passCheck, processor, int.MaxValue, extraRootTiles);
			this.group.RemoveAll((int x) => worldGrid[x].feature != null);
			if (this.group.Count < this.def.minSize || this.group.Count > this.def.maxSize)
			{
				return;
			}
			base.AddFeature(this.group, this.group);
		}

		private bool IsRoot(int tile)
		{
			WorldGrid worldGrid = Find.WorldGrid;
			return worldGrid.IsOnEdge(tile) && this.CanTraverse(tile) && worldGrid[tile].feature == null;
		}

		private bool CanTraverse(int tile)
		{
			BiomeDef biome = Find.WorldGrid[tile].biome;
			return biome == BiomeDefOf.Ocean || biome == BiomeDefOf.Lake;
		}

		[CompilerGenerated]
		private sealed class <GenerateWhereAppropriate>c__AnonStorey0
		{
			internal WorldGrid worldGrid;

			internal FeatureWorker_OuterOcean $this;

			public <GenerateWhereAppropriate>c__AnonStorey0()
			{
			}

			internal bool <>m__0(int x)
			{
				return this.$this.CanTraverse(x);
			}

			internal bool <>m__1(int tile, int traversalDist)
			{
				this.$this.group.Add(tile);
				return false;
			}

			internal bool <>m__2(int x)
			{
				return this.worldGrid[x].feature != null;
			}
		}
	}
}
