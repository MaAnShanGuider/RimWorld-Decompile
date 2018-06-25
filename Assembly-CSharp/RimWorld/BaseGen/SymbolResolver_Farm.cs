﻿using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003C7 RID: 967
	public class SymbolResolver_Farm : SymbolResolver
	{
		// Token: 0x060010AF RID: 4271 RVA: 0x0008DF1C File Offset: 0x0008C31C
		public override bool CanResolve(ResolveParams rp)
		{
			return base.CanResolve(rp) && (rp.cultivatedPlantDef != null || SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect) != null);
		}

		// Token: 0x060010B0 RID: 4272 RVA: 0x0008DF64 File Offset: 0x0008C364
		public override void Resolve(ResolveParams rp)
		{
			ThingDef thingDef = rp.cultivatedPlantDef ?? SymbolResolver_CultivatedPlants.DeterminePlantDef(rp.rect);
			bool flag = rp.rect.Width >= 7 && rp.rect.Height >= 7 && (rp.rect.Width > 12 || rp.rect.Height > 12 || Rand.Bool) && thingDef.plant.Harvestable;
			if (flag)
			{
				CellRect rect = new CellRect(rp.rect.maxX - 3, rp.rect.maxZ - 3, 4, 4);
				ThingDef harvestedThingDef = thingDef.plant.harvestedThingDef;
				int num = Rand.RangeInclusive(2, 3);
				for (int i = 0; i < num; i++)
				{
					ResolveParams resolveParams = rp;
					resolveParams.rect = rect.ContractedBy(1);
					resolveParams.singleThingDef = harvestedThingDef;
					resolveParams.singleThingStackCount = new int?(Rand.RangeInclusive(Mathf.Min(10, harvestedThingDef.stackLimit), Mathf.Min(50, harvestedThingDef.stackLimit)));
					BaseGen.symbolStack.Push("thing", resolveParams);
				}
				ResolveParams resolveParams2 = rp;
				resolveParams2.rect = rect;
				BaseGen.symbolStack.Push("ensureCanReachMapEdge", resolveParams2);
				ResolveParams resolveParams3 = rp;
				resolveParams3.rect = rect;
				BaseGen.symbolStack.Push("emptyRoom", resolveParams3);
			}
			ResolveParams resolveParams4 = rp;
			resolveParams4.cultivatedPlantDef = thingDef;
			BaseGen.symbolStack.Push("cultivatedPlants", resolveParams4);
		}
	}
}
