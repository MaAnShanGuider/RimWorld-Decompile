﻿using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020006F9 RID: 1785
	public class ThingSetMaker_Meteorite : ThingSetMaker
	{
		// Token: 0x060026D7 RID: 9943 RVA: 0x0014D1FD File Offset: 0x0014B5FD
		public static void Reset()
		{
			ThingSetMaker_Meteorite.nonSmoothedMineables.Clear();
			ThingSetMaker_Meteorite.nonSmoothedMineables.AddRange(from x in DefDatabase<ThingDef>.AllDefsListForReading
			where x.mineable && x != ThingDefOf.CollapsedRocks && !x.IsSmoothed
			select x);
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x0014D23C File Offset: 0x0014B63C
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			IntRange? countRange = parms.countRange;
			int randomInRange = ((countRange == null) ? ThingSetMaker_Meteorite.MineablesCountRange : countRange.Value).RandomInRange;
			ThingDef def = this.FindRandomMineableDef();
			for (int i = 0; i < randomInRange; i++)
			{
				Building building = (Building)ThingMaker.MakeThing(def, null);
				building.canChangeTerrainOnDestroyed = false;
				outThings.Add(building);
			}
		}

		// Token: 0x060026D9 RID: 9945 RVA: 0x0014D2B8 File Offset: 0x0014B6B8
		private ThingDef FindRandomMineableDef()
		{
			float value = Rand.Value;
			ThingDef result;
			if (value < 0.4f)
			{
				result = (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where !x.building.isResourceRock
				select x).RandomElement<ThingDef>();
			}
			else if (value < 0.75f)
			{
				result = (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue < 5f
				select x).RandomElement<ThingDef>();
			}
			else
			{
				result = (from x in ThingSetMaker_Meteorite.nonSmoothedMineables
				where x.building.isResourceRock && x.building.mineableThing.BaseMarketValue >= 5f
				select x).RandomElement<ThingDef>();
			}
			return result;
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x0014D37C File Offset: 0x0014B77C
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return ThingSetMaker_Meteorite.nonSmoothedMineables;
		}

		// Token: 0x04001595 RID: 5525
		public static List<ThingDef> nonSmoothedMineables = new List<ThingDef>();

		// Token: 0x04001596 RID: 5526
		public static readonly IntRange MineablesCountRange = new IntRange(8, 20);

		// Token: 0x04001597 RID: 5527
		private const float PreciousMineableMarketValue = 5f;
	}
}
