﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	public class GenStep_ManhunterPack : GenStep
	{
		public FloatRange pointsRange = new FloatRange(300f, 500f);

		private int MinRoomCells = 225;

		public GenStep_ManhunterPack()
		{
		}

		public override int SeedPart
		{
			get
			{
				return 457293335;
			}
		}

		public override void Generate(Map map, GenStepParams parms)
		{
			TraverseParms traverseParams = TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false);
			IntVec3 root;
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => x.Standable(map) && !x.Fogged(map) && map.reachability.CanReachMapEdge(x, traverseParams) && x.GetRoom(map, RegionType.Set_Passable).CellCount >= this.MinRoomCells, map, out root))
			{
				float randomInRange = this.pointsRange.RandomInRange;
				PawnKindDef animalKind;
				if (ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(randomInRange, map.Tile, out animalKind) || ManhunterPackIncidentUtility.TryFindManhunterAnimalKind(randomInRange, -1, out animalKind))
				{
					List<Pawn> list = ManhunterPackIncidentUtility.GenerateAnimals(animalKind, map.Tile, randomInRange);
					for (int i = 0; i < list.Count; i++)
					{
						IntVec3 loc = CellFinder.RandomSpawnCellForPawnNear(root, map, 10);
						GenSpawn.Spawn(list[i], loc, map, Rot4.Random, WipeMode.Vanish, false);
						list[i].mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.ManhunterPermanent, null, false, false, null, false);
					}
				}
			}
		}

		[CompilerGenerated]
		private sealed class <Generate>c__AnonStorey0
		{
			internal Map map;

			internal TraverseParms traverseParams;

			internal GenStep_ManhunterPack $this;

			public <Generate>c__AnonStorey0()
			{
			}

			internal bool <>m__0(IntVec3 x)
			{
				return x.Standable(this.map) && !x.Fogged(this.map) && this.map.reachability.CanReachMapEdge(x, this.traverseParams) && x.GetRoom(this.map, RegionType.Set_Passable).CellCount >= this.$this.MinRoomCells;
			}
		}
	}
}
