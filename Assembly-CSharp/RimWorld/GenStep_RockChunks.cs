﻿using System;
using System.Collections.Generic;
using Verse;
using Verse.Noise;

namespace RimWorld
{
	public class GenStep_RockChunks : GenStep
	{
		private ModuleBase freqFactorNoise = null;

		private const float ThreshLooseRock = 0.55f;

		private const float PlaceProbabilityPerCell = 0.006f;

		private const float RubbleProbability = 0.5f;

		public GenStep_RockChunks()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 1898758716;
			}
		}

		public override void Generate(Map map)
		{
			if (!map.TileInfo.WaterCovered)
			{
				this.freqFactorNoise = new Perlin(0.014999999664723873, 2.0, 0.5, 6, Rand.Range(0, 999999), QualityMode.Medium);
				this.freqFactorNoise = new ScaleBias(1.0, 1.0, this.freqFactorNoise);
				NoiseDebugUI.StoreNoiseRender(this.freqFactorNoise, "rock_chunks_freq_factor");
				MapGenFloatGrid elevation = MapGenerator.Elevation;
				foreach (IntVec3 intVec in map.AllCells)
				{
					float num = 0.006f * this.freqFactorNoise.GetValue(intVec);
					if (elevation[intVec] < 0.55f && Rand.Value < num)
					{
						this.GrowLowRockFormationFrom(intVec, map);
					}
				}
				this.freqFactorNoise = null;
			}
		}

		private void GrowLowRockFormationFrom(IntVec3 root, Map map)
		{
			ThingDef filth_RubbleRock = ThingDefOf.Filth_RubbleRock;
			ThingDef mineableThing = Find.World.NaturalRockTypesIn(map.Tile).RandomElement<ThingDef>().building.mineableThing;
			Rot4 random = Rot4.Random;
			MapGenFloatGrid elevation = MapGenerator.Elevation;
			IntVec3 intVec = root;
			for (;;)
			{
				Rot4 random2;
				do
				{
					random2 = Rot4.Random;
				}
				while (random2 == random);
				intVec += random2.FacingCell;
				if (!intVec.InBounds(map) || intVec.GetEdifice(map) != null || intVec.GetFirstItem(map) != null)
				{
					break;
				}
				if (elevation[intVec] > 0.55f)
				{
					break;
				}
				if (!map.terrainGrid.TerrainAt(intVec).affordances.Contains(TerrainAffordanceDefOf.Heavy))
				{
					break;
				}
				GenSpawn.Spawn(mineableThing, intVec, map, WipeMode.Vanish);
				IntVec3[] adjacentCellsAndInside = GenAdj.AdjacentCellsAndInside;
				int i = 0;
				while (i < adjacentCellsAndInside.Length)
				{
					IntVec3 b = adjacentCellsAndInside[i];
					if (Rand.Value < 0.5f)
					{
						IntVec3 c = intVec + b;
						if (c.InBounds(map))
						{
							bool flag = false;
							List<Thing> thingList = c.GetThingList(map);
							for (int j = 0; j < thingList.Count; j++)
							{
								Thing thing = thingList[j];
								if (thing.def.category != ThingCategory.Plant && thing.def.category != ThingCategory.Item && thing.def.category != ThingCategory.Pawn)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								FilthMaker.MakeFilth(c, map, filth_RubbleRock, 1);
							}
						}
					}
					IL_1AD:
					i++;
					continue;
					goto IL_1AD;
				}
			}
		}
	}
}
