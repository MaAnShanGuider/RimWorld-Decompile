﻿using System;
using UnityEngine;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x020003DE RID: 990
	public class SymbolResolver_Ship_Core : SymbolResolver
	{
		// Token: 0x060010F8 RID: 4344 RVA: 0x000909C4 File Offset: 0x0008EDC4
		public override void Resolve(ResolveParams rp)
		{
			float value = Rand.Value;
			rp.faction = Faction.OfPlayer;
			ResolveParams resolveParams = rp;
			resolveParams.rect = resolveParams.rect.ContractedBy(2);
			resolveParams.hpPercentRange = new FloatRange?(new FloatRange(Mathf.Lerp(1.2f, 0.2f, value + Rand.Range(-0.2f, 0.2f)), 1.5f));
			BaseGen.symbolStack.Push("ship_pregen", resolveParams);
			BaseGen.symbolStack.Push("ensureCanReachMapEdge", rp);
			ResolveParams resolveParams2 = rp;
			resolveParams2.clearFillageOnly = new bool?(true);
			resolveParams2.clearRoof = new bool?(true);
			BaseGen.symbolStack.Push("clear", resolveParams2);
		}
	}
}
